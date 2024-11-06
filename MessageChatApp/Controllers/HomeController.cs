using MessageChatApp.Hubs;
using MessageChatApp.Models;
using MessageChatApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace MessageChatApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly DBContext _db;

        public HomeController(ILogger<HomeController> logger, IHubContext<ChatHub> hubContext, DBContext db)
        {
            _logger = logger;
            _hubContext = hubContext;
            _db = db;
        }

        public IActionResult Index()
        {
            var currentUser = HttpContext.Session.GetString("UserId");
            var currentUserName = HttpContext.Session.GetString("UserName");

            if (!int.TryParse(currentUser, out int currentUserId))
            {
                TempData["Message"] = string.IsNullOrEmpty(currentUser)
                    ? "Bạn cần đăng nhập để truy cập trang này."
                    : "Lỗi UserId trong session.";
                return RedirectToAction("LoginUser");
            }

            Console.WriteLine($"currentUser: {currentUser}");

            var getAllUsers = _db.TbUsers.Where(u => u.UserId != currentUserId).ToList();

            var usersMessagedWith = _db.TbConversationMembers
                                            .Where(cm => cm.UserId == currentUserId)
                                            .Select(cm => cm.ConversationId)
                                            .Distinct()
                                            .Join(_db.TbMessages, cId => cId, msg => msg.ConversationId, (cId, msg) => new { cId, msg })
                                            .Where(x => x.msg.Content != null && x.msg.Content != "")
                                            .Select(x => x.cId)
                                            .Distinct()
                                            .Join(_db.TbConversationMembers, cId => cId, cm => cm.ConversationId, (cId, cm) => cm)
                                            .Where(cm => cm.UserId != currentUserId)
                                            .Select(cm => cm.User)
                                            .Distinct()
                                            .ToList();


            var model = new UserViewModel
            {
                Users = getAllUsers,
                CurrentUserName = currentUserName,
                UsersWith = usersMessagedWith
            };

            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("RegisterUser")]
        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }


        [Route("RegisterUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterUser(TbUser user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _db.TbUsers.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser == null)
                {
                    _db.Add(user);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "Người dùng đã tồn tại!";
                    return View(user);
                }

            }
            else
            {
                TempData["Message"] = "Dữ liệu không hợp lệ!";
                return View(user);
            }
        }

        [Route("LoginUser")]
        [HttpGet]
        public IActionResult LoginUser()
        {
            return View();
        }

        [Route("LoginUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["Message"] = "Email và mật khẩu không được để trống!";
                return View();
            }

            var user = _db.TbUsers.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserEmail", user.Email);

                TempData["Message"] = "Đăng nhập thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Email hoặc mật khẩu không đúng!";
                return View();
            }

        }

        [Route("LogoutUser")]
        public IActionResult LogoutUser()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("LoginUser", "Home");
        }

        [HttpGet]
        [Route("RediToChat/{id}")]
        public IActionResult RediToChat(int id)
        {


            var currentUserIdStr = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(currentUserIdStr) || !int.TryParse(currentUserIdStr, out int currentUserId))
            {
                _logger.LogInformation("Invalid or missing UserId in session");
                return RedirectToAction("Error");
            }

            var conversation = _db.TbConversations
                                        .Where(c => !c.IsGroup)
                                        .Join(_db.TbConversationMembers, c => c.ConversationId, m => m.ConversationId,
                                            (c, m) => new { c, m })
                                        .Where(cm => cm.m.UserId == id || cm.m.UserId == currentUserId)
                                        .GroupBy(cm => cm.c.ConversationId) // Nhóm theo ConversationId để kiểm tra thành viên
                                        .Where(g => g.Count() == 2)
                                        .Select(g => g.FirstOrDefault().c)
                                        .FirstOrDefault();


            if (conversation == null)
            {
                var newConversation = new TbConversation
                {
                    IsGroup = false,
                    CreatedAt = DateTime.Now
                };

                _db.TbConversations.Add(newConversation);
                _db.SaveChanges();

                _db.TbConversationMembers.Add(new TbConversationMember
                {
                    ConversationId = newConversation.ConversationId,
                    UserId = id,
                    JoinedAt = DateTime.Now
                });
                _db.TbConversationMembers.Add(new TbConversationMember
                {
                    ConversationId = newConversation.ConversationId,
                    UserId = currentUserId,
                    JoinedAt = DateTime.Now
                });

                _db.SaveChanges();

                conversation = newConversation;

            }
            else
            {

            }

            var messages = _db.TbMessages.Where(m => m.ConversationId == conversation.ConversationId).OrderBy(m => m.SentAt).ToList();

            var user = _db.TbUsers.FirstOrDefault(u => u.UserId == id);

            var model = new ChatViewModel
            {
                Conversation = conversation,
                Messages = messages,
                UserName = user?.UserName ?? "Unknown",
                UserId = user?.UserId ?? 0,
                CurrentUserId = currentUserId
            };

            return View("RediToChat", model);
        }

        [HttpPost]
        public IActionResult SendImage(ImageMessageViewModel imageMessageViewModel, [FromRoute] int id)
        {
            if (ModelState.IsValid && imageMessageViewModel.ImageUpload != null)
            {
                // Đường dẫn lưu ảnh trong thư mục `wwwroot/images`
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageMessageViewModel.ImageUpload.FileName;

                // Đường dẫn đầy đủ của file ảnh
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Lưu tệp ảnh vào thư mục
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageMessageViewModel.ImageUpload.CopyTo(fileStream);
                }

                var currentUserIdStr = HttpContext.Session.GetString("UserId");

                if (string.IsNullOrEmpty(currentUserIdStr) || !int.TryParse(currentUserIdStr, out int currentUserId))
                {
                    _logger.LogInformation("Invalid or missing UserId in session");
                    return RedirectToAction("Error");
                }

                // Tạo đối tượng `TbImageMessages` và lưu tên file vào CSDL
                TbMessage image = new TbMessage
                {
                    ConversationId = id,
                    SenderId = currentUserId,
                    Content = uniqueFileName,
                    SentAt = DateTime.Now,
                };

                _db.TbMessages.Add(image);
                _db.SaveChanges();


                return NoContent();
            }
            return View(imageMessageViewModel);
        }

    }
}

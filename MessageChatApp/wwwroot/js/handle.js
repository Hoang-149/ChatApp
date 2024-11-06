jQuery(document).ready(function ($) {
    const connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

    let userIdSender = document.getElementById("userIdCurrent").value;
    const userIdReceive = document.getElementById("userIdReceive").value;

    connection.on("ReceiveMessage", function (senderId, message) {

        const msg = document.createElement("div");

        let renderMess = '';

        if (!senderId) return

        if (userIdSender == senderId) {
            renderMess = `<div class="d-flex justify-content-end mb-4">
                            <div class="msg_cotainer_send">
                                ${message}
                                <span class="msg_time">Today</span>
                            </div>
                            <div class="img_cont_msg">
                                <img src="https://static.turbosquid.com/Preview/001292/481/WV/_D.jpg" class="rounded-circle user_img_msg">
                            </div>
                        </div>`;
        } else {
            renderMess = `<div class="d-flex justify-content-start mb-4">
                                <div class="img_cont_msg">
                                        <img src="https://static.turbosquid.com/Preview/001292/481/WV/_D.jpg" class="rounded-circle user_img_msg">
                                    </div>
                                <div class="msg_cotainer">
                                    ${message}
                                    <span class="msg_time">Today</span>
                                </div>
                            </div>`;
        }

        msg.innerHTML = renderMess;

        document.getElementById("messagesList").appendChild(msg);
    });

    connection.start().then(function () {
        // console.log("SignalR Connected");
    }).catch(function (err) {
        console.error("SignalR connection error: ", err.toString());
    });

    document.getElementById("sendButton").addEventListener("click", function (e) {

        if (document.getElementById("messageInput").value == "") return;
        const message = document.getElementById("messageInput").value;
        const idCon = document.getElementById("idConversation").value;
        const idSender = document.getElementById("userIdCurrent").value || "";

        let ImageMessage = document.getElementById("imageUpload").files.length > 0 ? document.getElementById("imageUpload").files : "";
        debugger;
        if (document.getElementById("imageUpload").files.length > 0) {
            console.log("Vo day");
        } else {
            console.log("Khong vo")
        }

        e.preventDefault();

        connection.invoke("SendMessage", idCon, idSender, message, ImageMessage).then(function () {
            // console.log("test");
        }).catch(function (err) {
            console.error("Error sending message: ", err.toString());
        });
        e.preventDefault();
        document.getElementById("messageInput").value = "";
    });

    var $f = jQuery(".attach-file #imageUpload");

    $f.change(function () {
        var file = this.files[0];

        if (file) {
            console.log("Updload");
            var reader = new FileReader();
            reader.onload = function (e) {
                jQuery("#previewImage").attr("src", e.target.result).show();
            }
            reader.readAsDataURL(file);
        }
    });


});
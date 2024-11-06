using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessageChatApp.Migrations
{
    /// <inheritdoc />
    public partial class RenameInvalidColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbConversations",
                columns: table => new
                {
                    ConversationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConversationName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsGroup = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbConver__C050D87720246C0F", x => x.ConversationId);
                });

            migrationBuilder.CreateTable(
                name: "tbUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbUsers__1788CC4CF373C7F1", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "tbConversationMembers",
                columns: table => new
                {
                    ConversationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbConver__112854B32782EA97", x => new { x.ConversationId, x.UserId });
                    table.ForeignKey(
                        name: "FK__tbConvers__Conve__2C3393D0",
                        column: x => x.ConversationId,
                        principalTable: "tbConversations",
                        principalColumn: "ConversationId");
                    table.ForeignKey(
                        name: "FK__tbConvers__UserI__2D27B809",
                        column: x => x.UserId,
                        principalTable: "tbUsers",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "tbMessages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConversationId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbMessag__C87C0C9C5C55AFFF", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK__tbMessage__Conve__30F848ED",
                        column: x => x.ConversationId,
                        principalTable: "tbConversations",
                        principalColumn: "ConversationId");
                    table.ForeignKey(
                        name: "FK__tbMessage__Sende__31EC6D26",
                        column: x => x.SenderId,
                        principalTable: "tbUsers",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "tbImageMessages ",
                columns: table => new
                {
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ConversationId = table.Column<int>(type: "int", nullable: false),
                    ImageMessageId = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TbMessageMessageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbImageMessages", x => new { x.ConversationId, x.SenderId });
                    table.ForeignKey(
                        name: "FK__tbImageMessages__ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "tbConversations",
                        principalColumn: "ConversationId");
                    table.ForeignKey(
                        name: "FK__tbImageMessages__UserId",
                        column: x => x.SenderId,
                        principalTable: "tbUsers",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_tbImageMessages _tbMessages_TbMessageMessageId",
                        column: x => x.TbMessageMessageId,
                        principalTable: "tbMessages",
                        principalColumn: "MessageId");
                });

            migrationBuilder.CreateTable(
                name: "tbMessageStatus",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbMessag__1904805809356569", x => new { x.MessageId, x.UserId });
                    table.ForeignKey(
                        name: "FK__tbMessage__Messa__35BCFE0A",
                        column: x => x.MessageId,
                        principalTable: "tbMessages",
                        principalColumn: "MessageId");
                    table.ForeignKey(
                        name: "FK__tbMessage__UserI__36B12243",
                        column: x => x.UserId,
                        principalTable: "tbUsers",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbConversationMembers_UserId",
                table: "tbConversationMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tbImageMessages _SenderId",
                table: "tbImageMessages ",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_tbImageMessages _TbMessageMessageId",
                table: "tbImageMessages ",
                column: "TbMessageMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_tbMessages_ConversationId",
                table: "tbMessages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_tbMessages_SenderId",
                table: "tbMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_tbMessageStatus_UserId",
                table: "tbMessageStatus",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__tbUsers__A9D105349499971D",
                table: "tbUsers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbConversationMembers");

            migrationBuilder.DropTable(
                name: "tbImageMessages ");

            migrationBuilder.DropTable(
                name: "tbMessageStatus");

            migrationBuilder.DropTable(
                name: "tbMessages");

            migrationBuilder.DropTable(
                name: "tbConversations");

            migrationBuilder.DropTable(
                name: "tbUsers");
        }
    }
}

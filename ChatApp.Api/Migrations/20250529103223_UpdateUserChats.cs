using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserChats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_ChatUser_Chats_ChatsId", "ChatUser");
            migrationBuilder.DropForeignKey("FK_ChatUser_Users_UsersId", "ChatUser");
            migrationBuilder.DropPrimaryKey("PK_ChatUser", "ChatUser");
            migrationBuilder.DropIndex("IX_ChatUser_UsersId", "ChatUser");
            migrationBuilder.DropColumn("ChatsId", "ChatUser");
            migrationBuilder.RenameColumn("UsersId", "ChatUser", "ChatId");

            // <<< Очищаем "плохие" строки, чтобы не было NULL
            migrationBuilder.Sql(@"DELETE FROM ""ChatUser"" WHERE ""UserId"" IS NULL;");

            // Делаем UserId NOT NULL (без defaultValue)
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ChatUser",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatUser",
                table: "ChatUser",
                columns: new[] { "ChatId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_Chats_ChatId",
                table: "ChatUser",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_Chats_ChatId",
                table: "ChatUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatUser",
                table: "ChatUser");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "ChatUser",
                newName: "UsersId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ChatUser",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "ChatsId",
                table: "ChatUser",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatUser",
                table: "ChatUser",
                columns: new[] { "ChatsId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_Chats_ChatsId",
                table: "ChatUser",
                column: "ChatsId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

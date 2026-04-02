using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotogramAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_AddresseeId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_RequesterId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_UserId",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_UserId",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "RequesterId",
                table: "Friendships",
                newName: "FollowingId");

            migrationBuilder.RenameColumn(
                name: "AddresseeId",
                table: "Friendships",
                newName: "FollowerId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_RequesterId",
                table: "Friendships",
                newName: "IX_Friendships_FollowingId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_AddresseeId",
                table: "Friendships",
                newName: "IX_Friendships_FollowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_FollowerId",
                table: "Friendships",
                column: "FollowerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_FollowingId",
                table: "Friendships",
                column: "FollowingId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_FollowerId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_FollowingId",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "FollowingId",
                table: "Friendships",
                newName: "RequesterId");

            migrationBuilder.RenameColumn(
                name: "FollowerId",
                table: "Friendships",
                newName: "AddresseeId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_FollowingId",
                table: "Friendships",
                newName: "IX_Friendships_RequesterId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_FollowerId",
                table: "Friendships",
                newName: "IX_Friendships_AddresseeId");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Friendships",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Friendships",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_UserId",
                table: "Friendships",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_AddresseeId",
                table: "Friendships",
                column: "AddresseeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_RequesterId",
                table: "Friendships",
                column: "RequesterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_UserId",
                table: "Friendships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}

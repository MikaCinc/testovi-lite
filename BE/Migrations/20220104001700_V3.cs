using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpojnicaID",
                table: "Tag",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpojnicaID",
                table: "Pitanje",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_SpojnicaID",
                table: "Tag",
                column: "SpojnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_Pitanje_SpojnicaID",
                table: "Pitanje",
                column: "SpojnicaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pitanje_Spojnica_SpojnicaID",
                table: "Pitanje",
                column: "SpojnicaID",
                principalTable: "Spojnica",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Spojnica_SpojnicaID",
                table: "Tag",
                column: "SpojnicaID",
                principalTable: "Spojnica",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pitanje_Spojnica_SpojnicaID",
                table: "Pitanje");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Spojnica_SpojnicaID",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_SpojnicaID",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Pitanje_SpojnicaID",
                table: "Pitanje");

            migrationBuilder.DropColumn(
                name: "SpojnicaID",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "SpojnicaID",
                table: "Pitanje");
        }
    }
}

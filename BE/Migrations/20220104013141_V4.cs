using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "SpojnicePitanja",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpojnicaID = table.Column<int>(type: "int", nullable: true),
                    PitanjeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpojnicePitanja", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SpojnicePitanja_Pitanje_PitanjeID",
                        column: x => x.PitanjeID,
                        principalTable: "Pitanje",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpojnicePitanja_Spojnica_SpojnicaID",
                        column: x => x.SpojnicaID,
                        principalTable: "Spojnica",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpojniceTagovi",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpojnicaID = table.Column<int>(type: "int", nullable: true),
                    TagID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpojniceTagovi", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SpojniceTagovi_Spojnica_SpojnicaID",
                        column: x => x.SpojnicaID,
                        principalTable: "Spojnica",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpojniceTagovi_Tag_TagID",
                        column: x => x.TagID,
                        principalTable: "Tag",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpojnicePitanja_PitanjeID",
                table: "SpojnicePitanja",
                column: "PitanjeID");

            migrationBuilder.CreateIndex(
                name: "IX_SpojnicePitanja_SpojnicaID",
                table: "SpojnicePitanja",
                column: "SpojnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_SpojniceTagovi_SpojnicaID",
                table: "SpojniceTagovi",
                column: "SpojnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_SpojniceTagovi_TagID",
                table: "SpojniceTagovi",
                column: "TagID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpojnicePitanja");

            migrationBuilder.DropTable(
                name: "SpojniceTagovi");

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
    }
}

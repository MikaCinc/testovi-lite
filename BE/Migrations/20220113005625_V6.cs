using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class V6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Set",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Set", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SetPitanja",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SetID = table.Column<int>(type: "int", nullable: true),
                    PitanjeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetPitanja", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SetPitanja_Pitanje_PitanjeID",
                        column: x => x.PitanjeID,
                        principalTable: "Pitanje",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SetPitanja_Set_SetID",
                        column: x => x.SetID,
                        principalTable: "Set",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SetSpojnice",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SetID = table.Column<int>(type: "int", nullable: true),
                    SpojnicaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetSpojnice", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SetSpojnice_Set_SetID",
                        column: x => x.SetID,
                        principalTable: "Set",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SetSpojnice_Spojnica_SpojnicaID",
                        column: x => x.SpojnicaID,
                        principalTable: "Spojnica",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SetTagovi",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SetID = table.Column<int>(type: "int", nullable: true),
                    TagID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetTagovi", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SetTagovi_Set_SetID",
                        column: x => x.SetID,
                        principalTable: "Set",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SetTagovi_Tag_TagID",
                        column: x => x.TagID,
                        principalTable: "Tag",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SetPitanja_PitanjeID",
                table: "SetPitanja",
                column: "PitanjeID");

            migrationBuilder.CreateIndex(
                name: "IX_SetPitanja_SetID",
                table: "SetPitanja",
                column: "SetID");

            migrationBuilder.CreateIndex(
                name: "IX_SetSpojnice_SetID",
                table: "SetSpojnice",
                column: "SetID");

            migrationBuilder.CreateIndex(
                name: "IX_SetSpojnice_SpojnicaID",
                table: "SetSpojnice",
                column: "SpojnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_SetTagovi_SetID",
                table: "SetTagovi",
                column: "SetID");

            migrationBuilder.CreateIndex(
                name: "IX_SetTagovi_TagID",
                table: "SetTagovi",
                column: "TagID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SetPitanja");

            migrationBuilder.DropTable(
                name: "SetSpojnice");

            migrationBuilder.DropTable(
                name: "SetTagovi");

            migrationBuilder.DropTable(
                name: "Set");
        }
    }
}

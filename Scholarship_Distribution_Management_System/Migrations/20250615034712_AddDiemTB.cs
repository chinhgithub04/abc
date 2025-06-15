using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scholarship_Distribution_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class AddDiemTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diems",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiemHocTap = table.Column<float>(type: "real", nullable: true),
                    DiemRenLuyen = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Diems_AspNetUsers_ID",
                        column: x => x.ID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Diems");
        }
    }
}

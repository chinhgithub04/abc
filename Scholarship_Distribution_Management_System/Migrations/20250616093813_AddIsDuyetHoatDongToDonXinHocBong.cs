using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scholarship_Distribution_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDuyetHoatDongToDonXinHocBong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDuyetHoatDong",
                table: "DonXinHocBongs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDuyetHoatDong",
                table: "DonXinHocBongs");
        }
    }
}

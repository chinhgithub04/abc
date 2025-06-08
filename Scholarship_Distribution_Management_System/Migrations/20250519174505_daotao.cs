using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scholarship_Distribution_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class daotao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HoatDongs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenHoatDong = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoatDongs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Khoas",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenKhoa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khoas", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NghienCuuKhoaHocs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenDeTai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThanhTich = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NghienCuuKhoaHocs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nganhs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDKhoa = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenNganh = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nganhs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Nganhs_Khoas_IDKhoa",
                        column: x => x.IDKhoa,
                        principalTable: "Khoas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LopSHs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDNganh = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenLopSH = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LopSHs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LopSHs_Nganhs_IDNganh",
                        column: x => x.IDNganh,
                        principalTable: "Nganhs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IDLopSH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    LopSHID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_LopSHs_LopSHID",
                        column: x => x.LopSHID,
                        principalTable: "LopSHs",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DotHocBongs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDNhanVien = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenDot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DieuKien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    Tien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayBatDauNop = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThucNop = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayHoiDongDuyet = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayPTCDuyet = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DotHocBongs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DotHocBongs_AspNetUsers_IDNhanVien",
                        column: x => x.IDNhanVien,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonXinHocBongs",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDSinhVien = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDDot = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayNop = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiemHocTap = table.Column<float>(type: "real", nullable: false),
                    DiemRenLuyen = table.Column<float>(type: "real", nullable: false),
                    DuyetHoiDong = table.Column<bool>(type: "bit", nullable: false),
                    DuyetCapPhatHocBong = table.Column<bool>(type: "bit", nullable: false),
                    KQDiemRL = table.Column<float>(type: "real", nullable: false),
                    KQDiemHT = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonXinHocBongs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DonXinHocBongs_AspNetUsers_IDSinhVien",
                        column: x => x.IDSinhVien,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonXinHocBongs_DotHocBongs_IDDot",
                        column: x => x.IDDot,
                        principalTable: "DotHocBongs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HoatDongCuaSinhViens",
                columns: table => new
                {
                    IDDon = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDHoatDong = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoatDongCuaSinhViens", x => new { x.IDDon, x.IDHoatDong });
                    table.ForeignKey(
                        name: "FK_HoatDongCuaSinhViens_DonXinHocBongs_IDDon",
                        column: x => x.IDDon,
                        principalTable: "DonXinHocBongs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoatDongCuaSinhViens_HoatDongs_IDHoatDong",
                        column: x => x.IDHoatDong,
                        principalTable: "HoatDongs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KqHoatDong",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDDon = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDSinhVien = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDHoatDong = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KqHoatDong", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KqHoatDong_AspNetUsers_IDSinhVien",
                        column: x => x.IDSinhVien,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KqHoatDong_DonXinHocBongs_IDDon",
                        column: x => x.IDDon,
                        principalTable: "DonXinHocBongs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KqHoatDong_HoatDongs_IDHoatDong",
                        column: x => x.IDHoatDong,
                        principalTable: "HoatDongs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KqNghienCuu",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDDon = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDSinhVien = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDNghienCuu = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KqNghienCuu", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KqNghienCuu_AspNetUsers_IDSinhVien",
                        column: x => x.IDSinhVien,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KqNghienCuu_DonXinHocBongs_IDDon",
                        column: x => x.IDDon,
                        principalTable: "DonXinHocBongs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KqNghienCuu_NghienCuuKhoaHocs_IDNghienCuu",
                        column: x => x.IDNghienCuu,
                        principalTable: "NghienCuuKhoaHocs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nghienCuuCuaSinhViens",
                columns: table => new
                {
                    IDDon = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDNghienCuu = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nghienCuuCuaSinhViens", x => new { x.IDDon, x.IDNghienCuu });
                    table.ForeignKey(
                        name: "FK_nghienCuuCuaSinhViens_DonXinHocBongs_IDDon",
                        column: x => x.IDDon,
                        principalTable: "DonXinHocBongs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_nghienCuuCuaSinhViens_NghienCuuKhoaHocs_IDNghienCuu",
                        column: x => x.IDNghienCuu,
                        principalTable: "NghienCuuKhoaHocs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LopSHID",
                table: "AspNetUsers",
                column: "LopSHID");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DonXinHocBongs_IDDot",
                table: "DonXinHocBongs",
                column: "IDDot");

            migrationBuilder.CreateIndex(
                name: "IX_DonXinHocBongs_IDSinhVien",
                table: "DonXinHocBongs",
                column: "IDSinhVien");

            migrationBuilder.CreateIndex(
                name: "IX_DotHocBongs_IDNhanVien",
                table: "DotHocBongs",
                column: "IDNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_HoatDongCuaSinhViens_IDHoatDong",
                table: "HoatDongCuaSinhViens",
                column: "IDHoatDong");

            migrationBuilder.CreateIndex(
                name: "IX_KqHoatDong_IDDon",
                table: "KqHoatDong",
                column: "IDDon");

            migrationBuilder.CreateIndex(
                name: "IX_KqHoatDong_IDHoatDong",
                table: "KqHoatDong",
                column: "IDHoatDong");

            migrationBuilder.CreateIndex(
                name: "IX_KqHoatDong_IDSinhVien",
                table: "KqHoatDong",
                column: "IDSinhVien");

            migrationBuilder.CreateIndex(
                name: "IX_KqNghienCuu_IDDon",
                table: "KqNghienCuu",
                column: "IDDon");

            migrationBuilder.CreateIndex(
                name: "IX_KqNghienCuu_IDNghienCuu",
                table: "KqNghienCuu",
                column: "IDNghienCuu");

            migrationBuilder.CreateIndex(
                name: "IX_KqNghienCuu_IDSinhVien",
                table: "KqNghienCuu",
                column: "IDSinhVien");

            migrationBuilder.CreateIndex(
                name: "IX_LopSHs_IDNganh",
                table: "LopSHs",
                column: "IDNganh");

            migrationBuilder.CreateIndex(
                name: "IX_Nganhs_IDKhoa",
                table: "Nganhs",
                column: "IDKhoa");

            migrationBuilder.CreateIndex(
                name: "IX_nghienCuuCuaSinhViens_IDNghienCuu",
                table: "nghienCuuCuaSinhViens",
                column: "IDNghienCuu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "HoatDongCuaSinhViens");

            migrationBuilder.DropTable(
                name: "KqHoatDong");

            migrationBuilder.DropTable(
                name: "KqNghienCuu");

            migrationBuilder.DropTable(
                name: "nghienCuuCuaSinhViens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "HoatDongs");

            migrationBuilder.DropTable(
                name: "DonXinHocBongs");

            migrationBuilder.DropTable(
                name: "NghienCuuKhoaHocs");

            migrationBuilder.DropTable(
                name: "DotHocBongs");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "LopSHs");

            migrationBuilder.DropTable(
                name: "Nganhs");

            migrationBuilder.DropTable(
                name: "Khoas");
        }
    }
}

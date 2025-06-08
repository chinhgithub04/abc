using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Scholarship_Distribution_Management_System.Models.Entities
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet cho các bảng khác từ ERD của bạn
        public DbSet<DotHocBong> DotHocBongs { get; set; }
        public DbSet<DonXinHocBong> DonXinHocBongs { get; set; }
        public DbSet<KQHoatDong> KqHoatDong { get; set; }
        public DbSet<KQNghienCuu> KqNghienCuu { get; set; }
        public DbSet<HoatDong> HoatDongs { get; set; }
        public DbSet<NghienCuuKhoaHoc> NghienCuuKhoaHocs { get; set; }
        public DbSet<HoatDongCuaSinhVien> HoatDongCuaSinhViens { get; set; }
        public DbSet<NghienCuuCuaSinhVien> nghienCuuCuaSinhViens { get; set; }
        public DbSet<LopSH> LopSHs { get; set; }
        public DbSet<Khoa> Khoas { get; set; }
        public DbSet<Nganh> Nganhs { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DonXinHocBong>()
               .HasOne(d => d.DotHocBong)
               .WithMany()
               .HasForeignKey(d => d.IDDot)
               .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<DonXinHocBong>()
                .HasOne(d => d.SinhVien)
                .WithMany()
                .HasForeignKey(d => d.IDSinhVien)
                .OnDelete(DeleteBehavior.Restrict);  


            modelBuilder.Entity<HoatDongCuaSinhVien>()
                .HasKey(h => new { h.IDDon, h.IDHoatDong });
            modelBuilder.Entity<NghienCuuCuaSinhVien>()
                .HasKey(n => new { n.IDDon, n.IDNghienCuu });

            // Quan hệ HoatDongCuaSinhVien
            modelBuilder.Entity<HoatDongCuaSinhVien>()
                .HasOne(h => h.DonXinHocBong)
                .WithMany(d => d.HoatDongCuaSinhViens)
                .HasForeignKey(h => h.IDDon);

            modelBuilder.Entity<HoatDongCuaSinhVien>()
                .HasOne(h => h.HoatDong)
                .WithMany(hd => hd.HoatDongCuaSinhViens)
                .HasForeignKey(h => h.IDHoatDong);

            // Quan hệ NghienCuuCuaSinhVien
            modelBuilder.Entity<NghienCuuCuaSinhVien>()
                .HasOne(n => n.DonXinHocBong)
                .WithMany(d => d.NghienCuuCuaSinhViens)
                .HasForeignKey(n => n.IDDon);

            modelBuilder.Entity<NghienCuuCuaSinhVien>()
                .HasOne(n => n.NghienCuu)
                .WithMany(nc => nc.NghienCuuCuaSinhViens)
                .HasForeignKey(n => n.IDNghienCuu);
        }

    }
}


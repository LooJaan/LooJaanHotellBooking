using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
	public class HotelDbContext : DbContext
	{
		public HotelDbContext(DbContextOptions options) : base(options)
		{
		}
		public HotelDbContext() { }
		public DbSet<LoaiPhong> LoaiPhongs { get; set; }
		public DbSet<DichVu> DichVus { get; set; }
		/*public DbSet<PhongChiTiet> PhongChiTiets { get; set; }*/
		public DbSet<TaiKhoann> TaiKhoans { get; set; }
		public DbSet<DatPhong> DatPhongs { get; set; }
		public DbSet<AnhChiTiet> AnhChiTiets { get; set; }
		public DbSet<LienHe> lienHes { get; set; }
		public DbSet<LoGo> loGos { get; set; }
		public DbSet<MenuItem> Menu { get; set; }
		public DbSet<Slide> Slides { get; set; }
		public DbSet<TinTuc> tinTucs { get; set; }
		public DbSet<WelCome> welComes { get; set; }
		public DbSet<LoaiPhongDichVu> LoaiPhongDichVu  { get; set; }
        public DbSet<LichDatPhong> LichDatPhongs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ nhiều-nhiều
            modelBuilder.Entity<LoaiPhongDichVu>()
                .HasKey(dv => new { dv.DichVusID, dv.LoaiPhongsId });  // Xác định khóa chính cho bảng trung gian

            modelBuilder.Entity<LoaiPhongDichVu>()
                .HasOne(dv => dv.DichVu)  // Mối quan hệ giữa DichVu và DichVuLoaiPhong
                .WithMany(dv => dv.DichVuLoaiPhongs)  // Liên kết với DichVu
                .HasForeignKey(dv => dv.DichVusID);  // Khóa ngoại

            modelBuilder.Entity<LoaiPhongDichVu>()
                .HasOne(lp => lp.LoaiPhong)  // Mối quan hệ giữa LoaiPhong và DichVuLoaiPhong
                .WithMany(lp => lp.DichVuLoaiPhongs)  // Liên kết với LoaiPhong
                .HasForeignKey(lp => lp.LoaiPhongsId);  // Khóa ngoại

            modelBuilder.Entity<AnhChiTiet>()
			   .HasOne(a => a.LoaiPhong)
			   .WithMany(l => l.HinhAnhPhongs)
			   .HasForeignKey(a => a.IdLoaiPhong) // Chỉ rõ ngoại khóa
			   .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<LichDatPhong>()
			   .HasOne(ldp => ldp.LoaiPhong)
			   .WithMany()
			   .HasForeignKey(ldp => ldp.LoaiPhongID);


        
        }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Data Source=NOONE\\MSSQLSERVER02;Initial Catalog=BOOKING20;Integrated Security=True;Trust Server Certificate=True");
		}

	}
}


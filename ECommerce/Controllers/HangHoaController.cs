using ECommerce.Data;
using ECommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly Hshop2023Context db;

        public HangHoaController(Hshop2023Context context) {
            db = context;
        }
        public IActionResult Index(int? loai, string sortOrder, int pageNumber = 1, int pageSize = 12)
        {
            var hangHoas = db.HangHoas.AsQueryable();

            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
            }

            if (!sortOrder.IsNullOrEmpty())
            {
                switch (sortOrder)
                {
                    case "price_asc":
                        hangHoas = hangHoas.OrderBy(p => p.DonGia);
                        break;
                    case "price_desc":
                        hangHoas = hangHoas.OrderByDescending(p => p.DonGia);
                        break;
                    default:
                        // Xử lý khi không có sắp xếp cụ thể
                        break;
                }
            }

            var items = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });

            // Tính tổng số phần tử
            int totalItems = items.Count();

            // Phân trang bằng cách sử dụng Skip và Take
            var paginatedResult = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Tạo ViewModel để truyền vào View
            var results = new HangHoaPagedVM
            {
                Items = paginatedResult,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                CurrentSort = sortOrder
            };
            return View(results);
        }

        public IActionResult Search(string? query)
        {
            var hangHoas = db.HangHoas.AsQueryable();

            if (query != null)
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }

            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });
            ViewBag.query = query;
            return View(result);
        }

        public IActionResult Detail(int id)
        {
            var data = db.HangHoas
                .Include(p => p.MaLoaiNavigation)
                .SingleOrDefault(p => p.MaHh == id);
            if(data == null)
            {
                TempData["Message"] = $"Không tìm thấy sản phẩm có mã {id}";
                return Redirect("/404");
            }
            var result = new ChiTietHangHoaVM
            {
                MaHh = data.MaHh,
                TenHH = data.TenHh,
                DonGia = data.DonGia ?? 0,
                ChiTiet = data.MoTa ?? string.Empty,
                Hinh = data.Hinh ?? string.Empty,
                MoTaNgan = data.MoTaDonVi ?? string.Empty,
                DiemDanhGia = 5, // check sau
                SoLuongTon = 10, //Tinh sau
                TenLoai = data.MaLoaiNavigation.TenLoai
            };
            return View(result);
        }
    }
}

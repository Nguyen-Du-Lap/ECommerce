namespace ECommerce.ViewModels
{
    public class HangHoaPagedVM
    {
        public IEnumerable<HangHoaVM> Items { get; set; }  // Danh sách hàng hóa phân trang
        public int PageNumber { get; set; }                 // Trang hiện tại
        public int PageSize { get; set; }                   // Số phần tử trên mỗi trang
        public int TotalItems { get; set; }                 // Tổng số phần tử
        public int TotalPages { get; set; }                 // Tổng số trang
        public string CurrentSort { get; set; }             // Thứ tự sắp xếp hiện tại
    }
}

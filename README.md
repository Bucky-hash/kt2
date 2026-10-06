# Quản lý sinh viên với LINQ to XML

Chương trình console C# đọc `students.xml` bằng `XDocument` và truy vấn trực tiếp bằng LINQ to XML. Không dùng cơ sở dữ liệu.

Yêu cầu
- .NET SDK 8.0 trở lên (đổi `TargetFramework` trong `StudentLinq.csproj` nếu dùng bản khác)

Cách chạy
bash
cd StudentLinq
dotnet run

File `students.xml` được tự động copy vào thư mục output khi build. Nếu chạy file .exe trực tiếp, đặt `students.xml` cùng thư mục với file .exe.

Menu
 Mục  Chức năng  Câu 
---------
 1  Danh sách sinh viên  1 
 2  GPA >= 8.0  2 
 3  Lớp CTK44  3 
 4  Sinh viên nữ  4 
 5  GPA cao nhất  5 
 6  GPA thấp nhất  6 
 7  Đếm sinh viên  7 
 8  GPA trung bình  8 
 9  Thống kê theo lớp  9 
 10  Sắp xếp GPA giảm dần  10 
 11  Tuổi 20 đến 21  11 
 12  CNTT và GPA >= 8.0  12 
 13  Tìm theo mã SV  13 
 14  Theo lớp nhập vào, GPA giảm dần  14 
 15  Top 3 GPA  15 
 0  Thoát  

Xử lý lỗi
- File không tồn tại: báo lỗi và thoát
- File sai cú pháp hoặc sai thẻ gốc: báo lỗi và thoát
- Dữ liệu thiếu thẻ hoặc sai kiểu số: báo lỗi, quay lại menu

Ghi chú
- Mục 5, 6: nếu nhiều sinh viên bằng điểm thì hiển thị tất cả
- Mục 13, 14: không phân biệt hoa thường
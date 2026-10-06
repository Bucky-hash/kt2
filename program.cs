using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace StudentLinq
{
    class Program
    {
        const string FilePath = "students.xml";
        static XDocument doc;

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            if (!LoadXml()) return;

            while (true)
            {
                PrintMenu();
                Console.Write("Chọn chức năng: ");
                string choice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                if (choice == "0") break;

                try
                {
                    Run(choice);
                }
                catch (Exception ex) when (ex is FormatException || ex is ArgumentNullException || ex is XmlException)
                {
                    Console.WriteLine("Dữ liệu XML sai định dạng: " + ex.Message);
                }

                Console.WriteLine("\nNhấn Enter để tiếp tục...");
                Console.ReadLine();
            }
        }

        // ---------- Đọc file ----------
        static bool LoadXml()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    Console.WriteLine($"Không tìm thấy file {FilePath}.");
                    return false;
                }
                doc = XDocument.Load(FilePath);
                if (doc.Root == null || doc.Root.Name != "Students")
                {
                    Console.WriteLine("File XML không đúng cấu trúc (thiếu thẻ gốc Students).");
                    return false;
                }
                return true;
            }
            catch (XmlException ex)
            {
                Console.WriteLine("File XML sai cú pháp: " + ex.Message);
                return false;
            }
            catch (IOException ex)
            {
                Console.WriteLine("Không đọc được file: " + ex.Message);
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Không có quyền đọc file: " + ex.Message);
                return false;
            }
        }

        // ---------- Truy vấn gốc ----------
        static IEnumerable<XElement> All() => doc.Root.Elements("Student");

        static double Gpa(XElement s) => (double)s.Element("GPA");
        static int Age(XElement s) => (int)s.Element("Age");
        static string Cls(XElement s) => (string)s.Element("Class");
        static string Id(XElement s) => (string)s.Attribute("id");

        // ---------- Menu ----------
        static void PrintMenu()
        {
            try { Console.Clear(); }
            catch (IOException) { Console.WriteLine(); } // Debug Console của VS Code không có console thật
            Console.WriteLine("===== QUẢN LÝ SINH VIÊN =====");
            Console.WriteLine("1. Hiển thị danh sách sinh viên");
            Console.WriteLine("2. Sinh viên GPA >= 8");
            Console.WriteLine("3. Sinh viên lớp CTK44");
            Console.WriteLine("4. Danh sách sinh viên nữ");
            Console.WriteLine("5. Sinh viên GPA cao nhất");
            Console.WriteLine("6. Sinh viên GPA thấp nhất");
            Console.WriteLine("7. Đếm số sinh viên");
            Console.WriteLine("8. GPA trung bình");
            Console.WriteLine("9. Thống kê theo lớp");
            Console.WriteLine("10. Sắp xếp theo GPA giảm dần");
            Console.WriteLine("11. Sinh viên tuổi từ 20 đến 21");
            Console.WriteLine("12. Ngành CNTT và GPA >= 8");
            Console.WriteLine("13. Tìm kiếm theo mã sinh viên");
            Console.WriteLine("14. Sinh viên theo lớp nhập vào (GPA giảm dần)");
            Console.WriteLine("15. Top 3 GPA cao nhất");
            Console.WriteLine("0. Thoát");
        }

        static void Run(string choice)
        {
            switch (choice)
            {
                case "1": Show(All()); break;

                case "2":
                    Show(All().Where(s => Gpa(s) >= 8.0));
                    break;

                case "3":
                    Show(All().Where(s => Cls(s) == "CTK44"));
                    break;

                case "4":
                    Show(All().Where(s => (string)s.Element("Gender") == "Nu"));
                    break;

                case "5":
                    {
                        double max = All().Max(Gpa);
                        Show(All().Where(s => Gpa(s) == max));
                        break;
                    }

                case "6":
                    {
                        double min = All().Min(Gpa);
                        Show(All().Where(s => Gpa(s) == min));
                        break;
                    }

                case "7":
                    Console.WriteLine("Số lượng sinh viên: " + All().Count());
                    break;

                case "8":
                    Console.WriteLine("GPA trung bình: " + All().Average(Gpa).ToString("0.00"));
                    break;

                case "9":
                    {
                        var stats = All()
                            .GroupBy(Cls)
                            .OrderBy(g => g.Key)
                            .Select(g => $"{g.Key}: {g.Count()} sinh viên");
                        foreach (var line in stats) Console.WriteLine(line);
                        break;
                    }

                case "10":
                    Show(All().OrderByDescending(Gpa));
                    break;

                case "11":
                    Show(All().Where(s => Age(s) >= 20 && Age(s) <= 21));
                    break;

                case "12":
                    Show(All().Where(s => (string)s.Element("Major") == "Cong nghe thong tin" && Gpa(s) >= 8.0));
                    break;

                case "13":
                    {
                        Console.Write("Nhập mã sinh viên: ");
                        string id = Console.ReadLine()?.Trim();
                        var found = All().Where(s => string.Equals(Id(s), id, StringComparison.OrdinalIgnoreCase));
                        if (!found.Any()) Console.WriteLine("Không tìm thấy sinh viên.");
                        else Show(found);
                        break;
                    }

                case "14":
                    {
                        Console.Write("Nhập tên lớp: ");
                        string cls = Console.ReadLine()?.Trim();
                        var list = All()
                            .Where(s => string.Equals(Cls(s), cls, StringComparison.OrdinalIgnoreCase))
                            .OrderByDescending(Gpa);
                        Show(list);
                        break;
                    }

                case "15":
                    Show(All().OrderByDescending(Gpa).Take(3));
                    break;

                default:
                    Console.WriteLine("Lựa chọn không hợp lệ.");
                    break;
            }
        }

        // ---------- Hiển thị ----------
        static void Show(IEnumerable<XElement> students)
        {
            var list = students.ToList();
            if (list.Count == 0)
            {
                Console.WriteLine("Không có sinh viên nào.");
                return;
            }

            Console.WriteLine("{0,-7}{1,-18}{2,-9}{3,-6}{4,-8}{5,-22}{6,-5}",
                "Mã SV", "Họ tên", "Giới tính", "Tuổi", "Lớp", "Chuyên ngành", "GPA");
            Console.WriteLine(new string('-', 75));
            foreach (var s in list)
            {
                Console.WriteLine("{0,-7}{1,-18}{2,-9}{3,-6}{4,-8}{5,-22}{6,-5}",
                    Id(s),
                    (string)s.Element("Name"),
                    (string)s.Element("Gender"),
                    Age(s),
                    Cls(s),
                    (string)s.Element("Major"),
                    Gpa(s).ToString("0.0"));
            }
        }
    }
}
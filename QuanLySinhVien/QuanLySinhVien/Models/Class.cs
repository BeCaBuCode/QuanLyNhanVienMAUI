namespace QuanLySinhVien;
using System.ComponentModel.DataAnnotations;

public class Class
{
    [Key]
    public int ClassId { get; set; }
    public string? MaSo {get; set; }
    public string? Name { get; set; }

}

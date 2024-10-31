namespace QuanLySinhVien;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Student
{
    [Key]
    public int Id { get; set; }
    public string? MSSV {get; set; }
    public string? Name { get; set; }
    public DateTime BirthDate { get; set; } 

    [ForeignKey("Class")]
    public int ClassId { get; set; }
}

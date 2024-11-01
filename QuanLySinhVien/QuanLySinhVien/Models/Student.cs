namespace QuanLySinhVien;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class Student : ObservableObject
{
    [Key]
    public int Id {get; set;}
    public string? MSSV {get; set; }

    public string? Name {get; set; }

    public DateTime Birthday {get; set; }

    [ForeignKey("Class")]
    public int ClassId { get; set; }
}

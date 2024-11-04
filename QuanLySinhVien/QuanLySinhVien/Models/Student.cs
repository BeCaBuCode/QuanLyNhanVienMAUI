namespace QuanLySinhVien;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class Student : ObservableObject
{
    [Key]
    [ObservableProperty]
    private int id;
    [ObservableProperty]
    private string mSSV="";
    [ObservableProperty]
    private string name="";
    [ObservableProperty]
    private DateTime birthday;

    [ForeignKey("Class")]
    public int ClassId { get; set; }
}

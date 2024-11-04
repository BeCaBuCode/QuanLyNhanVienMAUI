namespace QuanLySinhVien;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class Class:ObservableObject
{
    [Key]
    [ObservableProperty]
    private int classId ;
    [ObservableProperty]
    private  string classCode = "";
    [ObservableProperty]
    private string name="";
}

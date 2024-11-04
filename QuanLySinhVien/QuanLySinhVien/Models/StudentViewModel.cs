using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace QuanLySinhVien;

public partial class StudentViewModel:ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Student> students = [];
    //new ObservableCollection<Student>()
}

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace QuanLySinhVien;

public partial class ClassViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ClassItem> classItems=new ObservableCollection<ClassItem>();
}

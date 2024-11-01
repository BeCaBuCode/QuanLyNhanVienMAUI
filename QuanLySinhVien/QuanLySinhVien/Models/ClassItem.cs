using CommunityToolkit.Mvvm.ComponentModel;

namespace QuanLySinhVien;

public partial class ClassItem:ObservableObject
{
    [ObservableProperty]
    private Class classvm;

    [ObservableProperty]
    private bool isSelected;
     public ClassItem(Class classvm, bool isSelected)
    {
        this.classvm = classvm;
        this.isSelected = isSelected;
    }
}

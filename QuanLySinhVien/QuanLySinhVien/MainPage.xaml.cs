using System.Collections.ObjectModel;

namespace QuanLySinhVien;

public partial class MainPage : ContentPage
{
	private ClassViewModel classViewModel;
	public required string dbPath;
	public required AppDbContext dbContext;
	public MainPage()
	{
		InitializeComponent();
		classViewModel=new ClassViewModel();
		BindingContext=classViewModel;
		hide();
	}
	private async void OnConfirmDbPathClicked(object sender, EventArgs e)
	{
        try
        {
            dbPath = en_DbPathEntry.Text;
            if (string.IsNullOrWhiteSpace(dbPath))
                {
                    await DisplayAlert("Lỗi", "Vui lòng nhập đường dẫn hợp lệ.", "OK");
                    return;
                }
            dbContext = new AppDbContext(dbPath);
            dbContext.Database.EnsureCreated();
			LoadClasses();
        	show();
        }
        catch (Exception ex)
        {   
            dbPath = Path.Combine(FileSystem.AppDataDirectory, "mydb.db");
            await DisplayAlert("Lỗi", $"Có lỗi xảy ra: {ex.Message}, thay thế đường dẫn bằng {dbPath}", "OK");
            dbContext = new AppDbContext(dbPath);
        	dbContext.Database.EnsureCreated();
        	LoadClasses();
        	show();
        }
    }
	private void LoadClasses()
    {
		var classList = dbContext?.Classes.ToList();
    	foreach (var classItem in classList ?? Enumerable.Empty<Class>())
    	{
        	classViewModel.ClassItems.Add(new ClassItem (classItem,false));
    	}
    }
	private async void OnAddClassClicked(object sender, EventArgs e)
    {
		string classCode = await DisplayPromptAsync("Thêm Lớp", "Nhập mã lớp:");
		string className = await DisplayPromptAsync("Thêm Lớp", "Nhập tên lớp:");

        if (!string.IsNullOrWhiteSpace(className) && !string.IsNullOrWhiteSpace(classCode))
        {
            var newClass = new Class { Name = className , ClassCode = classCode };
            dbContext.Classes.Add(newClass);
            dbContext.SaveChanges();
			classViewModel.ClassItems.Add(new ClassItem (newClass,false));
        }
		else
		{
			await DisplayAlert("Thông tin không hợp lệ","Vui lòng điền đầy đủ thông tin","OK");
		}
    }
	private async void OnEditClassClicked(object sender, EventArgs e)
    {
        int count = classViewModel.ClassItems.Count(s=>s.IsSelected);
		if (count > 1)
        {
            await DisplayAlert("Lỗi","Vui lòng chỉ chọn 1 lớp","OK");
            return;
        }
        Class selectedClass=new Class();
		int index=-1;
		for (int i=classViewModel.ClassItems.Count-1;i>=0;i--)
		{
			if (classViewModel.ClassItems[i].IsSelected)
			{
				index=i;
				selectedClass=classViewModel.ClassItems[i].Classvm;
				break;
			}
		}
    	if (index!=-1)
    	{
			string newClassCode = await DisplayPromptAsync("Sửa Lớp", "Nhập mã lớp mới:", initialValue: selectedClass.ClassCode);
        	string newClassName = await DisplayPromptAsync("Sửa Lớp", "Nhập tên lớp mới:", initialValue: selectedClass.Name);
        	if (!string.IsNullOrWhiteSpace(newClassName) && !string.IsNullOrWhiteSpace(newClassCode))
        	{
				classViewModel.ClassItems[index].Classvm.ClassCode=newClassCode;
				classViewModel.ClassItems[index].Classvm.Name=newClassName;
            	dbContext.Classes.Update(selectedClass);
            	dbContext.SaveChanges();
        	}
			else
			{
				await DisplayAlert("Thông tin không hợp lệ","Vui lòng điền đầy đủ thông tin","OK");
			}
			classViewModel.ClassItems[index].IsSelected=false;
    	}
    	else
    	{
        	await DisplayAlert("Lỗi", "Vui lòng chọn lớp để sửa.", "OK");
    	}

    }
	private async void OnDeleteClassClicked(object sender, EventArgs e)
    {
		bool confirm = await DisplayAlert("Xóa Học Sinh", $"Bạn có chắc chắn muốn xóa các lớp đã được chọn?", "Yes", "No");
		if (confirm)
		{
			for (int i=classViewModel.ClassItems.Count-1;i>=0;i--)
			{
				if (classViewModel.ClassItems[i].IsSelected)
				{
					var studentsToDelete = dbContext?.Students.Where(s => s.ClassId == classViewModel.ClassItems[i].Classvm.ClassId).ToList();
        			foreach (var student in studentsToDelete ?? Enumerable.Empty<Student>())
        			{
        	    		dbContext?.Students.Remove(student);
        			}
        			dbContext?.Classes.Remove(classViewModel.ClassItems[i].Classvm);
					dbContext?.SaveChanges();
					classViewModel.ClassItems.RemoveAt(i);
				}
			}
		}
    }
	private async void OnClassSelected(object sender, ItemTappedEventArgs e)
    {
      	if (e.Item == null) return;
	   	Class? selectedClass = (e.Item as ClassItem)?.Classvm;
    	if (selectedClass!=null && !string.IsNullOrEmpty(dbPath))
    	await Navigation.PushAsync(new StudentPage(selectedClass.ClassId, dbPath));
    }
	private void show()
	{
		lb_Path.IsVisible=false;
        en_DbPathEntry.IsEnabled=false;
        en_DbPathEntry.IsVisible=false;
        btn_Confirm.IsVisible=false;
        lb_ClassList.IsVisible=true;
        gr_lb_name.IsVisible=true;
        btn_add.IsVisible=true;
        btn_del.IsVisible=true;
        btn_fix.IsVisible=true;
	}
	private void hide(){
		lb_ClassList.IsVisible=false;
        gr_lb_name.IsVisible=false;
        btn_add.IsVisible=false;
        btn_del.IsVisible=false;
        btn_fix.IsVisible=false;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
		ClassListView.SelectedItem=null;
		classViewModel.ClassItems.ToList().ForEach(classItem => classItem.IsSelected = false);
    }
}


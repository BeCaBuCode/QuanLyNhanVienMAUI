namespace QuanLySinhVien;
public partial class StudentPage : ContentPage
{
    private StudentViewModel studentViewModel;
    private Student? _selectedStudent;
    private readonly AppDbContext dbContext;
    private readonly int _classId;
    public StudentPage(int classId, string dbPath)
    {
        InitializeComponent();
        _classId = classId;
        studentViewModel=new();
        BindingContext=studentViewModel;
        dbContext = new AppDbContext(dbPath);
        LoadStudents();
    }
    private void LoadStudents()
    {
        var students = dbContext.Students.Where(s => s.ClassId == _classId).ToList();
        foreach (var student in students)
        {
            studentViewModel.Students.Add(student);
        }
    }
    private async void OnAddStudentClicked(object sender, EventArgs e)
    {
        string mssv= await DisplayPromptAsync("Thêm Học Sinh", "Nhập MSSV:");
        string fullName = await DisplayPromptAsync("Thêm Học Sinh", "Nhập họ tên:");
        string birthDateString = await DisplayPromptAsync("Thêm Học Sinh", "Nhập ngày sinh (yyyy-MM-dd):");

        if (DateTime.TryParse(birthDateString, out DateTime birthDate) && !string.IsNullOrWhiteSpace(fullName) && !string.IsNullOrWhiteSpace(mssv))
        {
            var newStudent = new Student {MSSV=mssv, Name = fullName, Birthday = birthDate, ClassId = _classId };
            dbContext.Students.Add(newStudent);
            dbContext.SaveChanges();
            studentViewModel.Students.Add(newStudent);
        }
    }

    private async void OnEditStudentClicked(object sender, EventArgs e)
    {
        if (_selectedStudent == null)
        {
            await DisplayAlert("Lỗi", "Vui lòng chọn học sinh để sửa.", "OK");
            return;
        }
        int index = studentViewModel.Students.IndexOf(_selectedStudent);
        string mssv = await DisplayPromptAsync("Sửa Học Sinh", "Nhập MSSV mới:", initialValue: _selectedStudent.MSSV);
        string fullName = await DisplayPromptAsync("Sửa Học Sinh", "Nhập họ tên mới:", initialValue: _selectedStudent.Name);
        string birthDateString = await DisplayPromptAsync("Sửa Học Sinh", "Nhập ngày sinh mới (yyyy-MM-dd):", initialValue: _selectedStudent.Birthday.ToString("yyyy-MM-dd"));

        if (DateTime.TryParse(birthDateString, out DateTime birthDate) && !string.IsNullOrWhiteSpace(fullName) && !string.IsNullOrWhiteSpace(mssv))
        {
            studentViewModel.Students[index].Name=fullName;
            studentViewModel.Students[index].Birthday = birthDate;
            studentViewModel.Students[index].MSSV=mssv;
            dbContext.Students.Update(_selectedStudent);
            dbContext.SaveChanges();
        }
    }

    private async void OnDeleteStudentClicked(object sender, EventArgs e)
    {
        if (_selectedStudent == null)
        {
            await DisplayAlert("Lỗi", "Vui lòng chọn học sinh để xóa.", "OK");
            return;
        }
        bool confirm = await DisplayAlert("Xóa Học Sinh", $"Bạn có chắc chắn muốn xóa học sinh {_selectedStudent.Name}?", "Yes", "No");
        if (confirm)
        {
            studentViewModel.Students.Remove(_selectedStudent);
            dbContext.Students.Remove(_selectedStudent);
            dbContext.SaveChanges();
        }
    }

    private void OnStudentSelected(object sender, ItemTappedEventArgs e)
    {
        _selectedStudent = e.Item as Student;
    }
}
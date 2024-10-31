using Microsoft.EntityFrameworkCore;
namespace QuanLySinhVien;

public class AppDbContext : DbContext
{
    private readonly string _dbPath;
    public AppDbContext(string dbPath)
    {
        _dbPath = dbPath;
    }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Student> Students { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_dbPath}");
    }
}

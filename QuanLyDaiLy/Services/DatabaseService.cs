using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using QuanLyDaiLy.Repositories;
using System.IO;

namespace QuanLyDaiLy.Services;

public class DatabaseService
{
    private SqliteConnection? sqliteConnection;
    private DataContext? dataContext;

    public DataContext DataContext => dataContext ?? throw new ArgumentNullException("Database not initialized!");

    public static string GetDefaultDatabasePath()
    {
        string appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "QuanLyDaiLy");

        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath);
        }

        return Path.Combine(appDataPath, "quanlydaily123.db");
    }

    public async Task Initialize(string? dbPath = null)
    {
        try
        {
            dbPath ??= GetDefaultDatabasePath();

            sqliteConnection = new SqliteConnection($"Data Source={dbPath}");
            await sqliteConnection.OpenAsync();

            var dbOptions = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(sqliteConnection)
                .Options;

            dataContext = new DataContext(dbOptions);
            await dataContext.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            // Log or handle the exception as needed
            throw new Exception($"Database initialization failed: {ex.Message}", ex);
        }
    }

    public void CloseConnection()
    {
        if (sqliteConnection != null)
        {
            sqliteConnection.Close();
            sqliteConnection = null;
        }
    }
}

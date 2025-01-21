using SQLite;

namespace Librarian.Constants;

public class DatabaseConstants
{
    private const string DatabaseFilename = "CompressorSQLite.db3";

    public const SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLiteOpenFlags.SharedCache;
    
    public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
}
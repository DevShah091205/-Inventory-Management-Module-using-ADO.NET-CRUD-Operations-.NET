using Microsoft.Data.SqlClient;

namespace InventoryManagement.Data;

/// <summary>
/// PART 3 : CONNECTION OF THE DATABASE
/// Reads the connection string from appsettings.json and hands out
/// SqlConnection objects (ADO.NET) to the repository.
/// </summary>
public class DbConnection
{
    private readonly string _connectionString;

    public DbConnection(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing in appsettings.json.");
    }

    /// <summary>Creates a new (closed) connection. Caller opens and disposes it with "using".</summary>
    public SqlConnection CreateConnection() => new SqlConnection(_connectionString);

    /// <summary>Quick check used to verify the database can be reached.</summary>
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            await using var con = CreateConnection();
            await con.OpenAsync();
            return true;
        }
        catch (SqlException)
        {
            return false;
        }
    }
}

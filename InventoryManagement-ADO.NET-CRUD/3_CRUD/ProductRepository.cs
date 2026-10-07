using InventoryManagement.Models;
using Microsoft.Data.SqlClient;

namespace InventoryManagement.Data;

/// <summary>
/// PART 4 : CRUD OPERATIONS using ADO.NET
/// SqlConnection + SqlCommand + SqlDataReader with parameterised queries.
/// </summary>
public class ProductRepository
{
    private readonly DbConnection _db;

    public ProductRepository(DbConnection db) => _db = db;

    // ------------------------------------------------------------ READ (all + search)
    public async Task<List<Product>> GetAllAsync(string? search = null)
    {
        var products = new List<Product>();
        const string sql = @"
            SELECT ProductId, ProductName, Category, Price, Quantity, Supplier
            FROM   dbo.Products
            WHERE  (@Search = N'' OR ProductName LIKE N'%' + @Search + N'%'
                                  OR Category    LIKE N'%' + @Search + N'%'
                                  OR Supplier    LIKE N'%' + @Search + N'%')
            ORDER  BY ProductId DESC;";

        await using var con = _db.CreateConnection();
        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@Search", search ?? string.Empty);

        await con.OpenAsync();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            products.Add(Map(reader));

        return products;
    }

    // ------------------------------------------------------------ READ (one)
    public async Task<Product?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT ProductId, ProductName, Category, Price, Quantity, Supplier
            FROM   dbo.Products
            WHERE  ProductId = @ProductId;";

        await using var con = _db.CreateConnection();
        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@ProductId", id);

        await con.OpenAsync();
        await using var reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync() ? Map(reader) : null;
    }

    // ------------------------------------------------------------ CREATE
    public async Task<int> CreateAsync(Product product)
    {
        const string sql = @"
            INSERT INTO dbo.Products (ProductName, Category, Price, Quantity, Supplier)
            OUTPUT INSERTED.ProductId
            VALUES (@ProductName, @Category, @Price, @Quantity, @Supplier);";

        await using var con = _db.CreateConnection();
        await using var cmd = new SqlCommand(sql, con);
        AddParameters(cmd, product);

        await con.OpenAsync();
        return Convert.ToInt32(await cmd.ExecuteScalarAsync());   // returns the new ProductId
    }

    // ------------------------------------------------------------ UPDATE
    public async Task<bool> UpdateAsync(Product product)
    {
        const string sql = @"
            UPDATE dbo.Products
            SET    ProductName = @ProductName,
                   Category    = @Category,
                   Price       = @Price,
                   Quantity    = @Quantity,
                   Supplier    = @Supplier
            WHERE  ProductId   = @ProductId;";

        await using var con = _db.CreateConnection();
        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
        AddParameters(cmd, product);

        await con.OpenAsync();
        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    // ------------------------------------------------------------ DELETE
    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM dbo.Products WHERE ProductId = @ProductId;";

        await using var con = _db.CreateConnection();
        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@ProductId", id);

        await con.OpenAsync();
        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    // ------------------------------------------------------------ helpers
    private static void AddParameters(SqlCommand cmd, Product p)
    {
        cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
        cmd.Parameters.AddWithValue("@Category", p.Category);
        cmd.Parameters.AddWithValue("@Price", p.Price);
        cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
        cmd.Parameters.AddWithValue("@Supplier", p.Supplier);
    }

    private static Product Map(SqlDataReader r) => new()
    {
        ProductId   = r.GetInt32(0),
        ProductName = r.GetString(1),
        Category    = r.GetString(2),
        Price       = r.GetDecimal(3),
        Quantity    = r.GetInt32(4),
        Supplier    = r.GetString(5)
    };
}

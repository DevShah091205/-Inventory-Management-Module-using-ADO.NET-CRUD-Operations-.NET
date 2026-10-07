using InventoryManagement.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Part 3 - database connection, Part 4 - CRUD repository
builder.Services.AddSingleton<DbConnection>();
builder.Services.AddScoped<ProductRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Product/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();

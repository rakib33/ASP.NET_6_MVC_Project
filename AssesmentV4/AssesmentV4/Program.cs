

using AssesmentV4.Data;
using AssesmentV4.Interfaces;
using AssesmentV4.Repositories;
using AssesmentV4.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// You can now directly access your configuration.
IConfiguration configuration = builder.Configuration;

//Configure the database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration["ConnectionStrings:DefaultConnection"]));

#region ExcelDataProcessing

var excelFilePath = Path.Combine(@"", configuration["ExcelDataSource"]);
string jsonFilePath = Path.Combine(@"", configuration["OuputJsonPath"]);


var excelParser = new ExcelParser();
var products = excelParser.ParseExcel(excelFilePath);

// Serialize to JSON and save to file (if the file does not exist)
var jsonSerializer = new JsonSerializer();
jsonSerializer.SerializeToFile(products, jsonFilePath);

#endregion


// Add services to the container.
builder.Services.AddControllersWithViews();


// Register repositories
builder.Services.AddScoped<IProductRepository>(provider =>
{
    var context = provider.GetRequiredService<ApplicationDbContext>();
    return new PostgresProductRepository(context);
});

// JSON repository requires a file path from configuration
builder.Services.AddSingleton<IProductRepository>(provider =>
{
    // var jsonFilePath = builder.Configuration["JsonData:FilePath"];
    return new JsonProductRepository(jsonFilePath);
});

var app = builder.Build();

// Populate the database (if the table or data does not exist)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var databasePopulator = new DatabasePopulator(dbContext);
    databasePopulator.PopulateDatabase(products);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();

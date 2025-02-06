# ASP.NET_6_MVC_Project

## Task A
1. Create an app that generates a JSON file from Excel
2. Parse a Excel file
3. Convert the Excel data into C# object
4. Serialize the data into JSON format.
5. Save the JSON data to a file.
6. Database Integration with PostgreSQL.

## Task B
7. Implement a controller action that retrieves data from the JSON file or PostgreSQL database.
8. Create a page to load content using JSON or PostgreSQL.
9. Provide an option to choose between loading data from the JSON file or the PostgreSQL database.

### Filtering and Searching:
10. Add functionality to filter content based on a date range.
11.Implement a textbox search to filter content based on user input.

## Task C

12. HTML Table Customization:
13 Create an HTML table with columns corresponding to the data fields.
14 Implement sorting functionality for each column (A-Z, Z-A).
15 Allow users to customize which columns are displayed in the table.

# Front-end Tasks

1. create a new MVC C# .NET 6 project with a basic structure.
2. Pass the created json file and display data in a page.
3. Page design looks like
   
   ![image](https://github.com/user-attachments/assets/c880bdd6-68b4-4bcc-90ab-27d6f1de658a)

   ![image](https://github.com/user-attachments/assets/afd7e3b5-cd39-4a29-b369-d71aa01ce7a7)
5. Use jQuery to fetch and parse the JSON data from data.json. Data in the design that are not part of the JSON can be static and hardcoded.
6. Generate a mobile-responsive card grid layout based on the data for all screen.
7. No functionality is required to be build. All buttons and forms can be static.

## Create an app that generates a JSON file from Excel

1. Create a solution in visual studio 2022 named AssesmentV4 or you can provide any Name.
2. Create a ASP.NET Core MVC project under this solution with same name or different name.

   ![image](https://github.com/user-attachments/assets/19c840b5-f99e-4fed-839b-c88d4bbd7191)

3. Select .Net 6 framework and create this project.
   
   ![image](https://github.com/user-attachments/assets/71a2b625-4d4f-4903-aa9c-75654cbdd7f1)

4. For database integration install postreSQL from [here](https://www.postgresql.org/download/windows/)
5. Install pgAdmin also.
6. Check connection string through pgAdmin.
   
   ![image](https://github.com/user-attachments/assets/30c7a4bc-6532-4799-966e-cecc05658b57)

7. Configure appsettings.json. Give Excel file path and json file path and PostreSQL connection string.
   
   ```
   
   "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=ProductDB;User Id=postgres;Password=1234"
    },
   "ExcelDataSource": "E:\\TechnicalAssesment\\vodus-test-excel.xlsx",
   "OuputJsonPath": "E:\\TechnicalAssesment\\Order.json",
    ```
 8. Ensure you have the following NuGet packages installed:

      - Microsoft.EntityFrameworkCore
      - Microsoft.EntityFrameworkCore.Tools
      - Npgsql.EntityFrameworkCore.PostgreSQL (for PostgreSQL support)
      - EPPlus (for Excel file parsing)
      - Newtonsoft.Json (for JSON serialization)
 
 9. Create Services folder and create below classes under this folder.
     
    - JsonSerializer.cs
      
    ```
     public class JsonSerializer
     {
     public void SerializeToFile(List<Product> products, string filePath)
     {
         if (!File.Exists(filePath))
         {
             string json = JsonConvert.SerializeObject(products, Formatting.Indented);
             File.WriteAllText(filePath, json);
         }
     }
    }
    ```

    - ExcelParser.cs

    ```
    public class ExcelParser
    {
        public List<Product> ParseExcel(string filePath)
        {
            var products = new List<Product>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                if (package.Workbook.Worksheets.Count > 0)
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
   
                    for (int row = 2; row <= rowCount; row++) // Start from row 2 to skip header
                    {
                        var product = new Product
                        {
                            Id = int.Parse(worksheet.Cells[row, 1].Text),
                            Image = worksheet.Cells[row, 2].Text,
                            Name = worksheet.Cells[row, 3].Text,
   
                            // Parse as DateTime with Kind=Unspecified
                           OrderDate = DateTime.SpecifyKind(
                                DateTime.Parse(worksheet.Cells[row, 4].Text),
                                DateTimeKind.Unspecified
                            ),
                            Price = worksheet.Cells[row, 5].Text,
                            DiscountedPrice = worksheet.Cells[row, 6].Text
                        };
   
                        products.Add(product);
                    }
                }
   
            }
   
            return products;
        }
    }
    
    ```    

    - DatabasePopulator.cs

    ```    
      public class DatabasePopulator
      {
          private readonly ApplicationDbContext _context;
      
          public DatabasePopulator(ApplicationDbContext context)
          {
              _context = context;
          }
      
          public void PopulateDatabase(List<Product> products)
          {
              if (!_context.Products.Any())
              {
                  _context.Products.AddRange(products);
                  _context.SaveChanges();
              }
          }
      }    
    ```
   10. Create a C# class Product according Excel data.

        ```
        public class Product
        {
            [Key]
            public int Id { get; set; }
            public string Image { get; set; }
            public string Name { get; set; }
            public DateTime OrderDate { get; set; }
            public string Price { get; set; }
            public string DiscountedPrice { get; set; }
        }
        ```

11. Create a folder named Data and create a Database Context class ApplicationDbContext for PostgreSQL.

      ```
      public class ApplicationDbContext : DbContext
      {
          public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
      
          public DbSet<Product> Products { get; set; }
      
          protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
              modelBuilder.Entity<Product>(entity =>
              {
              entity.ToTable("Products");
              entity.Property(p => p.OrderDate)
              .HasColumnType("timestamp without time zone");
              });
          }
      
      }
      ```
 12. This application will parse the Excel data and then convert and saved as json data file also PostreSQL database.

     - Saved Excel data as json file when application run. Open Pragram.cs file , copy and past this section of code.

     ```
      // You can now directly access your configuration.
      IConfiguration configuration = builder.Configuration;
      #region ExcelDataProcessing
      
      var excelFilePath = Path.Combine(@"", configuration["ExcelDataSource"]);
      string jsonFilePath = Path.Combine(@"", configuration["OuputJsonPath"]);
      
      var excelParser = new ExcelParser();
      var products = excelParser.ParseExcel(excelFilePath);
      
      // Serialize to JSON and save to file (if the file does not exist)
      var jsonSerializer = new JsonSerializer();
      jsonSerializer.SerializeToFile(products, jsonFilePath);
      
      #endregion
     ```
     - Inject database connection string in Program.cs file and also add the Excel data into Database

     ```
      //Configure the database context
      builder.Services.AddDbContext<ApplicationDbContext>(options =>
          options.UseNpgsql(configuration["ConnectionStrings:DefaultConnection"]));
     ```
     - Insert Excel data if data not exists when application run , add this code below builder.Build()

     ```
       var app = builder.Build();
      // Populate the database (if the table or data does not exist)
      using (var scope = app.Services.CreateScope())
      {
          var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
          var databasePopulator = new DatabasePopulator(dbContext);
          databasePopulator.PopulateDatabase(products);
      }
     ```
  13. Create Code First Migration to create Database and Table on PostgreSQL

      - Open package manager console
        ![image](https://github.com/user-attachments/assets/036e6225-3ef8-4e7c-9c19-82546dec880d)

      - Run command **Add-Migration InitailCommit** and **Update-Database**
        
      - Open pgAdmin and check database and table created.
        
        ![image](https://github.com/user-attachments/assets/38de993a-3422-4cf0-867e-88fd2c2f145f)

 14. Now run the application. It will create the json file with Excel data and also insert the data into database table. 

 ## Task B
 
 1. Implement a controller action that retrieves data from the JSON file or PostgreSQL database.
    
    - Before creating controller we use repository pattern for that. We have a Product model and two option
      for featching data. One for fetching data from created json file and another is from database.
      We will create a Interface **IProductRepository.cs** under Interfaces folder.

      ```
      public interface IProductRepository
      {
      
          /// <summary>
          /// get all products
          /// </summary>
          /// <returns></returns>
            IQueryable<Product> GetAllProducts();
          /// <summary>
          /// Retrieve all products with search criteria
          /// </summary>
          IQueryable<Product> GetProducts(ProductSearchCriteria criteria);
      }
      ```
      
     - Create a Repository for fetching data from Json under Repositories folder.

       ```
         /// <summary>
         /// Repository to retrive data from Json file
         /// </summary>
         public class JsonProductRepository : IProductRepository
         {
             //path to the json file
             private readonly string _jsonPath;
         
             public JsonProductRepository(string jsonPath)
             {
                 _jsonPath = jsonPath;
             }
         
             /// <summary>
             /// Get all products
             /// </summary>
             /// <returns></returns>
             /// <exception cref="FileNotFoundException"></exception>
             public IQueryable<Product> GetAllProducts()
             {
         
                 if (!File.Exists(_jsonPath))
                     throw new FileNotFoundException("JSON file not found.");
         
                 string json = File.ReadAllText(_jsonPath);
                 var products = JsonConvert.DeserializeObject<List<Product>>(json);
                 return products.AsQueryable();
             }
         
             /// <summary>
             /// Retrieve all products with search criteria
             /// </summary>
             /// <param name="criteria"></param>
             /// <returns></returns>
             public IQueryable<Product> GetProducts(ProductSearchCriteria criteria)
             {
                 var query = GetAllProducts();
         
                 if (!string.IsNullOrEmpty(criteria.ProductName))
                     query = query.Where(p => p.Name.ToLower().Contains(criteria.ProductName.ToLower()));
         
                 if (criteria.StartDate.HasValue)
                     query = query.Where(p => p.OrderDate >= criteria.StartDate.Value);
         
                 if (criteria.EndDate.HasValue)
                     query = query.Where(p => p.OrderDate <= criteria.EndDate.Value);
         
                 return query;
             }
         
         }
       ```

      - Create a Repository for fetching data from PostreSQL under Repositories folder.

        ```
          /// <summary>
          /// Repository to retrive data from database
          /// </summary>
          public class PostgresProductRepository : IProductRepository
          {
              private readonly ApplicationDbContext _dbContext;
         
              public PostgresProductRepository(ApplicationDbContext dbContext)
              {
                  _dbContext = dbContext;
              }
         
              /// <summary>
              /// Get all products
              /// </summary>
              /// <returns></returns>
              /// <exception cref="FileNotFoundException"></exception>
              public IQueryable<Product> GetAllProducts()
              {
                  return _dbContext.Products;
              }
         
         
              /// <summary>
              /// Retrieve all products with search criteria
              /// </summary>
              /// <param name="criteria"></param>
              /// <returns></returns>
              public IQueryable<Product> GetProducts(ProductSearchCriteria criteria)
              {
                  var query = GetAllProducts();
         
                  if (!string.IsNullOrEmpty(criteria.ProductName))
                       query = query.Where(p => p.Name.ToLower().Contains(criteria.ProductName.ToLower()));
         
         
                  if (criteria.StartDate.HasValue)
                      query = query.Where(p => p.OrderDate >= criteria.StartDate.Value);
         
                  if (criteria.EndDate.HasValue)
                      query = query.Where(p => p.OrderDate <= criteria.EndDate.Value);
         
                  return query;
              }
         
         
          }
        ```

     - Now we need to Inject this repository on program.cs

       ```
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
       ```
     - Now we can create a controller named ProductController.cs under Controlers folder.

       ```
       /// <summary>
       /// Product controller to retrive data for view
       /// </summary>
       public class ProductController : Controller
       {
           private IProductRepository _productRepository;
           private readonly IServiceProvider _serviceProvider;
           private  IQueryable<Product> products;
           public ProductController(IServiceProvider serviceProvider)
           {
              _serviceProvider = serviceProvider;
           }
      
           /// <summary>
           /// determine which repository to use based on the source
           /// </summary>
           /// <param name="source"></param>
           private void SetProductRepository(string source)
           {
               if (source?.ToLower() == "json")
               {
                   _productRepository = _serviceProvider.GetServices<IProductRepository>().OfType<JsonProductRepository>().First();
               }
               else
               {
                   _productRepository = _serviceProvider.GetServices<IProductRepository>().OfType<PostgresProductRepository>().First();
               }
           }
           /// <summary>
           /// Retrieve products from either JSON or the database with optional search criteria.
           /// </summary>
           /// <param name="source">Data source ("json" or "database")</param>
           /// <param name="criteria">Search criteria</param>
           public IActionResult Index(string source, ProductSearchCriteria criteria)
           {
               SetProductRepository(source);
               products = _productRepository.GetProducts(criteria);
               // Maintain search parameters in the view
               ViewBag.CurrentSource = source;
               ViewBag.CurrentCriteria = criteria;
      
               return View(products.ToList());
           }
      
           /// <summary>
           /// JQuery DataTable view with column sorting and search and filtering
           /// </summary>
           /// <returns></returns>
           public IActionResult DataTableView()
           {
               _productRepository = _serviceProvider.GetServices<IProductRepository>().OfType<JsonProductRepository>().First();
               products = _productRepository.GetAllProducts();
               return View(products.ToList());
           }
      
           [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
           public IActionResult Error()
           {
               return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
           }
       }
       ``` 
   - Change the conventional routing in program.cs file. Add Product controller as startup.

     ```
      app.MapControllerRoute(
          name: "default",
          pattern: "{controller=Product}/{action=Index}/{id?}");
     ```
2. Create a page to load content using JSON or PostgreSQL.
   - Provide an option to choose between loading data from the JSON file or the PostgreSQL database.
   - Add functionality to filter content based on a date range.
   - Implement a textbox search to filter content based on user input.
   - Create a view name Index under Views folder. The code with all facilities attached here.

     ```
      @{
          ViewData["Title"] = "Product Page";
      }
      @model List<Product>      
      
      <div class="card shadow mb-4">
          <!-- Card Header - Search Criteria -->
          <div class="card-header py-3">
              <h6 class="m-0 font-weight-bold text-primary">Products</h6>
              <hr>
              <form asp-action="Index" method="get" class="form-inline">
                  <div class="row align-items-center">
                      <div class="col-md-2">
                          <div class="form-group">
                              <label>Data Source:</label>
                              <select name="source" class="form-control">
                                  <option value="database" selected=@(ViewBag.CurrentSource == "database")>Database</option>
                                  <option value="json" selected=@(ViewBag.CurrentSource == "json")>JSON</option>
                              </select>
                          </div>
                      </div>
      
                      <div class="col-md-2">
                          <div class="form-group">
                              <label>Product Name:</label>
                              <input type="text" name="criteria.ProductName"
                                     class="form-control" placeholder="Search by name..."
                                     value="@ViewBag.CurrentCriteria?.ProductName">
                          </div>
                      </div>
      
                      <div class="col-md-2">
                          <div class="form-group">
                              <label>From Date:</label>
                              <input type="date" name="criteria.StartDate"
                                     class="form-control"
                                     value="@(ViewBag.CurrentCriteria?.StartDate?.ToString("yyyy-MM-dd"))">
                          </div>
                      </div>
      
                      <div class="col-md-2">
                          <div class="form-group">
                              <label>To Date:</label>
                              <input type="date" name="criteria.EndDate"
                                     class="form-control"
                                     value="@(ViewBag.CurrentCriteria?.EndDate?.ToString("yyyy-MM-dd"))">
                          </div>
                      </div>
                      <div class="col-md-2 mt-4">
                          <button type="submit" class="btn btn-primary">
                              <i class="fas fa-search"></i> Search
                          </button>
                          <a asp-action="Index" class="btn btn-secondary">Reset</a>
                      </div>
      
      
                  </div>
              </form>
          </div>
      
          <!-- Card Body - Results Table -->
          <div class="card-body">
              <div class="table-responsive">
                  <table class="table table-bordered table-striped table-hover" width="100%">
                      <thead class="thead-light">
                          <tr>
                              <th>ID</th>
                              <th>Image</th>
                              <th>Product Name</th>
                              <th>Order Date</th>
                              <th>Price</th>
                              <th>Discounted Price</th>
                          </tr>
                      </thead>
                      <tbody>
                          @foreach (var product in Model)
                          {
                              <tr>
                                  <td>@product.Id</td>
                                  <td class="text-center">
                                      <img src="@product.Image" class="img-thumbnail" style="max-width: 60px;">
                                  </td>
                                  <td>@product.Name</td>
                                  <td>@product.OrderDate.ToString("dd MMM yyyy")</td>
                                  <td class="text-right">@product.Price</td>
                                  <td class="text-right">@product.DiscountedPrice</td>
                              </tr>
                          }
                      </tbody>
                  </table>
              </div>
          </div>
      </div>
      
      <link rel="stylesheet" href="~/css/index.css" asp-append-version="true" />

     ```

 -  Create a css file index.css under wwwroot/css folder, and add this style
   
   ```
     .card-header {
         background-color: #f8f9fc !important;
     }
   
     .input-group-text {
         background-color: #fff;
     }
   
     .table thead th {
         border-bottom: 2px solid #e3e6f0;
     }
   
     .img-thumbnail {
         padding: 0.25rem;
         background-color: #fff;
         border: 1px solid #dddfeb;
         border-radius: 0.35rem;
     }
   ```
## Task C
 - HTML Table Customizatio
 - Implement sorting functionality for each column (A-Z, Z-A).
 - Allow users to customize which columns are displayed in the table.
 - Create another view page **DataTableView** under Views folder and write this code. For above feature we need to use JQuery datatable.

   ```
      @model IEnumerable<Product>
      
      <link href="https://cdn.datatables.net/1.13.6/css/dataTables.bootstrap4.min.css" rel="stylesheet">
      <link href="https://cdn.datatables.net/buttons/2.4.1/css/buttons.bootstrap4.min.css" rel="stylesheet">
      <link rel="stylesheet" href="~/css/index.css" asp-append-version="true" />
      <div class="card shadow mb-4">
          <!-- Card Header - Search Criteria -->
          <div class="card-header py-3">
              <h6 class="m-0 font-weight-bold text-primary">Products</h6>
          </div>
      
          <div class="card-body">
              <div class="table-responsive">
                  <table id="productsTable" class="table table-bordered table-striped  table-hover" width="100%">
                      <thead class="thead-light">
                          <tr>
                              <th>ID</th>
                              <th>Image</th>
                              <th>Product Name</th>
                              <th>Order Date</th>
                              <th>Price</th>
                              <th>Discounted Price</th>
                          </tr>
                      </thead>
                      <tbody>
                          @foreach (var product in Model)
                          {
                              <tr>
                                  <td>@product.Id</td>
                                  <td class="text-center">
                                      <img src="@product.Image" class="img-thumbnail" style="max-width: 60px;">
                                  </td>
                                  <td>@product.Name</td>
                                  <td>@product.OrderDate.ToString("dd MMM yyyy")</td>
                                  <td class="text-right">@product.Price</td>
                                  <td class="text-right">@product.DiscountedPrice</td>
                              </tr>
                          }
                      </tbody>
                  </table>
              </div>
          </div>
      </div>
      @section Scripts {
          <script src="https://code.jquery.com/jquery-3.7.0.js"></script>
          <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
          <script src="https://cdn.datatables.net/1.13.6/js/dataTables.bootstrap4.min.js"></script>
          <script src="https://cdn.datatables.net/buttons/2.4.1/js/dataTables.buttons.min.js"></script>
          <script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.bootstrap4.min.js"></script>
          <script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.colVis.min.js"></script>
      
          <script>
              $(document).ready(function () {
                  var table = $('#productsTable').DataTable({
                      dom: 'Bfrtip',
                      buttons: [
                          {
                              extend: 'colvis',
                              text: 'Column Visibility',
                              className: 'btn btn-primary'
                          }
                      ],
                      paging: true,
                      pageLength: 10,
                      responsive: true,
                      columnDefs: [
                          { orderable: true, targets: '_all' }
                      ]
                  });
              });
          </script>
      }
   ```
## Front-end Tasks
 1. create a simple ASP.NET 6 MVC appllication **JQueryAssessment**
 2. Now Create a Index.cshtml page and add this html code there.

    ```
      <!DOCTYPE html>
      <html lang="en">
      <head>
          <meta charset="UTF-8">
          <meta name="viewport" content="width=device-width, initial-scale=1.0">
          <title>Responsive Tabs</title>
          <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
          <link rel="stylesheet" href="~/css/styles.css" asp-append-version="true" />
      
           <!-- Include Ionicons from CDN -->
        <link href="https://cdnjs.cloudflare.com/ajax/libs/ionicons/7.4.0/collection/components/icon/icon.min.css" rel="stylesheet">
      </head>
      <body>
      
          <div class="container mt-3">
              <div class="row align-items-center">
                  <div class="col d-inline">
                      <h4 class="mb-0 title">MY PURCHASES</h4>
                  </div>
                  <div class="col d-inline">
                      <input type="text" class="form-control search-bar" placeholder="Search Orders to Receives">
                  </div>
              </div>
      
              <ul class="nav nav-tabs d-flex w-100 mt-3" id="myTab" role="tablist">
                  <li class="nav-item flex-fill">
                      <a class="nav-link  text-center" id="to-pay-tab" data-toggle="tab" href="#to-pay" role="tab" aria-controls="to-pay" aria-selected="true">ToPay</a>
                  </li>
                  <li class="nav-item flex-fill">
                      <a class="nav-link active text-center" id="to-receive-tab" data-toggle="tab" href="#to-receive" role="tab" aria-controls="to-receive" aria-selected="false">ToReceive</a>
                  </li>
                  <li class="nav-item flex-fill">
                      <a class="nav-link text-center" id="completed-tab" data-toggle="tab" href="#completed" role="tab" aria-controls="completed" aria-selected="false">Completed</a>
                  </li>
                  <li class="nav-item flex-fill">
                      <a class="nav-link text-center" id="refund-tab" data-toggle="tab" href="#refund" role="tab" aria-controls="refund" aria-selected="false">Refund</a>
                  </li>
              </ul>
              <div class="tab-content" id="myTabContent">
                  <div class="tab-pane fade" id="to-pay" role="tabpanel" aria-labelledby="to-pay-tab">
                      <p>Content for To Pay tab.</p>
                  </div>
                  <div class="tab-pane fade show active" id="to-receive" role="tabpanel" aria-labelledby="to-receive-tab">
      
                      <div id="output">
      
                      </div>
                  </div>
      
      
                  <div class="tab-pane fade" id="completed" role="tabpanel" aria-labelledby="completed-tab">
                      <p>Content for Completed tab.</p>
                  </div>
                  <div class="tab-pane fade" id="refund" role="tabpanel" aria-labelledby="refund-tab">
                      <p>Content for Refund tab.</p>
                  </div>
              </div>
          </div>
      
          <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
          <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
          <script src="https://cdn.jsdelivr.net/npm/dayjs@1.10.7/dayjs.min.js"></script>
        <script>
                $(document).ready(function () {
              $.ajax({
                  url: '/Order.json',  // Path relative to wwwroot
                  dataType: 'json',
                  success: function (data) {
                      var rows = '';
                      $.each(data, function (index, item) {
      
                          // Format the date using Day.js
                         var formattedDate = dayjs(item.OrderDate).format("DD MMM YYYY");
                          var name = item.Name.substring(0, 35) + '...';
                          rows += `
                       <div class="mt-3">
      
                  <div class="row align-items-center">
                      <div class="col d-inline" style="font-weight: 600;">
                           <p class="heading d-inline">Vodus Store  ></p>
                      </div>
                      <div class="col d-inline text-center">
                         <p class="heading d-inline">Order Date: ${formattedDate}</p>
                      </div>
                   </div>
      
      
      
                       <div class="row mt-1">
                      <div class="col-md-2 col-3">
                          <img src="${item.Image}" width="80" height="80" />
                      </div>
                      <div class="col-md-3 col-3 font-size-14">
                          <div class="row">
                              ${name}
                          </div>
                          <div class="row">
                              <select name="source" style="background-color: #eae8e8; border: 0px;color: #999;">
                                  <option value="database" selected style="background-color: #eae8e8; border: 0px;color: #999;">RM5 Discount(50 VPoints)</option>
                              </select>
                          </div>
                          <div class="row">
                              <panel style="color: #999;">Variation: Black XL</panel>
                          </div>
      
                      </div>
                      <div class="col-md-1 col-3 mt-4">
                          <div class="button">x2</div>
                      </div>
                      <div class="col-md-2 col-3 mt-4">
                          <div class="price-panel font-size-14">
                              <span class="original-price">${item.Price}</span>
                              <span class="new-price">${item.DiscountedPrice}</span>
                          </div>
                      </div>
                      <div class="col-md-2 col-sm-3 order-md-last mt-4" style="text-align: right;">
                          <button type="button" class="status-button">Pending Seller ></button>
                      </div>
                  </div>
                  </div>`;
      
                      });
                      $('#output').html(rows);
                  },
                  error: function (xhr, status, error) {
                      console.error("Error loading JSON:", error);
                  }
              });
          });
        </script>
      
      </body>
      </html>

    ```

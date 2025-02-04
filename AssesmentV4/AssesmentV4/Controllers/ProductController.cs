using AssesmentV4.Interfaces;
using AssesmentV4.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssesmentV4.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductRepository _databaseRepository;
        private readonly IProductRepository _jsonRepository;
        public ProductController(IProductRepository jsonProductRepository, IProductRepository postgresProductRepository)
        {
            _jsonRepository = jsonProductRepository;
            _databaseRepository = postgresProductRepository;
        }
        // Add to ProductController.cs
        [HttpGet]
        public IActionResult Index(string source, ProductSearchCriteria criteria,
                                  string sortColumn = "Name", string sortDirection = "asc",
                                  List<string> visibleColumns = null)
        {
            // Column visibility logic
            var allColumns = new List<string> { "Id", "Image", "Name", "OrderDate", "Price", "DiscountedPrice" };

            // Get visible columns from cookie or default
            //if(visibleColumns.Count == 0)
            //visibleColumns = allColumns;
            //else
                visibleColumns = Request.Cookies["VisibleColumns"]?.Split(',')?.ToList() ?? allColumns;

            // Save visible columns to cookie
            Response.Cookies.Append("VisibleColumns", string.Join(',', visibleColumns),
                                  new CookieOptions { Expires = DateTime.Now.AddDays(7) });

            IQueryable<Product> products = GetSortedProducts(source, criteria, sortColumn, sortDirection);

            var vm = new ProductViewModel
            {
                Products = products.ToList(),
                VisibleColumns = visibleColumns,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                SearchCriteria = criteria,
                Source = source
            };

            return View(vm);
        }

        private IQueryable<Product> GetSortedProducts(string source, ProductSearchCriteria criteria,
                                             string sortColumn, string sortDirection)
        {
            IQueryable<Product> products;
            var direction = sortDirection == "asc" ? "ASC" : "DESC";

            if (source?.ToLower() == "json")
            {
                products = _jsonRepository.GetProducts(criteria);
                    //.OrderBy($"{sortColumn} {direction}");
            }
            else
            {
                products = _databaseRepository.GetProducts(criteria);
                           //.OrderBy($"{sortColumn} {direction}");
            }

            return products;
        }
    }
}

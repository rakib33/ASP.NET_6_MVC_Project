using AssesmentV4.Interfaces;
using AssesmentV4.Models;
using AssesmentV4.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AssesmentV4.Controllers
{
    public class HomeController : Controller
    {
        private IProductRepository _productRepository;
        private readonly IServiceProvider _serviceProvider;
        private  IQueryable<Product> products;
        public HomeController(IServiceProvider serviceProvider)
        {
           _serviceProvider = serviceProvider;
        }

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
}

using AssesmentV4.Interfaces;
using AssesmentV4.Models;
using Newtonsoft.Json;

namespace AssesmentV4.Repositories
{
    public class JsonProductRepository : IProductRepository
    {
        //path to the json file
        private readonly string _jsonPath;

        public JsonProductRepository(string jsonPath)
        {

            _jsonPath = jsonPath;

        }

        public IQueryable<Product> GetAllProducts()
        {

            if (!File.Exists(_jsonPath))
                throw new FileNotFoundException("JSON file not found.");

            string json = File.ReadAllText(_jsonPath);
            var products = JsonConvert.DeserializeObject<List<Product>>(json);
            return products.AsQueryable();
        }

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

        /// <inheritdoc />

    }
}

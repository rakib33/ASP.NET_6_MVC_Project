using AssesmentV4.Data;
using AssesmentV4.Interfaces;
using AssesmentV4.Models;

namespace AssesmentV4.Repositories
{
    public class PostgresProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PostgresProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Product> GetAllProducts()
        {
            return _dbContext.Products;
        }


        /// <inheritdoc />
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
}

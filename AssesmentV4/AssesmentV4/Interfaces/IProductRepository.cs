using AssesmentV4.Models;

namespace AssesmentV4.Interfaces
{
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
}

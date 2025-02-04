using AssesmentV4.Data;
using AssesmentV4.Models;
using System.Collections.Generic;
using System.Linq;

namespace AssesmentV4.Services
{
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
}

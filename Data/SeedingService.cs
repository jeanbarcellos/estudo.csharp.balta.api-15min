using System;
using System.Linq;
using testeef.Models;

namespace testeef.Data
{

    public class SeedingService
    {
        private DataContext _context;

        public SeedingService(DataContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Categories.Any() || _context.Products.Any())
            {
                return;
            }

            Category c1 = new Category { Title = "Category 1" };
            Category c2 = new Category { Title = "Category 2" };
            Category c3 = new Category { Title = "Category 3" };

            Product p1 = new Product
            {
                Title = "Product 1",
                Description = "Description of Product 1",
                Price = 1299,
                Category = c1
            };

            Product p2 = new Product
            {
                Title = "Product 2",
                Description = "Product description 1",
                Price = 99.95m,
                Category = c2
            };

            Product p3 = new Product
            {
                Title = "Product 3",
                Description = "Description of Product 3",
                Price = 998.98m,
                Category = c2
            };

            Product p4 = new Product
            {
                Title = "Product 4",
                Description = "Description of Product 4",
                Price = 398.98m,
                Category = c2
            };

            _context.Categories.AddRange(c1, c2);

            _context.Products.AddRange(p1, p2, p3, p4);

            _context.SaveChanges();
        }

    }


}
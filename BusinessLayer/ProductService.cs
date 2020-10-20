using BusinessLayer.Entities;
using DataLayer;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ProductService : IDisposable
    {
        private bool disposed = false;
        private DataSource _ds;
       


        public ProductService(string connectionString)
        {
            _ds = new DataSource(connectionString);
           
        } 

        public IEnumerable<ProductBLL> GetAllProducts()
        {
            List<ProductBLL> items = new List<ProductBLL>();

            foreach (var p in _ds.GetAllProducts())
            {
                items.Add(new ProductBLL
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Image = p.Image
                });
            }

            return items;
        }
        public IEnumerable<ProductBLL> GetProductsByCategoryId(int id)
        {
            List<ProductBLL> items = new List<ProductBLL>();

            foreach (var p in _ds.GetProductsByCategoryId(id))
            {
                items.Add(new ProductBLL
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Image = p.Image
                });
            }

            return items;
        }

        public ProductBLL GetProductById(int id)
        {
            Product prod = _ds.GetProductById(id);
            return new ProductBLL()
            {
                Id = prod.Id,
                Name = prod.Name,
                Description = prod.Description,
                Price = prod.Price,
                Image = prod.Image
            };
        }

        public IEnumerable<CategoryBLL> GetCategories()
        {
            List<CategoryBLL> categories = new List<CategoryBLL>();

            foreach (var c in _ds.GetCategories())
            {
                categories.Add(new CategoryBLL
                {
                    Id = c.Id,
                    Name = c.Name
                });
            }

            return categories;
        }

        public void Dispose()
        {
            if (!disposed)
            {
                _ds.Dispose();
                disposed = true;
            }

        }

    }
}

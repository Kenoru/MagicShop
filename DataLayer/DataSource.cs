using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DataSource : IDisposable
    {

        private SqlConnection _connection;
        private bool disposed = false;
        

        public DataSource(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }
        /*
        public IEnumerable<Product> GetAllProducts()
        {
            return new List<Product>()
            {
                new Product(){Id = 1, Name = "Healing potion", Price = 50, Description = "Heals for 2d4 +2"},
                new Product(){Id = 2, Name = "Scroll of Fireball", Price = 250, Description = "Casts a Goddamn FIREBALL"},
                new Product(){Id = 3, Name = "Dack of Many Faces", Price = 500, Description = "Take a card and see what happens"}
            };
        }*/

        public IEnumerable<Product> GetAllProducts()
        {
            List<Product> items = new List<Product>();

            string sqlExpression = @"select p.Id, p.Name, p.Price, p.Description, p.Image from Products p";

            using (SqlCommand command = new SqlCommand(sqlExpression, _connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new Product()
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Price = (int)reader["Price"],
                            Description = (string)reader["Description"],
                            Image = (string)reader["Image"]
                        });

                    }
                }
            }
            return items;
        }

        public Product GetProductById(int id)
        {

            string sqlExpression = @"select p.Id, p.Name, p.Price, p.Description, p.Image from Products p where p.Id = @id";
            using (SqlCommand command = new SqlCommand(sqlExpression, _connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Product()
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Price = (int)reader["Price"],
                            Description = (string)reader["Description"],
                            Image = (string)reader["Image"]
                        };

                    }
                    else
                        throw new Exception("Cannot find a product with such id");
                }
            }
        }

        public IEnumerable<Product> GetProductsByCategoryId(int id)
        {
            List<Product> items = new List<Product>();

            string sqlExpression = @"select p.Id, p.Name, p.Price, p.Description, p.Image from Products p join Categories c on p.CategoryId = c.Id where c.Id = @id";

            using (SqlCommand command = new SqlCommand(sqlExpression, _connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new Product()
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Price = (int)reader["Price"],
                            Description = (string)reader["Description"],
                            Image = (string)reader["Image"]
                        });

                    }
                }
            }
            return items;
        }

        public IEnumerable<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();
            string sqlExpression = @"select c.Id, c.Name from Categories c";

            using (SqlCommand command = new SqlCommand(sqlExpression, _connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category()
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"]                           
                        });

                    }
                }
            }
            return categories;
        }

        public void Dispose()
        {
            if (!disposed)
            {
                _connection.Dispose();
                disposed = true;
            }
        }
    }
}

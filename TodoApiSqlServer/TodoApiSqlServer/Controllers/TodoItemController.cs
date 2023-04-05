using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using TodoApiSqlServer.Models;

namespace TodoApiSqlServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {      

        private readonly string connectionString = "Server=DATNGUYEN\\SQLEXPRESS;Database=testdb;Integrated Security=True;";

        [HttpGet]
        public List<Itemmodel> GetAllItems()
        {
            List<Itemmodel> itemmodels = new List<Itemmodel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM product";
                using (SqlCommand command = new SqlCommand(query, connection))
                {                 
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Itemmodel model = new Itemmodel();
                            model.id = (int)reader["id"];
                            model.product = (string)reader["product"];
                            model.price = (double)reader["price"];
                            itemmodels.Add(model);
                        }
                    }                   
                }
                connection.Close();
            }
            return itemmodels;
        }

        [HttpPost]
        public Itemmodel AddItem(Itemmodel model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO product (id, product, price) VALUES (@id, @product, @price)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", model.id);
                    command.Parameters.AddWithValue("@product", model.product);
                    command.Parameters.AddWithValue("@price", model.price);                 
                    command.ExecuteNonQuery();
                    connection.Close();                   
                }
                connection.Close();
                return model;
            }
        }

        [HttpPut("{id}")]
        public Itemmodel UpdateItem(int id, Itemmodel model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE product SET product = @product, price = @price WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@product", model.product);
                    command.Parameters.AddWithValue("@price", model.price);;
                    int rows = command.ExecuteNonQuery();
                    if (rows == 0)
                    {
                        return null!;
                    }
                }
                connection.Close();
            }
            return model;
        }

        [HttpDelete("{id}")]
        public string RemoveItem(int id)
        {            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM product WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        return "Product deleted successfully.";
                    }                                                           
                }
                connection.Close();
            }
            return "Failed to delete product.";
        }       
    }
}

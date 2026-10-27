
using CRUD_Razor.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace CRUD_Razor.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Book> Books { get; set; } = new();

        public void OnGet()
        {
            string connectionString =
                _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "DefaultConnection is not configured.");

            using SqlConnection connection =
                new SqlConnection(connectionString);

            connection.Open();

            string query = @"
                SELECT Id, title, author, description,
                       publisher, status, category
                FROM dbo.books
                ORDER BY Id DESC";

            using SqlCommand command =
                new SqlCommand(query, connection);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Books.Add(new Book
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    title = reader["title"] as string,
                    author = reader["author"] as string,
                    description = reader["description"] as string,
                    publisher = reader["publisher"] as string,
                    status = reader["status"] as string,
                    category = reader["category"] as string
                });
            }
        }
    }
}

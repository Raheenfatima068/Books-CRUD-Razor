
using CRUD_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CRUD_Razor.Pages.Books
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Book Book { get; set; } = new();

        public IActionResult OnGet(int id)
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
                WHERE Id = @Id";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            using SqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return NotFound();
            }

            Book = new Book
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                title = reader["title"] as string,
                author = reader["author"] as string,
                description = reader["description"] as string,
                publisher = reader["publisher"] as string,
                status = reader["status"] as string,
                category = reader["category"] as string
            };

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            string connectionString =
                _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "DefaultConnection is not configured.");

            using SqlConnection connection =
                new SqlConnection(connectionString);

            connection.Open();

            string query = "DELETE FROM dbo.books WHERE Id = @Id";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                return NotFound();
            }

            return RedirectToPage("/Books/Index");
        }
    }
}

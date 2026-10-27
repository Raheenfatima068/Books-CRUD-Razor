
using CRUD_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CRUD_Razor.Pages.Books
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
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
            if (!ModelState.IsValid)
            {
                Book.Id = id;
                return Page();
            }

            string connectionString =
                _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "DefaultConnection is not configured.");

            using SqlConnection connection =
                new SqlConnection(connectionString);

            connection.Open();

            string query = @"
                UPDATE dbo.books
                SET title = @title,
                    author = @author,
                    description = @description,
                    publisher = @publisher,
                    status = @status,
                    category = @category
                WHERE Id = @Id";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            command.Parameters.Add("@title", SqlDbType.NVarChar, 50).Value =
                (object?)Book.title ?? DBNull.Value;

            command.Parameters.Add("@author", SqlDbType.NVarChar, 50).Value =
                (object?)Book.author ?? DBNull.Value;

            command.Parameters.Add("@description", SqlDbType.NVarChar, 50).Value =
                (object?)Book.description ?? DBNull.Value;

            command.Parameters.Add("@publisher", SqlDbType.NVarChar, 50).Value =
                (object?)Book.publisher ?? DBNull.Value;

            command.Parameters.Add("@status", SqlDbType.NVarChar, 50).Value =
                (object?)Book.status ?? DBNull.Value;

            command.Parameters.Add("@category", SqlDbType.NVarChar, 50).Value =
                (object?)Book.category ?? DBNull.Value;

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                return NotFound();
            }

            return RedirectToPage("/Books/Index");
        }
    }
}

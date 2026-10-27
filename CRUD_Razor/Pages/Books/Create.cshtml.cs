
using CRUD_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CRUD_Razor.Pages.Books
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public Book Book { get; set; } = new();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
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
                INSERT INTO dbo.books
                    (title, author, description, publisher, status, category)
                VALUES
                    (@title, @author, @description, @publisher, @status, @category)";

            using SqlCommand command =
                new SqlCommand(query, connection);

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

            command.ExecuteNonQuery();

            return RedirectToPage("/Books/Index");
        }
    }
}

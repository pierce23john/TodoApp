using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace TodoApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;

        public IEnumerable<Todo> Todos { get; set; }

        [BindProperty]
        public string Filter { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                Todos = await connection.QueryAsync<Todo>("SELECT * FROM Todos");
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                Todos = await connection.QueryAsync<Todo>("[dbo].[SP_GetTodosByName]", new { Name = Filter }, null, null, CommandType.StoredProcedure);
            }

            return Page();
        }
    }
}
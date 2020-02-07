using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

// Benefit of Entity framework
// 1. Faster development speed but perfomance not good on porduction.
// 2. You dont need to known SQL.
// 3. Good for small project with small amount of users.

// Benefit of Dapper
// 1.Faster in production
// 2.Easier to work with for SQL Developer
// 3.Design for loose coupling

namespace EFDemoWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly PeopleContext _db;

        public IndexModel(ILogger<IndexModel> logger, PeopleContext db)
        {
            _logger = logger;
            _db = db;
        }

        public void OnGet()
        {
            LoadSampleData();

            var people = _db.Person
                .Include(a => a.Addresses)
                .Include(e => e.EmailAddresses)      
                .ToList();              
        } 

        private void LoadSampleData()
        {
            if (_db.Person.Count() == 0)
            {
                string file = System.IO.File.ReadAllText(@"generated.json");
                var people = JsonSerializer.Deserialize<List<Person>>(file);
                _db.AddRange(people);
                _db.SaveChanges();
            }
        }
    }
}

using APIforMyNUnit.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace APIforMyNUnit.Controllers
{
    /// <summary>
    /// Controller for showing the history of test queries
    /// </summary>
    public class TestHistoryController : Controller
    {
        private HistoryTestingContext dbContext;
        public TestHistoryController(HistoryTestingContext context)
        {
            dbContext = context;
        }

        /// <summary>
        /// /TestHistory/Show
        /// </summary>
        /// <returns>View with launch history</returns>
        [ActionName("Show")]
        public async Task<IActionResult> ShowHistoryAsync()
        {
            var historyFromDB = await dbContext.Assemblys
                .AsNoTracking()
                .Include(t => t.Succeeded)
                .Include(t => t.Failed)
                .Include(t => t.Ignored)
                .ToListAsync();

            return View(historyFromDB);
        }
    }
}
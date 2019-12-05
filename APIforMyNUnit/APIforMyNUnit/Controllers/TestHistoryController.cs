using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIforMyNUnit.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APIforMyNUnit.Controllers
{
    public class TestHistoryController : Controller
    {
        private HistoryTestingContext dbContext;
        public TestHistoryController(HistoryTestingContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
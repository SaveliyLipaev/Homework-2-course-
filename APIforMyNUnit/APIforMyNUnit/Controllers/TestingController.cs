using APIforMyNUnit.Models;
using APIforMyNUnit.Repositories;
using Microsoft.AspNetCore.Mvc;
using MyNUnit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace APIforMyNUnit.Controllers
{
    /// <summary>
    /// Assembly testing contoller
    /// </summary>
    public class TestingController : Controller
    {
        private HistoryTestingContext dbContext;
        public TestingController(HistoryTestingContext context)
        {
            dbContext = context;
        }

        /// <summary>
        /// /Testing/Upload
        /// </summary>
        /// <returns>View with upload form</returns>
        public ActionResult Upload()
        {
            return View();
        }

        /// <summary>
        /// Processing post request, starts downloading and testing assembly
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> StartTestingAsync(UploadAssembilesModel files)
        {
            var assemblies = files.Assemblies
                .Where(file => file.FileName.EndsWith(".dll"))
                .Select(assemblyFile =>
                {
                    Assembly assembly;
                    using (var memoryStream = new MemoryStream())
                    {
                        assemblyFile.CopyTo(memoryStream);
                        assembly = Assembly.Load(memoryStream.ToArray());
                    }
                    return assembly;
                }).ToList();

            MyNUnitRunner.Run(assemblies, DynamicTestSubmission);

            foreach (KeyValuePair<string, TestedAssemblyModel> testedAssembly in MyNUnitRunner.AssemblyInformation)
            {
                await dbContext.Assemblys.AddAsync(testedAssembly.Value);
            }

            await dbContext.SaveChangesAsync();

            return DynamicTestSubmission();
        }

        /// <summary>
        /// For dynamically showing test progress
        /// </summary>
        public ActionResult DynamicTestSubmission()
        {
            return View("TestingProcess", MyNUnitRunner.TestsInformation);
        }
    }
}

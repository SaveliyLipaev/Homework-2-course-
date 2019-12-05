using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using APIforMyNUnit.Models;
using APIforMyNUnit.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyNUnit;

namespace APIforMyNUnit.Controllers
{
    public class TestingController : Controller
    {
        private HistoryTestingContext dbContext;
        public TestingController(HistoryTestingContext context)
        {
            dbContext = context;
        }

        public ActionResult Upload()
        {
            return View();
        }

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

            var assemblyTestResults = assemblies.Select(assembly =>
            {
                MyNUnitRunner.Run(new List<Assembly> { assembly }, DynamicTestSubmission);
                var assemblyTestResult = new TestedAssemblyModel()
                {
                    Name = assembly.GetName().Name,
                    Failed = MyNUnitRunner.Failed.Select(t => new TestInformationModel(t)).ToList(),
                    Succeeded = MyNUnitRunner.Succeeded.Select(t => new TestInformationModel(t)).ToList(),
                    Ignored = MyNUnitRunner.Ignored.Select(t => new TestInformationModel(t)).ToList(),
                };

                return assemblyTestResult;
            })
            .ToList();

            await dbContext.Assemblys.AddRangeAsync(assemblyTestResults);
            await dbContext.SaveChangesAsync();

            return View("TestingProcess", MyNUnitRunner.TestsInformation);
        }

        public ActionResult DynamicTestSubmission()
        {
            return View("TestingProcess", MyNUnitRunner.TestsInformation);
        }
    }
}

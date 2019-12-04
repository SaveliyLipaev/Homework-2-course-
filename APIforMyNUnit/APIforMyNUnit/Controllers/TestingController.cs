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

namespace APIforMyNUnit.Controllers
{
    public class TestingController : Controller
    {
        public TestingController()
        {

        }

        public ActionResult UploadAsync()
        {
            return View();
        }

        public ActionResult StartTesting(UploadAssembiles files) 
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


            return View("Upload");
        }

    }
}

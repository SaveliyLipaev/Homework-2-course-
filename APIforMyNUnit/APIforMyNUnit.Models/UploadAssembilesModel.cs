using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace APIforMyNUnit.Models
{
    /// <summary>
    /// Class to load the assembly into the application
    /// </summary>
    public class UploadAssembilesModel
    {
        public List<IFormFile> Assemblies { get; set; } = new List<IFormFile>();
    }
}

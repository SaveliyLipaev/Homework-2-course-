﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIforMyNUnit.Models
{
    public class UploadAssembilesModel
    {
        public List<IFormFile> Assemblies { get; set; } = new List<IFormFile>();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfForFtp.Models
{
    class DirectoryModel
    {
        public List<FileModel> Files { get; set; }
        public int Size { get; set; }
    }
}

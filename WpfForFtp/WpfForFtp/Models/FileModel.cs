using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WpfForFtp.AppEnums;

namespace WpfForFtp.Models
{
    class FileModel
    {
        public string Name { get; set; }
        public FileType FileType { get; set; }
        public StateInstall StateInstall { get; set; }
    }
}

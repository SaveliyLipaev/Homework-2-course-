using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfForFtp
{
    public class AppEnums
    {
        public enum StateInstall
        {
            installed,
            notInstalled,
            loading, 
        }

        public enum FileType
        {
            isDir,
            isFile,
            isBack
        }
    }
}

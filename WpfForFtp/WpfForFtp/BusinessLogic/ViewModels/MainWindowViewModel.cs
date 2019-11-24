using SimpleFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfForFtp.Models;
using static WpfForFtp.AppEnums;

namespace WpfForFtp.BusinessLogic.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private string path = "../../../../";
        private Client client;

        public string Port
        {
            get => Get<string>();
            set => Set(value);
        }

        public string Address
        {
            get => Get<string>();
            set => Set(value);
        }

        public string StateConnection
        {
            get => Get<string>();
            set => Set(value);
        }

        public DirectoryModel Files
        {
            get => Get<DirectoryModel>();
            set => Set(value);
        }

        public FileModel SelectedFile
        {
            get => Get<FileModel>();
            set => Set(value);
        }

        public string StateDownload
        {
            get => Get<string>();
            set => Set(value);
        }

        public ICommand ConnectButton => MakeCommand(async (obj) =>
        {
            if (!int.TryParse(Port, out int result))
            {
                StateConnection = "Port input error";
            }
            else
            {
                client = new Client(Address, result);

                if (await client.Connect())
                {
                    StateConnection = "Connection established";
                    var answer = await client.ListCommand(path);
                    Files = Spliter(answer);
                }
                else
                {
                    StateConnection = "Connection error";
                }
            }
        });

        public ICommand SelectedItemDoubleClickCommand => MakeCommand(async (obj) =>
        {
            var file = obj as FileModel;

            if (file.FileType == FileType.isDir)
            {
                var answer = await client.ListCommand(path += file.Name + '/');
                Files = Spliter(answer);
            }
            else if (file.FileType == FileType.isBack) 
            {
                GetOldPath(ref path);
                var answer = await client.ListCommand(path);
                Files = Spliter(answer);
            }
            else
            {
                
            }
        });

        public ICommand DownloadAllButton => MakeCommand((obj) =>
        {

        });

        private DirectoryModel Spliter((string, List<(string, bool)>) answer)
        {
            var directoryModel = new DirectoryModel();
            directoryModel.Files = new List<FileModel>();

            if (path != "../../../../") 
            {
                directoryModel.Files.Add(new FileModel
                    { FileType = FileType.isBack, Name = "back"});
            }

            foreach (var file in answer.Item2)
            {
                directoryModel.Files.Add(new FileModel
                    { FileType = file.Item2 ? FileType.isDir : FileType.isFile, 
                    Name = file.Item1, StateInstall = StateInstall.notInstalled });
            }

            return directoryModel;
        }

        private void GetOldPath(ref string path)
        {
            var count = 0;
            foreach (var i in path)
            {
                if (i == '/')
                {
                    count++;
                }
            }

            var newPath = "";
            var secondCount = 0;

            foreach (var i in path)
            {
                newPath += i;

                if (i == '/')
                {
                    secondCount++;
                }

                if (secondCount == count - 1)
                {
                    break;
                }
            }

            path = newPath;
        }
    }
}

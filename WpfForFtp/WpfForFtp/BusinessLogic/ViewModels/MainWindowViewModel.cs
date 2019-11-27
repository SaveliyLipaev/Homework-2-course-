using SimpleFTP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfForFtp.Models;
using static WpfForFtp.AppEnums;

namespace WpfForFtp.BusinessLogic.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private string path = "../../../../";
        private CancellationTokenSource token = new CancellationTokenSource();
        private Client client;

        public MainWindowViewModel()
        {
            DownloadableFiles = new ObservableCollection<FileModel>();
            DownloadHistoryFiles = new ObservableCollection<FileModel>();
            Log = new ObservableCollection<string>();
            Task.Run(Download);
        }

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

        public ObservableCollection<FileModel> Files
        {
            get => Get<ObservableCollection<FileModel>>();
            set => Set(value);
        }

        public ObservableCollection<FileModel> DownloadableFiles
        {
            get => Get<ObservableCollection<FileModel>>();
            set => Set(value);
        }

        public ObservableCollection<FileModel> DownloadHistoryFiles
        {
            get => Get<ObservableCollection<FileModel>>();
            set => Set(value);
        }

        public ObservableCollection<string> Log
        {
            get => Get<ObservableCollection<string>>();
            set => Set(value);
        }

        public FileModel SelectedFile
        {
            get => Get<FileModel>();
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
            var newFile = obj as FileModel;

            if (newFile.FileType == FileType.isDir)
            {
                var answer = await client.ListCommand(path += newFile.Name + '/');
                Files = Spliter(answer);
            }
            else if (newFile.FileType == FileType.isBack)
            {
                GetOldPath(ref path);
                var answer = await client.ListCommand(path);
                Files = Spliter(answer);
            }
            else
            {
                DownloadableFiles.Add(newFile);
                await DownloadFileAsync(newFile);
            }
        });

        public ICommand DownloadAllButton => MakeCommand(async (obj) =>
        {
            foreach (var file in Files)
            {
                if (file.FileType == FileType.isFile)
                {
                    DownloadableFiles.Add(file);
                }
            }
        });

        private ObservableCollection<FileModel> Spliter((string, List<(string, bool)>) answer)
        {
            var files = new ObservableCollection<FileModel>();

            if (path != "../../../../")
            {
                files.Add(new FileModel
                { FileType = FileType.isBack, Name = "back" });
            }

            foreach (var file in answer.Item2)
            {
                files.Add(new FileModel
                {
                    FileType = file.Item2 ? FileType.isDir : FileType.isFile,
                    Name = file.Item1,
                    StateInstall = StateInstall.notInstalled
                });
            }

            return files;
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

        private void Download()
        {
            while (token.Token.IsCancellationRequested)
            {
                if (DownloadableFiles.Count != 0)
                {
                    DownloadFileAsync(DownloadableFiles[0]);
                }
                Task.Delay(300);
            }
        }

        private async Task DownloadFileAsync(FileModel file)
        {
            Log.Add($"Download start {file.Name}");
            var answer = await client.GetCommand(path + file.Name);
            if (answer.Item1 != null)
            {
                try
                {
                    File.WriteAllBytes(file.Name, answer.Item2);
                    Log.Add($"Successfully download {file.Name}");
                    DownloadableFiles.Remove(file);
                    DownloadHistoryFiles.Add(file);
                }
                catch (Exception e)
                {
                    Log.Add($"Error download {file.Name}: {e.Message}");
                }
            }
            else
            {
                Log.Add($"Error download {file.Name}: {answer.Item3}");
            }
        }
    }
}

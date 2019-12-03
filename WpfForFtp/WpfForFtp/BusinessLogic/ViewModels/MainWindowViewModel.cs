using SimpleFTP;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfForFtp.Helpers;
using WpfForFtp.Models;
using static WpfForFtp.AppEnums;

namespace WpfForFtp.BusinessLogic.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private string path = "../../../../";
        private Client client;
        private Thread threadForDownload;
        private object locker = new object();

        public MainWindowViewModel()
        {
            DownloadableFiles = new AsyncObservableCollection<FileModel>();
            DownloadHistoryFiles = new AsyncObservableCollection<FileModel>();
            Log = new AsyncObservableCollection<string>();
            threadForDownload = new Thread(Download);
            threadForDownload.Start();
            DownloadableFiles.CollectionChanged += GoTask;
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

        public AsyncObservableCollection<FileModel> Files
        {
            get => Get<AsyncObservableCollection<FileModel>>();
            set => Set(value);
        }

        public AsyncObservableCollection<FileModel> DownloadableFiles
        {
            get => Get<AsyncObservableCollection<FileModel>>();
            set => Set(value);
        }

        public AsyncObservableCollection<FileModel> DownloadHistoryFiles
        {
            get => Get<AsyncObservableCollection<FileModel>>();
            set => Set(value);
        }

        public AsyncObservableCollection<string> Log
        {
            get => Get<AsyncObservableCollection<string>>();
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
            }
        });

        public void GoTask(object sender, EventArgs e)
        {
            if (threadForDownload.ThreadState == ThreadState.WaitSleepJoin)
            {
                threadForDownload.Interrupt();
            }
        }

        public ICommand DownloadAllButton => MakeCommand((obj) =>
        {
            foreach (var file in Files)
            {
                if (file.FileType == FileType.isFile)
                {
                    DownloadableFiles.Add(file);
                }
            }
        });

        private AsyncObservableCollection<FileModel> Spliter((string, List<(string, bool)>) answer)
        {
            var files = new AsyncObservableCollection<FileModel>();

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
            while (true)
            {
                while (DownloadableFiles.Count != 0)
                {
                    DownloadFile(DownloadableFiles?[0]);
                }
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }
                catch (ThreadInterruptedException) { }
            }
        }

        private void DownloadFile(FileModel file)
        {
            if (file == null) return;
            Log.Add($"{file.Name} start download");
            var answer = client.GetCommand(path + file.Name);
            if (answer.Result.Item1 != null)
            {
                try
                {
                    File.WriteAllBytes(file.Name, answer.Result.Item2);
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
                Log.Add($"Error download {file.Name}: {answer.Result.Item3}");
            }
        }
    }
}

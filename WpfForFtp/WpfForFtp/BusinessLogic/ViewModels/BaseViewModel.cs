using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfForFtp.Helpers;

namespace WpfForFtp.BusinessLogic.ViewModels
{
    public class BaseViewModel : Bindable, IDisposable
    {
        private readonly CancellationTokenSource _networkTokenSource = new CancellationTokenSource();
        private readonly ConcurrentDictionary<string, ICommand> _cachedCommands = new ConcurrentDictionary<string, ICommand>();

        public CancellationToken CancellationToken => _networkTokenSource?.Token ?? CancellationToken.None;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            CancelNetworkRequests();
        }

        public void CancelNetworkRequests()
        {
            _networkTokenSource.Cancel();
        }

        protected ICommand MakeCommand(Action<object> commandAction, [CallerMemberName] string propertyName = null)
        {
            return GetCommand(propertyName) ?? SaveCommand(new Command(commandAction), propertyName);
        }

        private ICommand SaveCommand(ICommand command, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (!_cachedCommands.ContainsKey(propertyName))
            {
                _cachedCommands.TryAdd(propertyName, command);
            }

            return command;
        }

        private ICommand GetCommand(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            return _cachedCommands.TryGetValue(propertyName, out var cachedCommand)
                ? cachedCommand : null;
        }
    }
}
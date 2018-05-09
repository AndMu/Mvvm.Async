using System.Threading.Tasks;
using System.Windows.Input;
using NUnit.Framework;

namespace Wikiled.Mvvm.Async.Tests
{
    [TestFixture]
    public class AsyncCommandUnitTests
    {
        [Test]
        public void AfterConstruction_IsNotExecuting()
        {
            var command = new AsyncCommand(_ => Task.FromResult(0));
            Assert.False(command.IsExecuting);
            Assert.Null(command.Execution);
            Assert.True(((ICommand)command).CanExecute(null));
        }

        [Test]
        public void AfterSynchronousExecutionComplete_IsNotExecuting()
        {
            var command = new AsyncCommand(_ => Task.FromResult(0));

            ((ICommand)command).Execute(null);

            Assert.False(command.IsExecuting);
            Assert.NotNull(command.Execution);
            Assert.True(((ICommand)command).CanExecute(null));
        }

        [Test]
        public void StartExecution_IsExecuting()
        {
            var signal = new TaskCompletionSource<object>();
            var command = new AsyncCommand(_ => signal.Task);

            ((ICommand)command).Execute(null);

            Assert.True(command.IsExecuting);
            Assert.NotNull(command.Execution);
            Assert.False(((ICommand)command).CanExecute(null));

            signal.SetResult(null);
        }

        [Test]
        public void Execute_DelaysExecutionUntilCommandIsInExecutingState()
        {
            bool isExecuting = false;
            NotifyTask execution = null;
            bool canExecute = true;

            AsyncCommand command = null;
            command = new AsyncCommand(() =>
            {
                isExecuting = command.IsExecuting;
                execution = command.Execution;
                canExecute = ((ICommand) command).CanExecute(null);
                return Task.FromResult(0);
            });

            ((ICommand)command).Execute(null);

            Assert.True(isExecuting);
            Assert.NotNull(execution);
            Assert.False(canExecute);
        }

        [Test]
        public void StartExecution_NotifiesPropertyChanges()
        {
            var signal = new TaskCompletionSource<object>();
            var command = new AsyncCommand(_ => signal.Task);
            var isExecutingNotification = TestUtils.PropertyNotified(command, n => n.IsExecuting);
            var executionNotification = TestUtils.PropertyNotified(command, n => n.Execution);
            var sawCanExecuteChanged = false;
            ((ICommand)command).CanExecuteChanged += (_, __) => { sawCanExecuteChanged = true; };

            ((ICommand)command).Execute(null);

            Assert.True(isExecutingNotification());
            Assert.True(executionNotification());
            Assert.True(sawCanExecuteChanged);

            signal.SetResult(null);
        }
    }
}
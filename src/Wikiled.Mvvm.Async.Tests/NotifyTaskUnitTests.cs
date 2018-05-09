using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Wikiled.Mvvm.Async.Tests
{
    [TestFixture]
    public class NotifyTaskUnitTests
    {
        [Test]
        public void Notifier_TaskCompletesSuccessfully_NotifiesProperties()
        {
            var tcs = new TaskCompletionSource<object>();
            var notifier = NotifyTask.Create(() => (Task)tcs.Task);
            var taskNotification = TestUtils.PropertyNotified(notifier, n => n.Task);
            var statusNotification = TestUtils.PropertyNotified(notifier, n => n.Status);
            var isCompletedNotification = TestUtils.PropertyNotified(notifier, n => n.IsCompleted);
            var isSuccessfullyCompletedNotification = TestUtils.PropertyNotified(notifier, n => n.IsSuccessfullyCompleted);
            var isCanceledNotification = TestUtils.PropertyNotified(notifier, n => n.IsCanceled);
            var isFaultedNotification = TestUtils.PropertyNotified(notifier, n => n.IsFaulted);
            var exceptionNotification = TestUtils.PropertyNotified(notifier, n => n.Exception);
            var innerExceptionNotification = TestUtils.PropertyNotified(notifier, n => n.InnerException);
            var errorMessageNotification = TestUtils.PropertyNotified(notifier, n => n.ErrorMessage);

            Assert.AreSame(tcs.Task, notifier.Task);
            Assert.False(notifier.TaskCompleted.IsCompleted);
            Assert.AreEqual(tcs.Task.Status, notifier.Status);
            Assert.False(notifier.IsCompleted);
            Assert.True(notifier.IsNotCompleted);
            Assert.False(notifier.IsSuccessfullyCompleted);
            Assert.False(notifier.IsCanceled);
            Assert.False(notifier.IsFaulted);
            Assert.Null(notifier.Exception);
            Assert.Null(notifier.InnerException);
            Assert.Null(notifier.ErrorMessage);

            tcs.SetResult(null);

            Assert.AreSame(tcs.Task, notifier.Task);
            Assert.True(notifier.TaskCompleted.IsCompleted && !notifier.TaskCompleted.IsCanceled && !notifier.TaskCompleted.IsFaulted);
            Assert.AreEqual(tcs.Task.Status, notifier.Status);
            Assert.True(notifier.IsCompleted);
            Assert.False(notifier.IsNotCompleted);
            Assert.True(notifier.IsSuccessfullyCompleted);
            Assert.False(notifier.IsCanceled);
            Assert.False(notifier.IsFaulted);
            Assert.Null(notifier.Exception);
            Assert.Null(notifier.InnerException);
            Assert.Null(notifier.ErrorMessage);

            Assert.False(taskNotification());
            Assert.True(statusNotification());
            Assert.True(isCompletedNotification());
            Assert.True(isSuccessfullyCompletedNotification());
            Assert.False(isCanceledNotification());
            Assert.False(isFaultedNotification());
            Assert.False(exceptionNotification());
            Assert.False(innerExceptionNotification());
            Assert.False(errorMessageNotification());
        }

        [Test]
        public void Notifier_TaskCanceled_NotifiesProperties()
        {
            var tcs = new TaskCompletionSource<object>();
            var notifier = NotifyTask.Create(() => (Task)tcs.Task);
            var taskNotification = TestUtils.PropertyNotified(notifier, n => n.Task);
            var statusNotification = TestUtils.PropertyNotified(notifier, n => n.Status);
            var isCompletedNotification = TestUtils.PropertyNotified(notifier, n => n.IsCompleted);
            var isSuccessfullyCompletedNotification = TestUtils.PropertyNotified(notifier, n => n.IsSuccessfullyCompleted);
            var isCanceledNotification = TestUtils.PropertyNotified(notifier, n => n.IsCanceled);
            var isFaultedNotification = TestUtils.PropertyNotified(notifier, n => n.IsFaulted);
            var exceptionNotification = TestUtils.PropertyNotified(notifier, n => n.Exception);
            var innerExceptionNotification = TestUtils.PropertyNotified(notifier, n => n.InnerException);
            var errorMessageNotification = TestUtils.PropertyNotified(notifier, n => n.ErrorMessage);

            Assert.AreSame(tcs.Task, notifier.Task);
            Assert.False(notifier.TaskCompleted.IsCompleted);
            Assert.AreEqual(tcs.Task.Status, notifier.Status);
            Assert.False(notifier.IsCompleted);
            Assert.True(notifier.IsNotCompleted);
            Assert.False(notifier.IsSuccessfullyCompleted);
            Assert.False(notifier.IsCanceled);
            Assert.False(notifier.IsFaulted);
            Assert.Null(notifier.Exception);
            Assert.Null(notifier.InnerException);
            Assert.Null(notifier.ErrorMessage);

            tcs.SetCanceled();

            Assert.AreSame(tcs.Task, notifier.Task);
            Assert.True(notifier.TaskCompleted.IsCompleted && !notifier.TaskCompleted.IsCanceled && !notifier.TaskCompleted.IsFaulted);
            Assert.AreEqual(tcs.Task.Status, notifier.Status);
            Assert.True(notifier.IsCompleted);
            Assert.False(notifier.IsNotCompleted);
            Assert.False(notifier.IsSuccessfullyCompleted);
            Assert.True(notifier.IsCanceled);
            Assert.False(notifier.IsFaulted);
            Assert.Null(notifier.Exception);
            Assert.Null(notifier.InnerException);
            Assert.Null(notifier.ErrorMessage);

            Assert.False(taskNotification());
            Assert.True(statusNotification());
            Assert.True(isCompletedNotification());
            Assert.False(isSuccessfullyCompletedNotification());
            Assert.True(isCanceledNotification());
            Assert.False(isFaultedNotification());
            Assert.False(exceptionNotification());
            Assert.False(innerExceptionNotification());
            Assert.False(errorMessageNotification());
        }

        [Test]
        public void Notifier_TaskFaulted_NotifiesProperties()
        {
            var tcs = new TaskCompletionSource<object>();
            var notifier = NotifyTask.Create(() => (Task)tcs.Task);
            var exception = new NotImplementedException(Guid.NewGuid().ToString("N"));
            var taskNotification = TestUtils.PropertyNotified(notifier, n => n.Task);
            var statusNotification = TestUtils.PropertyNotified(notifier, n => n.Status);
            var isCompletedNotification = TestUtils.PropertyNotified(notifier, n => n.IsCompleted);
            var isSuccessfullyCompletedNotification = TestUtils.PropertyNotified(notifier, n => n.IsSuccessfullyCompleted);
            var isCanceledNotification = TestUtils.PropertyNotified(notifier, n => n.IsCanceled);
            var isFaultedNotification = TestUtils.PropertyNotified(notifier, n => n.IsFaulted);
            var exceptionNotification = TestUtils.PropertyNotified(notifier, n => n.Exception);
            var innerExceptionNotification = TestUtils.PropertyNotified(notifier, n => n.InnerException);
            var errorMessageNotification = TestUtils.PropertyNotified(notifier, n => n.ErrorMessage);

            Assert.AreSame(tcs.Task, notifier.Task);
            Assert.False(notifier.TaskCompleted.IsCompleted);
            Assert.AreEqual(tcs.Task.Status, notifier.Status);
            Assert.False(notifier.IsCompleted);
            Assert.True(notifier.IsNotCompleted);
            Assert.False(notifier.IsSuccessfullyCompleted);
            Assert.False(notifier.IsCanceled);
            Assert.False(notifier.IsFaulted);
            Assert.Null(notifier.Exception);
            Assert.Null(notifier.InnerException);
            Assert.Null(notifier.ErrorMessage);

            tcs.SetException(exception);

            Assert.AreSame(tcs.Task, notifier.Task);
            Assert.True(notifier.TaskCompleted.IsCompleted && !notifier.TaskCompleted.IsCanceled && !notifier.TaskCompleted.IsFaulted);
            Assert.AreEqual(tcs.Task.Status, notifier.Status);
            Assert.True(notifier.IsCompleted);
            Assert.False(notifier.IsNotCompleted);
            Assert.False(notifier.IsSuccessfullyCompleted);
            Assert.False(notifier.IsCanceled);
            Assert.True(notifier.IsFaulted);
            Assert.NotNull(notifier.Exception);
            Assert.AreSame(exception, notifier.InnerException);
            Assert.AreEqual(exception.Message, notifier.ErrorMessage);

            Assert.False(taskNotification());
            Assert.True(statusNotification());
            Assert.True(isCompletedNotification());
            Assert.False(isSuccessfullyCompletedNotification());
            Assert.False(isCanceledNotification());
            Assert.True(isFaultedNotification());
            Assert.True(exceptionNotification());
            Assert.True(innerExceptionNotification());
            Assert.True(errorMessageNotification());
        }

        [Test]
        public void NotifierT_TaskCompletesSuccessfully_NotifiesProperties()
        {
            var tcs = new TaskCompletionSource<object>();
            var notifier = NotifyTask.Create(() => tcs.Task);
            var result = new object();
            var taskNotification = TestUtils.PropertyNotified(notifier, n => n.Task);
            var statusNotification = TestUtils.PropertyNotified(notifier, n => n.Status);
            var isCompletedNotification = TestUtils.PropertyNotified(notifier, n => n.IsCompleted);
            var isSuccessfullyCompletedNotification = TestUtils.PropertyNotified(notifier, n => n.IsSuccessfullyCompleted);
            var isCanceledNotification = TestUtils.PropertyNotified(notifier, n => n.IsCanceled);
            var isFaultedNotification = TestUtils.PropertyNotified(notifier, n => n.IsFaulted);
            var exceptionNotification = TestUtils.PropertyNotified(notifier, n => n.Exception);
            var innerExceptionNotification = TestUtils.PropertyNotified(notifier, n => n.InnerException);
            var errorMessageNotification = TestUtils.PropertyNotified(notifier, n => n.ErrorMessage);
            var resultNotification = TestUtils.PropertyNotified(notifier, n => n.Result);

            Assert.AreSame(tcs.Task, notifier.Task);
            Assert.False(notifier.TaskCompleted.IsCompleted);
            Assert.AreEqual(tcs.Task.Status, notifier.Status);
            Assert.False(notifier.IsCompleted);
            Assert.True(notifier.IsNotCompleted);
            Assert.False(notifier.IsSuccessfullyCompleted);
            Assert.Null(notifier.Result);
            Assert.False(notifier.IsCanceled);
            Assert.False(notifier.IsFaulted);
            Assert.Null(notifier.Exception);
            Assert.Null(notifier.InnerException);
            Assert.Null(notifier.ErrorMessage);

            tcs.SetResult(result);

            Assert.AreSame(tcs.Task, notifier.Task);
            Assert.True(notifier.TaskCompleted.IsCompleted && !notifier.TaskCompleted.IsCanceled && !notifier.TaskCompleted.IsFaulted);
            Assert.AreEqual(tcs.Task.Status, notifier.Status);
            Assert.True(notifier.IsCompleted);
            Assert.False(notifier.IsNotCompleted);
            Assert.True(notifier.IsSuccessfullyCompleted);
            Assert.AreSame(result, notifier.Result);
            Assert.False(notifier.IsCanceled);
            Assert.False(notifier.IsFaulted);
            Assert.Null(notifier.Exception);
            Assert.Null(notifier.InnerException);
            Assert.Null(notifier.ErrorMessage);

            Assert.False(taskNotification());
            Assert.True(statusNotification());
            Assert.True(isCompletedNotification());
            Assert.True(isSuccessfullyCompletedNotification());
            Assert.False(isCanceledNotification());
            Assert.False(isFaultedNotification());
            Assert.False(exceptionNotification());
            Assert.False(innerExceptionNotification());
            Assert.False(errorMessageNotification());
            Assert.True(resultNotification());
        }

        [Test]
        public void NotifierT_TaskCanceled_NotifiesProperties()
        {
            var tcs = new TaskCompletionSource<object>();
            var notifier = NotifyTask.Create(() => tcs.Task);
            var taskNotification = TestUtils.PropertyNotified(notifier, n => n.Task);
            var statusNotification = TestUtils.PropertyNotified(notifier, n => n.Status);
            var isCompletedNotification = TestUtils.PropertyNotified(notifier, n => n.IsCompleted);
            var isSuccessfullyCompletedNotification = TestUtils.PropertyNotified(notifier, n => n.IsSuccessfullyCompleted);
            var isCanceledNotification = TestUtils.PropertyNotified(notifier, n => n.IsCanceled);
            var isFaultedNotification = TestUtils.PropertyNotified(notifier, n => n.IsFaulted);
            var exceptionNotification = TestUtils.PropertyNotified(notifier, n => n.Exception);
            var innerExceptionNotification = TestUtils.PropertyNotified(notifier, n => n.InnerException);
            var errorMessageNotification = TestUtils.PropertyNotified(notifier, n => n.ErrorMessage);
            var resultNotification = TestUtils.PropertyNotified(notifier, n => n.Result);

            Assert.AreSame(tcs.Task, notifier.Task);
            Assert.False(notifier.TaskCompleted.IsCompleted);
            Assert.AreEqual(tcs.Task.Status, notifier.Status);
            Assert.False(notifier.IsCompleted);
            Assert.True(notifier.IsNotCompleted);
            Assert.False(notifier.IsSuccessfullyCompleted);
            Assert.Null(notifier.Result);
            Assert.False(notifier.IsCanceled);
            Assert.False(notifier.IsFaulted);
            Assert.Null(notifier.Exception);
            Assert.Null(notifier.InnerException);
            Assert.Null(notifier.ErrorMessage);

            tcs.SetCanceled();

            Assert.AreSame(tcs.Task, notifier.Task);
            Assert.True(notifier.TaskCompleted.IsCompleted && !notifier.TaskCompleted.IsCanceled && !notifier.TaskCompleted.IsFaulted);
            Assert.AreEqual(tcs.Task.Status, notifier.Status);
            Assert.True(notifier.IsCompleted);
            Assert.False(notifier.IsNotCompleted);
            Assert.False(notifier.IsSuccessfullyCompleted);
            Assert.Null(notifier.Result);
            Assert.True(notifier.IsCanceled);
            Assert.False(notifier.IsFaulted);
            Assert.Null(notifier.Exception);
            Assert.Null(notifier.InnerException);
            Assert.Null(notifier.ErrorMessage);

            Assert.False(taskNotification());
            Assert.True(statusNotification());
            Assert.True(isCompletedNotification());
            Assert.False(isSuccessfullyCompletedNotification());
            Assert.True(isCanceledNotification());
            Assert.False(isFaultedNotification());
            Assert.False(exceptionNotification());
            Assert.False(innerExceptionNotification());
            Assert.False(errorMessageNotification());
            Assert.False(resultNotification());
        }

        [Test]
        public void NotifierT_TaskFaulted_NotifiesProperties()
        {
            var tcs = new TaskCompletionSource<object>();
            var notifier = NotifyTask.Create(() => tcs.Task);
            var exception = new NotImplementedException(Guid.NewGuid().ToString("N"));
            var taskNotification = TestUtils.PropertyNotified(notifier, n => n.Task);
            var statusNotification = TestUtils.PropertyNotified(notifier, n => n.Status);
            var isCompletedNotification = TestUtils.PropertyNotified(notifier, n => n.IsCompleted);
            var isSuccessfullyCompletedNotification = TestUtils.PropertyNotified(notifier, n => n.IsSuccessfullyCompleted);
            var isCanceledNotification = TestUtils.PropertyNotified(notifier, n => n.IsCanceled);
            var isFaultedNotification = TestUtils.PropertyNotified(notifier, n => n.IsFaulted);
            var exceptionNotification = TestUtils.PropertyNotified(notifier, n => n.Exception);
            var innerExceptionNotification = TestUtils.PropertyNotified(notifier, n => n.InnerException);
            var errorMessageNotification = TestUtils.PropertyNotified(notifier, n => n.ErrorMessage);
            var resultNotification = TestUtils.PropertyNotified(notifier, n => n.Result);

            Assert.AreSame(tcs.Task, notifier.Task);
            Assert.False(notifier.TaskCompleted.IsCompleted);
            Assert.AreEqual(tcs.Task.Status, notifier.Status);
            Assert.False(notifier.IsCompleted);
            Assert.True(notifier.IsNotCompleted);
            Assert.False(notifier.IsSuccessfullyCompleted);
            Assert.Null(notifier.Result);
            Assert.False(notifier.IsCanceled);
            Assert.False(notifier.IsFaulted);
            Assert.Null(notifier.Exception);
            Assert.Null(notifier.InnerException);
            Assert.Null(notifier.ErrorMessage);

            tcs.SetException(exception);

            Assert.AreSame(tcs.Task, notifier.Task);
            Assert.True(notifier.TaskCompleted.IsCompleted && !notifier.TaskCompleted.IsCanceled && !notifier.TaskCompleted.IsFaulted);
            Assert.AreEqual(tcs.Task.Status, notifier.Status);
            Assert.True(notifier.IsCompleted);
            Assert.False(notifier.IsNotCompleted);
            Assert.False(notifier.IsSuccessfullyCompleted);
            Assert.Null(notifier.Result);
            Assert.False(notifier.IsCanceled);
            Assert.True(notifier.IsFaulted);
            Assert.NotNull(notifier.Exception);
            Assert.AreSame(exception, notifier.InnerException);
            Assert.AreEqual(exception.Message, notifier.ErrorMessage);

            Assert.False(taskNotification());
            Assert.True(statusNotification());
            Assert.True(isCompletedNotification());
            Assert.False(isSuccessfullyCompletedNotification());
            Assert.False(isCanceledNotification());
            Assert.True(isFaultedNotification());
            Assert.True(exceptionNotification());
            Assert.True(innerExceptionNotification());
            Assert.True(errorMessageNotification());
            Assert.False(resultNotification());
        }

        [Test]
        public void PropertyChanged_NoListeners_DoesNotThrow()
        {
            var tcs = new TaskCompletionSource<object>();
            var notifier = NotifyTask.Create(() => (Task)tcs.Task);
            tcs.SetResult(null);
        }

        [Test]
        public void NotifierT_NonDefaultResult_TaskCompletesSuccessfully_UpdatesResult()
        {
            var tcs = new TaskCompletionSource<object>();
            var defaultResult = new object();
            var notifier = NotifyTask.Create(() => tcs.Task, defaultResult);
            var result = new object();
            var resultNotification = TestUtils.PropertyNotified(notifier, n => n.Result);

            Assert.AreSame(defaultResult, notifier.Result);

            tcs.SetResult(result);

            Assert.AreSame(result, notifier.Result);
            Assert.True(resultNotification());
        }

        [Test]
        public void NotifierT_NonDefaultResult_TaskCanceled_KeepsNonDefaultResult()
        {
            var tcs = new TaskCompletionSource<object>();
            var defaultResult = new object();
            var notifier = NotifyTask.Create(() => tcs.Task, defaultResult);

            Assert.AreSame(defaultResult, notifier.Result);

            tcs.SetCanceled();

            Assert.AreSame(defaultResult, notifier.Result);
        }

        [Test]
        public void NotifierT_NonDefaultResult_TaskFaulted_KeepsNonDefaultResult()
        {
            var tcs = new TaskCompletionSource<object>();
            var defaultResult = new object();
            var notifier = NotifyTask.Create(() => tcs.Task, defaultResult);
            var exception = new NotImplementedException(Guid.NewGuid().ToString("N"));

            Assert.AreSame(defaultResult, notifier.Result);

            tcs.SetException(exception);

            Assert.AreSame(defaultResult, notifier.Result);
        }

        [Test]
        public void Notifier_SynchronousTaskCompletedSuccessfully_SetsProperties()
        {
            var task = Task.CompletedTask;
            var notifier = NotifyTask.Create(() => task);

            Assert.AreSame(task, notifier.Task);
            Assert.True(notifier.TaskCompleted.IsCompleted && !notifier.TaskCompleted.IsCanceled && !notifier.TaskCompleted.IsFaulted);
            Assert.AreEqual(task.Status, notifier.Status);
            Assert.True(notifier.IsCompleted);
            Assert.False(notifier.IsNotCompleted);
            Assert.True(notifier.IsSuccessfullyCompleted);
            Assert.False(notifier.IsCanceled);
            Assert.False(notifier.IsFaulted);
            Assert.Null(notifier.Exception);
            Assert.Null(notifier.InnerException);
            Assert.Null(notifier.ErrorMessage);
        }

        [Test]
        public void Notifier_SynchronousTaskCanceled_SetsProperties()
        {
            var task = Task.FromCanceled(new CancellationToken(true));
            var notifier = NotifyTask.Create(() => task);

            Assert.AreSame(task, notifier.Task);
            Assert.True(notifier.TaskCompleted.IsCompleted && !notifier.TaskCompleted.IsCanceled && !notifier.TaskCompleted.IsFaulted);
            Assert.AreEqual(task.Status, notifier.Status);
            Assert.True(notifier.IsCompleted);
            Assert.False(notifier.IsNotCompleted);
            Assert.False(notifier.IsSuccessfullyCompleted);
            Assert.True(notifier.IsCanceled);
            Assert.False(notifier.IsFaulted);
            Assert.Null(notifier.Exception);
            Assert.Null(notifier.InnerException);
            Assert.Null(notifier.ErrorMessage);
        }

        [Test]
        public void Notifier_SynchronousTaskFaulted_SetsProperties()
        {
            var exception = new Exception(Guid.NewGuid().ToString("N"));
            var task = Task.FromException(exception);
            var notifier = NotifyTask.Create(() => task);

            Assert.AreSame(task, notifier.Task);
            Assert.True(notifier.TaskCompleted.IsCompleted && !notifier.TaskCompleted.IsCanceled && !notifier.TaskCompleted.IsFaulted);
            Assert.AreEqual(task.Status, notifier.Status);
            Assert.True(notifier.IsCompleted);
            Assert.False(notifier.IsNotCompleted);
            Assert.False(notifier.IsSuccessfullyCompleted);
            Assert.False(notifier.IsCanceled);
            Assert.True(notifier.IsFaulted);
            Assert.NotNull(notifier.Exception);
            Assert.AreSame(exception, notifier.InnerException);
            Assert.AreEqual(exception.Message, notifier.ErrorMessage);
        }

        [Test]
        public void NotifierT_SynchronousTaskCompletedSuccessfully_SetsProperties()
        {
            var task = Task.FromResult(13);
            var notifier = NotifyTask.Create(() => task, 17);

            Assert.AreSame(task, notifier.Task);
            Assert.True(notifier.TaskCompleted.IsCompleted && !notifier.TaskCompleted.IsCanceled && !notifier.TaskCompleted.IsFaulted);
            Assert.AreEqual(13, notifier.Result);
            Assert.AreEqual(task.Status, notifier.Status);
            Assert.True(notifier.IsCompleted);
            Assert.False(notifier.IsNotCompleted);
            Assert.True(notifier.IsSuccessfullyCompleted);
            Assert.False(notifier.IsCanceled);
            Assert.False(notifier.IsFaulted);
            Assert.Null(notifier.Exception);
            Assert.Null(notifier.InnerException);
            Assert.Null(notifier.ErrorMessage);
        }

        [Test]
        public void NotifierT_SynchronousTaskCanceled_SetsProperties()
        {
            var task = Task.FromCanceled<int>(new CancellationToken(true));
            var notifier = NotifyTask.Create(() => task, 17);

            Assert.AreSame(task, notifier.Task);
            Assert.True(notifier.TaskCompleted.IsCompleted && !notifier.TaskCompleted.IsCanceled && !notifier.TaskCompleted.IsFaulted);
            Assert.AreEqual(17, notifier.Result);
            Assert.AreEqual(task.Status, notifier.Status);
            Assert.True(notifier.IsCompleted);
            Assert.False(notifier.IsNotCompleted);
            Assert.False(notifier.IsSuccessfullyCompleted);
            Assert.True(notifier.IsCanceled);
            Assert.False(notifier.IsFaulted);
            Assert.Null(notifier.Exception);
            Assert.Null(notifier.InnerException);
            Assert.Null(notifier.ErrorMessage);
        }

        [Test]
        public void NotifierT_SynchronousTaskFaulted_SetsProperties()
        {
            var exception = new Exception(Guid.NewGuid().ToString("N"));
            var task = Task.FromException<int>(exception);
            var notifier = NotifyTask.Create(() => task, 17);

            Assert.AreSame(task, notifier.Task);
            Assert.True(notifier.TaskCompleted.IsCompleted && !notifier.TaskCompleted.IsCanceled && !notifier.TaskCompleted.IsFaulted);
            Assert.AreEqual(17, notifier.Result);
            Assert.AreEqual(task.Status, notifier.Status);
            Assert.True(notifier.IsCompleted);
            Assert.False(notifier.IsNotCompleted);
            Assert.False(notifier.IsSuccessfullyCompleted);
            Assert.False(notifier.IsCanceled);
            Assert.True(notifier.IsFaulted);
            Assert.NotNull(notifier.Exception);
            Assert.AreSame(exception, notifier.InnerException);
            Assert.AreEqual(exception.Message, notifier.ErrorMessage);
        }
    }
}

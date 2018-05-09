using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Wikiled.Disposables.Tests
{
    [TestFixture]
    public class SingleNonblockingDisposableUnitTests
    {
        [Test]
        public void ConstructedWithContext_DisposeReceivesThatContext()
        {
            var providedContext = new object();
            object seenContext = null;
            var disposable = new DelegateSingleDisposable<object>(providedContext, context => { seenContext = context; });
            disposable.Dispose();
            Assert.AreSame(providedContext, seenContext);
        }

        [Test]
        public void DisposeOnlyCalledOnce()
        {
            var counter = 0;
            var disposable = new DelegateSingleDisposable<object>(new object(), _ => { ++counter; });
            disposable.Dispose();
            disposable.Dispose();
            Assert.AreEqual(1, counter);
        }

        [Test]
        public async Task DisposeIsNonblocking()
        {
            var ready = new ManualResetEventSlim();
            var signal = new ManualResetEventSlim();
            var disposable = new DelegateSingleDisposable<object>(new object(), _ =>
            {
                ready.Set();
                signal.Wait();
            });

            var task1 = Task.Run(() => disposable.Dispose());
            ready.Wait();

            await Task.Run(() => disposable.Dispose());

            signal.Set();
            await task1;
        }

        private sealed class DelegateSingleDisposable<T> : SingleNonblockingDisposable<T>
            where T : class
        {
            private readonly Action<T> _callback;

            public DelegateSingleDisposable(T context, Action<T> callback)
                : base(context)
            {
                _callback = callback;
            }

            protected override void Dispose(T context)
            {
                _callback(context);
            }
        }
    }
}

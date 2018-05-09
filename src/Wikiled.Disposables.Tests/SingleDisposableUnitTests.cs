﻿using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Wikiled.Disposables.Tests
{
    [TestFixture]
    public class SingleDisposableUnitTests
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
        public async Task DisposableWaitsForDisposeToComplete()
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

            var task2 = Task.Run(() => disposable.Dispose());
            var timer = Task.Delay(500);
            Assert.AreSame(timer, await Task.WhenAny(task1, task2, timer));

            signal.Set();
            await task1;
            await task2;
        }

        private sealed class DelegateSingleDisposable<T> : SingleDisposable<T>
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

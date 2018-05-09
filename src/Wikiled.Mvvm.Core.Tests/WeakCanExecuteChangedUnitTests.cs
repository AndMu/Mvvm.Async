using System;
using NUnit.Framework;

namespace Wikiled.Mvvm.Core.Tests
{
    [TestFixture]
    public class WeakCanExecuteChangedUnitTests
    {
        [Test]
        public void CanExecuteChanged_Unsubscribed_IsNotNotified()
        {
            var sender = new object();
            object observedSender = null;
            var command = new WeakCanExecuteChanged(sender);
            EventHandler subscription = (s, _) =>
            {
                observedSender = s;
            };

            command.CanExecuteChanged += subscription;
            command.CanExecuteChanged -= subscription;
            command.OnCanExecuteChanged();

            Assert.Null(observedSender);

            GC.KeepAlive(subscription);
        }
    }
}
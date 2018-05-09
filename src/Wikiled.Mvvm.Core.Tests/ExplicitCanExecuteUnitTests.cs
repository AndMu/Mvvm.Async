using System;
using NUnit.Framework;

namespace Wikiled.Mvvm.Core.Tests
{
    [TestFixture]
    public class ExplicitCanExecuteUnitTests
    {
        [Test]
        public void CanExecute_DefaultValue_IsFalse()
        {
            var command = new ExplicitCanExecute(new object());
            Assert.False(command.CanExecute);
            Assert.False(((ICanExecute)command).CanExecute(null));
        }

        [Test]
        public void CanExecute_SetToTrue_RaisesNotification()
        {
            var sender = new object();
            object observedSender = null;
            var command = new ExplicitCanExecute(sender);
            bool observedValue = false;
            EventHandler subscription = (s, _) =>
            {
                observedSender = s;
                observedValue = ((ICanExecute)command).CanExecute(null);
            };

            ((ICanExecute)command).CanExecuteChanged += subscription;
            command.CanExecute = true;

            Assert.True(command.CanExecute);
            Assert.True(((ICanExecute)command).CanExecute(null));
            Assert.True(observedValue);
            Assert.AreSame(sender, observedSender);

            GC.KeepAlive(subscription);
        }

        [Test]
        public void CanExecute_TrueSetToFalse_RaisesNotification()
        {
            var sender = new object();
            object observedSender = null;
            var command = new ExplicitCanExecute(sender);
            command.CanExecute = true;
            bool observedValue = true;
            EventHandler subscription = (s, _) =>
            {
                observedSender = s;
                observedValue = ((ICanExecute)command).CanExecute(null);
            };

            ((ICanExecute)command).CanExecuteChanged += subscription;
            command.CanExecute = false;

            Assert.False(command.CanExecute);
            Assert.False(((ICanExecute)command).CanExecute(null));
            Assert.False(observedValue);
            Assert.AreSame(sender, observedSender);

            GC.KeepAlive(subscription);
        }

        [Test]
        public void CanExecute_FalseSetToFalse_DoesNotRaiseNotification()
        {
            var sender = new object();
            object observedSender = null;
            var command = new ExplicitCanExecute(sender);
            EventHandler subscription = (s, _) =>
            {
                observedSender = s;
            };

            ((ICanExecute)command).CanExecuteChanged += subscription;
            command.CanExecute = false;

            Assert.False(command.CanExecute);
            Assert.False(((ICanExecute)command).CanExecute(null));
            Assert.Null(observedSender);

            GC.KeepAlive(subscription);
        }

        [Test]
        public void CanExecuteChanged_Unsubscribed_IsNotNotified()
        {
            var sender = new object();
            object observedSender = null;
            var command = new ExplicitCanExecute(sender);
            EventHandler subscription = (s, _) =>
            {
                observedSender = s;
            };

            ((ICanExecute)command).CanExecuteChanged += subscription;
            ((ICanExecute)command).CanExecuteChanged -= subscription;
            command.CanExecute = true;

            Assert.True(command.CanExecute);
            Assert.True(((ICanExecute)command).CanExecute(null));
            Assert.Null(observedSender);

            GC.KeepAlive(subscription);
        }
    }
}
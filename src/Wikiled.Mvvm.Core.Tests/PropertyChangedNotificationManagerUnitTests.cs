using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Wikiled.Mvvm.Core.Tests
{
    [TestFixture]
    public class PropertyChangedNotificationManagerUnitTests
    {
        [Test]
        public void Register_NotDeferred_RaisesEvent()
        {
            var pc = new DelegatePropertyChanged();
            var name = Guid.NewGuid().ToString("N");
            PropertyChangedNotificationManager.Instance.Register(pc, name);
            Assert.AreEqual(new[] { name }, pc.ObservedArgs.Select(x => x.PropertyName));
        }

        [Test]
        public void Register_Deferred_DefersEvent()
        {
            var pc = new DelegatePropertyChanged();
            var name = Guid.NewGuid().ToString("N");
            using (PropertyChangedNotificationManager.Instance.DeferNotifications())
            {
                PropertyChangedNotificationManager.Instance.Register(pc, name);
                Assert.AreEqual(0, pc.ObservedArgs.Count);
            }
            Assert.AreEqual(new[] { name }, pc.ObservedArgs.Select(x => x.PropertyName));
        }

        [Test]
        public void DeferNotifications_IsRefCounted()
        {
            var pc = new DelegatePropertyChanged();
            var name = Guid.NewGuid().ToString("N");
            using (PropertyChangedNotificationManager.Instance.DeferNotifications())
            {
                using (PropertyChangedNotificationManager.Instance.DeferNotifications())
                    PropertyChangedNotificationManager.Instance.Register(pc, name);
                Assert.AreEqual(0, pc.ObservedArgs.Count);
            }
            Assert.AreEqual(new[] { name }, pc.ObservedArgs.Select(x => x.PropertyName));
        }

        [Test]
        public void Register_Deferred_ConsolidatesEvents()
        {
            var pc = new DelegatePropertyChanged();
            var name = Guid.NewGuid().ToString("N");
            using (PropertyChangedNotificationManager.Instance.DeferNotifications())
            {
                PropertyChangedNotificationManager.Instance.Register(pc, name);
                PropertyChangedNotificationManager.Instance.Register(pc, name);
            }
            Assert.AreEqual(new[] { name }, pc.ObservedArgs.Select(x => x.PropertyName));
        }

        [Test]
        public void Consolidation_DifferentNames_AreDifferent()
        {
            var pc = new DelegatePropertyChanged();
            var name1 = Guid.NewGuid().ToString("N");
            var name2 = Guid.NewGuid().ToString("N");
            using (PropertyChangedNotificationManager.Instance.DeferNotifications())
            {
                PropertyChangedNotificationManager.Instance.Register(pc, name1);
                PropertyChangedNotificationManager.Instance.Register(pc, name2);
                PropertyChangedNotificationManager.Instance.Register(pc, name1);
                PropertyChangedNotificationManager.Instance.Register(pc, name2);
            }
            Assert.AreEqual(new[] { name1, name2 }.OrderBy(x => x), pc.ObservedArgs.Select(x => x.PropertyName).OrderBy(x => x));
        }

        [Test]
        public void Consolidation_DifferentObjects_AreDifferent()
        {
            var pc1 = new DelegatePropertyChanged();
            var pc2 = new DelegatePropertyChanged();
            var name = Guid.NewGuid().ToString("N");
            using (PropertyChangedNotificationManager.Instance.DeferNotifications())
            {
                PropertyChangedNotificationManager.Instance.Register(pc1, name);
                PropertyChangedNotificationManager.Instance.Register(pc2, name);
                PropertyChangedNotificationManager.Instance.Register(pc1, name);
                PropertyChangedNotificationManager.Instance.Register(pc2, name);
            }
            Assert.AreEqual(new[] { name }, pc1.ObservedArgs.Select(x => x.PropertyName));
            Assert.AreEqual(new[] { name }, pc2.ObservedArgs.Select(x => x.PropertyName));
        }

        private sealed class DelegatePropertyChanged : IRaisePropertyChanged
        {
            public void RaisePropertyChanged(PropertyChangedEventArgs e)
            {
                ObservedArgs.Add(e);
            }

            public List<PropertyChangedEventArgs> ObservedArgs = new List<PropertyChangedEventArgs>();
        }
    }
}
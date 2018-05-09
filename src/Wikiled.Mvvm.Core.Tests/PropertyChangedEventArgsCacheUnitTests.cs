using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Wikiled.Mvvm.Core.Tests
{
    [TestFixture]
    public class PropertyChangedEventArgsCacheUnitTests
    {
        [Test]
        public void Get_NotInCache_IsCreated()
        {
            var name = Guid.NewGuid().ToString("N");
            var result = PropertyChangedEventArgsCache.Instance.Get(name);
            Assert.AreEqual(name, result.PropertyName);
        }

        [Test]
        public void Get_InCache_IsReturned()
        {
            var name = Guid.NewGuid().ToString("N");
            var result1 = PropertyChangedEventArgsCache.Instance.Get(name);
            var result2 = PropertyChangedEventArgsCache.Instance.Get(name);
            Assert.AreEqual(name, result1.PropertyName);
            Assert.AreSame(result1, result2);
        }

        [Test]
        public async Task Cache_IsSharedBetweenThreads()
        {
            var name = Guid.NewGuid().ToString("N");
            var result1 = await Task.Run(() => PropertyChangedEventArgsCache.Instance.Get(name));
            var result2 = PropertyChangedEventArgsCache.Instance.Get(name);
            Assert.AreEqual(name, result1.PropertyName);
            Assert.AreSame(result1, result2);
        }
    }
}
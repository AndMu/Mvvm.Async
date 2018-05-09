using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace Wikiled.Mvvm.Core.Tests
{
    [TestFixture]
    public class PropertyChangedSubscriptionUnitTests
    {
        [Test]
        public void PropertyChanged_ForAllProperties_WithActionCallback_WhenOneChanges_InvokesCallback()
        {
            var vm = new TestViewModel();
            var invoked = false;
            using (PropertyChangedSubscription.Create(vm, null, () => { invoked = true; }))
            {
                vm.Bob = "test";
            }
            Assert.True(invoked);
        }

        [Test]
        public void PropertyChanged_ForAllProperties_WithHandlerCallback_WhenOneChanges_InvokesCallback()
        {
            var vm = new TestViewModel();
            var invoked = false;
            using (PropertyChangedSubscription.Create(vm, null, (sender, e) => { invoked = sender == vm && e.PropertyName == "Bob"; }))
            {
                vm.Bob = "test";
            }
            Assert.True(invoked);
        }

        [Test]
        public void PropertyChanged_ForSpecificProperty_WithActionCallback_WhenMatchingChanges_InvokesCallback()
        {
            var vm = new TestViewModel();
            var invoked = false;
            using (PropertyChangedSubscription.Create(vm, "Bob", () => { invoked = true; }))
            {
                vm.Bob = "test";
            }
            Assert.True(invoked);
        }

        [Test]
        public void PropertyChanged_ForSpecificProperty_WithActionCallback_WhenOtherChanges_DoesNotInvokeCallback()
        {
            var vm = new TestViewModel();
            var invoked = false;
            using (PropertyChangedSubscription.Create(vm, "Bob", () => { invoked = true; }))
            {
                vm.Mary = "test";
            }
            Assert.False(invoked);
        }


        [Test]
        public void PropertyChanged_AfterDisposal_DoesNotInvokeCallback()
        {
            var vm = new TestViewModel();
            var invoked = false;
            using (PropertyChangedSubscription.Create(vm, null, () => { invoked = true; }))
            {
            }
            vm.Bob = "test";
            Assert.False(invoked);
        }
        private sealed class TestViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public string Bob { set {  OnPropertyChanged();} }
            public string Mary { set { OnPropertyChanged(); } }
        }
    }
}
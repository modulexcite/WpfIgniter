﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using NUnit.Framework;

namespace NorthHorizon.Samples.InpcTemplate.Tests
{
    [TestFixture]
    class ExpressionCommandTestFixture
    {
        [Test]
        public void TestSimpleCommand()
        {
            var sut = new TestViewModel();

            int updateCount = 0;
            sut.SimpleCommand.CanExecuteChanged += (s, e) => updateCount++;

            sut.Value1 = 3;

            updateCount.ShouldEqual(1);
        }

        [Test]
        public void TestComplexCommand()
        {
            var sut = new TestViewModel
            {
                Value2 = new TestSubViewModel()
            };

            int updateCount = 0;
            sut.ComplexCommand.CanExecuteChanged += (s, e) => updateCount++;

            sut.Value2 = new TestSubViewModel();

            updateCount.ShouldEqual(1);

            sut.Value2.Value4 = new TestSubViewModel();

            updateCount.ShouldEqual(2);

            sut.Value2.Value4.Value3 = 3;

            updateCount.ShouldEqual(3);
        }

        [Test]
        public void TestComplexCollectionCommand()
        {
            var sut = new TestViewModel
            {
                Value2 = new TestSubViewModel()
            };

            int updateCount = 0;
            sut.ComplexCollectionCommand.CanExecuteChanged += (s, e) => updateCount++;

            sut.Value2 = new TestSubViewModel();

            updateCount.ShouldEqual(1);

            sut.Value2.Value5.Add(sut.Value2);

            updateCount.ShouldEqual(2);
        }

        private class TestViewModel : BindableBase
        {
            public TestViewModel()
            {
                SimpleCommand = new ExpressionCommand(OnExecute, () => Value1 > 5);

                ComplexCommand = new ExpressionCommand(OnExecute, () => Value2.Value4.Value3 > 5);

                ComplexCollectionCommand = new ExpressionCommand(OnExecute, () => Value2.Value5.Contains(Value2));
            }

            public ICommand SimpleCommand { get; private set; }

            public ICommand ComplexCommand { get; private set; }

            public ICommand ComplexCollectionCommand { get; private set; }

            private int _value1;
            public int Value1
            {
                get { return _value1; }
                set { SetProperty(ref _value1, value, "Value1"); }
            }

            private TestSubViewModel _value2;
            public TestSubViewModel Value2
            {
                get { return _value2; }
                set { SetProperty(ref _value2, value, "Value2"); }
            }

            private void OnExecute() { }
        }

        private class TestSubViewModel : BindableBase
        {
            private int _value3;
            public int Value3
            {
                get { return _value3; }
                set { SetProperty(ref _value3, value, "Value3"); }
            }

            private TestSubViewModel _value4;
            public TestSubViewModel Value4
            {
                get { return _value4; }
                set { SetProperty(ref _value4, value, "Value4"); }
            }

            public ObservableCollection<TestSubViewModel> Value5 = new ObservableCollection<TestSubViewModel>();
        }
    }
}

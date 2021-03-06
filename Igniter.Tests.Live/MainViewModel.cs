﻿using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows;

namespace Igniter.Tests.Live
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            Tests =
                typeof(MainViewModel)
                .Assembly
                .GetTypes()
                .Where(t => typeof(Window).IsAssignableFrom(t))
                .Where(t => Attribute.IsDefined(t, typeof(LiveTestAttribute)))
                .Select(t => new
                {
                    TestName = ((LiveTestAttribute)Attribute.GetCustomAttribute(t, typeof(LiveTestAttribute))).Name,
                    RunCommand = new DelegateCommand(() => ((Window)Activator.CreateInstance(t)).Show())
                })
                .Concat(new[] {
                    new {
                        TestName = "Garbage Collect",
                        RunCommand = new DelegateCommand(GC.Collect)
                    }
                });
        }

        public IEnumerable Tests { get; private set; }
    }
}

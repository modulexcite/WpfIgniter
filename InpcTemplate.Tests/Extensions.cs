﻿using System.Windows.Threading;

namespace NorthHorizon.Samples.InpcTemplate.Tests
{
    public static class Extensions
    {
        public static void DoEvents(this Dispatcher dispatcher, DispatcherPriority priority = DispatcherPriority.Background)
        {
            var frame = new DispatcherFrame();
            dispatcher.BeginInvoke(() => frame.Continue = false, priority);
            Dispatcher.PushFrame(frame);
        }
    }
}

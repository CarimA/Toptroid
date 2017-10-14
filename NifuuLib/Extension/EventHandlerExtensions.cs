using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.Extension
{
    public static class EventHandlerExtensions
    {
        public static void Raise<T>(this EventHandler<T> eventHandler, object sender, T e) where T : EventArgs => eventHandler?.Invoke(sender, e);
        public static void Raise(this EventHandler eventHandler, object sender, EventArgs e) => eventHandler?.Invoke(sender, e);
    }
}

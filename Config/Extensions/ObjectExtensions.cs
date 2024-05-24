using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.Config.Extensions
{
    public static class ObjectExtensions
    {
        public static void IfPresent<T>(this T obj, Action<T> action) where T : class
        {
            if (obj != null)
            {
                action(obj);
            }
        }
    }
}

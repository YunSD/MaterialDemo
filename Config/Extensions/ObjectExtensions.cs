using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.Config.Extensions
{
    public static class ObjectExtensions
    {
        public static void GetFirstIfPresent<TSource>(this IEnumerable<TSource> source, Action<TSource> action) where TSource : class
        {
            if (source != null && source.Any())
            {   
                TSource obj = source.First();
                action(obj);
            }
        }
    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.Utils
{
    public class MapperUtil
    {
        public static TTarget Map<TSource, TTarget>(TSource value)where TSource : class where TTarget : class
        {
            var config = new MapperConfiguration(cfg =>  cfg.CreateMap<TSource, TTarget>());
            var mapper = new Mapper(config);

            var result = mapper.Map<TSource, TTarget>(value);
            return result;
        }
    }
}

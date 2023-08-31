using AutoMapper;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Clases
{
    public static class AutomapperTypeAdapter
    {
        public static TTarget ProyectarComo<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class, new()
        {
            try
            {
                var projection = Mapper.Map<TSource, TTarget>(source);
                return projection;
            }
            catch
            {
                return Mapper.Map<TSource, TTarget>(source);
            }
        }

        public static TTarget ProyectarColeccionComo<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class
        {
            try
            {
                var projection = Mapper.Map<TSource, TTarget>(source);
                return projection;
            }
            catch
            {
                return Mapper.Map<TSource, TTarget>(source);
            }
        }

        public static TDestination ConvertObject<TSource, TDestination>(TSource source)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, TDestination>();
            });
            IMapper mapper = config.CreateMapper();

            TDestination destination = mapper.Map<TDestination>(source);
            return destination;
        }

        /// <summary>
        /// convert lists of type Dto to Entities and vice versa
        /// </summary>
        /// <typeparam name="TSource">TSource</typeparam>
        /// <typeparam name="TDestination">TDestination</typeparam>
        /// <param name="sourceList">sourceList</param>
        /// <returns>a new list converted to the desired data type</returns>
        public static List<TDestination> ConvertList<TSource, TDestination>(List<TSource> sourceList)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, TDestination>();
            });
            IMapper mapper = config.CreateMapper();

            List<TDestination> destinationList = mapper.Map<List<TDestination>>(sourceList);
            return destinationList;
        }
    }
}

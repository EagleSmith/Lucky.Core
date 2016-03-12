using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Lucky.Hr.ViewModels
{
    public static class EntityMapper
    {
        public static T Map<T>(params object[] sources) where T : class
        {
            if (!sources.Any())
            {
                return default(T);
            }

            var initialSource = sources[0];

            var mappingResult = Map<T>(initialSource);

            // Now map the remaining source objects
            if (sources.Count() > 1)
            {
                Map(mappingResult, sources.Skip(1).ToArray());
            }

            return mappingResult;
        }

        private static void Map(object destination, params object[] sources)
        {
            if (!sources.Any())
            {
                return;
            }

            var destinationType = destination.GetType();

            foreach (var source in sources)
            {
                var sourceType = source.GetType();
                Mapper.Map(source, destination, sourceType, destinationType);
            }
        }

        private static T Map<T>(object source) where T : class
        {
            var destinationType = typeof(T);
            var sourceType = source.GetType();

            var mappingResult = Mapper.Map(source, sourceType, destinationType);

            return mappingResult as T;
        }
    // Mapper.CreateMap<Tuple<People, Phone>, PeoplePhoneDto >()
    //.ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.People.FirstName))
    //.ForMember(d => d.LastName, opt => opt.MapFrom(s => s.People.LastName))
    //.ForMember(d => d.Number, opt => opt.MapFrom(s => s.Phone.Number ));
    }
}

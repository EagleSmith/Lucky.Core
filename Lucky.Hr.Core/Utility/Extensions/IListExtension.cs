using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Hr.Core.Utility.Extensions
{
    public static class IListExtension
    {
        public delegate bool Condtion<T>(T t);
        public static IEnumerable<T> FindBy<T>(this  IEnumerable<T> items, Condtion<T> condition)
        {
            foreach (T t in items)
            {
                if (condition(t))
                {
                    yield return t;
                }
            }
        }

        public static T FindByEntity<T>(this IEnumerable<T> items, Condtion<T> condtion) where T : new()
        {
            IEnumerable<T> list = FindBy(items, condtion);
            if (list.Count() == 1)
                return (T)list.First();
            else
            {
                return new T();
            }
        }
        public static IList<T> Remove<T>(this IList<T> items, Condtion<T> condtion) where T : new()
        {
            items.Remove(FindByEntity<T>(items, condtion));
            return items;
        }
    }
}

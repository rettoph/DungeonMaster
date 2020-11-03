using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonMaster.Extensions.System
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetTypesAssignableFrom<TBase>(this IEnumerable<Type> types)
        {
            return types.GetTypesAssignableFrom(typeof(TBase));
        }
        public static IEnumerable<Type> GetTypesAssignableFrom(this IEnumerable<Type> types, Type baseType)
            => types.Where(t => baseType.IsAssignableFrom(t) || (baseType.IsGenericType && t.IsSubclassOfRawGeneric(baseType)));

        /// <summary>
        /// As advertised, stolen from here:
        /// https://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class
        /// </summary>
        /// <param name="type"></param>
        /// <param name="generic"></param>
        /// <returns></returns>
        public static bool IsSubclassOfRawGeneric(this Type type, Type generic)
        {
            while (type != null && type != typeof(object))
            {
                var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (generic == cur)
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

        public static IEnumerable<Type> GetTypesWithAttribute<TBase, TAttribute>(this IEnumerable<Type> types, Boolean inherit = true)
            where TAttribute : Attribute
                => types.GetTypesWithAttribute(typeof(TBase), typeof(TAttribute), inherit);

        public static IEnumerable<Type> GetTypesWithAttribute(this IEnumerable<Type> types, Type baseType, Type attribute, Boolean inherit = true)
        {
            if (!typeof(Attribute).IsAssignableFrom(attribute))
                throw new Exception("Unable to load types with attribute, attribute type does not extend Attribute.");

            return types.GetTypesAssignableFrom(baseType)
                .Where(t =>
                {
                    var info = t.GetCustomAttributes(attribute, inherit);
                    return info != null && info.Length > 0;
                });
        }
    }
}

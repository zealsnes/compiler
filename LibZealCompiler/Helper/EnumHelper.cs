using System;
using System.Linq;

namespace Zeal.Compiler.Helper
{
    static class EnumHelper
    {
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            return GetAttributes<T>(value).FirstOrDefault();
        }

        public static T[] GetAttributes<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString())[0];
            var attributes = memberInfo.GetCustomAttributes(typeof(T), false) as T[];
            return attributes;
        }
    }
}

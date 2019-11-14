using System;
using System.Reflection;

namespace DartsScoreMaster.Common
{
    public static class EnumHelper
    {
        public static T GetAttribute<T>(this Enum enumValue)
            where T : Attribute
        {
            return enumValue
                .GetType()
                .GetTypeInfo()
                .GetDeclaredField(enumValue.ToString())
                .GetCustomAttribute<T>();
        }
    }
}
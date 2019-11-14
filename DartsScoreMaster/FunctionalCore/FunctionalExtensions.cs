using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class FunctionalExtensions
    {
        public static T AddEx<T>(this List<T> self, T itemToAdd)
        {
            self.Add(itemToAdd);

            return itemToAdd;
        }
      
        public static T AddIfNotPresent<T>(this List<T> self, T itemToAdd, Func<bool> predicate)
        {
            return predicate()
                ? self.AddEx(itemToAdd)
                : itemToAdd;
        }

        public static T Do<T>(this T @this, Action<T> action)
        {
            action(@this);
            return @this;
        }
    }
}
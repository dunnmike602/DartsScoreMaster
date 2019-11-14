// ReSharper disable once CheckNamespace
namespace System
{
    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;
    }
}
using System.Runtime.Serialization;

namespace DartsScoreMaster.FunctionalCore
{
    [DataContract]
    public class DeepClonable<T>
    {
        public virtual T Clone()
        {
            return (T)MemberwiseClone();
        }
    }
}
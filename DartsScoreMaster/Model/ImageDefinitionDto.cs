using System.Runtime.Serialization;

namespace DartsScoreMaster.Model
{
    [DataContract]
    public class ImageDefinitionDto
    {
        // Properties
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public byte[] SourceBytes { get; set; }
    }
}
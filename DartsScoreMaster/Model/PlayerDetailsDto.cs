using System;
using System.Runtime.Serialization;

namespace DartsScoreMaster.Model
{
    [DataContract]
    public class PlayerDetailsDto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Flight SelectedFlight { get; set; }

        [DataMember]
        public ImageDefinitionDto PlayerImageDefinition { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string NickName { get; set; }

        [DataMember]
        public int Handicap501 { get; set; }
        
        [DataMember]
        public int Handicap401 { get; set; }


        [DataMember]
        public int Handicap301 { get; set; }


        [DataMember]
        public int Handicap201 { get; set; }


        [DataMember]
        public int Handicap101 { get; set; }


        [DataMember]
        public int HandicapCricket { get; set; }
    }
}
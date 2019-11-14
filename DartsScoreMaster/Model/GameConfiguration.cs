using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DartsScoreMaster.Model
{
    [DataContract]
    public class GameConfiguration
    {
        [DataMember]
        public bool ShowCheckoutHints { get; set; }

        [DataMember]
        public bool PlaySounds { get; set; }

        [DataMember]
        public bool EnableSoundRecognition { get; set; }

        [DataMember]
        public GameType GameType { get; set; }

        [DataMember]
        public int Sets { get; set; }

        [DataMember]
        public int LegsPerSet { get; set; }
        
        [DataMember]
        public bool ShowSimpleBoard{ get; set; }

        [DataMember]
        public int PlayerCount { get; set; }

        [DataMember]
        public List<Guid> PlayerIndexes { get; set; }

        [DataMember]
        public List<int> PlayerIds { get; set; }
    }
}
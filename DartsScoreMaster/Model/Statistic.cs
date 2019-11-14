using System;
using System.Runtime.Serialization;
using DartsScoreMaster.FunctionalCore;
using DartsScoreMaster.Repositories.Serialization;

namespace DartsScoreMaster.Model
{
    [DataContract]
    public class Statistic : DeepClonable<Statistic>
    {
        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public decimal OneDartAverage { get; set; }

        [DataMember]
        public int DartsThrown { get; set; }

        [DataMember]
        public int LegsPlayed { get; set; }

        [DataMember]
        public int LegsWon { get; set; }

        [DataMember]
        public int MatchesPlayed { get; set; }

        [DataMember]
        public int MatchesWon{ get; set; }

        [DataMember]
        public int DartsThrownInWinningLegs { get; set; }
        
        [DataMember]
        public int HighestCheckout { get; set; }

        [DataMember]
        public int HighestScore { get; set; }
        
        [DataMember]
        public int NineDartCheckouts { get; set; }

        [DataMember]
        public int TwelveDartCheckouts { get; set; }

        [DataMember]
        public int BestGame { get; set; }
        
        [DataMember]
        public int HundredsScored { get; set; }

        [DataMember]
        public int HundredFortiesScored { get; set; }

        [DataMember]
        public int HundredEightiesScored { get; set; }

        [DataMember]
        public int CheckoutPossibleCount { get; set; }

        [DataMember]
        public int CheckoutAchievedCount { get; set; }

        public int Handicap { get; set; }

        public Decimal CheckoutPercentage => Math.Round(CheckoutPossibleCount == 0 ? 0 : (Decimal)CheckoutAchievedCount / CheckoutPossibleCount * 100,2);

        public Decimal LegsWonPc => LegsPlayed == 0 ? 0 : Math.Round((Decimal)LegsWon / LegsPlayed * 100,2);

        public string LastPlayed => Date == DateTime.MinValue ? null : Date.ToString(DartsDataSerializerHelper.LongDateFormat);

        public Decimal MatchesWonPc => MatchesPlayed == 0 ? 0 : Math.Round((Decimal)MatchesWon / MatchesPlayed * 100,2);

        public Decimal ThreeDartAverage => Math.Round(OneDartAverage * 3,2);
    }
}
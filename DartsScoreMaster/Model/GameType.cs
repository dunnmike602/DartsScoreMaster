using System.ComponentModel.DataAnnotations;

namespace DartsScoreMaster.Model
{
    public enum GameType
    {
        [Display(Name = "501")] T501 = 0,
        [Display(Name = "401")] T401,
        [Display(Name = "301")] T301,
        [Display(Name = "201")] T201,
        [Display(Name = "101")] T101,
        [Display(Name = "Cricket")] Cricket,
    }
}
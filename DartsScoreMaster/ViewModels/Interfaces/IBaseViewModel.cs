using System;

namespace DartsScoreMaster.ViewModels.Interfaces
{
    public interface IBaseViewModel
    {
        Guid ParentUniqueKey { get; set; }

        void UpdateSizes();
    }
}

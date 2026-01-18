using System;

namespace CrossQuestUI.ViewModels
{
    public abstract class PageViewModelBase : ViewModelBase
    {
        public abstract bool HasNavigation { get; protected set; }
        public abstract bool CanNavigateNext { get; protected set; }
        public abstract bool CanNavigatePrevious { get; protected set; }
    }
}
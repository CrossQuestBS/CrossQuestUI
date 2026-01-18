using System;

namespace CrossQuestUI.ViewModels
{
    public class DashboardViewModel : PageViewModelBase
    {
        public string Title => "Dashboard";
        public string Message => "Game is now modded!";
        
        public override bool HasNavigation
        {
            get => false;
            protected set => throw new NotSupportedException();
        }
        
        public override bool CanNavigateNext
        {
            get => true;
            protected set => throw new NotSupportedException();
        }

        public override bool CanNavigatePrevious
        {
            get => true;
            protected set => throw new NotSupportedException();
        }
    }
}
using System;

namespace CrossQuestUI.ViewModels
{
    public class IntroductionViewModel : PageViewModelBase
    {
        public static string Title => "Welcome to CrossQuest!";
        public static string Message => "Make sure to have Quest connected, and Press \"Next\" to start process.";
        
        public override bool HasNavigation
        {
            get => true;
            protected set => throw new NotSupportedException();
        }
        
        public override bool CanNavigateNext
        {
            get => true;
            protected set => throw new NotSupportedException();
        }

        // You cannot go back from this page
        public override bool CanNavigatePrevious
        {
            get => false;
            protected set => throw new NotSupportedException();
        }
    }
}
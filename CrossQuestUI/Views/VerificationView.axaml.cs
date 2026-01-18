using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CrossQuestUI.ViewModels;

namespace CrossQuestUI.Views
{
    public partial class VerificationView : UserControl
    {
        protected override void OnUnloaded(RoutedEventArgs e)
        {
            base.OnUnloaded(e);

            var vm = (VerificationViewModel)DataContext;
            
            vm?.OnUnload();
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            var vm = (VerificationViewModel)DataContext;
            
            vm?.OnLoad();
        }
        
        public VerificationView()
        {
            InitializeComponent();
        }
    }
}
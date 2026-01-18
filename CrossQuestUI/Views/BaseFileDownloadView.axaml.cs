using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CrossQuestUI.ViewModels;

namespace CrossQuestUI.Views
{
    public partial class BaseFileDownloadView : UserControl
    {
        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);
            var vm = (BaseFileDownloadViewModel)DataContext;
            vm?.OnLoad();
        }

        public BaseFileDownloadView()
        {
            InitializeComponent();
        }
    }
}
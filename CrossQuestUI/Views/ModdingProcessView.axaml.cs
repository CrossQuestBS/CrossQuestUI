using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CrossQuestUI.ViewModels;

namespace CrossQuestUI.Views
{
    public partial class ModdingProcessView : UserControl
    {
        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);
            var vm = (ModdingProcessViewModel)DataContext;
            vm?.OnLoad();
        }

        public ModdingProcessView()
        {
            InitializeComponent();
        }
    }
}
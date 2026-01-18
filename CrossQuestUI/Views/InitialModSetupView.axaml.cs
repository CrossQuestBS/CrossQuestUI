using Avalonia.Controls;
using Avalonia.Interactivity;
using CrossQuestUI.ViewModels;

namespace CrossQuestUI.Views
{
    public partial class InitialModSetupView : UserControl
    {
        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            var vm = (InitialModSetupViewModel)DataContext;
            
            vm?.OnLoad();
        }

        public InitialModSetupView()
        {
            InitializeComponent();
        }

        private void ToggleButton_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
        {
            var vm = (InitialModSetupViewModel)DataContext;
            var checkbox = (CheckBox)sender;
            
            vm.ChangedCheckbox((string)checkbox.Content, checkbox.IsChecked.Value);
        }
    }
}
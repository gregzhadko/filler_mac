using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace UIApp
{
    public class EditingView : Window
    {
        public EditingView(EditingViewModel viewModel)
        {
            this.InitializeComponent();
            this.AttachDevTools();

            DataContext = viewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

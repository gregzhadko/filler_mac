using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace UIApp
{
    public class EditingView : Window
    {
        public EditingView()
        {
            this.InitializeComponent();
            this.AttachDevTools();

            DataContext = new EditingViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

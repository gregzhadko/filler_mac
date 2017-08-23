using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace UIApp
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.AttachDevTools();

            DataContext = new MainViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void HandleClosed()
        {
            ((MainViewModel) DataContext)?.OnClosed();

            base.HandleClosed();
        }
    }
}

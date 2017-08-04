using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace UIApp
{
    public class EditingView : Window
    {
        public EditingView(EditingViewModel viewModel)
        {
            InitializeComponent();
            this.AttachDevTools();

            viewModel.Close += Close;
            DataContext = viewModel;

            Closed += EditingView_Closed;
            
        }

        private void EditingView_Closed(object sender, System.EventArgs e)
        {
            ((EditingViewModel)DataContext).Close -= Close;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

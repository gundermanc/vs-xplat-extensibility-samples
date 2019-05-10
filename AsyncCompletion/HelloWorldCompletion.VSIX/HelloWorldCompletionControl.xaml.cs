namespace HelloWorldCompletion.VSIX
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using HelloWorldCompletion;

    public partial class HelloWorldCompletionControl : UserControl
    {
        public HelloWorldCompletionControl()
        {
            this.InitializeComponent();
        }

        private void DelayButtonClicked(object sender, RoutedEventArgs e)
        {
            var button = (ToggleButton)sender;
            button.IsChecked = true; // It's a radio button, so other buttons will automatically set IsChecked to false

            HelloWorldCompletionSource.Delay = double.Parse(button.Content.ToString());
        }

        private void ActionButtonClicked(object sender, RoutedEventArgs e)
        {
            var button = (ToggleButton)sender;

            switch (button.Content.ToString())
            {
                case "return":
                    HelloWorldCompletionSource.ShouldReturnItems = button.IsChecked.GetValueOrDefault();
                    break;
                case "dismiss":
                    HelloWorldCompletionSource.ShouldDismiss = button.IsChecked.GetValueOrDefault();
                    break;
                default:
                    break;
            }            
        }
    }
}
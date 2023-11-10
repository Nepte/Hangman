using System.Windows;

namespace Hangman
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            HangmanGameWindow gameWindow = new HangmanGameWindow();
            gameWindow.Show();
            this.Close();
        }
    }
}

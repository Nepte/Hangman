using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Hangman
{
    public partial class HangmanGameWindow : Window
    {
        private readonly List<string> wordList = new List<string>
        {
            "POMME", "BANANE", "CERISE", "KIWI", "ANANAS",
            "RAISIN", "MANGUE", "ORANGE", "FRAISE", "PECHE"
        };
        private string currentWord;
        private char[] guessedLetters;
        private int wrongGuesses;
        private const int maxWrongGuesses = 7;

        public HangmanGameWindow()
        {
            InitializeComponent();
            NouvellePartie();
        }

        private void NouvellePartie()
        {
            Random rand = new Random();
            currentWord = wordList[rand.Next(wordList.Count)];
            guessedLetters = new string('_', currentWord.Length).ToCharArray();
            wrongGuesses = 0;
            MettreAJourAffichageMot();
            MettreAJourInfosJeu();
            GenererBoutonsLettres();
        }

        private void MettreAJourAffichageMot()
        {
            wordDisplay.Text = string.Join(" ", guessedLetters);
        }

        private void MettreAJourInfosJeu()
        {
            lettersCountText.Text = $"Lettres restantes: {currentWord.Length}";
            attemptsLeftText.Text = $"Essais restants: {maxWrongGuesses - wrongGuesses}";
        }

        private void GenererBoutonsLettres()
        {
            lettersWrapPanel.Children.Clear();
            foreach (char c in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            {
                var letterButton = new Button
                {
                    Content = c.ToString(),
                    FontSize = 24,
                    Margin = new Thickness(5),
                    Padding = new Thickness(5)
                };
                letterButton.Click += ClicBoutonLettre;
                lettersWrapPanel.Children.Add(letterButton);
            }
        }

        private void ClicBoutonLettre(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            char lettre = char.Parse(button.Content.ToString());
            button.IsEnabled = false;

            TraiterDevinetteLettre(lettre);
        }

        private void TraiterDevinetteLettre(char lettreDevinee)
        {
            bool estCorrect = false;
            for (int i = 0; i < currentWord.Length; i++)
            {
                if (char.ToUpperInvariant(currentWord[i]) == char.ToUpperInvariant(lettreDevinee))
                {
                    guessedLetters[i] = currentWord[i];
                    estCorrect = true;
                }
            }

            if (!estCorrect)
            {
                wrongGuesses++;
                MettreAJourInfosJeu();
                if (wrongGuesses >= maxWrongGuesses)
                {
                    // Utiliser Dispatcher pour permettre à l'interface utilisateur de se mettre à jour
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Dommage, vous avez perdu ! Le mot était : {currentWord}", "Perdu", MessageBoxButton.OK, MessageBoxImage.Information);
                        NouvellePartie();
                    });
                }
            }
            else
            {
                MettreAJourAffichageMot();
                if (!new string(guessedLetters).Contains('_'))
                {
                    MessageBox.Show("Félicitations ! Vous avez deviné le mot !", "Gagné", MessageBoxButton.OK, MessageBoxImage.Information);
                    NouvellePartie();
                }
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            NouvellePartie();
        }
    }
}

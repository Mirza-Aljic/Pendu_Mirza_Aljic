using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Pendu_Mirza_Aljic
{
    public partial class MainWindow : Window
    {
        private string[] mots = { "PROGRAMMATION", "INFORMATIQUE", "PENDU", "JOUEUR", "HANGMAN" };
        private string motSecret;
        private int vies = 0;
        private List<Button> lettreButtons = new List<Button>();
        private Image img; // Ajout d'une variable pour l'image
        private List<string> imagePaths = new List<string>
        {
            "D:\\Mirza ALJIC\\SUBRAMANI\\C#\\Prrojet Jeu Pendu\\Pendu_Mirza_Aljic\\Ressource\\1.png",
            "D:\\Mirza ALJIC\\SUBRAMANI\\C#\\Prrojet Jeu Pendu\\Pendu_Mirza_Aljic\\Ressource\\2.png",
            "D:\\Mirza ALJIC\\SUBRAMANI\\C#\\Prrojet Jeu Pendu\\Pendu_Mirza_Aljic\\Ressource\\3.png",
            "D:\\Mirza ALJIC\\SUBRAMANI\\C#\\Prrojet Jeu Pendu\\Pendu_Mirza_Aljic\\Ressource\\4.png",
            "D:\\Mirza ALJIC\\SUBRAMANI\\C#\\Prrojet Jeu Pendu\\Pendu_Mirza_Aljic\\Ressource\\5.png",
            "D:\\Mirza ALJIC\\SUBRAMANI\\C#\\Prrojet Jeu Pendu\\Pendu_Mirza_Aljic\\Ressource\\6.png",
            "D:\\Mirza ALJIC\\SUBRAMANI\\C#\\Prrojet Jeu Pendu\\Pendu_Mirza_Aljic\\Ressource\\7.png",
        };

        public MainWindow()
        {
            InitializeComponent();

            BTN_Start.Click += StartButton_Click; // Associe le clic du bouton Start à la méthode StartButton_Click
           
            CreateAlphabetButtons(); // Crée les boutons pour chaque lettre
            DisableAlphabetButtons(); // Désactive les boutons du clavier au démarrage
        }

        private void CreateAlphabetButtons()
        {
            // Crée des boutons pour chaque lettre de l'alphabet et ajoute des gestionnaires d'événements
            foreach (Button button in gridKeyboard.Children)
            {
                lettreButtons.Add(button);
                button.Click += KeyButton_Click; // Associe le clic des boutons à la méthode KeyButton_Click
            }
        }

        private void DisableAlphabetButtons()
        {
            // Désactive tous les boutons du clavier
            foreach (Button button in lettreButtons)
            {
                button.IsEnabled = false;
            }
        }

        private void EnableAlphabetButtons()
        {
            // Active tous les boutons du clavier
            foreach (Button button in lettreButtons)
            {
                button.IsEnabled = true;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        public void StartGame()
        {
            // Choix aléatoire d'un mot dans la liste
            Random random = new Random();
            string newMot = mots[random.Next(mots.Length)];

            // Assure que le nouveau mot n'est pas le même que l'ancien
            while (newMot == motSecret)
            {
                newMot = mots[random.Next(mots.Length)];
            }
            motSecret = newMot;

            // Affichage du mot sous forme de pointillés
            TB_Mot.Text = new string('*', motSecret.Length);
            vies = 7; // Réinitialisation du nombre de vies à 7
            TB_Vie.Text = vies.ToString(); // Affichage du nombre de vies

            EnableAlphabetButtons(); // Active les boutons du clavier

 
            // Après avoir instancié l'objet img avec le chemin d'image
            Image.Source = new BitmapImage(new Uri("D:\\Mirza ALJIC\\SUBRAMANI\\C#\\Prrojet Jeu Pendu\\Pendu_Mirza_Aljic\\Ressource\\1.png"));

        }

        private void KeyButton_Click(object sender, RoutedEventArgs e)
        {
            // Gestion du clic des boutons clavier
            Button button = (Button)sender;
            char cont = button.Content.ToString()[0];

            verification(cont); // Vérifie si la lettre est dans le mot
            button.IsEnabled = false; // Désactive le bouton après utilisation
        }
      public void verification(char lettre)
{
    if (motSecret.Contains(lettre))
    {
        MettreAJourMotAvecLettre(lettre); // Met à jour le mot avec la lettre

        if (!TB_Mot.Text.Contains("*"))
        {
            MessageBox.Show("Félicitations, vous avez deviné le mot : " + motSecret);
            DisableAlphabetButtons(); // Désactive les boutons après avoir trouvé le mot
        }
    }
    else
    {
        vies--; // Réduction du nombre de vies
        TB_Vie.Text = vies.ToString(); // Met à jour l'affichage des vies

        // Charger l'image ici
        if (vies >= 0 && vies < imagePaths.Count)
        {
            // Supprimer l'image précédente s'il y en a une
            if (GridKeyboardImage.Children.Count > 1)
            {
                GridKeyboardImage.Children.RemoveAt(1);
            }
            
            img = new Image();
            img.Source = new BitmapImage(new Uri(imagePaths[imagePaths.Count - 1 - vies]));
            // Ajouter la nouvelle image à votre interface
            GridKeyboardImage.Children.Add(img);
            Grid.SetColumn(img, 1); // Pour positionner l'image
            Grid.SetRow(img, 0); // Pour positionner l'image
        }
        
        if (vies == 0) {
            MessageBox.Show("Désolé, vous avez perdu ! Le mot était : " + motSecret);
            DisableAlphabetButtons(); // Désactiver le clavier
        }
    }
}

        private void MettreAJourMotAvecLettre(char lettre)
        {
            // Met à jour le mot avec la lettre trouvée
            StringBuilder nouveauMot = new StringBuilder(TB_Mot.Text);

            for (int i = 0; i < nouveauMot.Length; i++)
            {
                if (motSecret[i] == lettre)
                {
                    nouveauMot[i] = lettre;
                }
            }

            TB_Mot.Text = nouveauMot.ToString();
        }

        private void BTN_Indice_Click(object sender, RoutedEventArgs e)
        {
            // Affiche un message d'indice en fonction du mot
            if (!string.IsNullOrEmpty(motSecret))
            {
                string message = string.Empty;

                if (motSecret == "PROGRAMMATION")
                {
                    message = "C'est un terme lié à l'informatique.";
                }
                else if (motSecret == "INFORMATIQUE")
                {
                    message = "Cela concerne le traitement de l'information.";
                }
                else if (motSecret == "PENDU")
                {
                    message = "Le jeu que vous jouez en ce moment.";
                }
                else if (motSecret == "JOUEUR")
                {
                    message = "Vous êtes le joueur actuel.";
                }
                else if (motSecret == "HANGMAN")
                {
                    message = "Un autre nom pour le jeu du pendu.";
                }
                else
                {
                    message = "Aucun message disponible pour ce mot.";
                }

                MessageBox.Show(message); // Affiche le message
            }
            else
            {
                MessageBox.Show("Veuillez commencer le jeu en appuyant sur le bouton 'Start'.");
            }
        }

        private void BTN_Continue_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
            //ChangeMotSecret(); // Change le mot secret
            //EnableAlphabetButtons(); // Active les boutons du clavier
            //TB_Mot.Text = new string('*', motSecret.Length); // Réinitialise l'affichage du mot
            //vies = 7; // Réinitialisation du nombre de vies à 7
            //TB_Vie.Text = vies.ToString(); // Affichage du nombre de vies

        }

        private void ChangeMotSecret()
        {
            // Change le mot secret en choisissant un nouveau mot aléatoire
            Random random = new Random();
            string newMot = mots[random.Next(mots.Length)];

            while (newMot == motSecret)
            {
                newMot = mots[random.Next(mots.Length)];
            }
            motSecret = newMot;
        }
    }
}

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System;
using System.Text; // Ajout de l'espace de noms pour StringBuilder
using System.IO.Packaging;
using System.Windows.Threading;

namespace Pendu_Mirza_Aljic
{
    public partial class MainWindow : Window
    {
        // Liste de mots pour le jeu du pendu
        private string[] mots = { "PROGRAMMATION", "INFORMATIQUE", "PENDU", "JOUEUR", "HANGMAN" };
        private string motSecret; // Variable qui sert à stocker le mot que doit deviner le joueur
        private int vies = 0; // Modifier la valeur initiale de vies à 0
        private List<Button> lettreButtons = new List<Button>(); // Liste pour stocker les boutons du clavier. Cela permet de les activer ou désactiver

        public MainWindow()
        {
            InitializeComponent();
            BTN_Start.Click += StartButton_Click;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(120);
            timer.Tick += timer_Tick;
            timer.Start();

            CreateAlphabetButtons();

            // Désactivez les boutons du clavier au départ
            DisableAlphabetButtons();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            LB_Timer.Content = DateTime.Now.ToLongTimeString();
        }
        private void CreateAlphabetButtons()
        {
            // Crée des boutons pour chaque lettre de l'alphabet et ajoute des gestionnaires d'événements
            foreach (Button button in gridKeyboard.Children)
            {
                lettreButtons.Add(button);
                button.Click += KeyButton_Click;
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
            // Choix aléatoire d'un mot parmi la liste
            Random random = new Random();
            motSecret = mots[random.Next(mots.Length)];

            // Affichage du mot sous forme de pointillés avec des espaces entre les lettres
             TB_Mot.Text = new string('*', motSecret.Length );


            // Réinitialisation du nombre de vies
            vies = 7; // Réinitialisation du nombre de vies à 7
            TB_Vie.Text = vies.ToString(); // Affichage du nombre de vies

            // Activation des boutons du clavier
            EnableAlphabetButtons();
        }

        private void KeyButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            char cont = button.Content.ToString()[0];

            verification(cont);
            // Désactivez le bouton du clavier après utilisation
            button.IsEnabled = false;
        }

        public void verification(char lettre )
        {

            if (motSecret.Contains(lettre))
            {
                MettreAJourMotAvecLettre(lettre); // Utilisation de la nouvelle fonction

                // Vérifiez si le joueur a gagné
                if (!TB_Mot.Text.Contains("*"))
                {
                    MessageBox.Show("Félicitations, vous avez deviné le mot : " + motSecret);
                    DisableAlphabetButtons();
                }
            }
            else
            {
                // Réduction du nombre de vies
                vies--;
                TB_Vie.Text = vies.ToString();

                // Vérifiez si le joueur a perdu
                if (vies == 0)
                {
                    MessageBox.Show("Désolé, vous avez perdu. Le mot était : " + motSecret);
                    DisableAlphabetButtons();
                }


            }
        }

        private void MettreAJourMotAvecLettre(char lettre)
        {
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
    }
}

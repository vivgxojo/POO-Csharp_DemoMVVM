using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace DemoMVVM
{
    // Créer un ViewModel; il doit implémenter INotifyPropertyChanged 
    // pour aviser l'interface utilisateur des changements
    public class MonViewModel : INotifyPropertyChanged
    {
        //C'est l'évènement nécessaire pour implémenter INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        //ObservableCollection permet d'utiliser le binding
        public ObservableCollection<Personne> ListePersonnes { get; set; }
        //Objet courrant sélectionné, avec encapsulation
        private Personne _selectedPersonne;
        public Personne PersonneSelectionnee
        {
            get { return _selectedPersonne; }
            set { _selectedPersonne = value; 
                OnPropertyChanged(nameof(PersonneSelectionnee)); }
        }

        // Propriétés pour la paginations 
        private int _currentIndex; //indice de la personne sélectionnée
        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                if (_currentIndex != value && value >= 0 && value < ListePersonnes.Count)
                {
                    _currentIndex = value;
                    OnPropertyChanged(nameof(CurrentIndex));
                    PersonneSelectionnee = ListePersonnes[_currentIndex]; // Synchroniser la sélection
                    OnPropertyChanged(nameof(CanGoPrevious)); // Mettre à jour les boutons
                    OnPropertyChanged(nameof(CanGoNext));
                }
            }
        }

        public ICommand PreviousCommand { get; } //commandes pour les boutons
        public ICommand NextCommand { get; }

        public MonViewModel()
        {   //Initialiser les propriétés du ViewModel avec des valeurs qui seront affichées 
            ListePersonnes = new ObservableCollection<Personne>
            {
                new Personne { Nom = "Alice", Age = 25 }, // Utilise le constructeur
                new Personne { Nom = "Bob", Age = 30 },     // par défaut
                new Personne { Nom = "Charlie", Age = 35 }
            };
            PersonneSelectionnee = ListePersonnes[0];
            
            _currentIndex = 0; //initialiser les propriétés et les commandes
            PreviousCommand = new RelayCommand(Previous, CanGoPrevious);
            NextCommand = new RelayCommand(Next, CanGoNext);

        }

        private void Previous()
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                PersonneSelectionnee = ListePersonnes[_currentIndex];
            }

            try
            {
                string fichier = "fichier.json";
                string json = File.ReadAllText(fichier);
                Personne personne = JsonSerializer.Deserialize<Personne>(json);
            }
            catch (Exception ex) { 
                MessageBox.Show(ex.Message);
            }
        }
        private bool CanGoPrevious() => _currentIndex > 0;

        private void Next()
        {
            if (_currentIndex < ListePersonnes.Count - 1)
            {
                _currentIndex++;
                PersonneSelectionnee = ListePersonnes[_currentIndex];
            }

            string fichier = "fichier.json";
            Personne personne = new Personne { Nom = "Justine", Age = 25 };
            string json = JsonSerializer.Serialize(personne);
            File.WriteAllText(fichier, json);
            
        }
        private bool CanGoNext() => _currentIndex < ListePersonnes.Count - 1;


        protected void OnPropertyChanged(string propertyName)
        {
            // Déclencher l'évènement pour informer l'interface utilisateur d'un changement
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
       
    }
}

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

        private string _msg;
        public string Message
        {
            get { return _msg; }
            set
            {
                _msg = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        //ObservableCollection permet d'utiliser le binding
        private ObservableCollection<Personne> _listePersonnes;
        public ObservableCollection<Personne> ListePersonnes 
        { 
            get { return _listePersonnes; }
            set { _listePersonnes = value;
                //Mettre à jour l'interface utilisateur
                OnPropertyChanged(nameof(ListePersonnes));
            } 
        }
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

        public ICommand OuvrirCommand { get; }
        public ICommand EnregistrerCommand { get; }

        public MonViewModel()
        {   //Initialiser les propriétés du ViewModel avec des valeurs qui seront affichées 
            ListePersonnes = new ObservableCollection<Personne>
            {
                new Personne { Nom = "Alice", Age = 25 }, // Utilise le constructeur
                new Personne { Nom = "Bob", Age = 30 },     // par défaut
                new Personne { Nom = "Charlie", Age = 35 }
            };
            PersonneSelectionnee = ListePersonnes[0];
            Message = "Status";
            _currentIndex = 0; //initialiser les propriétés et les commandes
            PreviousCommand = new RelayCommand(Previous, CanGoPrevious);
            NextCommand = new RelayCommand(Next, CanGoNext);
            OuvrirCommand = new RelayCommand(Ouvrir);
            EnregistrerCommand = new RelayCommand(Enregistrer);
        }

        private void Ouvrir() 
        {
            //Désérialiser
            try
            {
                /*
                string fichier = "fichier.json";
                string json = File.ReadAllText(fichier);
                // Deserialize utilise le constructeur par défaut (vide)
                Personne personne = JsonSerializer.Deserialize<Personne>(json);
                MessageBox.Show(personne.ToString()); 
                */
                string file = "listePersonnes.json";
                string listeJson = File.ReadAllText(file);
                ListePersonnes = JsonSerializer.Deserialize<ObservableCollection<Personne>>(listeJson);
                Message = "Fichier ouvert";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
        private void Enregistrer() 
        {
            //Sérialisation 
            /*
            string fichier = "fichier.json";
            Personne personne = new Personne { Nom = "Justine", Age = 25 };
            string json = JsonSerializer.Serialize(personne);
            File.WriteAllText(fichier, json);
            MessageBox.Show("Fichier sauvegardé."); 
            */
            string file = "listePersonnes.json";
            string listeJson = JsonSerializer.Serialize(ListePersonnes);
            File.WriteAllText(file, listeJson);
            Message = "Liste sauvegardée.";
            
        }

        private void Previous()
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                PersonneSelectionnee = ListePersonnes[_currentIndex];
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
            
        }
        private bool CanGoNext() => _currentIndex < ListePersonnes.Count - 1;


        protected void OnPropertyChanged(string propertyName)
        {
            // Déclencher l'évènement pour informer l'interface utilisateur d'un changement
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
       
    }
}

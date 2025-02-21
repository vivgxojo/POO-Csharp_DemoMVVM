using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMVVM
{
    public class MonViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Personne> People { get; set; }
        private Personne _selectedPersonne;
        public Personne SelectedPersonne
        {
            get { return _selectedPersonne; }
            set { _selectedPersonne = value; OnPropertyChanged(nameof(SelectedPersonne)); }
        }

        public MonViewModel()
        {
            People = new ObservableCollection<Personne>
            {
                new Personne { Nom = "Alice", Age = 25 },
                new Personne { Nom = "Bob", Age = 30 },
                new Personne { Nom = "Charlie", Age = 35 }
            };
            SelectedPersonne = People[0];
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

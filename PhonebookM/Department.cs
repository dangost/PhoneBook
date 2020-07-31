using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace PhonebookM
{
    public class Departament : INotifyPropertyChanged
    {
        private int id;
        private string name;

        public int Id
        {
            get { return id; }

            set
            {
                id = value;
                OnPropertyChanged("id");
            }
        }

        public string Department
        {
            get { return name; }

            set
            {
                name = value;
                OnPropertyChanged("name");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

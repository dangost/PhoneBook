using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PhonebookM
{
    public class ContactModel : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string surname;
        private string number;
        private string email;
        private string department;

        public int Id
        {
            get { return id; }

            set { id = value; OnPropertyChanged("id"); }
        }

        public string Name
        {
            get { return name; }

            set { name = value; OnPropertyChanged("name"); }
        }

        public string Surname
        {
            get { return surname; }

            set { surname = value; OnPropertyChanged("surname"); }
        }


        public string Number
        {
            get { return number; }

            set { number = value; OnPropertyChanged("number"); }
        }

        public string Email
        {
            get { return email; }

            set { email = value; OnPropertyChanged("email"); }
        }

        public string Department
        {
            get { return department; }

            set { department = value; OnPropertyChanged("department"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

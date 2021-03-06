﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Domain
{
    public class Contact : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string surname;
        private string number;
        private string email;
        private int depId; 

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

        public int DepId
        {
            get { return depId; }

            set { depId = value; OnPropertyChanged("depId"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

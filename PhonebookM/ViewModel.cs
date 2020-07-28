using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace PhonebookM
{
    public class ViewModel : INotifyPropertyChanged
    {
        public string Form;
        public IPhoneBook PhoneBook = new JsonPhoneBook();
        private readonly IMainView _view;

        public ViewModel(IMainView view)
        {
            _view = view;

            PhoneBook.Load();

            contacts = PhoneBook.GetAll();
        }

        public Contact selectedContact;

        public ObservableCollection<Contact> contacts { get; set; }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(obj =>
                    {
                        Form = "add";
                        
                        AddEditViewModel viewModel = new AddEditViewModel(this);
                        
                        _view.ShowAddEditDialog(this, viewModel);
                        
                    }));
            }
        }

        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                    (removeCommand = new RelayCommand(obj =>
                    {
                        Contact contact = obj as Contact;                        
                        {
                            if (selectedContact != null)
                                if (MainWindow.DeleteWarning())
                                {
                                    PhoneBook.Delete(selectedContact);
                                    contacts.Remove(selectedContact);
                                    PhoneBook.UpdateList(contacts);
                                }
                        }
                    },
                    (obj) => contacts.Count > 0));
            }
        }


        private RelayCommand editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                    (editCommand = new RelayCommand(obj =>
                    {
                        if (selectedContact != null)
                        {
                            Form = "edit";
                            AddEditViewModel viewModel = new AddEditViewModel(this);
                            _view.ShowAddEditDialog(this, viewModel);
                        }
                    }));
            }
        }


        public Contact SelectedContact
        {
            get { return selectedContact; }
            set
            {
                selectedContact = value;
                OnPropertyChanged("selectedContact");
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
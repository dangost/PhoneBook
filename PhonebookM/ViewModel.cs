using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace PhonebookM
{
    public class ViewModel : INotifyPropertyChanged
    {
        public string Form;
        public IPhoneBook PhoneBook = new SQlitePhoneBook();
        private readonly IMainView _view;

        public ViewModel(IMainView view)
        {
            _view = view;

            PhoneBook.Load();

            contacts = PhoneBook.GetAll();
        }

        private Contact selectedContact;

        private ObservableCollection<Contact> contacts { get; set; }

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
                            if (SelectedContact != null)
                                if (MainWindow.DeleteWarning())
                                {
                                    PhoneBook.Delete(SelectedContact);
                                    contacts.Remove(SelectedContact);
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
                        if (SelectedContact != null)
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
            get { return SelectedContact; }
            set
            {
                SelectedContact = value;
                OnPropertyChanged("SelectedContact");
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
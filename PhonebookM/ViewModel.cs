using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

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

            contacts = PhoneBook.GetAllContacts();
            departments = PhoneBook.GetAllDepartments();
            ContactsModel = PhoneBook.GetAllContactsModels();
        }
        

        public Contact selectedContact;
        public Department selectedDepartment;
        public ContactModel selectedContactModel;

        public ObservableCollection<Contact> contacts { get; set; }
        public ObservableCollection<Department> departments { get; set; }

        public ObservableCollection<ContactModel> contactModel;
        public ObservableCollection<ContactModel> ContactsModel
        {
            get { return contactModel; }
            set
            {
                contactModel = value;
                OnPropertyChanged("contactsModel");
            }
        } 


        public ContactModel ContactToContactModel(Contact contact)
        {
            ContactModel contactModel = new ContactModel();

            contactModel.Id = contact.Id;
            contactModel.Name = contact.Name;
            contactModel.Surname = contact.Surname;
            contactModel.Number = contact.Number;
            contactModel.Email = contact.Email;

            foreach (var d in departments)
            {
                if(contact.DepId == d.Id)
                {
                    contactModel.Department = d.Name;
                    break;
                }
            }

            return contactModel;
        }

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
                        PhoneBook.UpdateContactsModel(contacts);
                        ContactsModel = PhoneBook.GetAllContactsModels();
                        
                    }));
            }
        }

        private RelayCommand addDepartment;
        public RelayCommand AddDepartment
        {
            get
            {
                return addDepartment ??
                    (addDepartment = new RelayCommand(obj =>
                    {
                        Form = "addDep";

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
                            if (selectedContactModel != null)
                                if (MainWindow.DeleteWarning())
                                {
                                    foreach (var c in contacts)
                                    {
                                        if(c.Id == SelectedContactModel.Id)
                                        {
                                            selectedContact = c;
                                        }
                                    }
                                    PhoneBook.Delete(selectedContact);
                                    contacts.Remove(selectedContact);
                                    PhoneBook.UpdateList(contacts);

                                    PhoneBook.Delete(selectedContactModel);
                                    ContactsModel.Remove(selectedContactModel);
                                    PhoneBook.UpdateContactsModel(contacts);
                                }
                        }
                    },
                    (obj) => contacts.Count > 0));
            }
        }

        private RelayCommand removeDepartment;
        public RelayCommand RemoveDepartment
        {
            get
            {
                return removeDepartment ??
                    (removeDepartment = new RelayCommand(obj =>
                    {
                        Department department = obj as Department;
                        {
                            if (selectedDepartment != null)
                                if (MainWindow.DeleteWarning())
                                {
                                    PhoneBook.Delete(selectedDepartment);
                                    departments.Remove(selectedDepartment);
                                    PhoneBook.UpdateList(departments);
                                }
                        }
                    },
                    (obj) => departments.Count > 0));
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
                        if (selectedContactModel != null)
                        {
                            ContactModel temp = selectedContactModel;
                            Form = "edit";
                            AddEditViewModel viewModel = new AddEditViewModel(this);

                            foreach (var c in contacts)
                            {
                                if (c.Id == temp.Id)
                                {
                                    SelectedContact = c;
                                }
                            }
                            _view.ShowAddEditDialog(this, viewModel);
                            
                                                       

                            contacts = PhoneBook.GetAllContacts();
                            departments = PhoneBook.GetAllDepartments();
                            PhoneBook.UpdateContactsModel(PhoneBook.GetAllContacts());
                            ContactsModel = PhoneBook.GetAllContactsModels();
                            ContactsModel = PhoneBook.GetAllContactsModels();
                        }
                    }));
            }
        }

        private RelayCommand editDepartment;
        public RelayCommand EditDepartment
        {
            get
            {
                return editDepartment ??
                    (editDepartment = new RelayCommand(obj =>
                    {
                        if (selectedDepartment != null)
                        {
                            Form = "editDep";
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

        public ContactModel SelectedContactModel
        {
            get { return selectedContactModel; }
            set
            {
                selectedContactModel = value;
                OnPropertyChanged("selectedContactModel");
            }
        }

        public Department SelectedDepartment
        {
            get { return selectedDepartment; }
            set
            {
                selectedDepartment = value;
                OnPropertyChanged("selectedDepartment");
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
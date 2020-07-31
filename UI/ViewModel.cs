using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System;
using Domain;
using Infrastructure;

namespace UI
{
    public class ViewModel : INotifyPropertyChanged
    {
        public string Form;
        public IPhoneBook PhoneBook = new XMLphoneBook();
        private readonly IMainView _view;

        public ViewModel(IMainView view)
        {
            _view = view;

            PhoneBook.Load();

            contacts = PhoneBook.GetAllContacts();
            Departments = PhoneBook.GetAllDepartments();
            ContactsModel = PhoneBook.GetAllContactsModels();
        }
        

        public Contact selectedContact;
        public Departament selectedDepartment;
        public ContactModel selectedContactModel;

        public ObservableCollection<Contact> contacts { get; set; }
        
        public ObservableCollection<Departament> departaments;
        public ObservableCollection<Departament> Departments
        {
            get { return departaments; }
            set
            {
                departaments = value;
                OnPropertyChanged("departaments");
            }
        }

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

            foreach (var d in Departments)
            {
                if(contact.DepId == d.Id)
                {
                    contactModel.Department = d.Department;
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
                        Departament department = obj as Departament;
                        {
                            if (selectedDepartment != null)
                                if (MainWindow.DeleteWarning())
                                {
                                    PhoneBook.Delete(selectedDepartment);
                                    Departments.Remove(selectedDepartment);
                                    PhoneBook.UpdateList(Departments);
                                }
                        }
                    },
                    (obj) => Departments.Count > 0));
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
                            Departments = PhoneBook.GetAllDepartments();
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

        public Departament SelectedDepartment
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

        public void ContactsSearch(string l, string form)
        {
            if (form == "dep")
            {
                PhoneBook.UpdateContactsModel(contacts);
                ContactsModel = PhoneBook.GetAllContactsModels();

                if(l.Length == 0)
                {
                    return;
                }

                else
                {
                    ObservableCollection<ContactModel> temp = new ObservableCollection<ContactModel> { };

                    foreach(var c in ContactsModel)
                    {
                        if (Convert.ToString(c.Id).IndexOf(l) != -1 || c.Name.ToLower().IndexOf(l.ToLower()) != -1 || c.Surname.ToLower().IndexOf(l.ToLower()) != -1 || c.Number.ToLower().IndexOf(l.ToLower()) != -1 || c.Email.ToLower().IndexOf(l.ToLower()) != -1 || c.Department.ToLower().IndexOf(l.ToLower()) != -1)
                        {
                            temp.Add(c);
                        }
                        else continue;
                    }

                    ContactsModel = temp;
                }

            }

            else if (form == "contact")
            {
                Departments = PhoneBook.GetAllDepartments();

                if (l.Length == 0)
                {
                    return;
                }

                else
                {
                    ObservableCollection<Departament> temp = new ObservableCollection<Departament> { };

                    foreach (var d in Departments)
                    {
                        if (Convert.ToString(d.Id).IndexOf(l) != -1 || d.Department.ToLower().IndexOf(l.ToLower()) != -1)
                        {
                            temp.Add(d);
                        }
                        else continue;
                    }

                    Departments = temp;
                }
            }
        }
    }
}
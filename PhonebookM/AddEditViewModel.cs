using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Controls;
using Domain;

namespace PhonebookM
{
    public class AddEditViewModel : INotifyPropertyChanged
    {
        ComboBox box;
        List<TextBox> textBoxes;
        IAddEditView dialog;
        public ViewModel mainModel;
        IPhoneBook pbook;
        string form;

        public Contact selectedContact;
        public Departament selectedDepartment;
        public ContactModel selectedContactModel;

        ObservableCollection<Departament> departments { get; set; }
        public List<string> depNames { get; set; }

        public void CreateInterface(IAddEditView addEditView)
        {
            dialog = addEditView;
        }

        public AddEditViewModel(ViewModel viewModel)
        {  
            mainModel = viewModel;        
            
            pbook = viewModel.PhoneBook;            
            form = viewModel.Form;
            selectedContact = viewModel.SelectedContact;
            selectedDepartment = viewModel.SelectedDepartment;
            selectedContactModel = viewModel.SelectedContactModel;

            departments = pbook.GetAllDepartments();

            
        }

        private RelayCommand createCommand;
        public RelayCommand CreateCommand
        {
            get
            {
                return createCommand ??
                    (createCommand = new RelayCommand(obj =>
                    {
                        var deps = pbook.GetAllDepartments();
                        box = dialog.GetBox();
                        textBoxes = dialog.GetTextBoxes();
                        if (form == "add")
                        {
                            Departament depart = new Departament();

                            Contact contact = new Contact();
                            ContactModel contactModel = new ContactModel();

                            contact.Id = pbook.CreateId("contact");

                            if (textBoxes[0].Text != "" || textBoxes[1].Text != "" || textBoxes[2].Text != "" || textBoxes[3].Text != "" || box.Text != "")
                            {
                                contact.Name = textBoxes[0].Text;
                                contact.Surname = textBoxes[1].Text;
                                contact.Number = textBoxes[2].Text;
                                contact.Email = textBoxes[3].Text;

                                string depName = box.Text;


                                foreach(var d in pbook.GetAllDepartments())
                                {
                                    if(depName == d.Department)
                                    {
                                        contact.DepId = d.Id;
                                        break;
                                    }
                                }
                                pbook.Add(contact);
                                pbook.Add(contactModel);                                
                                pbook.Update();                                
                                dialog.CloseDialog();

                            }

                            else
                            {
                                dialog.Warning("Fill the all rows");
                            }
                        }

                        else if (form == "edit")
                        {
                            ObservableCollection<Contact> updatedContacts = pbook.GetAllContacts();
                            ObservableCollection<ContactModel> updatedContacModel = pbook.GetAllContactsModels();

                            if (textBoxes[0].Text != "" || textBoxes[1].Text != "" || textBoxes[2].Text != "" || textBoxes[3].Text != "")
                            {
                                
                                for (int i = 0; i < pbook.GetAllContacts().Count; i++)
                                {
                                    if (updatedContacts[i].Id == selectedContactModel.Id)
                                    {
                                        updatedContacts[i].Name = textBoxes[0].Text;
                                        updatedContacts[i].Surname = textBoxes[1].Text;
                                        updatedContacts[i].Number = textBoxes[2].Text;
                                        updatedContacts[i].Email = textBoxes[3].Text;

                                        string depName = box.Text;

                                        foreach (var d in pbook.GetAllDepartments())
                                        {
                                            if (depName == d.Department)
                                            {
                                                updatedContacts[i].DepId = d.Id;
                                                break;
                                            }
                                        }

                                        
                                        pbook.UpdateList(updatedContacts);
                                        
                                        pbook.Update();
                                        dialog.CloseDialog();
                                    }
                                }
                            }

                            else
                            {
                                dialog.Warning("Fill the all rows");
                            }
                        }

                        else if (form == "addDep")
                        {
                            Departament department = new Departament();
                            department.Id = pbook.CreateId("dep");

                            if (textBoxes[0].Text != "")
                            {
                                foreach (var d in pbook.GetAllDepartments())
                                {
                                    if(d.Department == textBoxes[0].Text)
                                    {
                                        dialog.Warning("You cannot create departments with the same names");
                                        return;
                                    }
                                }

                                department.Department = textBoxes[0].Text;

                                pbook.Add(department);
                                pbook.Update();
                                dialog.CloseDialog();
                            }

                            else
                            {
                                dialog.Warning("Fill the all rows");
                            }
                        }

                        else if (form == "editDep")
                        {
                            ObservableCollection<Departament> updatedDepartments = pbook.GetAllDepartments();
                            if (textBoxes[0].Text != "")
                            {
                                for (int i = 0; i < pbook.GetAllDepartments().Count; i++)
                                {
                                    if (updatedDepartments[i].Id == selectedDepartment.Id)
                                    {
                                        updatedDepartments[i].Department = textBoxes[0].Text;

                                        pbook.Update();
                                        dialog.CloseDialog();
                                    }
                                }
                            }

                            else
                            {
                                dialog.Warning("Fill the all rows");
                            }
                        }

                       

                    }));
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

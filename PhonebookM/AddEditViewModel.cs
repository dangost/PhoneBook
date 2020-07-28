using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.IO;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PhonebookM
{
    public class AddEditViewModel : INotifyPropertyChanged
    {
        List<TextBox> textBoxes;
        IAddEditView dialog;
        public ViewModel mainModel;
        IPhoneBook pbook;
        string form;
        Contact selectedContact;

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
        }

        private RelayCommand createCommand;
        public RelayCommand CreateCommand
        {
            get
            {
                return createCommand ??
                    (createCommand = new RelayCommand(obj =>
                    {
                        textBoxes = dialog.GetTextBoxes();
                        if (form == "add")
                        {
                            Contact contact = new Contact();
                            contact.Id = pbook.CreateId();                            

                            if (textBoxes[0].Text != "" || textBoxes[1].Text != "" || textBoxes[2].Text != "" || textBoxes[3].Text != "")
                            {
                                contact.Name = textBoxes[0].Text;
                                contact.Surname = textBoxes[1].Text;
                                contact.Number = textBoxes[2].Text;
                                contact.Email = textBoxes[3].Text;

                                pbook.Add(contact);
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
                            ObservableCollection<Contact> updatedContacts = pbook.GetAll();
                            if (textBoxes[0].Text != "" || textBoxes[1].Text != "" || textBoxes[2].Text != "" || textBoxes[3].Text != "")
                            {
                                for (int i = 0; i < pbook.GetAll().Count; i++)
                                {
                                    if (updatedContacts[i].Id == selectedContact.Id)
                                    {
                                        updatedContacts[i].Name = textBoxes[0].Text;
                                        updatedContacts[i].Surname = textBoxes[1].Text;
                                        updatedContacts[i].Number = textBoxes[2].Text;
                                        updatedContacts[i].Email = textBoxes[3].Text;
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

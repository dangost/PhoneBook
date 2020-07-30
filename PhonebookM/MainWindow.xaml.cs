using System.Windows;
using System.Collections.Generic;
using System;

namespace PhonebookM
{
    public partial class MainWindow : Window, IMainView
    {
        Contact selectedContact;
        Department selectedDepartment;
        ContactModel selectedContactModel;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ViewModel(this);
        }

        public static bool DeleteWarning()
        {
            bool res;

            if (MessageBox.Show("Are you sure to delete this contact?", "Warning", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                res = true;
            }

            else res = false;

            return res;
        }

        public static void Warning(string line)
        {
            MessageBox.Show(line);
        }

        public void Refresh()
        {
            contactsGrid.Items.Refresh();
        }


        void IMainView.ShowAddEditDialog(ViewModel viewModel, AddEditViewModel addEditView)
        {
            selectedContact = addEditView.selectedContact;
            selectedDepartment = addEditView.selectedDepartment;
            selectedContactModel = addEditView.selectedContactModel;

            if (viewModel.Form == "add")
            {
                AddEditDialog addDialog = new AddEditDialog(addEditView);
                addDialog.Title = "Create";
                addDialog.button.Content = "Create";

                List<string> depsNames = new List<string> { };
               
                foreach (var d in viewModel.PhoneBook.GetAllDepartments())
                {
                    depsNames.Add(d.Name);
                }
                addDialog.comboBoxDepartment.ItemsSource = depsNames;
                addDialog.ShowDialog();
            }

            else if (viewModel.Form == "addDep")
            {
                AddEditDepartament addDepDialog = new AddEditDepartament(addEditView);
                addDepDialog.Title = "Create";
                addDepDialog.button.Content = "Create";


                addDepDialog.ShowDialog();
            }

            else if (viewModel.Form == "edit")
            {
                AddEditDialog editDialog = new AddEditDialog(addEditView);
                editDialog.Title = "Edit";
                editDialog.button.Content = "Save";                
                
                editDialog.textBoxName.Text = selectedContactModel.Name;
                editDialog.textBoxSurname.Text = selectedContactModel.Surname;
                editDialog.textBoxNumber.Text = selectedContactModel.Number;
                editDialog.textBoxEmail.Text = selectedContactModel.Email;
                editDialog.button.IsEnabled = false;

                List<string> depsNames = new List<string> { };

                foreach (var d in viewModel.PhoneBook.GetAllDepartments())
                {
                    depsNames.Add(d.Name);
                }
                editDialog.comboBoxDepartment.ItemsSource = depsNames;

                editDialog.comboBoxDepartment.Text = selectedContactModel.Department;
                /*
                foreach (var d in viewModel.PhoneBook.GetAllDepartments())
                {
                    if(selectedContact.DepId == d.Id)
                    {
                        editDialog.comboBoxDepartment.Text = d.Name;
                        break;
                    }
                }*/
                
                editDialog.ShowDialog();
            }

            else if (viewModel.Form == "editDep")
            {
                AddEditDepartament editDepDialog = new AddEditDepartament(addEditView);
                editDepDialog.Title = "Edit";
                editDepDialog.button.Content = "Save";

                editDepDialog.textBoxName.Text = selectedDepartment.Name;

                editDepDialog.ShowDialog();
            }
        }
    }
}

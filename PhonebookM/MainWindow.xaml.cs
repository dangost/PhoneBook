using System.Windows;

namespace PhonebookM
{
    public partial class MainWindow : Window, IMainView
    {
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

        void IMainView.ShowAddEditDialog(ViewModel viewModel, AddEditViewModel addEditView)
        {
            if (viewModel.Form == "add")
            {
                AddEditDialog addDialog = new AddEditDialog(addEditView);
                addDialog.Title = "Create";
                addDialog.button.Content = "Create";
                addDialog.ShowDialog();
            }

            else if (viewModel.Form == "edit")
            {
                AddEditDialog editDialog = new AddEditDialog(addEditView);
                editDialog.Title = "Edit";
                editDialog.button.Content = "Save";                

                editDialog.textBoxName.Text = viewModel.SelectedContact.Name;
                editDialog.textBoxSurname.Text = viewModel.SelectedContact.Surname;
                editDialog.textBoxNumber.Text = viewModel.SelectedContact.Number;
                editDialog.textBoxEmail.Text = viewModel.SelectedContact.Email;
                editDialog.button.IsEnabled = false;

                editDialog.ShowDialog();
            }
        }
    }
}

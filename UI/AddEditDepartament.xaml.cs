using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace UI
{
    public partial class AddEditDepartament : Window, IAddEditView
    {
        List<TextBox> boxes;
        public AddEditDepartament(AddEditViewModel viewModel)
        {
            boxes = new List<TextBox> { };
            InitializeComponent();

            viewModel.CreateInterface(this);

            DataContext = viewModel;
        }

        public void Warning(string line)
        {
            MessageBox.Show(line);
        }

        public List<TextBox> GetTextBoxes()
        {
            boxes.Clear();

            boxes.Add(textBoxName);

            return boxes;
        }

        public void CloseDialog()
        {
            Close();
        }

        public ComboBox GetBox()
        {
            return null;
        }

        public Button GetButton()
        {
            return button;
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            button.IsEnabled = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhonebookM
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

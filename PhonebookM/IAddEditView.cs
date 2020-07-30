using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace PhonebookM
{
    public interface IAddEditView
    {
        List<TextBox> GetTextBoxes();

        void Warning(string line);

        Button GetButton();

        ComboBox GetBox();

        void CloseDialog();
    }
}

using System.Collections.Generic;
using System.Windows.Controls;

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

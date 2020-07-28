using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookM
{
    public interface IMainView
    {
        void ShowAddEditDialog(ViewModel viewModel, AddEditViewModel addEditViewModel);
    }
}

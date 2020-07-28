using System.Collections.ObjectModel;

namespace PhonebookM
{
    public interface IPhoneBook
    {
        void Load();

        ObservableCollection<Contact> GetAll();

        void Delete(Contact SelectedItem);

        void Update();

        void Add(Contact contact);

        int CreateId();

        void UpdateList(ObservableCollection<Contact> newContacts);       
    }
}

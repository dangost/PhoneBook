using System.Collections.ObjectModel;

namespace PhonebookM
{
    public interface IPhoneBook
    {
        void Load();

        ObservableCollection<Contact> GetAllContacts();
        ObservableCollection<Department> GetAllDepartments();
        ObservableCollection<ContactModel> GetAllContactsModels();

        void Delete(Contact SelectedItem);
        bool Delete(Department SelectedItem);
        void Delete(ContactModel contactModel);

        void Update();

        void Add(Contact contact);
        void Add(Department department);
        void Add(ContactModel contactModel);

        void UpdateContactsModel(ObservableCollection<Contact> contacts);

        int CreateId(string form);

        void UpdateList(ObservableCollection<Contact> newContacts);
        void UpdateList(ObservableCollection<Department> newDepartment);
        void UpdateList(ObservableCollection<ContactModel> newContactsModel);
    }
}

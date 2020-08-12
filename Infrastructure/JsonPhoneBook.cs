using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Domain;

namespace Infrastructure
{
    public class JsonPhoneBook : IPhoneBook
    {
        private ObservableCollection<Contact> contacts = new ObservableCollection<Contact> { };
        private ObservableCollection<Departament> departments = new ObservableCollection<Departament> { };
        private ObservableCollection<ContactModel> contactsModel = new ObservableCollection<ContactModel> { };

        const string jsonPath = @"json\contactsList.json";
        const string jsonDepPath = @"json\departmentsList.json";

        public ObservableCollection<Contact> GetAllContacts()
        {
            return contacts;
        }

        public ObservableCollection<Departament> GetAllDepartments()
        {
            int a;
            a = 5; // delete me
            return departments;
        }

        public ObservableCollection<ContactModel> GetAllContactsModels()
        {
            return contactsModel;
        }

        public void Load()
        {
            if (!Directory.Exists("json"))
            {
                Directory.CreateDirectory("json");
            }

            if (File.Exists(jsonPath))
            {
                contacts = JsonConvert.DeserializeObject<ObservableCollection<Contact>>(File.ReadAllText(jsonPath));
            }
            else
            {
                File.WriteAllText(jsonPath, JsonConvert.SerializeObject(contacts));
            }

            if (File.Exists(jsonDepPath))
            {
                departments = JsonConvert.DeserializeObject<ObservableCollection<Departament>>(File.ReadAllText(jsonDepPath));
            }
            else
            {
                File.WriteAllText(jsonDepPath, JsonConvert.SerializeObject(departments));
            }

            UpdateContactsModel(contacts);
        }

        public void UpdateContactsModel(ObservableCollection<Contact> contacts)
        {
            contactsModel = new ObservableCollection<ContactModel> { };

            foreach(var c in contacts)
            {
                ContactModel temp = new ContactModel();

                temp.Id = c.Id;
                temp.Name = c.Name;
                temp.Surname = c.Surname;
                temp.Number = c.Number;
                temp.Email = c.Email;

                temp.Department = GetDepartmentName(c.DepId);

                contactsModel.Add(temp);
            }
        }

        string GetDepartmentName(int depId)
        {
            string res = "unknown";

            foreach (var d in departments)
            {
                if (depId == d.Id)
                {
                    res = d.Department;
                    break;
                }
            }

            return res;
        }

        public void Update()
        {
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(contacts));
            File.WriteAllText(jsonDepPath, JsonConvert.SerializeObject(departments));           
        }

        public void UpdateList(ObservableCollection<Contact> contactList)
        {
            contacts = contactList;
            Update();
        }

        public void UpdateList(ObservableCollection<Departament> depList)
        {
            departments = depList;
            Update();
        }

        public void UpdateList(ObservableCollection<ContactModel> newContactsModel)
        {
            contactsModel = newContactsModel;
            Update();
        }

        public void Add(Contact contact)
        {
            contacts.Add(contact);
            
            Update();           
        }

        public void Add(Departament department)
        {
            departments.Add(department);
            Update();
        }

        public void Add(ContactModel contactModel)
        {
            contactsModel.Add(contactModel);

            Update();
        }

        public void Delete(Contact selectedContact)
        {
            foreach (Contact c in contacts)
            {
                if (c == selectedContact)
                {
                    contacts.Remove(c);
                    break;
                }
            }
        }

        public bool Delete(Departament selectedItem)
        {
            bool res = true;

            foreach (var c in contacts)
            {
                if (c.DepId == selectedItem.Id)
                {
                    res = false;
                    return res;
                }
            }

            departments.Remove(selectedItem);
            Update();

            return res;
        }

        public void Delete(ContactModel contactModel)
        {
            contactsModel.Remove(contactModel);
            Update();
        }

        public int CreateId(string form)
        {
            int id = 0;

            if (form == "contact")
            {
                if (contacts.Count == 0)
                    id = 0;
                else
                {
                    int[] idArr = new int[contacts.Count];
                    for (int i = 0; i < contacts.Count; i++)
                    {
                        idArr[i] = contacts[i].Id;
                    }

                    Array.Sort(idArr);

                    for (int i = 0; i < contacts.Count; i++)
                    {
                        if (idArr[i] != i)
                        {
                            return i;
                        }
                    }
                    id = contacts.Count;
                }
            }

            else if (form == "dep")
            {
                if (departments.Count == 0)
                    id = 0;
                else
                {
                    int[] idArr = new int[departments.Count];
                    for (int i = 0; i < departments.Count; i++)
                    {
                        idArr[i] = departments[i].Id;
                    }

                    Array.Sort(idArr);

                    for (int i = 0; i < departments.Count; i++)
                    {
                        if (idArr[i] != i)
                        {
                            return i;
                        }
                    }
                    id = departments.Count;
                }
            }

            return id;
        }
    }
}
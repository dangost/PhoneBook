using System.Collections.ObjectModel;
using System.IO;
using System;
using System.Xml.Serialization;
using Domain;

namespace Infrastructure
{
    public class XMLphoneBook : IPhoneBook
    {
        private ObservableCollection<Contact> contacts = new ObservableCollection<Contact> { };

        private ObservableCollection<Departament> departments = new ObservableCollection<Departament> { };

        private ObservableCollection<ContactModel> contactsModel = new ObservableCollection<ContactModel> { };

        const string xmlPathContacts = @"xml\contactsList.xml";
        const string xmlPathDepartment = @"xml\departmentsList.xml";

        public ObservableCollection<Contact> GetAllContacts()
        {
            return contacts;
        }

        public ObservableCollection<Departament> GetAllDepartments()
        {
            return departments;
        }

        public ObservableCollection<ContactModel> GetAllContactsModels()
        {
            return contactsModel;
        }

        public void UpdateContactsModel(ObservableCollection<Contact> contacts)
        {
            contactsModel = new ObservableCollection<ContactModel> { };

            foreach (var c in contacts)
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

        public void Load()
        {
            if (!Directory.Exists("xml"))
            {
                Directory.CreateDirectory("xml");
            }

            if (File.Exists(xmlPathContacts))
            {
                XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<Contact>));

                using (FileStream stream = new FileStream(xmlPathContacts, FileMode.OpenOrCreate))
                {
                    contacts = (ObservableCollection<Contact>)xml.Deserialize(stream);                    
                }
            }
            else
            {
                Update();
            }

            if (File.Exists(xmlPathDepartment))
            {
                XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<Departament>));

                using (FileStream stream = new FileStream(xmlPathDepartment, FileMode.OpenOrCreate))
                {
                    departments = (ObservableCollection<Departament>)xml.Deserialize(stream);
                }
            }
            else
            {
                Update();
            }

            UpdateContactsModel(contacts);
        }

        public void Update()
        {
            File.Delete(xmlPathContacts);
            
            XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<Contact>));
            
            using (FileStream stream = new FileStream(xmlPathContacts, FileMode.OpenOrCreate))
            {
                xml.Serialize(stream, contacts);
            }

            File.Delete(xmlPathDepartment);

            xml = new XmlSerializer(typeof(ObservableCollection<Departament>));

            using (FileStream stream = new FileStream(xmlPathDepartment, FileMode.OpenOrCreate))
            {
                xml.Serialize(stream, departments);
            }
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



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace PhonebookM
{
    public class JsonPhoneBook //: IPhoneBook
    {
        private ObservableCollection<Contact> contacts = new ObservableCollection<Contact> { };
        private ObservableCollection<Department> departments = new ObservableCollection<Department> { };

        const string jsonPath = "contactsList.json";
        const string jsonDepPath = "departmentsList.json";

        public ObservableCollection<Contact> GetAllContacts()
        {
            return contacts;
        }

        public ObservableCollection<Department> GetAllDepartments()
        {
            return departments;
        }

        public void Load()
        {
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
                departments = JsonConvert.DeserializeObject<ObservableCollection<Department>>(File.ReadAllText(jsonDepPath));
            }
            else
            {
                File.WriteAllText(jsonDepPath, JsonConvert.SerializeObject(departments));

                departments = new ObservableCollection<Department> { };
                for (int i = 0; i < departments.Count; i++)
                {
                   // departments[i].Contacts = new ObservableCollection<Contact> { };
                }
            }
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

        public void UpdateList(ObservableCollection<Department> depList)
        {
            departments = depList;
            Update();
        }

        public void Add(Contact contact)
        {
            contacts.Add(contact);
            
            Update();

            
        }

        public void Add(Department department)
        {
            departments.Add(department);
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

        public bool Delete(Department selectedDepartment)
        {
            bool res = true;

            /*if (selectedDepartment.Contacts != null)
            {
                res = true;
                departments.Remove(selectedDepartment);
            }*/

            return res;
        }

        public int CreateId(string form)
        {
            int id = 0;
            if (form == "contact")
            {
                id = 0;

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
                id = 0;

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
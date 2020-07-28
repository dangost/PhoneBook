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
    public class JsonPhoneBook : IPhoneBook
    {
        private ObservableCollection<Contact> contacts = new ObservableCollection<Contact> { };

        const string jsonPath = "contactsList.json";

        public ObservableCollection<Contact> GetAll()
        {
            return contacts;
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
        }

        public void Update()
        {
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(contacts));
        }

        public void UpdateList(ObservableCollection<Contact> contactList)
        {
            contacts = contactList;
            Update();
        }

        public void Add(Contact contact)
        {
            contacts.Add(contact);
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

        public int CreateId()
        {
            int id = 0;

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

            return id;
        }
    }
}


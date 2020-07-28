using System.Collections.ObjectModel;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace PhonebookM
{
    public class XMLphoneBook : IPhoneBook
    {
        private ObservableCollection<Contact> contacts = new ObservableCollection<Contact> { };

        const string xmlPath = "contactsList.xml";

        public void Load()
        {
            if (File.Exists(xmlPath))
            {
                XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<Contact>));

                using (FileStream stream = new FileStream(xmlPath, FileMode.OpenOrCreate))
                {
                    contacts = (ObservableCollection<Contact>)xml.Deserialize(stream);                    
                }
            }
            else
            {
                Update();
            }
        }

        public ObservableCollection<Contact> GetAll()
        {
            return contacts;
        }

        public void Delete(Contact selectedItem)
        {
            foreach (Contact c in contacts)
            {
                if (c == selectedItem)
                {
                    contacts.Remove(c);
                    break;
                }
            }
            Update();
        }

        public void Update()
        {
            File.Delete(xmlPath);
            XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<Contact>));
            
            using (FileStream stream = new FileStream(xmlPath, FileMode.OpenOrCreate))
            {
                xml.Serialize(stream, contacts);
            }
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


using System.Collections.ObjectModel;
using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;


namespace PhonebookM
{
    public class SQlitePhoneBook : IPhoneBook
    {
        class ApplicationContext : DbContext
        {
            public ApplicationContext() : base("DefaultConnection")
            {
                
            }
            public DbSet<Department> Departments { get; set; }
            public DbSet<Contact> Contacts { get; set; }

        }

        private string sqLitePath = @"database.db";

        private ApplicationContext dataBase = new ApplicationContext();

        private ObservableCollection<Contact> contacts = new ObservableCollection<Contact> { };
        private ObservableCollection<Department> departments = new ObservableCollection<Department> { };

        private ObservableCollection<ContactModel> contactsModel = new ObservableCollection<ContactModel> { };

        public ObservableCollection<Contact> GetAllContacts()
        {
            Sync();
            return contacts;
        }

        public ObservableCollection<ContactModel> GetAllContactsModels()
        {
            return contactsModel;
        }

        public ObservableCollection<Department> GetAllDepartments()
        {
            Sync();
            return departments;
        }

        public void Load()
        {
            if(!File.Exists(sqLitePath))
            {
                string baseName = sqLitePath;

                SQLiteConnection.CreateFile(baseName);
                
                using (SQLiteConnection connection = new SQLiteConnection())
                {
                    connection.ConnectionString = "Data Source = " + baseName;
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        string departs =
                       @"CREATE TABLE [Departments] (
                    [Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [Name] TEXT NOT NULL
                    );";
                        string contacts = @"CREATE TABLE [Contacts] (

    [Id]    INTEGER NOT NULL,
	[Name]  TEXT,
	[Surname]   TEXT,
	[Number]    TEXT,
	[Email] TEXT,
	[DepId] INTEGER,
	PRIMARY KEY([Id] AUTOINCREMENT)
);";
                        command.CommandText = departs + contacts;
                        command.CommandType = System.Data.CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }               
            }

            dataBase = new ApplicationContext();
            dataBase = new ApplicationContext();

            dataBase.Contacts.Load();
            dataBase.Departments.Load();

            dataBase.SaveChanges();

            Sync();

            UpdateContactsModel(contacts);
        }

        public void UpdateContactsModel(ObservableCollection<Contact> contacts)
        {
            contactsModel = new ObservableCollection<ContactModel> { };

            foreach (var c in contacts)
            {
                ContactModel co = new ContactModel();
                co.Id = c.Id;
                co.Name = c.Name;
                co.Surname = c.Surname;
                co.Number = c.Number;
                co.Email = c.Email;
                co.Department = GetDepartmentName(c.DepId);
                contactsModel.Add(co);
            }            
        }

        string GetDepartmentName(int depId)
        {
            string res = "unknown";
            
            foreach (var d in departments)
            {
                if (depId == d.Id)
                {
                    res = d.Name;
                    break;
                }
            }

            return res;
        }

        public void Update()
        {
            dataBase.SaveChanges();

            Sync();
        }

        public void Delete(Contact selectedItem)
        {
            dataBase.Contacts.Remove(selectedItem);
            
            dataBase.SaveChanges();
            Sync();
            UpdateContactsModel(contacts);
        }

        public bool Delete(Department selectedItem)
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

            dataBase.Departments.Remove(selectedItem);

            dataBase.SaveChanges();
            Sync();

            return res;
        }

        public void Delete(ContactModel contactModel)
        {
            contactsModel.Remove(contactModel);
        }

        void Sync()
        {
            contacts.Clear();
            departments.Clear();
            foreach (var set in dataBase.Contacts)
            {
                contacts.Add(set);
            }

            foreach(var set in dataBase.Departments)
            {
                departments.Add(set);
            }

            dataBase.SaveChanges();
        }

        public void Add(Contact contact)
        {
            dataBase.Contacts.Add(contact);

            dataBase.SaveChanges();
            Sync();
            UpdateContactsModel(contacts);
        }

        public void Add(Department department)
        {
            dataBase.Departments.Add(department);

            dataBase.SaveChanges();
            Sync();
        }

        public void Add(ContactModel contactModel)
        {
            contactsModel.Add(contactModel);
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

            return id++;
        }

        public void UpdateList(ObservableCollection<Contact> newContacts)
        {
            contacts = newContacts;
            Update();
        }

        public void UpdateList(ObservableCollection<Department> newDepartments)
        {
            departments = newDepartments;
            Update();
        }

        public void UpdateList(ObservableCollection<ContactModel> newContactsModel)
        {
            contactsModel = newContactsModel;
            Update();
        }
    }
}

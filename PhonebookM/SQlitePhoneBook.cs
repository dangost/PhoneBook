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
            public DbSet<Contact> Contacts { get; set; }
        }

        private string sqLitePath = @"db\Contacts.db";

        private ApplicationContext dataBase = new ApplicationContext();

        private ObservableCollection<Contact> contacts = new ObservableCollection<Contact> { };

        public ObservableCollection<Contact> GetAll()
        {
            Sync();
            return contacts;
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
                        command.CommandText = @"CREATE TABLE [Contacts] (
                    [Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [Name] TEXT NOT NULL,
                    [Surname] TEXT NOT NULL,
                    [Number] TEXT NOT NULL,
                    [Email] TEXT NOT NULL
                    );";
                        command.CommandType = System.Data.CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }               
            }

            dataBase = new ApplicationContext();

            dataBase.Contacts.Load();
            dataBase.SaveChanges();
            Sync();
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
        }

        void Sync()
        {
            contacts.Clear();

            foreach (var set in dataBase.Contacts)
            {
                contacts.Add(set);
            }
            dataBase.SaveChanges();
        }

        public void Add(Contact contact)
        {
            dataBase.Contacts.Add(contact);
            dataBase.SaveChanges();
            Sync();
        }

        public int CreateId()
        {
            int id;
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
            return id++;
        }

        public void UpdateList(ObservableCollection<Contact> newContacts)
        {
            contacts = newContacts;
        }
    }
}

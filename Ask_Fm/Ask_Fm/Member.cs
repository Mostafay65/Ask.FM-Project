using System;
using System.Collections;

namespace Ask_Fm
{
    public class Member
    {
        public string name;
        public string username;
        public string password;
        public int id;
        public ArrayList Questions = new ArrayList();
        
        public void signup(int id)
        {
            this.id = id;
            Console.Write("Name : ");
            this.name = Console.ReadLine();
            Console.Write("Username : ");
            this.username = Console.ReadLine();
            Console.Write("Password : ");
            this.password = Console.ReadLine();
        }

        public bool checklogin(string username, string password)
        {
            return this.username == username && this.password == password;
        }

        public void showdata()
        {
            Console.WriteLine($"ID - {this.id} : {this.name}");
        }

        public void Ask(Question q)
        {
            this.Questions.Add(q);
        }
    }
}
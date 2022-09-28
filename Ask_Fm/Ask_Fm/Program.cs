using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace Ask_Fm
{
    internal class Program
    {
        public static void BuildMembers(ref ArrayList Members)
        {
            string filename = "/Users/mostafayoussef/RiderProjects/Ask_Fm/Dummy Data/Members/Names.txt";
            using (StreamReader reader=File.OpenText(filename))
            {
                string MemberName = reader.ReadLine();
                int cnt = 0;
                while (MemberName!=null)
                {
                    Member m = new Member();
                    m.name = MemberName;
                    m.id = cnt;
                    Members.Add(m);
                    MemberName = reader.ReadLine();
                    cnt++;
                }
                reader.Close();
            }
            filename = "/Users/mostafayoussef/RiderProjects/Ask_Fm/Dummy Data/Members/pass.txt";
            using (StreamReader reader=File.OpenText(filename))
            {
                string line = reader.ReadLine();
                int cnt = 0;
                while (line!=null)
                {
                    string[] Memberdata = line.Split(' ');
                    Member m = (Member)Members[cnt];
                    m.username = Memberdata[0];
                    m.password = Memberdata[1];
                    line = reader.ReadLine();
                    cnt++;
                }
                reader.Close();
            }
        }

        public static void BuildQuestion(ref ArrayList Questions,ref ArrayList Members)
        {
            string filename = "/Users/mostafayoussef/RiderProjects/Ask_Fm/Dummy Data/Question/Q&A.txt";
            using (StreamReader reader=File.OpenText(filename))
            {
                string q = reader.ReadLine();
                string A = reader.ReadLine();
                int cnt = 0;
                while (q!=null)
                {
                    Question NQ = new Question(q,A,cnt);
                    Questions.Add(NQ);
                    q = reader.ReadLine();
                    A = reader.ReadLine();
                    cnt++;
                }
            }
            filename = "/Users/mostafayoussef/RiderProjects/Ask_Fm/Dummy Data/Question/Data.txt";
            using (StreamReader reader=File.OpenText(filename))
            {
                string line = reader.ReadLine();
                int cnt = 0;
                while (line!=null)
                {
                    string[] data = line.Split(' ');
                    Question q = (Question)Questions[cnt];
                    Member from=null,to=null;
                    foreach (Member m in Members)
                    {
                        if (m.id==int.Parse(data[0]))
                        {
                            from = m;
                        }
                        if (m.id==int.Parse(data[1]))
                        {
                            to = m;
                        }
                    }
                    q.setdata(from,to,data[2]);
                    to.Questions.Add(q);
                    line = reader.ReadLine();
                    cnt++;
                }
            }
        }

        public static void storeMembers(ref ArrayList Members)
        {
            string filename="/Users/mostafayoussef/RiderProjects/Ask_Fm/Dummy Data/Members/Names.txt";
            StreamWriter writer = new StreamWriter(filename);
            foreach (Member m in Members)
            {
                writer.WriteLine(m.name);
            }
            writer.Close();
            filename="/Users/mostafayoussef/RiderProjects/Ask_Fm/Dummy Data/Members/pass.txt";
            writer = new StreamWriter(filename);
            foreach (Member m in Members)
            {
                writer.WriteLine($"{m.username} {m.password}");
            }
            writer.Close();
            
        }

        public static void storeQuestion(ref ArrayList Questions)
        {
            string filename = "/Users/mostafayoussef/RiderProjects/Ask_Fm/Dummy Data/Question/Q&A.txt";
            StreamWriter writer = new StreamWriter(filename);
            foreach (Question q in Questions)
            {
                writer.WriteLine(q.question);
                writer.WriteLine(q.answer);
            }
            writer.Close();
            filename = "/Users/mostafayoussef/RiderProjects/Ask_Fm/Dummy Data/Question/Data.txt";
            writer = new StreamWriter(filename);
            foreach (Question q in Questions)
            {
                writer.Write($"{q.from.id} {q.to.id} ");
                if (q.common)
                {
                    writer.Write("T\n");
                }
                else
                {
                    writer.Write("F\n");
                }
            }
            writer.Close();
        }
        
        public static void Main(string[] args)
        {
            Console.Clear();
            ArrayList Members = new ArrayList();
            ArrayList Questions = new ArrayList();
            BuildMembers(ref Members);
            BuildQuestion(ref Questions,ref Members);
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\t\t Welcome to our Ask.fm App.\n\nPlease make a choice\n\t1 - Login.\n\t2 - Sign Up.\n\t3 - Shutdown The System.\n\t");
                int choice = int.Parse(Console.ReadLine());
                while (choice>3 || choice<1)
                {
                    Console.Write("Please enter a valid choice : ");
                    choice = int.Parse(Console.ReadLine());
                }
                if (choice==1)
                {
                    Console.Clear();
                    bool mem = false;
                    Member curMember = new Member();
                    while (!mem)
                    {
                        Console.Write("Username : ");
                        string username = Console.ReadLine();
                        Console.Write("Password : ");
                        string password = Console.ReadLine();
                        foreach (Member m in Members)
                        {
                            if (m.checklogin(username,password))
                            {
                                curMember = m;
                                mem = true;
                                break;
                            }
                        }
                        Console.Clear();
                        if (!mem) Console.WriteLine("Username and Passwod are incorrect :( \n\tEnter a valid data\n");

                    }
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine($"Welcome {curMember.name} you are logged in ");
                        Console.WriteLine("Please make a choice\n\t1 - Ask member.\n\t2 - Show my Question.\n\t3 - Show The Common Question.\n\t4 - Show The Question that I asked.\n\t5 - log out.");
                        choice = int.Parse(Console.ReadLine());
                        while (choice>5 || choice<1)
                        {
                            Console.Write("Please enter a valid choice : ");
                            choice = int.Parse(Console.ReadLine());
                        }

                        if (choice==1)
                        {
                            Console.Clear();
                            if (Members.Count<=1)
                            {
                                Console.WriteLine("There is no members to ask :)");
                                Console.ReadLine();
                                continue;
                            }
                            ArrayList ids = new ArrayList();
                            foreach (Member m in Members)
                            {
                                if (m==curMember)continue;
                                m.showdata();
                                ids.Add(m.id);
                            }
                            Console.Write("which ID member to ask ? ");
                            int id = int.Parse(Console.ReadLine());
                            while (!ids.Contains(id))
                            {
                                Console.Write("Please enter a valid ID : ");
                                id = int.Parse(Console.ReadLine());
                            }
                            Console.Write("Your Question : ");
                            string question = Console.ReadLine();
                            foreach (Member m in Members)
                            {
                                if (m.id==id)
                                {
                                    Question q = new Question(question,curMember,m,Questions.Count+1);
                                    m.Ask(q);
                                    Questions.Add(q);
                                    break;
                                }
                            }

                        }
                        else if (choice==2)
                        {
                            if (curMember.Questions.Count==0)
                            {
                                Console.WriteLine("You don't have any Question.");
                                Console.ReadLine();
                                continue;
                            }
                            Console.Clear();
                            ArrayList ids = new ArrayList();
                            foreach (Question q in curMember.Questions)
                            {
                                Console.WriteLine($"You are asked a question from {q.from.name}");
                                q.showdata();
                                ids.Add(q.id);
                            }

                            while (true)
                            {
                                Console.Clear();
                                foreach (Question q in curMember.Questions)
                                {
                                    Console.WriteLine($"You are asked a question from {q.from.name}");

                                    q.showdata();
                                }
                                Console.WriteLine("Please make a choice : \n\t1 - Answer a Question.\n\t2 - Make a question common.\n\t3 - Remove a question form the common list.\n\t4 - Remove a question.\n\t5 - Back. ");
                                choice = int.Parse(Console.ReadLine());
                                while (choice>5||choice<1)
                                {
                                    Console.Write("Please enter a valid choice : ");
                                    choice = int.Parse(Console.ReadLine());
                                }

                                if (choice==1)
                                {
                                    Console.Write("Which Question ID to answer : ");
                                    int id = int.Parse(Console.ReadLine());
                                    while (!ids.Contains(id))
                                    {
                                        Console.Write("Please enter a valid ID : ");
                                        id = int.Parse(Console.ReadLine());
                                    }
                                    Console.Write("Your answer : ");
                                    string ans = Console.ReadLine();
                                    foreach (Question q in Questions)
                                    {
                                        if (q.id==id)
                                        {
                                            q.Answer(ans);
                                            break;
                                        }
                                    }
                                    Console.WriteLine("The question is answered successfully");
                                    Console.ReadLine();
                                }
                                else if (choice==2)
                                {
                                    Console.Write("Which question ID to make it common : ");
                                    int id = int.Parse(Console.ReadLine());
                                    while (!ids.Contains(id))
                                    {
                                        Console.Write("Please enter a valid ID : ");
                                        id = int.Parse(Console.ReadLine());
                                    }

                                    foreach (Question q in curMember.Questions)
                                    {
                                        if (q.id==id)
                                        {
                                            q.common = true;
                                            break;
                                        }
                                    }
                                
                                }
                                else if (choice==3)
                                {
                                    if (curMember.Questions.Count==0)
                                    {
                                        Console.WriteLine("You don't have any Qustion!");
                                        continue;
                                    }
                                    Console.Write("Which Question ID to remove from the common list : ");
                                    int id = int.Parse(Console.ReadLine());
                                    while (!ids.Contains(id))
                                    {
                                        Console.Write("Please enter a valid ID : ");
                                        id = int.Parse(Console.ReadLine());
                                    }
                                    foreach (Question q in curMember.Questions)
                                    {
                                        if (q.id==id)
                                        {
                                            q.common = false;
                                            break;
                                        }
                                    }
                                    
                                }
                                else if (choice==4)
                                {
                                    Console.Write("Which Question ID to remove : ");
                                    int id = int.Parse(Console.ReadLine());
                                    while (!ids.Contains(id))
                                    {
                                        Console.Write("Please enter a valid ID : ");
                                        id = int.Parse(Console.ReadLine());
                                    }

                                    foreach (Question q in curMember.Questions)
                                    {
                                        if (q.id==id)
                                        {
                                            curMember.Questions.Remove(q);
                                            break;
                                        }
                                    }
                                    foreach (Question q in Questions)
                                    {
                                        if (q.id==id)
                                        {
                                            Questions.Remove(q);
                                            break;
                                        }
                                    }

                                    Console.WriteLine("The question is removed successfully.");
                                    Console.ReadLine();
                                }
                                else if (choice==5) break;
                            }
                        
                        }
                        else if (choice==3)
                        {
                            bool com = false;
                            foreach (Question q in Questions)
                            {
                                if (q.common)
                                {
                                    com = true;
                                    Console.WriteLine($"{q.from.name} asked {q.to.name}");
                                    q.showdata();
                                }
                            }

                            if (!com)
                            {
                                Console.WriteLine("There are no common questions");
                            }
                            Console.ReadLine();
                        }
                        else if (choice==4)
                        {
                            bool ask = false;
                            foreach (Question q in Questions)
                            {
                                if (q.from==curMember)
                                {
                                    ask = true;
                                    Console.WriteLine($"You have asked {q.to.name} a Question");
                                    q.showdata();
                                }
                            }

                            if (!ask)
                            {
                                Console.WriteLine("You haven't asked anyone yet!");
                            }
                            Console.ReadLine();
                        }
                        else if (choice==5)
                        {
                            Console.Write("Logging out");
                            for (int i = 0; i < 7; i++)
                            {
                                Thread.Sleep(300);
                                Console.Write(".");
                            }

                            break;
                        }
                    }
                    
                }
                else if (choice==2)
                {
                    Console.Clear();
                    Member curmember = new Member();
                    curmember.signup(Members.Count);
                    Members.Add(curmember);
                    Console.WriteLine("\nYour account is created successfully try to login ");
                    Console.ReadLine();
                }
                else if (choice==3)
                {
                    Console.Clear();
                    Console.WriteLine("Your Application is shutsown ");
                    break;
                }
            }
            storeMembers(ref Members);
            storeQuestion(ref Questions);
        }
    }
}
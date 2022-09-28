using System;

namespace Ask_Fm
{
    public class Question
    {
        public Member from;
        public Member to;
        public string question;
        public string answer;
        public int id;
        public bool common;
        public Question(string q,string A,int id)
        {
            this.question = q;
            this.answer = A;
            this.id = id;
        }
        public Question(string q,Member from,Member to,int id)
        {
            this.question = q;
            this.from = from;
            this.to = to;
            this.answer = " ";
            this.id = id;
            this.common = false;
        }

        public void showdata()
        {
            Console.WriteLine($"Question ID {this.id} : {this.question}");
            if (this.answer==" ") Console.WriteLine("Answer : Not answered yet! \n\n");
            else Console.WriteLine($"Answer : {this.answer}\n\n");
        }

        public void Answer(string ans)
        {
            this.answer = ans;
        }

        public void setdata(Member from, Member to, string com)
        {
            this.from = from;
            this.to = to;
            if (com=="T")
            {
                this.common = true;
            }
            else  this.common = false;
        }
    }
}
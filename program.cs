using System;

namespace toDoList{
    class user{
        protected string username;
    }
    class task
    {
        protected string name;
        protected int taskid;
        protected string task_description;
        protected bool is_done;
        protected int dtstart;
        protected int dtend;
        protected string tag;
        public task(string name,int taskid, string task_description, bool is_done, int dtstart, int dtend, string tag)
        {
        this.name=name;
        this.taskid=taskid;
        this.task_description=task_description;
        this.is_done=is_done;
        this.dtstart=dtstart;
        this.dtend=dtend;
        this.tag=tag;
        }
    }
    class userTask{
        protected string task;
    }
    class program{
        static function Main(string[] args){
            Console.WriteLine("-----");
            Console.WriteLine("-----");
        }
    }
}
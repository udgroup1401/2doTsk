using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace toDoList
{
    class user
    {
        protected string username;
        protected string password;
        protected bool loggedIn;
        public user(string u, string p)
        {
            this.username = u;
            this.password = p;
            this.loggedIn = false;
        }
        public void register()
        {
            string jsonContent = File.ReadAllText("db.json");
            JObject dbFile = JObject.Parse(jsonContent);
            List<string[]> allUsersAndPassword = new List<string[]>();
            foreach (var user in dbFile["users"])
            {
                allUsersAndPassword.Add(new string[] { user["username"].ToString(), user["password"].ToString() });
            }
            bool isValidUser = allUsersAndPassword.Exists(arr => arr[0] == this.username);
            if (isValidUser == false)
            {
                var newResultToAdd = new JObject(
                    new JProperty("username", this.username),
                    new JProperty("password", this.password),
                    new JProperty("task", new JArray())
                );

                JArray usersArray = (JArray)dbFile["users"];
                usersArray.Add(newResultToAdd);

                string updatedJsonContent = dbFile.ToString();
                File.WriteAllText("db.json", updatedJsonContent);
                this.loggedIn = true;
            }
        }
        public void login()
        {
            List<string[]> allUsersAndPassword = new List<string[]>();
            string jsonContent = File.ReadAllText("db.json");
            dynamic dbFile = JsonConvert.DeserializeObject(jsonContent);
            foreach (var user in dbFile.users)
            {
                allUsersAndPassword.Add(new string[] { user.username.ToString(), user.password.ToString() });
            }
            bool isValidUser = allUsersAndPassword.Exists(arr => arr[0] == this.username && arr[1] == this.password);
            this.loggedIn = isValidUser;
        }


        public string getUsername() { return this.username; }
        public bool isLogin() { return this.loggedIn; }
        public void logout() { this.loggedIn = false; }
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
        public task(string name, int taskid, string task_description, bool is_done, int dtstart, int dtend, string tag)
        {
            this.name = name;
            this.taskid = taskid;
            this.task_description = task_description;
            this.is_done = is_done;
            this.dtstart = dtstart;
            this.dtend = dtend;

            this.tag = tag;
        }
        DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;
        public string tidNumber()
        {
            Random rand = new Random();
            string num = "";
            for (int i = 0; i < 9; i++)
            {
                string number = rand.Next(0, 9).ToString();
                if (num.Contains(number) == false)
                    num += number;
                else
                    i--;
            }
            return num;
        }
        public void replacetd(string task_discription)
        {
            this.task_description = task_discription;
        }
        public void changedone(bool is_done)
        {
            this.is_done = is_done;
        }
        public void tostring()
        {
            Console.WriteLine($"name:{this.name}\n taskid:{this.taskid}\n task_description:{this.task_description}\n is_done:{this.is_done}\n dtstart:{this.dtstart}\n dtend:{this.dtend}\n tag:{this.tag}");
        }
    }
    class userTask
    {


        protected string order;
        protected int tasks;
        protected task[] taskDetail;
        public userTask(string userName)
        {
            this.tasks = 0;
            int HowMTask = 0;
            string jsonContent = File.ReadAllText("db.json");
            JObject dbFile = JObject.Parse(jsonContent);
            foreach (var user in dbFile["users"])
            {
                if (userName == user["username"].ToString())
                {
                    foreach (object taskObjectT in user["task"])
                    {
                        this.tasks++;
                    }

                    taskDetail = new task[this.tasks];
                    int indexT = 0;

                    foreach (var taskObjectT in user["task"])
                    {
                        taskDetail[indexT] = new task("aaaa", Convert.ToInt32(taskObjectT["taskId"].ToString()), taskObjectT["taskDes"].ToString(), Convert.ToBoolean(taskObjectT["isDone"].ToString()), Convert.ToInt32(taskObjectT["timeDateStart"].ToString()), Convert.ToInt32(taskObjectT["timeDateDone"].ToString()), taskObjectT["tag"].ToString());
                        indexT++;
                    }

                }
            }
        }
        public int howManyTask()
        {
            return this.tasks;
        }
        public task[] detailedTask()
        {
            return taskDetail;
        }
    }
    class program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            while (true)
            {
                Console.Write("USER? ");
                string fuser = Console.ReadLine();
                while(fuser == ""){
                    Console.Write("USER? ");
                    fuser = Console.ReadLine();
                }
                Console.Write("PASS? ");
                string fpass = Console.ReadLine();
                while(fpass == ""){
                    Console.Write("PASS? ");
                    fpass = Console.ReadLine();
                }
                user loggedInUser = new user(fuser, fpass);
                while (true)
                {
                    if (loggedInUser.isLogin() == false)
                    {
                        Console.WriteLine("WELCOME\n\n1. register user\n2. login user\n3. change user\n4. exit");
                        int userInputMenu = Convert.ToInt32(Console.ReadLine());
                        if (userInputMenu == 1)
                        {
                            loggedInUser.register();
                            Console.WriteLine("try to register");
                        }
                        if (userInputMenu == 2)
                        {
                            loggedInUser.login();
                            Console.WriteLine("Try to login");
                        }
                        if (userInputMenu == 3) { break; }
                        if (userInputMenu == 4)
                        {
                            Console.WriteLine("EXIT PROGRAM");
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        userTask ut = new userTask(loggedInUser.getUsername());
                        Console.WriteLine($"Logged in as {loggedInUser.getUsername()} - you have {ut.howManyTask()} task\n1. logout\n2. show task");
                        int userInputMenuL = Convert.ToInt32(Console.ReadLine());
                        if (userInputMenuL == 1) { loggedInUser.logout(); }
                        if (userInputMenuL == 2)
                        {
                            task[] userTaskForShow = ut.detailedTask();
                            foreach (task tskFSF in userTaskForShow)
                            {
                                tskFSF.tostring();
                            }
                        }
                    }
                }
            }
        }
    }
}

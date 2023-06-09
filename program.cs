﻿using System;
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
        protected long dtstart;
        protected long dtend;
        protected string tag;
        public task(string name, int taskid, string task_description, bool is_done, long dtstart, long dtend, string tag)
        {
            this.name = name;
            this.taskid = taskid;
            this.task_description = task_description;
            this.is_done = is_done;
            this.dtstart = dtstart;
            this.dtend = dtend;
            this.tag = tag;
        }
        public static long GetTimestamp()
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Timestamp = Convert.ToInt64(Timestamp);
            return Timestamp;
        }
        public static string getDataTimeFromTimestamp(long tms){
            DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(tms).DateTime;
            return dateTime.ToString("yyyy/MM/dd HH:mm");
        }
        public static long getTimestampFromDataAndTime(string dateTimeString){
            //Enter date and time in yyyy/MM/dd HH:mm
            DateTime dateTime;
            if (!DateTime.TryParseExact(dateTimeString, "yyyy/MM/dd HH:mm", null, System.Globalization.DateTimeStyles.None, out dateTime)){
                return 0;
            }
            long timestamp = (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            return timestamp;
        }
        public static int taskidNumber()
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
            return Convert.ToInt32(num);
        }
         public void namechange(string name)
        {
            this.name = name;
        }
         public void task_discriptionchange(string task_discription)
        {
            this.task_description = task_discription;
        }
        public void is_donechange(bool is_done)
        {
            this.is_done = is_done;
        }
        public void dtstartchange(long dtstart)
        {
            this.dtstart = dtstart;
        }
        public void dtendchange(long dtend)
        {
            this.dtend = dtend;
        }
        public void tagchange(string tag)
        {
            this.tag = tag;
        }
        public void tostring()
        {
            Console.WriteLine($"name:{this.name}\n taskid:{this.taskid}\n task_description:{this.task_description}\n is_done:{this.is_done}\n dtstart:{this.dtstart}\n dtend:{this.dtend}\n tag:{this.tag}");
        }
        public string getname()
        {
            return this.name;
        }
        public int gettaskid()
        {
            return this.taskid;
        }
        public string gettask_discription()
        {
            return this.task_description;
        }
        public bool getis_done()
        {
            return this.is_done;
        }
        public long getdtstart()
        {
            return this.dtstart;
        }
        public long getdtend()
        {
            return this.dtend;
        }
        public string gettag()
        {
            return this.tag;
        }
        public task(int taskid,string userName)
        {
            string jsonContent = File.ReadAllText("db.json");
            JObject dbFile = JObject.Parse(jsonContent);
            foreach (var user in dbFile["users"])
            {
                if (userName == user["username"].ToString())
                {
                    foreach (var taskObjectT in user["task"])
                    {
                        if(Convert.ToInt32(taskObjectT["taskId"].ToString())==taskid)
                        {
                            this.name=taskObjectT["name"].ToString();
                            this.taskid = Convert.ToInt32(taskObjectT["taskId"].ToString());
                            this.task_description =  taskObjectT["taskDes"].ToString();
                            this.is_done = Convert.ToBoolean(taskObjectT["isDone"].ToString());
                            this.dtstart = Convert.ToInt32(taskObjectT["timeDateStart"].ToString());
                            this.dtend =  Convert.ToInt32(taskObjectT["timeDateDone"].ToString());
                            this.tag = taskObjectT["tag"].ToString();
                        }
                    }
                }
            }
        }
    }
    class userTask
    {
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
                        taskDetail[indexT] = new task(taskObjectT["name"].ToString(), Convert.ToInt32(taskObjectT["taskId"].ToString()), taskObjectT["taskDes"].ToString(), Convert.ToBoolean(taskObjectT["isDone"].ToString()), Convert.ToInt32(taskObjectT["timeDateStart"].ToString()), Convert.ToInt32(taskObjectT["timeDateDone"].ToString()), taskObjectT["tag"].ToString());
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
        public void save(string userName)
        {
            string jsonContent = File.ReadAllText("db.json");
            JObject dbFile = JObject.Parse(jsonContent);
            JArray JNewTasks = new JArray();
            int whereIsUser = 0;
            foreach (var user in dbFile["users"])
            {
                if (userName == user["username"].ToString())
                {
                    foreach (task item in this.detailedTask())
                    {
                        var newResultToAdd = new JObject(
                     new JProperty("name", item.getname()),
                     new JProperty("taskId", item.gettaskid()),
                     new JProperty("taskDes", item.gettask_discription()),
                     new JProperty("isDone", item.getis_done()),
                     new JProperty("timeDateStart", item.getdtstart()),
                     new JProperty("timeDateDone", item.getdtend()),
                     new JProperty("tag", item.gettag())
                     );
                        JNewTasks.Add(newResultToAdd);
                    }
                    break;
                }else{
                    whereIsUser++;
                }
            }
            dbFile["users"][whereIsUser]["task"] = JNewTasks;
            File.WriteAllText("db.json", JsonConvert.SerializeObject(dbFile, Formatting.Indented));

        }
        public task[] addTask(task addy, string userName)
        {
            task[] newTasks = new task[(this.tasks + 1)];
            int indexT = 0;
            foreach (task item in this.detailedTask())
            {
                newTasks[indexT] = item;
                indexT++;
            }
            newTasks[indexT] = addy;
            this.taskDetail = newTasks;
            this.save(userName);
            return this.taskDetail;
        }
        public task[] deleteTask(int taskId, string userName)
        {
            task[] newTasks = new task[(this.tasks - 1)];
            int indexT = 0;
            foreach (task item in this.detailedTask())
            {
                    if (item.gettaskid() != taskId)
                    {
                        newTasks[indexT] = item;
                        indexT++;    
                    }   
            }
            this.taskDetail = newTasks;
            this.save(userName);
            return this.taskDetail;
        }
        public task[] updateTask(task editedTask, string userName)
        {
            task[] newTasks = new task[(this.tasks)];
            int indexT = 0;
            foreach (task item in this.detailedTask())
            {
                    if (item.gettaskid() == editedTask.gettaskid())
                    {
                        newTasks[indexT] = editedTask;
                        indexT++;    
                    }
                    else
                    {
                        newTasks[indexT] = item;
                        indexT++;    

                    }   
            }
            this.taskDetail = newTasks;
            this.save(userName);
            return this.taskDetail;
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
                while (fuser == "")
                {
                    Console.Write("USER? ");
                    fuser = Console.ReadLine();
                }
                Console.Write("PASS? ");
                string fpass = Console.ReadLine();
                while (fpass == "")
                {
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
                        userTask loggedInUserTask = new userTask(loggedInUser.getUsername());
                        Console.WriteLine($"Logged in as {loggedInUser.getUsername()} - you have {loggedInUserTask.howManyTask()} task\n1. logout\n2. show task\n3. new task\n4. delete task\n5.edit task");
                        int userInputMenuL = Convert.ToInt32(Console.ReadLine());
                        if (userInputMenuL == 1) { loggedInUser.logout(); }
                        if (userInputMenuL == 2)
                        {
                            task[] userTaskForShow = loggedInUserTask.detailedTask();
                            foreach (task tskFSF in userTaskForShow)
                            {
                                tskFSF.tostring();
                                Console.WriteLine("*-*-*-*-*-*-*-*-*-*");
                            }
                        }
                        if(userInputMenuL == 3){
                            Console.WriteLine("enter task name");
                            string newTaskName = Console.ReadLine();
                            Console.WriteLine("enter task des");
                            string newTaskDes = Console.ReadLine();
                            Console.WriteLine("enter task tag");
                            string newTaskTag = Console.ReadLine();
                            Console.WriteLine("enter start time : yyyy/MM/dd HH:mm");
                            long startTimeStamp = task.getTimestampFromDataAndTime(Console.ReadLine());
                            Console.WriteLine("enter end time : yyyy/MM/dd HH:mm");
                            long endTimeStamp = task.getTimestampFromDataAndTime(Console.ReadLine());
                            task newTaskForAdd = new task(newTaskName,Convert.ToInt32(task.taskidNumber()),newTaskDes,false,startTimeStamp,endTimeStamp,newTaskTag);
                            loggedInUserTask.addTask(newTaskForAdd,loggedInUser.getUsername());                   
                        }
                        if (userInputMenuL == 4)
                        {
                            Console.WriteLine("Enter task Id for delete");
                            int deleteTaskId = Convert.ToInt32(Console.ReadLine());
                            loggedInUserTask.deleteTask(deleteTaskId, loggedInUser.getUsername());
                        }
                        if (userInputMenuL == 5)
                        {
                            Console.WriteLine("Enter taskId for edit");
                            int taskIdnew = Convert.ToInt32(Console.ReadLine());
                            task editableTask = new task(taskIdnew, loggedInUser.getUsername());
                            Console.WriteLine("enter task name");
                            editableTask.namechange(Console.ReadLine());
                            Console.WriteLine("enter task des");
                            editableTask.task_discriptionchange(Console.ReadLine());
                            Console.WriteLine("enter task tag");
                            editableTask.tagchange(Console.ReadLine());
                            Console.WriteLine("enter start time : yyyy/MM/dd HH:mm");
                            long startTimeStamp = task.getTimestampFromDataAndTime(Console.ReadLine());
                            editableTask.dtstartchange(startTimeStamp);
                            Console.WriteLine("enter end time : yyyy/MM/dd HH:mm");
                            long endTimeStamp = task.getTimestampFromDataAndTime(Console.ReadLine());
                            editableTask.dtendchange(endTimeStamp);
                            loggedInUserTask.updateTask(editableTask, loggedInUser.getUsername());
                        }
                        
                    }
                }
            }
        }
    }
}

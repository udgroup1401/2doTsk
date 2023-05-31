using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace toDoList
{
    class user
    {
        protected string username;
        protected string password;
        protected bool loggedIn;
        public user(string u,string p){
            this.username = u;
            this.password = p;
            this.loggedIn = false;
        }
        public void register(){
            List<string[]> allUsersAndPassword = new List<string[]>();
            string jsonContent = File.ReadAllText("db.json");
            dynamic dbFile = JsonConvert.DeserializeObject(jsonContent);
            foreach (var user in dbFile.users){
                allUsersAndPassword.Add(new string[] { user.username.ToString(), user.password.ToString() });
            }
            bool isValidUser = allUsersAndPassword.Exists(arr => arr[0] == this.username && arr[1] == this.password);
            if(isValidUser == false){
                dynamic newResultToAdd = new{
                    username = this.username,
                    password = this.password,
                    task = new List<string>()
                };
                dbFile.users.Add(newResultToAdd);
                string updatedJsonContent = JsonConvert.SerializeObject(dbFile, Formatting.Indented);
                File.WriteAllText("db.json", updatedJsonContent);
                this.loggedIn = true;
            }
        } 
        public void login(){
            List<string[]> allUsersAndPassword = new List<string[]>();
            string jsonContent = File.ReadAllText("db.json");
            dynamic dbFile = JsonConvert.DeserializeObject(jsonContent);
            foreach (var user in dbFile.users){
                allUsersAndPassword.Add(new string[] { user.username.ToString(), user.password.ToString() });
            }
            bool isValidUser = allUsersAndPassword.Exists(arr => arr[0] == this.username && arr[1] == this.password);
            this.loggedIn = isValidUser;
        }
        

        public string getUsername(){ return this.username; }
        public bool isLogin(){ return this.loggedIn; }
    }
    class task
    {
        protected bool is_done;
    }
    class userTask
    {
        protected string order;

        public userTask(string order)
        {



        }
    }
    class program{
        static void Main(string[] args){
            //Console.Clear();
            Console.Write("USER? ");
            string fuser = Console.ReadLine();
            Console.Write("PASS? ");
            string fpass = Console.ReadLine();
            user loggedInUser = new user(fuser,fpass);
            while(true){
                if(loggedInUser.isLogin() == false){
                    Console.WriteLine("WELCOME\n\n1. register user\n2. login user\n3. exit");
                    int userInputMenu = Convert.ToInt32(Console.ReadLine());
                    if(userInputMenu == 1){
                        loggedInUser.register();
                        Console.WriteLine("try to register");
                    }
                    if(userInputMenu == 2){
                        loggedInUser.login();
                        Console.WriteLine("Try to login");
                    }
                    if(userInputMenu == 3){
                        Console.WriteLine("EXIT PROGRAM");
                        Environment.Exit(0);
                    }
                }else{
                    Console.WriteLine($"Logged in as {loggedInUser.getUsername()}");
                    Console.ReadLine();
                }
            }
        }
    }
}
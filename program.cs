using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace toDoList{
    class user{
        protected string username;
        protected string password;
        protected bool loggedIn;
        public user(string u,string p){
            this.username = u;
            this.password = p;
            this.loggedIn = false;
        }
        public void register(){
            string jsonContent = File.ReadAllText("db.json");
            JObject dbFile = JObject.Parse(jsonContent);
            List<string[]> allUsersAndPassword = new List<string[]>();
            foreach (var user in dbFile["users"]){
                allUsersAndPassword.Add(new string[] { user["username"].ToString(), user["password"].ToString() });
            }
            bool isValidUser = allUsersAndPassword.Exists(arr => arr[0] == this.username);
            if(isValidUser == false){
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
        public void logout(){ this.loggedIn = false; }
    }
    class task
    {
        protected bool is_done;
    }
    class userTask{
        protected string task;
    }
    class program{
        static void Main(string[] args){
            //Console.Clear();
            while(true){
                Console.Write("USER? ");
                string fuser = Console.ReadLine();
                Console.Write("PASS? ");
                string fpass = Console.ReadLine();
                user loggedInUser = new user(fuser,fpass);
                while(true){
                    if(loggedInUser.isLogin() == false){
                        Console.WriteLine("WELCOME\n\n1. register user\n2. login user\n3. change user\n4. exit");
                        int userInputMenu = Convert.ToInt32(Console.ReadLine());
                        if(userInputMenu == 1){
                            loggedInUser.register();
                            Console.WriteLine("try to register");
                        }
                        if(userInputMenu == 2){
                            loggedInUser.login();
                            Console.WriteLine("Try to login");
                        }
                        if(userInputMenu == 3){ break; }
                        if(userInputMenu == 4){
                            Console.WriteLine("EXIT PROGRAM");
                            Environment.Exit(0);
                        }
                    }else{
                        Console.WriteLine($"Logged in as {loggedInUser.getUsername()}\n1. logout");
                        int userInputMenuL = Convert.ToInt32(Console.ReadLine());
                        if(userInputMenuL == 1){ loggedInUser.logout(); }
                    }
                }
            }
        }
    }
}
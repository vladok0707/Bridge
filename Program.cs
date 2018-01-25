using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using WebSocket4Net;
using System.IO;

namespace Kek
{
    [DataContract]
    class typemsg
    {
        [DataMember]
        public string msg;
        [DataMember]
        public string from;
        public void display()
        {
            Console.WriteLine($"\t\t{from}: {msg}");
        }
    }



    [DataContract]
    class typedata
    {
        [DataMember]
       public  string command;
        [DataMember]
       public string msg;
        [DataMember]
       public string[] data;
        [DataMember]
       public string target;
        public void display()
        {
            Console.WriteLine("\tdata:");
            Console.WriteLine($"\tcommand:{this.command}");
            if (msg!=null)
            {
                Console.WriteLine($"\tmsg:{this.msg}");
            }
            for (int i = 0; i < data.Length; i++)
            {
                if (this.command!="updmsg")
                Console.WriteLine($"\tdata {i}:{this.data[i]}");
                else
                {
                    typemsg tmsg = new typemsg();
                    tmsg = (typemsg)Dessir.DeserializeObject(data[i], tmsg.GetType());
                    tmsg.display();
                }
            }
            if (target != null)
            {
                Console.WriteLine($"\tmsg:{this.target}");
            }
            
            Console.WriteLine("\tEndofdata");
        }
    }
    static class Dessir
    {
        public static object DeserializeObject(string json, Type type)
        {
            var serializer = new DataContractJsonSerializer(type);

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return serializer.ReadObject(ms);
            };
        }
    }
    class Controller
    {
        public typedata command()
        {
            typedata a = new typedata();
            Console.Write("Команда:");
            a.command = Console.ReadLine();
            if (a.command == "reguser")
            {
                Console.Write("Логин:");
                a.data = new string[2];
                a.data[0] = Console.ReadLine();
                Console.Write("Пароль:");
                a.data[1] = Console.ReadLine();
            }
            else if (a.command == "authuser")
            {
                Console.Write("Логин:");
                a.data = new string[2];
                a.data[0] = Console.ReadLine();
                Console.Write("Пароль:");
                a.data[1] = Console.ReadLine();
            }
            else if (a.command == "makegr")
            {
                Console.Write("Логин:");
                a.msg = Console.ReadLine();
                Console.Write("Название группы:");
                a.target = Console.ReadLine();
            }
            else if (a.command == "joingr")
            {
                Console.Write("Логин:");
                a.msg = Console.ReadLine();
                Console.Write("Название группы:");
                a.target = Console.ReadLine();
            }
            else if (a.command == "leavegr")
            {
                Console.Write("Логин:");
                a.msg = Console.ReadLine();
                Console.Write("Название группы:");
                a.target = Console.ReadLine();
            }
            else if (a.command == "sendmsg")
            {
                Console.Write("Логин:");
                a.data = new string[1];
                a.data[0] = Console.ReadLine();
                Console.Write("Название группы:");
                a.target = Console.ReadLine();
                Console.Write("Сообщение:");
                a.msg = Console.ReadLine();
            }
            else if (a.command == "updmsg")
            {
                Console.Write("Название группы:");
                a.target = Console.ReadLine();
            }
            else if (a.command == "enduser")
            {
                Console.Write("Логин:");
                a.msg = Console.ReadLine();
            }
            else if (a.command == "exit") Environment.Exit(0);
            return a;

        }        
    }
  class kek
    {
        WebSocket websocket = new WebSocket("ws://gndlfserverbd.herokuapp.com");
        public kek()
        {
            websocket.Opened += new EventHandler(websocket_Opened);
            
            websocket.Open();
            
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocketmessage);
           
        }
        private void websocket_Opened(object sender, EventArgs e)
        {
            websocket.Send("Hello World!");
        }
        private void websocketmessage(object sender, WebSocket4Net.MessageReceivedEventArgs e)
        {
            string s = e.Message;
            Console.WriteLine("мы получили сообщение");
            Console.WriteLine(s);
        }
    }
  class ser
    {
        WebSocket jsok = new WebSocket("ws://gndlfserverbd.herokuapp.com");
        private object a;
        public object _a { get; set; }
        public ser(object a)
        {

            jsok.Opened += new EventHandler(jsok_Opened);
            jsok.Open();
            _a = a;
            jsok.MessageReceived += new EventHandler<MessageReceivedEventArgs>(jsokmessage);

        }
        private void jsokmessage(object sender, WebSocket4Net.MessageReceivedEventArgs e)
        {
            string s = e.Message;
            Console.WriteLine("мы получили сообщение");
            typedata q = new typedata();
            q = (typedata)DeserializeObject(s, q.GetType());
            q.display();
        }
        private void jsok_Opened(object sender, EventArgs e)
        {
            string s = SerializeObject(_a);
            jsok.Send(s);
        }

        protected virtual string SerializeObject(object target)
        {
            var serializer = new DataContractJsonSerializer(target.GetType());

            string result;
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, target);
                result = Encoding.UTF8.GetString(ms.ToArray());
            };

            return result;
        }
        protected virtual object DeserializeObject(string json, Type type)
        {
            var serializer = new DataContractJsonSerializer(type);

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return serializer.ReadObject(ms);
            };
        }
        


    }
    class Program
    {

        
        static void Main(string[] args)
        {
            while (true)
            {

                Controller contr = new Controller();
                ser b = new ser(contr.command());
                Console.ReadKey();
            }
            
        }

       

        

    }
}

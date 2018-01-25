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
    class typedata
    {
        [DataMember]
       public  string command;
        [DataMember]
        string msg;
        [DataMember]
        string[] data;
        [DataMember]
        string target;
    }
  class kek
    {
        WebSocket websocket = new WebSocket("ws://gndlfserver.herokuapp.com");
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
        WebSocket jsok = new WebSocket("ws://gndlfserver.herokuapp.com");
        private object a;
        public object _a { get; set; }
        public ser(object a)
        {

            jsok.Opened += new EventHandler(jsok_Opened);
            jsok.Open();
            _a = a;
            

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
        /*protected virtual object DeserializeObject(string json, Type type)
        {
            var serializer = new DataContractJsonSerializer(type);

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return serializer.ReadObject(ms);
            };
        }*/



    }
    class Program
    {

        
        static void Main(string[] args)
        {
            typedata q = new typedata();
            q.command = "asdd";
            //kek a = new kek();
            ser b = new ser(q);
            Console.ReadKey();
        }

       

        

    }
}

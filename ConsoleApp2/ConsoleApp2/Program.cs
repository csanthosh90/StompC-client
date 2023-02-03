using System;
using WebSocketSharp;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            string hostname = "localhost";
            int port = 8083;
            using (var ws = new WebSocket(string.Format("ws://{0}:{1}/api/socket/websocket", hostname, port)))
            {

                string userId = "1";
                int tableId = 1;

                ws.OnOpen += (sender, e) =>
                {
                    Console.WriteLine("Spring says: open");
                    StompMessageSerializer serializer = new StompMessageSerializer();

                    var connect = new StompMessage("CONNECT");
                    connect["accept-version"] = "1.1";
                    connect["heart-beat"] = "10000,10000";
                    connect["userId"] = userId;
                    ws.Send(serializer.Serialize(connect));



                    var sub = new StompMessage("SUBSCRIBE");
                    sub["id"] = "sub-" + tableId;
                    sub["destination"] = "/user/notification/" + tableId;
                    ws.Send(serializer.Serialize(sub));

                    /*sub = new StompMessage("UNSUBSCRIBE");
                    sub["id"] = "sub-" + clientId;
                    sub["destination"] = "/user/notification/" + clientId;
                    ws.Send(serializer.Serialize(sub));*/

                };

                ws.OnError += (sender, e) =>
                Console.WriteLine("Error: " + e.Message);
                ws.OnMessage += (sender, e) =>
                Console.WriteLine("Spring says: " + e.Data);

                ws.Connect();

                Console.ReadKey(true);
            }
        }
    }
}

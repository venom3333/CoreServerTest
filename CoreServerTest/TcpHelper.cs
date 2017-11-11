using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CoreServerTest
{
    class TcpHelper
    {
        private static TcpListener Listener { get; set; }
        private static bool Accept { get; set; } = false;

        public static void StartServer(int port)
        {
            //IPAddress address = IPAddress.Parse("127.0.0.1");
            IPAddress address = IPAddress.Any;

            Listener = new TcpListener(address, port);
            Listener.Start();
            Accept = true;

            Console.WriteLine($"Server started. Listening to TCP clients at 127.0.0.1:{port}");
        }

        public static void Listen()
        {
            if (Listener != null && Accept)
            {
                // Continue listening.  
                while (true)
                {
                    Console.WriteLine("Waiting for client...");
                    var clientTask = Listener.AcceptTcpClientAsync(); // Get the client  

                    if (clientTask.Result != null)
                    {
                        Console.WriteLine("Client connected. Waiting for data.");
                        var client = clientTask.Result;
                        string message = "";

                        while (message != null && !message.StartsWith("quit"))
                        {
                            byte[] data = Encoding.ASCII.GetBytes("Send next data: [enter 'quit' to terminate] ");
                            client.GetStream().Write(data, 0, data.Length);

                            byte[] buffer = new byte[1024];
                            client.GetStream().Read(buffer, 0, buffer.Length);

                            message = Encoding.ASCII.GetString(buffer);
                            Console.WriteLine(message.TrimEnd('\0').TrimEnd('\n'));
                        }
                        Console.WriteLine("Closing connection.");
                        client.GetStream().Dispose();
                    }
                }
            }
        }
    }
}

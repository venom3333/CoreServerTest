using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CoreServerTest
{
    internal static class TcpHelper
    {
        private static TcpListener Listener { get; set; }
        private static bool Accept { get; set; }

        public static void StartServer(int port)
        {
            var address = IPAddress.Any;

            Listener = new TcpListener(address, port);
            Listener.Start();
            Accept = true;

            Console.WriteLine($"Server started. Listening to TCP clients at {address}:{port}");
        }

        public static void Listen()
        {
            if (Listener == null || !Accept) return;
            
            // Continue listening.  
            while (true)
            {
                Console.WriteLine("Waiting for client...");
                var clientTask = Listener.AcceptTcpClientAsync(); // Get the client  

                if (clientTask.Result == null) continue;
                
                Console.WriteLine("Client connected. Waiting for data.");
                var client = clientTask.Result;
                var message = "";

                while (!message.StartsWith("quit"))
                {
                    var data = Encoding.ASCII.GetBytes("Send next data: [enter 'quit' to terminate] ");
                    client.GetStream().Write(data, 0, data.Length);

                    var buffer = new byte[1024];
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

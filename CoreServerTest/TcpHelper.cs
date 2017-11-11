using System;
using System.Globalization;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

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

                var clientTask = Listener.AcceptTcpClientAsync();

                if (clientTask.Result == null) continue;

                new Task(delegate { WorkWithClient(clientTask.Result); }).Start();
            }
        }

        private static void WorkWithClient(TcpClient client)
        {
            //var client = clientTask.Result;
            Console.WriteLine(
                $"New client connected ({DateTime.Now.ToString(CultureInfo.CurrentCulture)}). Waiting for data...");
            var message = "";

            var data = Encoding.ASCII.GetBytes(
                $"Connected to server: {Listener.LocalEndpoint.AddressFamily.ToString()}\n");
            client.GetStream().Write(data, 0, data.Length);

            while (!message.StartsWith("quit"))
            {
                data = Encoding.ASCII.GetBytes("Send next data ('quit' to terminate connection): ");
                client.GetStream().Write(data, 0, data.Length);

                var buffer = new byte[client.ReceiveBufferSize];
                client.GetStream().Read(buffer, 0, client.ReceiveBufferSize);

                message = Encoding.ASCII.GetString(buffer);
                message = message.TrimEnd('\0').TrimEnd('\n');
                Console.WriteLine(message);
            }
            Console.WriteLine("Closing connection.");
            client.GetStream().Dispose();
            client.Dispose();
        }
    }
}
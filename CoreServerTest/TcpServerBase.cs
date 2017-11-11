using System;
using System.Globalization;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CoreServerTest
{
    internal static class TcpServerBase
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
                $"New client connected ({client.Client.RemoteEndPoint} ({client.Client.ProtocolType}) - {DateTime.Now.ToString(CultureInfo.CurrentCulture)}). Waiting for data...");
            var message = "";

            var data = Encoding.ASCII.GetBytes(
                $"Connected to server: {client}\n");
            client.GetStream().Write(data, 0, data.Length);

            while (!message.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                data = Encoding.ASCII.GetBytes("Send next data ('quit' to terminate connection): ");
                client.GetStream().Write(data, 0, data.Length);

                var buffer = new byte[client.ReceiveBufferSize];

                try
                {
                    client.GetStream().Read(buffer, 0, client.ReceiveBufferSize);
                }
                catch
                {
                    break;
                }

                message = Encoding.ASCII.GetString(buffer);
                message = message.TrimEnd('\0').TrimEnd('\n');
                Console.WriteLine($"{client.Client.RemoteEndPoint}: {message}");
            }
            Console.WriteLine($"Closing connection of {client.Client.RemoteEndPoint}");
            client.Dispose();
        }
    }
}
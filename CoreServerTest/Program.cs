
namespace CoreServerTest
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var port = 5678;
            if (args.Length > 0)
            {
                port = int.TryParse(args[0], out var parse)? parse : port;
            }
            // Start the server  
            TcpServerBase.StartServer(port);
            TcpServerBase.Listen(); // Start listening.  
        }
    }
}

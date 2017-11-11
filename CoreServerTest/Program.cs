
namespace CoreServerTest
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                // Do something
            }
            // Start the server  
            TcpHelper.StartServer(5678);
            TcpHelper.Listen(); // Start listening.  
        }
    }
}

using System;

namespace CoreServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start the server  
            TcpHelper.StartServer(5678);
            TcpHelper.Listen(); // Start listening.  
        }
    }
}

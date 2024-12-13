using CommonUtil.IO.SocketUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTester.Services
{
    public class SocketClientManager
    {
        private int MaxClients = 100;
        
        private Dictionary<int, AsyncSocketClient> SocketClients;

        public SocketClientManager() 
        {
        
        }
 
        //소켓을 생성하고, Connection 하는 함수

        public void TryConnect(string host, int port)
        {

        }
    




    
    }
}

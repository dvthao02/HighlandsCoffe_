﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLandsCoffee_Manager.DTO
{
    public class DTO_ConnectServer
    {
        private string tenServer;
        //private string databaseName;
        private string userName;
        private string pass;

        public string TenServer { get => tenServer; set => tenServer = value; }
        //public string DatabaseName { get => databaseName; set => databaseName = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Pass { get => pass; set => pass = value; }

        public DTO_ConnectServer()
        {

        }

        public DTO_ConnectServer(string tenServer, string userName, string pass)
        {
            TenServer = tenServer;
           // DatabaseName = databaseName;
            UserName = userName;
            Pass = pass;
        }
    }
}

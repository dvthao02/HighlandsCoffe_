using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLandsCoffee_Manager.DTO
{
    public class DTO_Login
    {
        private string pass;
        private string userName;

        public string UserName { get => userName; set => userName = value; }
        public string Pass { get => pass; set => pass = value; }

        public DTO_Login()
        {

        }

        public DTO_Login(string userName, string pass)
        {
            UserName = userName;
            Pass = pass;
        }
    }
}

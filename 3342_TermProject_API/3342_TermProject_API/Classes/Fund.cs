using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3342_TermProject_API.Classes
{
    public class Fund
    {
        private string email = "";
        private double amount = 0.0;

        public Fund()
        {

        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public double Amount
        {
            get { return amount; }
        set { amount = value; }
        }
    }
}

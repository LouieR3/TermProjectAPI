using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3342_TermProject_API.Classes
{
    public class Transaction
    {
        private int walletID = 0; 
        private double amount = 0.0;
        private string type = "";
        private int cardNumber = 0;
        private int merchantID = 0;

        public Transaction()
        {

        }
        public int WalletID
        {
            get { return walletID; }
            set { walletID = value; }
        }
        public double Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public string Type
        {
            get{ return type; }
            set { type = value; }
        }
        public int CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }
        public int MerchantID
        {
            get { return merchantID; }
            set { merchantID = value; }
        }
    }
}

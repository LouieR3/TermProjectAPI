using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3342_TermProject_API.Classes
{
    public class Payment
    {
        string paymentReciever = "";
        string paymentSender = "";
        string paymentType = "";
        float paymentAmount;
        int cardNumber = 0;

        public Payment()
        {

        }
        public string Reciever
        {
            get { return paymentReciever; }
            set { paymentReciever = value; }
        }
        public string Sender
        {
            get { return paymentSender; }
            set { paymentSender = value; }
        }
        public string Type
        {
            get { return paymentType; }
            set { paymentType = value; }
        }
        public float Amount
        {
            get { return paymentAmount; }
            set { paymentAmount = value; }
        }
        public int CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }

    }
}

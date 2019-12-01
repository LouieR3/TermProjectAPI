using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using _3342_TermProject_API.Classes;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlTypes;
using System.Collections;
using System.Text;

namespace _3342_TermProject_API.Controllers
{
    [Produces("application/json")]
    [Route("api/service/PaymentProcessor")]
    [ApiController]
    public class MerchantTransactionController : ControllerBase
    {
        Validation validate = new Validation();
        DBConnect db = new DBConnect();
        SqlCommand dbCommand = new SqlCommand();
        private int length = 10;
        [HttpPost("CreateVirtualWallet/{APIKey}")] //Working
        public Boolean CreateVirtualWallet(string APIKey, [FromBody] Wallet AccountHolderInformation)
        {
            int exists = validate.checkAPIKey(APIKey);
            Boolean virtualWalletID = false;
            if (exists == 1)
            {
                string name = AccountHolderInformation.Name.ToString();
                string address = AccountHolderInformation.Address.ToString();
                string email = AccountHolderInformation.Email.ToString();
                string bankName = AccountHolderInformation.BankName.ToString();
                string cardType = AccountHolderInformation.CardType.ToString();
                int cardNumber = int.Parse(AccountHolderInformation.CardNumber.ToString());
                int merchantAccountID = int.Parse(AccountHolderInformation.MerchantAccountID.ToString());

                dbCommand.Parameters.Clear();
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "TP_CreateVirtualWallet";

                SqlParameter inputName = new SqlParameter("@Name", name);
                SqlParameter inputAddress = new SqlParameter("@Address", address);
                SqlParameter inputEmail = new SqlParameter("@Email", email);
                SqlParameter inputBankName = new SqlParameter("@BankName", bankName);
                SqlParameter inputCardType = new SqlParameter("@CardType", cardType);
                SqlParameter inputCardNumber = new SqlParameter("@CardNumber", cardNumber);
                SqlParameter inputMerchantAccountID = new SqlParameter("@MerchantAccountID", merchantAccountID);

                inputName.Direction = ParameterDirection.Input;
                inputName.SqlDbType = SqlDbType.VarChar;
                inputAddress.Direction = ParameterDirection.Input;
                inputAddress.SqlDbType = SqlDbType.VarChar;
                inputEmail.Direction = ParameterDirection.Input;
                inputEmail.SqlDbType = SqlDbType.VarChar;
                inputBankName.Direction = ParameterDirection.Input;
                inputBankName.SqlDbType = SqlDbType.VarChar;
                inputCardType.Direction = ParameterDirection.Input;
                inputCardType.SqlDbType = SqlDbType.VarChar;
                inputCardNumber.Direction = ParameterDirection.Input;
                inputCardNumber.SqlDbType = SqlDbType.Int;
                inputMerchantAccountID.Direction = ParameterDirection.Input;
                inputMerchantAccountID.SqlDbType = SqlDbType.Int;

                dbCommand.Parameters.Add(inputName);
                dbCommand.Parameters.Add(inputAddress);
                dbCommand.Parameters.Add(inputEmail);
                dbCommand.Parameters.Add(inputBankName);
                dbCommand.Parameters.Add(inputCardType);
                dbCommand.Parameters.Add(inputCardNumber);
                dbCommand.Parameters.Add(inputMerchantAccountID);
                int count = db.DoUpdateUsingCmdObj(dbCommand);
                if (count == 1)
                {
                    virtualWalletID = true;
                }
            }

            return virtualWalletID ;
        }
        // GET: api/MerchantTransaction
        [HttpGet("GetTransactions/{VirtualWalletID}/{MerchantAccountID}/{APIKey}")]
        public DataSet GetTransactions(int VirtualWalletID,int MerchantAccountID, string APIKey)
        {
            DataSet ds;
            int exists = validate.checkAPIKey(APIKey);
            if (exists == 1)
            {
                dbCommand.Parameters.Clear();
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "TP_GetTransactions";

                SqlParameter inputVirtualWalletID = new SqlParameter("@WalletID", VirtualWalletID);
                SqlParameter inputMerchantAccountID = new SqlParameter("@MerchantAccountID", MerchantAccountID);

                inputVirtualWalletID.Direction = ParameterDirection.Input;
                inputVirtualWalletID.SqlDbType = SqlDbType.VarChar;
                inputMerchantAccountID.Direction = ParameterDirection.Output;
                inputMerchantAccountID.SqlDbType = SqlDbType.Int;

                dbCommand.Parameters.Add(inputVirtualWalletID);
                dbCommand.Parameters.Add(inputMerchantAccountID);

                ds = db.GetDataSetUsingCmdObj(dbCommand);
                return ds;
            }
            else
            {
                return ds = null;
            }
        }
        [HttpPost("ProcessPayment/{VirtualWalletID}/{Amount}/{Type}/{MerchantAccountID}/{APIKey}")]
        public int ProcessPayment(int VirtualWalletIDReciever, int VirtualWalletIDSender, double Amount, int MerchantAccountID, string APIKey)
        {
            int exists = validate.checkAPIKey(APIKey);
            int count = 0;
            if (exists == 1)
            {
                dbCommand.Parameters.Clear();
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "TP_ProcessPayment";

                SqlParameter inputVirtualWalletIDReciever = new SqlParameter("@VirtualWalletIDReciever", VirtualWalletIDReciever);
                SqlParameter inputVirtualWalletIDSender = new SqlParameter("@VirtualWalletIDSender", VirtualWalletIDSender);
                SqlParameter inputMerchantAccountID = new SqlParameter("@VirtualWalletIDSender", VirtualWalletIDSender);
                SqlParameter inputAmount = new SqlParameter("@Amount", Amount);

                inputVirtualWalletIDReciever.Direction = ParameterDirection.Input;
                inputVirtualWalletIDReciever.SqlDbType = SqlDbType.VarChar;
                inputVirtualWalletIDSender.Direction = ParameterDirection.Input;
                inputVirtualWalletIDSender.SqlDbType = SqlDbType.Int;
                inputMerchantAccountID.Direction = ParameterDirection.Input;
                inputMerchantAccountID.SqlDbType = SqlDbType.Int;
                inputAmount.Direction = ParameterDirection.Input;
                inputAmount.SqlDbType = SqlDbType.Int;

                dbCommand.Parameters.Add(inputVirtualWalletIDReciever);
                dbCommand.Parameters.Add(inputVirtualWalletIDSender);
                dbCommand.Parameters.Add(inputMerchantAccountID);
                dbCommand.Parameters.Add(inputAmount);

                count = db.DoUpdateUsingCmdObj(dbCommand);
            }
            return count;
        }
        [HttpPut("UpdatePaymentAccount/{APIKey}")]//Working
        public Boolean UpdatePaymentAccount(string APIKey,[FromBody] Wallet AccountHolderInformation)
        {
            Boolean success = false;           
            int exists = validate.checkAPIKey(APIKey);
            int count = 0;
            if (exists == 1)
            {
                string name = AccountHolderInformation.Name.ToString();
                string address = AccountHolderInformation.Address.ToString();
                string email = AccountHolderInformation.Email.ToString();
                string bankName = AccountHolderInformation.BankName.ToString();
                string cardType = AccountHolderInformation.CardType.ToString();
                int cardNumber = int.Parse(AccountHolderInformation.CardNumber.ToString());
                int MerchantAccountID = int.Parse(AccountHolderInformation.MerchantAccountID.ToString());

                dbCommand.Parameters.Clear();
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "TP_UpdatePaymentAccount";

                SqlParameter inputName = new SqlParameter("@Name", name);
                SqlParameter inputAddress = new SqlParameter("@Address", address);
                SqlParameter inputEmail = new SqlParameter("@Email", email);
                SqlParameter inputBankName = new SqlParameter("@BankName", bankName);
                SqlParameter inputCardType = new SqlParameter("@CardType", cardType);
                SqlParameter inputCardNumber = new SqlParameter("@CardNumber", cardNumber);
                SqlParameter inputMerchantAccountID = new SqlParameter("@MerchantAccountID", MerchantAccountID);

                inputName.Direction = ParameterDirection.Input;
                inputName.SqlDbType = SqlDbType.VarChar;
                inputAddress.Direction = ParameterDirection.Input;
                inputAddress.SqlDbType = SqlDbType.VarChar;
                inputEmail.Direction = ParameterDirection.Input;
                inputEmail.SqlDbType = SqlDbType.VarChar;
                inputBankName.Direction = ParameterDirection.Input;
                inputBankName.SqlDbType = SqlDbType.VarChar;
                inputCardType.Direction = ParameterDirection.Input;
                inputCardType.SqlDbType = SqlDbType.VarChar;
                inputCardNumber.Direction = ParameterDirection.Input;
                inputCardNumber.SqlDbType = SqlDbType.Int;
                inputMerchantAccountID.Direction = ParameterDirection.Input;
                inputMerchantAccountID.SqlDbType = SqlDbType.Int;

                dbCommand.Parameters.Add(inputName);
                dbCommand.Parameters.Add(inputAddress);
                dbCommand.Parameters.Add(inputEmail);
                dbCommand.Parameters.Add(inputBankName);
                dbCommand.Parameters.Add(inputCardType);
                dbCommand.Parameters.Add(inputCardNumber);
                dbCommand.Parameters.Add(inputMerchantAccountID);

                count = db.DoUpdateUsingCmdObj(dbCommand);
                if(count >= 1)
                {
                    success = true;
                }
            }
            return success;
        }
        [HttpPut("FundAccount/{Email}/{Amount}/{MerchantAccountID}/{APIKey}")] //Working
        public Boolean FundAccount(string Email, double Amount, int MerchantAccountID, string APIKey)
        {
            Boolean success = false;
            int exists = validate.checkAPIKey(APIKey);
            int count = 0;
            if (exists == 1)
            {
                dbCommand.Parameters.Clear();
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "TP_FundAccount";

                SqlParameter inputEmail = new SqlParameter("@Email", Email);
                SqlParameter inputAmount = new SqlParameter("@Amount", Amount);
                SqlParameter inputMerchantAccountID = new SqlParameter("@MerchantAccountID", MerchantAccountID);

                inputEmail.Direction = ParameterDirection.Input;
                inputEmail.SqlDbType = SqlDbType.VarChar;
                inputAmount.Direction = ParameterDirection.Input;
                inputAmount.SqlDbType = SqlDbType.Float;
                inputMerchantAccountID.Direction = ParameterDirection.Input;
                inputMerchantAccountID.SqlDbType = SqlDbType.Int;

                dbCommand.Parameters.Add(inputEmail);
                dbCommand.Parameters.Add(inputAmount);
                dbCommand.Parameters.Add(inputMerchantAccountID);

                count = db.DoUpdateUsingCmdObj(dbCommand);
                if(count == 1)
                {
                    success = true;
                }
            }
            return success;
        } 
        [HttpGet("GetBalance/{Email}/{MerchantAccountID}/{APIKey}")]
	    public double GetBalance(string Email, int MerchantAccountID, string APIKey)
        {
            double balance = 0.0;
            int exists = validate.checkAPIKey(APIKey);
            if (exists == 1)
            {
                dbCommand.Parameters.Clear();
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "TP_GetWalletBalance";

                SqlParameter inputWalletID = new SqlParameter("@Email", Email);
                SqlParameter inputMerchantAccountID = new SqlParameter("@MerchantAccountID", MerchantAccountID);
                SqlParameter outputBalance = new SqlParameter("@Balance", 0);

                inputWalletID.Direction = ParameterDirection.Input;
                inputWalletID.SqlDbType = SqlDbType.VarChar;
                inputMerchantAccountID.Direction = ParameterDirection.Input;
                inputMerchantAccountID.SqlDbType = SqlDbType.Int;
                outputBalance.Direction = ParameterDirection.Output;
                outputBalance.SqlDbType = SqlDbType.Float;

                dbCommand.Parameters.Add(inputWalletID);
                dbCommand.Parameters.Add(inputMerchantAccountID);
                dbCommand.Parameters.Add(outputBalance);

                db.GetDataSetUsingCmdObj(dbCommand);
                balance = double.Parse(dbCommand.Parameters["@Balance"].Value.ToString());
            }
            return balance;
        }
        [HttpPost("CreateMerchant/{Name}/{Email}")] //Working
        public string CreateMerchant(string Name, string Email)
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            string apiKey = result.ToString();
            int insert = 0;
                dbCommand.Parameters.Clear();
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "TP_CreateMerchant";

                SqlParameter inputMerchantName = new SqlParameter("@Name", Name);
                SqlParameter inputMerchantEmail = new SqlParameter("@Email", Email);
                SqlParameter inputAPIKey = new SqlParameter("@APIKey", apiKey);

                inputMerchantName.Direction = ParameterDirection.Input;
                inputMerchantName.SqlDbType = SqlDbType.VarChar;
                inputMerchantEmail.Direction = ParameterDirection.Input;
                inputMerchantEmail.SqlDbType = SqlDbType.VarChar;
                inputAPIKey.Direction = ParameterDirection.Input;
                inputAPIKey.SqlDbType = SqlDbType.VarChar;

                dbCommand.Parameters.Add(inputMerchantName);
                dbCommand.Parameters.Add(inputMerchantEmail);
                dbCommand.Parameters.Add(inputAPIKey);
                insert = db.DoUpdateUsingCmdObj(dbCommand);
            
            if(insert == 1)
            {
                return "The merchant was created"; 
            }
            else
            {
                return "The merchant was NOT created.";
            }
        }

    }
}

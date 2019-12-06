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

            return virtualWalletID;
        }
        // GET: api/MerchantTransaction
        [HttpGet("GetTransactions/{Email}/{MerchantAccountID}/{APIKey}")] //Working
        public List<Transaction> GetTransactions(string Email, int MerchantAccountID, string APIKey)
        {
            List<Transaction> transactions = new List<Transaction>();
            int exists = validate.checkAPIKey(APIKey);
            if (exists == 1)
            {
                dbCommand.Parameters.Clear();
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "TP_GetTransactions";

                SqlParameter inputVirtualWalletID = new SqlParameter("@Email", Email);
                SqlParameter inputMerchantAccountID = new SqlParameter("@MerchantAccountID", MerchantAccountID);

                inputVirtualWalletID.Direction = ParameterDirection.Input;
                inputVirtualWalletID.SqlDbType = SqlDbType.VarChar;
                inputMerchantAccountID.Direction = ParameterDirection.Input;
                inputMerchantAccountID.SqlDbType = SqlDbType.Int;

                dbCommand.Parameters.Add(inputVirtualWalletID);
                dbCommand.Parameters.Add(inputMerchantAccountID);

                DataSet ds = db.GetDataSetUsingCmdObj(dbCommand);
                int count = ds.Tables[0].Rows.Count;
                for(int i=0; i < count; i++)
                {
                    Transaction tran = new Transaction();
                    tran.Email = db.GetField("Email", i).ToString();
                    tran.TransactionID = int.Parse(db.GetField("Transaction_ID", i).ToString());
                    tran.Type = db.GetField("Transaction_Type", i).ToString();
                    tran.CardNumber = int.Parse(db.GetField("Card_Number", i).ToString());
                    tran.Amount = float.Parse(db.GetField("Transaction_Amount", i).ToString());
                    tran.MerchantID = int.Parse(db.GetField("Merchant_ID", i).ToString());
                    transactions.Add(tran);
                }
                return transactions;
            }
            else
            {
                return transactions = null;
            }
        }
        [HttpGet ("GetAccountInformation/{Email}/{MerchantAccountID}/{APIKey}")]//working
        public Wallet GetAccountInformation(string Email, int MerchantAccountID, string APIKey)
        {
            Wallet wallet = new Wallet();
            int exists = validate.checkAPIKey(APIKey);
            if (exists == 1)
            {
                dbCommand.Parameters.Clear();
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "TP_GetAccountInfo";

                SqlParameter inputVirtualWalletID = new SqlParameter("@Email", Email);
                SqlParameter inputMerchantAccountID = new SqlParameter("@MerchantAccountID", MerchantAccountID);

                inputVirtualWalletID.Direction = ParameterDirection.Input;
                inputVirtualWalletID.SqlDbType = SqlDbType.VarChar;
                inputMerchantAccountID.Direction = ParameterDirection.Input;
                inputMerchantAccountID.SqlDbType = SqlDbType.Int;

                dbCommand.Parameters.Add(inputVirtualWalletID);
                dbCommand.Parameters.Add(inputMerchantAccountID);

                db.GetDataSetUsingCmdObj(dbCommand);
                wallet.Name = db.GetField("Name",0).ToString();
                wallet.Address = db.GetField("Address", 0).ToString();
                wallet.Email = Email;
                wallet.BankName = db.GetField("Bank_Name", 0).ToString();
                wallet.CardType = db.GetField("Card_Type", 0).ToString();
                wallet.CardNumber =int.Parse( db.GetField("Card_Number", 0).ToString());
                wallet.MerchantAccountID = MerchantAccountID;
                return wallet;
            }
            else
            {
                return wallet = null;
            }
        }
        [HttpPost("ProcessPayment/{MerchantAccountID}/{APIKey}")] //Working
        public Boolean ProcessPayment(int MerchantAccountID, string APIKey, [FromBody] Payment paymentInfo)
        {
            Boolean success = false;
            int exists = validate.checkAPIKey(APIKey);
            int count = 0;
            if (exists == 1)
            {
                dbCommand.Parameters.Clear();
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "TP_ProcessPayment";

                SqlParameter inputEmailReciever = new SqlParameter("@EmailReciever", paymentInfo.Reciever);
                SqlParameter inputEmailSender = new SqlParameter("@EmailSender", paymentInfo.Sender);
                SqlParameter inputMerchantAccountID = new SqlParameter("@MerchantAccountID", MerchantAccountID);
                SqlParameter inputType = new SqlParameter("@Type", paymentInfo.Type);
                SqlParameter inputCardNumber = new SqlParameter("@CardNumber", paymentInfo.CardNumber);
                SqlParameter inputAmount = new SqlParameter("@Amount", paymentInfo.Amount);

                inputEmailReciever.Direction = ParameterDirection.Input;
                inputEmailReciever.SqlDbType = SqlDbType.VarChar;
                inputEmailSender.Direction = ParameterDirection.Input;
                inputEmailSender.SqlDbType = SqlDbType.VarChar;
                inputMerchantAccountID.Direction = ParameterDirection.Input;
                inputMerchantAccountID.SqlDbType = SqlDbType.Int;
                inputType.Direction = ParameterDirection.Input;
                inputType.SqlDbType = SqlDbType.VarChar;
                inputCardNumber.Direction = ParameterDirection.Input;
                inputCardNumber.SqlDbType = SqlDbType.Int;
                inputAmount.Direction = ParameterDirection.Input;
                inputAmount.SqlDbType = SqlDbType.Float;

                dbCommand.Parameters.Add(inputEmailReciever);
                dbCommand.Parameters.Add(inputEmailSender);
                dbCommand.Parameters.Add(inputMerchantAccountID);
                dbCommand.Parameters.Add(inputAmount);
                dbCommand.Parameters.Add(inputType);
                dbCommand.Parameters.Add(inputCardNumber);

                count = db.DoUpdateUsingCmdObj(dbCommand);
                if (count >= 1)
                {
                    success = true;
                }
            }
            return success;
        }
        [HttpPut("UpdatePaymentAccount/{APIKey}")]//Working
        public Boolean UpdatePaymentAccount(string APIKey, [FromBody] Wallet AccountHolderInformation)
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
                if (count >= 1)
                {
                    success = true;
                }
            }
            return success;
        }
        [HttpPut("FundAccount/{MerchantAccountID}/{APIKey}")] //Working
        public Boolean FundAccount(int MerchantAccountID, string APIKey, [FromBody] Fund fund)
        {
            Boolean success = false;
            int exists = validate.checkAPIKey(APIKey);
            int count = 0;
            if (exists == 1)
            {
                dbCommand.Parameters.Clear();
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandText = "TP_FundAccount";

                SqlParameter inputEmail = new SqlParameter("@Email", fund.Email.ToString());
                SqlParameter inputAmount = new SqlParameter("@Amount", double.Parse(fund.Amount.ToString()));
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
                if (count == 1)
                {
                    success = true;
                }
            }
            return success;
        }
        [HttpGet("GetBalance/{Email}/{MerchantAccountID}/{APIKey}")] //Working
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

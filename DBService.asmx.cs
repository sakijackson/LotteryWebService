using System;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services.Protocols;
using Newtonsoft.Json;

namespace LotteryWebService
{
    /// <summary>
    /// Summary description for DBService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DBService : System.Web.Services.WebService
    {
       public SqlConnection SqlCon;
       public SqlCommand SqlCmd;
       public SqlDataReader Sqldr;

        public DBService()
        {
            SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["LotteryDBCon"].ConnectionString);

        }
        [WebMethod]
        public bool InsertData(string FirstName, string LastName, string Email, string Password, string DOB, string Country, string IdType, string IdNo, string Address, string State, string City, string Code, string PhoneNumer)
        {
            try
            {
                SqlCmd= new SqlCommand("insert into UserInfo values('" + FirstName + "','" + LastName + "','" + Email + "','" + Password + "','" + DOB + "','" + Country + "','" + IdType + "','" + IdNo + "','" + Address + "','" + State + "','" + City + "','" + Code + "','" + PhoneNumer + "')", SqlCon);
                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();                
                return true;
            }
            catch (Exception ex)     
            {
                //throw ex;
                throw new Exception(ex.Message);
                //return false;
            }
            

        }
        [WebMethod] 
        public string VerifyUserLogin(string UserId, string Password)
        {
            try
            {
                SqlCmd = new SqlCommand("Select UserName From UserLogininfo where UserName='" + UserId + "' and Password='" + Password + "'", SqlCon);
                SqlCon.Open();
                Sqldr = SqlCmd.ExecuteReader();
                if (Sqldr.Read())
                {
                    
                    return Sqldr.GetValue(0).ToString();
                   
                }
                else
                {                 
                    return "Invalid User";

                }

            }
            catch (Exception ex)
            {
                //throw ex;
                throw new Exception(ex.Message);
               // throw new SoapException("Valid User Error", SoapException.ServerFaultCode, "Verify User", ex);
            }
           

        }
        [WebMethod]
        public bool InsertTicketInfo(string TicketNo,int TicketPrice,int PriceAmount,DateTime DisplayDate,DateTime CloseDate,DateTime DrawDate,string Status)
        {
            try
            {
                SqlCmd = new SqlCommand("insert into TicketInfo(TicketNo,TicketPrice,PriceAmount,DisplayDate,CloseDate,DrawDate,Status) values('" + TicketNo + "','" + TicketPrice + "','" + PriceAmount + "','" + Convert.ToDateTime(DisplayDate).ToString("yyy/MM/dd HH:mm:ss") + "','" + Convert.ToDateTime(CloseDate).ToString("yyy/MM/dd HH:mm:ss") + "','" + Convert.ToDateTime(DrawDate).ToString("yyy/MM/dd HH:mm:ss") + "','" + Status + "')", SqlCon);
                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCmd.Dispose();
                SqlCon.Close();
                return true;
            }
            catch(Exception ex)
            {          
               SqlCmd.Dispose();
               SqlCon.Close();
                  

               // throw new SoapException("Valid User Error", SoapException.ServerFaultCode, "Verify User", ex);
              
               throw new Exception(ex.Message);
            }
            

        }
        [WebMethod]
        public String GetTicketCount(string TicketNo)
        {
            SqlCon.Open();
            SqlCmd = new SqlCommand("SELECT ((CASE WHEN User1 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User2 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User3 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User4 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User5 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User6 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User7 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User8 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User9 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User10 IS NULL THEN 1 ELSE 0 END)) AS Sum_Of_Null  FROM TicketInfo WHERE TicketNo='"+TicketNo+"'",SqlCon);
            Sqldr = SqlCmd.ExecuteReader();
            if(Sqldr.Read())
            {
                return Sqldr.GetValue(0).ToString();
            }
            else
                {
                return "No Ticket";
            }
            
        }
    }
}

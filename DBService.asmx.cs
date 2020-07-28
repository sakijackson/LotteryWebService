using System;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Runtime.Serialization;
using System.Web.Services.Protocols;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web.Management;
using System.Runtime.InteropServices.WindowsRuntime;

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
        public SqlDataAdapter Sqlda;
        WebServiceResponse wsr;
        UserInfo ui;
        TicketInfo ti;

        public DBService()
        {
            SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["LotteryDBCon"].ConnectionString);

        }
        [WebMethod]
        public WebServiceResponse InsertUserInfo(string FirstName, string LastName, string PhoneNumber, string Email, string Password, string DOB, string Country, string IdType, string IdNo, string Address, string State, string City, string Code)
        {
           try
            {
                wsr = new WebServiceResponse();
                using (SqlCmd = new SqlCommand("insert into UserInfo values('" + FirstName + "','" + LastName + "','" + PhoneNumber + "','" + Email + "','" + Password + "','" + DOB + "','" + Country + "','" + IdType + "','" + IdNo + "','" + Address + "','" + State + "','" + City + "','" + Code + "')", SqlCon))
                {
                    SqlCon.Open();
                    int res=SqlCmd.ExecuteNonQuery();
                   
                    if (res==1)
                    {
                        wsr.Result = "1";
                        
                    }
                    else
                    {
                        wsr.Result = "0";
                    }                    

                }
                SqlCon.Close();
                return wsr;

            }
            catch (Exception ex)     
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    wsr.Error = ex.Message;

                }
                else
                {
                    wsr.Error = ex.Message;

                }
                return wsr;
            }           

        }
        [WebMethod] 
        public WebServiceResponse VerifyUserLogin(string UserId, string Password)
        {

            try
            {
                wsr = new WebServiceResponse();
                using (SqlCmd = new SqlCommand("Select UserId From UserLoginInfo where Userid='" + UserId + "' and Password='" + Password + "' and Status=1 ", SqlCon))
                {               
                     SqlCon.Open();
                    var name = SqlCmd.ExecuteScalar();

                    if (name != null)
                    {
                        wsr.Result = name.ToString();
                        
                    }
                    // Sqldr = SqlCmd.ExecuteReader();
                    //if (Sqldr.Read())
                    //{


                    //        wsr.Result= Sqldr.GetValue(0).ToString();
                    //        SqlCon.Close();

                    //    }
                    else
                    {
                      
                        wsr.Result = "0";
                  

                    }
                }
                SqlCon.Close();
                return wsr;

            }
            catch (Exception ex)
            {

                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    wsr.Error = ex.Message;
                   
                }
                else
                {
                    wsr.Error = ex.Message;

                }
                return wsr;
            }
            

        }
        [WebMethod]
        public WebServiceResponse InsertTicketInfo(string TicketNo,int TicketPrice,int PriceAmount,DateTime DisplayDate,DateTime CloseDate,DateTime DrawDate,string Status)
        {
            try
            {
                wsr = new WebServiceResponse();
                using (SqlCmd = new SqlCommand("insert into TicketInfo(TicketNo,TicketPrice,PriceAmount,DisplayDate,CloseDate,DrawDate,Status) values('" + TicketNo + "','" + TicketPrice + "','" + PriceAmount + "','" + Convert.ToDateTime(DisplayDate).ToString("yyy/MM/dd HH:mm:ss") + "','" + Convert.ToDateTime(CloseDate).ToString("yyy/MM/dd HH:mm:ss") + "','" + Convert.ToDateTime(DrawDate).ToString("yyy/MM/dd HH:mm:ss") + "','" + Status + "')", SqlCon))
                {
                    SqlCon.Open();
                    int res = SqlCmd.ExecuteNonQuery();
                    if (res == 1)
                    {
                       
                        wsr.Result = "1";
                    }
                    else
                    {
                        wsr.Result = "0";
                    }
                   
                }
                SqlCon.Close();
                return wsr;

            }
            catch(Exception ex)
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    wsr.Error = ex.Message;

                }
                else
                {
                    wsr.Error = ex.Message;

                }

                return wsr;
            }
            

        }
        [WebMethod]
        public TicketInfo GetTicketInfo()
        {
            try
            {
                ti = new TicketInfo();
                SqlCon.Open();
                using (SqlCmd = new SqlCommand("SELECT ((CASE WHEN User1 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User2 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User3 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User4 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User5 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User6 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User7 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User8 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User9 IS NULL THEN 1 ELSE 0 END)+(CASE WHEN User10 IS NULL THEN 1 ELSE 0 END)) AS TicketCount,TicketNo,TicketPrice,PriceAmount,CloseDate FROM TicketInfo ORDER BY TicketNo DESC", SqlCon))
                {
                    Sqldr = SqlCmd.ExecuteReader();
                    if (Sqldr.Read())
                    {
                        ti.TicketCount = Sqldr.GetInt32(0);
                        ti.TicketNo = Sqldr.GetValue(1).ToString();
                        ti.TicketPrice =Sqldr.GetInt32(2);
                        ti.PriceAmount = Sqldr.GetInt32(3); 
                        ti.CloseDate = Sqldr.GetDateTime(4);
                        ti.Status = 1;

                    }
                    else
                    {
                        ti.Status = 0;
                    }
                    SqlCon.Close();
                    return ti;
                }
                    
            }
            catch(Exception ex)
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    ti.Error = ex.Message;

                }
                else
                {
                    ti.Error = ex.Message;

                }

                return ti;
            }
            
        }
        [WebMethod]
        public WebServiceResponse IsExistingUser(string UserId)
        {
            try
            {
                wsr = new WebServiceResponse();
                using (SqlCmd = new SqlCommand("Select UserId From UserLoginInfo where UserId='"+UserId+"'  ", SqlCon))
                {
                    SqlCon.Open();
                    var name = SqlCmd.ExecuteScalar();

                    if (name != null)
                    {
                        wsr.Result = name.ToString();

                    }
                    else
                    {
                        wsr.Result = "0";
                        
                    }
                }
                SqlCon.Close();
                return wsr;

            }
            catch (Exception ex)
            {

                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    wsr.Error = ex.Message;

                }
                else
                {
                    wsr.Error = ex.Message;

                }
                return wsr;
            }

        }
        [WebMethod]
        public UserInfo GetUserInfo(string EmailId)
        {
            try
            {
                ui = new UserInfo();
                SqlCon.Open();
                using (SqlCmd = new SqlCommand("SELECT *FROM UserInfo Where Email='"+EmailId+"'", SqlCon))
                {
                    Sqldr = SqlCmd.ExecuteReader();
                    if (Sqldr.Read())
                    {
                       
                        ui.FirstName = Sqldr.GetValue(0).ToString();
                        ui.LastName = Sqldr.GetValue(1).ToString();
                        ui.PhoneNumber = Sqldr.GetValue(2).ToString();
                        ui.Email = Sqldr.GetValue(3).ToString();
                        ui.Password = Sqldr.GetValue(4).ToString();
                        ui.DateOfBirth = Sqldr.GetValue(5).ToString();
                        ui.Nationality = Sqldr.GetValue(6).ToString();
                        ui.IDType = Sqldr.GetValue(7).ToString();
                        ui.IdNo = Sqldr.GetValue(8).ToString();
                        ui.Address = Sqldr.GetValue(9).ToString();
                        ui.State = Sqldr.GetValue(10).ToString();
                        ui.City = Sqldr.GetValue(11).ToString();
                        ui.Code = Sqldr.GetValue(12).ToString();
                        ui.Status = 1;
                    }
                    else
                    {
                        ui.Status = 0;
                    }
                    SqlCon.Close();
                    return ui;
                }
            }
            catch (Exception ex)
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    ui.Error = ex.Message;

                }
                else
                {
                    ui.Error = ex.Message;

                }
                return ui;
            }
        }
        [WebMethod]
        public DataSet GetUsersInfo()
        {
            DataSet Usersds = new DataSet();
            //DataTable Response = new DataTable("Response");
            //Response.Columns.Add(new DataColumn("Result", typeof(string)));
            //Response.Columns.Add(new DataColumn("Error", typeof(string)));
            //DataRow dr = Response.NewRow();            

            //Usersds.Tables["Respone"].Columns.Add("Result", typeof(string));

            try
            {
                Usersds = new DataSet();
                DataTable UsersInfo = Usersds.Tables.Add("UsersInfo");
                DataTable Response = Usersds.Tables.Add("Response");


                Usersds.Tables["Response"].Columns.Add("Status", typeof(string));
                using (SqlCmd = new SqlCommand("SELECT *FROM UserInfo", SqlCon))
                {
                    using (Sqlda = new SqlDataAdapter(SqlCmd))
                    {
                        Sqlda.Fill(Usersds,"UsersInfo");
                        if (Usersds.Tables["UsersInfo"].Rows.Count>0)
                        {
                            Usersds.Tables["Response"].Rows.Add("1");
                            return Usersds;
                        }
                        else
                        {                          
                            Usersds.Tables["Response"].Rows.Add("0");                          
                        }
                        SqlCon.Close();
                    }                       
                    return Usersds;
                }
            }
            catch (Exception ex)
            {
                Usersds.Tables["Response"].Columns.Add("Error", typeof(string));
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();                  
                    Usersds.Tables["Respone"].Rows.Add(ex.Message);
                }
                else
                {
                    Usersds.Tables["Respone"].Rows.Add(ex.Message);
                }
                return Usersds;
            }
        }
        public DataSet GetTransactionsInfo()
        {
            DataSet Transactionds = new DataSet();            

            try
            {
                Transactionds = new DataSet();
                DataTable TransactionInfo = Transactionds.Tables.Add("TransactionInfo");
                DataTable Response = Transactionds.Tables.Add("Response");


                Transactionds.Tables["Response"].Columns.Add("Status", typeof(string));
                using (SqlCmd = new SqlCommand("SELECT *FROM UserInfo", SqlCon))
                {
                    using (Sqlda = new SqlDataAdapter(SqlCmd))
                    {
                        Sqlda.Fill(Transactionds, "TransactionInfo");
                        if (Transactionds.Tables["TransactionInfo"].Rows.Count > 0)
                        {
                            Transactionds.Tables["Response"].Rows.Add("1");
                            return Transactionds;
                        }
                        else
                        {
                            Transactionds.Tables["Response"].Rows.Add("0");
                        }
                        SqlCon.Close();
                    }
                    return Transactionds;
                }
            }
            catch (Exception ex)
            {
                Transactionds.Tables["Response"].Columns.Add("Error", typeof(string));
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    Transactionds.Tables["Respone"].Rows.Add(ex.Message);
                }
                else
                {
                    Transactionds.Tables["Respone"].Rows.Add(ex.Message);
                }
                return Transactionds;
            }
        }

        public DataSet GetTransactionsInfo(string UserId)
        {
            DataSet Transactionds = new DataSet();

            try
            {
                Transactionds = new DataSet();
                DataTable TransactionInfo = Transactionds.Tables.Add("TransactionInfo");
                DataTable Response = Transactionds.Tables.Add("Response");


                Transactionds.Tables["Response"].Columns.Add("Status", typeof(string));
                using (SqlCmd = new SqlCommand("SELECT *FROM UserInfo", SqlCon))
                {
                    using (Sqlda = new SqlDataAdapter(SqlCmd))
                    {
                        Sqlda.Fill(Transactionds, "TransactionInfo");
                        if (Transactionds.Tables["TransactionInfo"].Rows.Count > 0)
                        {
                            Transactionds.Tables["Response"].Rows.Add("1");
                            return Transactionds;
                        }
                        else
                        {  
                            Transactionds.Tables["Response"].Rows.Add("0");
                        }
                        SqlCon.Close();
                    }
                    return Transactionds;
                }
            }
            catch (Exception ex)
            {
                Transactionds.Tables["Response"].Columns.Add("Error", typeof(string));
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    Transactionds.Tables["Respone"].Rows.Add(ex.Message);
                }
                else
                {
                    Transactionds.Tables["Respone"].Rows.Add(ex.Message);
                }
                return Transactionds;
            }
        }
    }
}

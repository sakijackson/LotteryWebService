using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Web.Services;

namespace LotteryWebService
{
    /// <summary>
    /// Summary description for Mail
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Mail : System.Web.Services.WebService
    {

        SqlConnection SqlCon;
        SqlCommand SqlCmd;
        SqlDataReader Sqldr;
        public Mail()
        {
            SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["LotteryDBCon"].ConnectionString);
        }

        [WebMethod]
        public bool sendActivationEmail(string EmailId, string Name, string url)
        {
            try
            {
              
                string activationCode = Guid.NewGuid().ToString();
                using (MailMessage mm = new MailMessage("sakigokul97@gmail.com", EmailId))
                {
                    string newurl = url.Replace("Signup.aspx", "Activation.aspx?ActivationCode=" + activationCode);
                    mm.Subject = "Account Activation";
                    string body = "Hello " + Name + ",";
                    body += "<br /><br />Please click the following link to activate your account";
                    body += "<br /><a href = '" + newurl + "'>Click here to activate your account.</a>";
                    body += "<br /><br />Thanks";
                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("sakigokul97@gmail.com", "(Sakilove2ani)");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }

                using (SqlCmd = new SqlCommand("INSERT INTO UserActivation VALUES('" + EmailId + "','" + activationCode + "','0')", SqlCon))
                {
                    SqlCon.Open();
                    SqlCmd.ExecuteNonQuery();
                    SqlCon.Close();
                }
                return true;
            }
            catch (Exception ex)

            {

                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    throw new Exception(ex.Message);
                }
                else
                {
                    throw new Exception(ex.Message);
                }

            }

        }
        [WebMethod]
        public bool verifyActivationEmail(string activationCode)
        {
            try
            {

                 using (SqlCommand cmd = new SqlCommand("SELECT ActivationCode FROM UserActivation WHERE ActivationCode = '" + activationCode + "' and Status='0' ", SqlCon))
                {
                    SqlCon.Open();
                    Sqldr = cmd.ExecuteReader();
                    if (Sqldr.Read())
                    {
                        Sqldr.Close();
                        using (SqlCmd = new SqlCommand("UPDATE UserActivation SET Status='1' WHERE ActivationCode='" + activationCode + "'",SqlCon)) 
                        {
                            int rel = SqlCmd.ExecuteNonQuery();
                            if (rel == 1)
                            {
                                SqlCon.Close();
                                SqlCmd.Dispose();
                                return true;
                            }
                           
                        }

                    }
                    
                    return false;
                    
                    
                }
            }
            catch (Exception ex)
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    throw new Exception(ex.Message);
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }



        }
    }
}

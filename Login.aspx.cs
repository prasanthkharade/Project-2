using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.DirectoryServices;

namespace VS2010_TuitionPayment
{
    public partial class Login : System.Web.UI.Page
    {
        private string _path = "LDAP://kcom.edu/DC=kcom,DC=edu";
        private string _filterAttribute;

        protected void Page_Load(object sender, EventArgs e)
        {
            Username.Focus();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Authenticate user
            if (IsAuthenticated("kcom2k", Username.Text, Password.Text))
            {
                //check CV and determine if student is valid student
                if (ChkCVValidStudent())
                {
                    Session["UserType"] = "Student";
                    FormsAuthentication.SetAuthCookie(Username.Text, false);
                    FormsAuthentication.RedirectFromLoginPage(Username.Text, false);
                }
                else if (PaymentAccess())
                {
                    Session["UserType"] = "Cntrl";
                    Session["CntrlUser"] = Username.Text;
                    FormsAuthentication.SetAuthCookie(Username.Text, false);
                    FormsAuthentication.RedirectFromLoginPage(Username.Text, false);
                }
                else
                    Response.Redirect("Unauthorized.aspx?Re=UnauthStudent");
            }
            else
            {
                Response.Redirect("Unauthorized.aspx?Re=Unauthorized");

            }

        }

        private bool ChkCVValidStudent()
        {
            //Check if student is a valid student in CampusVue system.  See Web.config for query.
              //<add key="SqlSelect" value="SELECT syStudentID FROM syStudent where email = '"/>
              //<add key="SqlAND" value="@atsu.edu'"/>

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionStringCV"]);

            SqlCommand cmd = new SqlCommand(System.Configuration.ConfigurationManager.AppSettings["SqlSelect"] + Username.Text + System.Configuration.ConfigurationManager.AppSettings["SqlAND"], conn);
            cmd.CommandType = CommandType.Text;

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["syStudentID"] != DBNull.Value)
                        {
                            Session["StudentID"] = (int)reader["syStudentID"];
                            break;
                        }
                        else
                            return false;
                    }

                    reader.Close();
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: ChkCVValidStudent - " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return true;
        } //ChkCVValidStudent

        public bool IsAuthenticated(string domain, string username, string pwd)
        {
            string domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);

            try
            {
                //Bind to the native AdsObject to force authentication.			
                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    return false;
                }

                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (string)result.Properties["cn"][0];
            }
            catch //(Exception ex)
            {
                return (false); // throw new Exception("Error authenticating user. " + ex.Message);
            }

            return true;
        } //IsAuthenticated


        //********************************************************************************
        //*  VIEWING/TESTING ONLY
        //********************************************************************************
        public bool PaymentAccess()
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionStringProcessing"]);

            SqlCommand cmd = new SqlCommand("Select * from Users where Active = 'True' and PaymentAccess = 'True' and UserID = '" + Username.Text.Trim() + "'", conn);
            cmd.CommandType = CommandType.Text;

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: PaymentAccess - " + ex.Message);
            }

            return false;

        } //PaymentAccess()

    }

}
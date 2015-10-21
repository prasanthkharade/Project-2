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

namespace VS2010_TuitionPayment
{
    public partial class ViewTestOnly : System.Web.UI.Page
    {
        protected void Page_Init()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (Session["UserType"] != null || Session["CntrlUser"] != null)
            {
                switch (Session["CntrlUser"].ToString().ToLower())
                {
                    case "etillotson":
                        LabelName.Text = "Eppie:";
                        break;
                    case "kchamblee":
                        LabelName.Text = "Kathy:";
                        break;
                    case "mkrobison":
                        LabelName.Text = "Marieta:";
                        break;
                    case "rshockley":
                        LabelName.Text = "Randy:";
                        break;
                    case "claw":
                        LabelName.Text = "Curt:";
                        break;
                    case "lsutton":
                        LabelName.Text = "Linda:";
                        break;
                    case "bfitzgerald":
                        LabelName.Text = "Barb:";
                        break;
                    default:
                        FormsAuthentication.SignOut();
                        Session.Clear();
                        Session.Abandon();

                        // Clear authentication cookie
                        HttpCookie rFormsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                        rFormsCookie.Expires = DateTime.Now.AddYears(-1);
                        Response.Cookies.Add(rFormsCookie);

                        // Clear session cookie 
                        HttpCookie rSessionCookie = new HttpCookie("ASP.NET_SessionId", "");
                        rSessionCookie.Expires = DateTime.Now.AddYears(-1);
                        Response.Cookies.Add(rSessionCookie);

                        //Response.Redirect("Login.aspx", true);
                        break;
                }
            }
            else
            {
                //Session null
                FormsAuthentication.SignOut();
                Session.Clear();
                Session.Abandon();

                // Clear authentication cookie
                HttpCookie rFormsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                rFormsCookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(rFormsCookie);

                // Clear session cookie 
                HttpCookie rSessionCookie = new HttpCookie("ASP.NET_SessionId", "");
                rSessionCookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(rSessionCookie);

                //Response.Redirect("Login.aspx", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void GetStudentID()
        {
            /////Why aren't you getting the Session["StudentID"] when redirected from Login page????

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionStringCV"]);
            string querycmd = "select syStudentID from syStudent where stunum = '" + tbStuNum.Text + "'";

            SqlCommand cmd = new SqlCommand(querycmd, conn);
            cmd.CommandType = CommandType.Text;

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read(); // Advance to the one and only row
                    Session["StudentID"] = reader["syStudentID"].ToString();
                    if (reader != null)
                        reader.Close();
                }
                else
                    Session["StudentID"] = "";
            }
            catch (Exception ex)
            {
                throw new Exception("GetStudentID:  " + ex.Message);
            }
            finally
            {
                conn.Close();
            }


        }  //GetStudentID

        protected void ButtonShow_Click(object sender, EventArgs e)
        {
            GetStudentID();
            if (Session["StudentID"].ToString() != "")
            {
                Session["UserType"] = "View";
                Response.Redirect("Default.aspx");
            }
            else
            {
                Label1.Text = "Invalid Student Number.";
                Label1.Visible = true;
            }

        }

        protected void ButtonLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            // Clear authentication cookie
            HttpCookie rFormsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            rFormsCookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(rFormsCookie);

            // Clear session cookie 
            HttpCookie rSessionCookie = new HttpCookie("ASP.NET_SessionId", "");
            rSessionCookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(rSessionCookie);

            Response.Redirect("Login.aspx", true);

        }

    }
}
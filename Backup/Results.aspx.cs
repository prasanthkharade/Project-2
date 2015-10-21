using System;
using System.Collections.Generic;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VS2010_TuitionPayment
{
    public partial class Results : System.Web.UI.Page
    {
        string PmtType, Msg;

        protected void Page_Init()
        {
            PmtType = Request.QueryString["Re"];
            LabelName.Text = Session["Name"].ToString() + ":";

            switch (PmtType)
            {
                case "FA":
                    Msg = "An email has been sent to you in confirmation that you will be funding your class using Financial Aid.<br><br>Thank you for using the ATSU Distance Programs Online Tuition Payment system.<br>";
                    break;
                case "Check":
                    Msg = "An email has been sent to you in confirmation that you will be paying for your class by check.<br><br>Please make your check payable to ATSU and mail to the address below:<br><br>";
                    Panel2.Visible = true;
                    break;
                case "ThirdParty":
                    Msg = "An email has been sent to you in confirmation that you will be using a 3rd party to fund your class.<br><br>Thank you for using the ATSU Distance Programs Online Tuition Payment system.<br>";
                    break;
                case "PPlan":
                    Msg = "An email has been sent to you in confirmation that you will be using a payment plan set up through the Controller's office.<br><br>Thank you for using the ATSU Distance Programs Online Tuition Payment system.<br>";
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = Msg;
            Panel1.Visible = true;
            Label1.Visible = true;
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

        } // ButtonLogout    
    }
}
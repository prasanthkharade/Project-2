using System;
using System.Data;
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
    public partial class Unauthorized : System.Web.UI.Page
    {
        string MsgType;         //QueryString["Re"] = Message identifier FROM Login.aspx
        //   "Unauthorized" = Not in Student Group/Not Authenticated

        string Msg;             //the message to user

        protected void Page_Init()
        {
            MsgType = Request.QueryString["Re"];

            switch (MsgType)
            {
                case "Unauthorized":
                    Msg = "You are not an authorized user.<br>";
                    break;
                case "UnauthStudent":
                    Msg = "You are not an authorized Student.<br>";
                    break;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = Msg;
        }
    }
}
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace VS2010_TuitionPayment
{
    //THIS IS THE Non-secure server portal piece of DistanceOnlineLearningTuitionPmt 'system'
    //See CCTuitionPmt project for the CC secure-server piece.

    public partial class _Default : System.Web.UI.Page
    {
        //Authentication token
        static string tokenId;
        static int correlationID = 1;

        int StudentID, NumRecs, ActivityID, EventType;
        string StudentNumber,StudentName,DateBilled, PmtType, EmailCodeStu, EmailCodeStaff;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        protected void Page_Init()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (Session["UserType"] != null || Session["CntrlUser"] != null)
            {
                if (Session["UserType"].ToString() == "Cntrl")
                    Response.Redirect("ViewTestOnly.aspx");
                else
                {
                    if (Session["StudentID"] != null)
                    {
                        //Get the Name using Session StudentID
                        int.Parse(Session["StudentID"].ToString());  ////             StudentID = 171196;  

                        if (Session["UserType"].ToString() == "View")
                            PanelTest.Visible = true;

                        Authenticate();

                        GetStudentName();
                        LabelName.Text = Session["Name"].ToString().Trim() + ":";

                        GetStudentNumber();
                        //Must get count of recs due to err balances in GL
                        //Get the number of adEnroll recs that apply.
  
                        CountadEnrollRecs("InitialCount");  //false = Not a recount
                        //******the call to sql = select Count(*) as NumRecs from adenroll where (syschoolstatusid = 5 or syschoolstatusid = 7 or syschoolstatusid = 13 or syschoolstatusid = 14 or syschoolstatusid = 20  or syschoolstatusid = 62 or syschoolstatusid = 63 or syschoolstatusid = 67 or syschoolstatusid = 76 or syschoolstatusid = 80) and systudentid = 

                        if (NumRecs == 0)
                            PanelMsg4.Visible = true;  //PanelMsg4 = No tuition charges due
                        else
                        {  //if Enroll rec found falls into those schoolstatusid's...
                            if (NumRecs > 1)
                            {
                                //re-check to see if all recs are adshiftid = 1
                                int Recount = NumRecs;
                                CountadEnrollRecs("CheckShift1");
                                //******sql = checks for correct shiftid

                                if (Recount != NumRecs)        //adEnroll recs found are multiple shiftid's
                                    PanelMsg1.Visible = true;  //PanelMsg1 = Online tuition payments cannot be made for your program.  
                                                               //= Dual Enrolled
                                else
                                {
                                    //found correct shiftid enroll record(s)
                                    //check for any pending charges
                                    int ChargesPending = ChkPendingCharges();

                                    CountadEnrollRecs("CheckShift1Bal0");  
                                    //******sql = get numrecs found with balance of 0

                                    if (ChargesPending > 0)
                                    {
                                        //int ChargesPending = ChkPendingCharges();
                                        //if (ChargesPending > 0)
                                            //PanelMsg2.Visible = true;
                                            PanelMsg5.Visible = true;
                                    }
                                    else if (NumRecs > 1)  //must return only 1 with arbalance != 0
                                        PanelMsg1.Visible = true;  //PanelMsg1 = Payment cannot be made for program = Multiple shiftid 1 recs with bal > 0
                                    else
                                    {
                                        if (TermIdOkay())  //check for TUIT saTrans record
                                            GetadEnrollRec();
                                        else
                                            PanelMsg1.Visible = true;  //TermId = 0 // Program payment one time // cannot pay online
                                    }
                                }
                            }
                            else
                            {
                                if (TermIdOkay())
                                    GetadEnrollRec();
                                else
                                    PanelMsg1.Visible = true;  //TermId = 0 // Program payment one time // cannot pay online
                            }
                        }//end else numrecs>1
                    }//end if student
                    else
                    {
                        //Session["StudentID"] null
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

                        //PanelMsg1.Visible = false;
                        //PanelMsg2.Visible = false;
                        //PanelMsg3.Visible = true;

                    }  //end else not student

                }  //end else student
            } //end if Session["UserType"] != null
            else
            {
                //Session var is null
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

            }  //end else not student
        }  //end Page_Init()

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

        }

        private void Authenticate()
        {
            //Initialize Authentication Token Request
            Authentication.TokenRequest requestToken = new Authentication.TokenRequest();
            requestToken.UserName = System.Configuration.ConfigurationManager.AppSettings["UserName"];
            requestToken.Password = System.Configuration.ConfigurationManager.AppSettings["Password"];

            //SET TOKEN NEVER EXPIRES FOR NOW -- DEFAULT WAS SET TO 3 MINUTES (see Installation Guide)
            requestToken.TokenNeverExpires = true;

            //Instantiate a new authentication service and url
            Authentication.Authentication serviceAuthentication = new Authentication.Authentication();
            string urlAuthentication = System.Configuration.ConfigurationManager.AppSettings["TokenURL"];
            serviceAuthentication.Url = urlAuthentication;

            //Create TokenResponse and pass in the request through the Authentication service
            Authentication.TokenResponse responseToken = serviceAuthentication.GetAuthorizationToken(requestToken);

            //Check TokenRequest Response - If "OK" then continue, else give message !:-)
            if (responseToken.TrxResult == "OK")
            {
                tokenId = responseToken.TokenId;
            }
            else
            {
                Label1.Text = "ERROR:  Failed 'Authentication' to System.  Please logout and try again.";
                Label1.Visible = true;
            }

        }  //Authenticate

        private void CCMakePmt()
        {
            //Check to see if payment for this EnrollID is waiting to be processed by Controller's Office 
            //     (or if pmt has just been made).
            //Query .101/.26 TuitionProcessing db
            if (!IsPmtReceived())
            {
                //Redirect to Secure server - see web.config
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["SSLRef"] + "?ID=" + Session["StudentID"].ToString());
            }
            else
                PanelMsg4.Visible = true;

        }  //CCMakePmt()

        private int ChkPendingCharges()
        {
            // check for pending charges in saPendingCharge table and display PanelMsg4 if pending charges found.
            int NumPending = 0;
            
            //See Web.config for query.
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionStringCV"]);

            SqlCommand cmd = new SqlCommand(System.Configuration.ConfigurationManager.AppSettings["SqlPending"] + int.Parse(Session["StudentID"].ToString()), conn);
            cmd.CommandType = CommandType.Text;

            //web.config
            // <add key="SqlPending" value="select * from saPendingCharge where (sabillcodeid = 1 or sabillcodeid = 73 or sabillcodeid = 71) and systudentid = "/>

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NumPending++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: ChkPendingCharges - " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return NumPending;

        }  //ChkPendingCharges

        private void CountadEnrollRecs(string CountType)  //, bool recount
        {
            //SySchoolStatusID's counted in query are:
            //5 = FUT  Future Start
            //7 = REENTRY  Re-Entry
            //13= ATT  Active
            //14= PROB Probation
            //20= DROP Withdrawn
            //62= NDSFUT  NDS-Future Start
            //63= NDSATT  NDS-Attending
            //67= NDSINCOM  NDS-Complete
            //76= DISMISS  Dismissed
            //80= INCWD  Incomplete-Withdraw

            //See Web.config for query.
 
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionStringCV"]);
            SqlCommand cmd = new SqlCommand();

            if (CountType == "InitialCount")
                cmd = new SqlCommand(System.Configuration.ConfigurationManager.AppSettings["SqlEnrollCount"] + int.Parse(Session["StudentID"].ToString()), conn);
            else if (CountType == "CheckShift1")
                cmd = new SqlCommand(System.Configuration.ConfigurationManager.AppSettings["SqlEnrollCount"] + int.Parse(Session["StudentID"].ToString()) + " and adshiftid = 1", conn);
            else if (CountType == "CheckShift1Bal0")
                cmd = new SqlCommand(System.Configuration.ConfigurationManager.AppSettings["SqlEnrollCount"] + int.Parse(Session["StudentID"].ToString()) + " and arbalance != 0 and adshiftid = 1", conn);

            cmd.CommandType = CommandType.Text;

            //webconfig
            //<add key="SqlEnrollCount" value="select Count(*) as NumRecs from adenroll where (syschoolstatusid = 5 or syschoolstatusid = 7 or syschoolstatusid = 13 or syschoolstatusid = 14 or syschoolstatusid = 20  or syschoolstatusid = 62 or syschoolstatusid = 63 or syschoolstatusid = 67 or syschoolstatusid = 76 or syschoolstatusid = 80) and systudentid = "/>

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["NumRecs"] != DBNull.Value)
                        {
                            NumRecs = (int)reader["NumRecs"];  //if recount, must return one
                        }
                    }

                    reader.Close();
                }
                else
                    NumRecs = 0;
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: CountEnrollRecs - " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }  //CountadEnrollRecs

        private void EmailsToIT()
        {
            //Send notification emails to IT that Activities have been sent.
            try 
            {
                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient();

                message.From = new MailAddress(ConfigurationManager.AppSettings["doNotReplyEmail"].ToString(), 
                    "Distance Online Learning Tuition Payment System ");
                                
                message.To.Add("lsutton@atsu.edu");
                message.To.Add("bfitzgerald@atsu.edu");
                message.To.Add("claw@atsu.edu");
                message.To.Add("jvann@atsu.edu");

                message.Subject = "Distance Online Learning Tuition Payment System - CampusVue Activities posted notification.";

                message.Body = "IT Staff Notification:\n\r\n\rCampusVue Activities have been posted for Student \n\r\n\rStudent Number: " + StudentNumber +
                "\n\r Student Name: " + Session["Name"].ToString() + "\n\r\n\r";

                switch(PmtType)
                {
                    case "FA":
                        message.Body += "FA - Pay by FA to Student\n\rFA - Pay by FA to Staff\n\r\n\r";
                        break;
                    case "Check":
                        message.Body += "SA - Pay by Check to Student\n\rSA - Pay by Check to Staff\n\r\n\r";
                        break;
                    case "ThirdParty":
                        message.Body += "SA - Pay by 3rd Party to Student\n\rSA - Pay by 3rd Party to Staff\n\r\n\r";
                        break;
                    case "PPlan:":
                        message.Body += "SA - Pay by Payment Plan to Student\n\rSA - Pay by Payment Plan to Staff\n\r\n\r";
                        break;
                }

                message.Body += "This message is notification to IT Staff only.\n\r\n\rThank you!";

                client.Port = 25;  // 25 default smtp port; //587 gmail port
                client.Host = "appmail.atsu.edu";  //from Randy:10.1.1.121     Hotvedt:10.1.92.21
                //client.Host = "smtp.gmail.com";  //gmail
                //client.EnableSsl = true;         //gmail
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(message);   
            }
            catch (Exception ex)
            {  
                Label1.Text += ": IT Notifications Failed.  Please report this error message to helpdesk@atsu.edu - " + ex.Message;  
            }
        }  //EmailsToIT()

        private void GetActivityID(string ActivityCode)
        {
            ActivityID = 0;

            //Initialize GetList Request
            GetList.GetListRequest requestGetList = new GetList.GetListRequest();
            requestGetList.TokenId = tokenId;

            //Instantiate a new GetList service and url.
            GetList.GetListWebService serviceGetList = new GetList.GetListWebService();
            string urlGetList = System.Configuration.ConfigurationManager.AppSettings["GetListURL"];
            serviceGetList.Url = urlGetList;

            //syCampusID  Code from syCampus
                //6	    CGHS     

            //Create the request
            int[] campusIds = new int[1] { 6 };   //create the array for campusIds parameter, not used in filtering ActivityTemplate results.
            requestGetList.ListFilteredInMsgs = new GetList.ListFilteredInMsg[] {
                CreateInMsgFiltered(GetList.EntityTypeFiltered.ActivityTemplate, true, campusIds, new GetList.CustomAttributeMsg[] 
                {
                    WsCreateCustomAttribute("EventTypeID", EventType.ToString())   //cmEventType EMAILSTU = 9 
                } )                                                                            //EMAIL  =   8
            };

            //Create GetList Response and pass in the request through the GetList service
            GetList.GetListResponse responseGetList = serviceGetList.GetList(requestGetList);

            //Check GetList Response - If not "OK" then handle somehow somewhere.
            if (responseGetList.TrxResult == "OK")
            {
                int i = responseGetList.FilteredLists[0].Items.Length;
                int j = 0;
                do
                {
                    if (string.Compare(responseGetList.FilteredLists[0].Items[j].Code.Trim(), ActivityCode.Trim(), true) == 0)
                    {
                        ActivityID = responseGetList.FilteredLists[0].Items[j].Id;
                        break;
                    }
                    j++;
                } while (j < i);
            }


        }  //GetActivityID

        private void GetadEnrollRec()
        {
            //See Web.config for query.
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionStringCV"]);

            SqlCommand cmd = new SqlCommand(System.Configuration.ConfigurationManager.AppSettings["SqlEnroll"] + int.Parse(Session["StudentID"].ToString()), conn);
            cmd.CommandType = CommandType.Text;

            //web.config
            //<add key="SqlEnroll" value="select adenrollid, Convert(varchar,datebilled,101) as Date, adprogramdescrip as Program, Convert(decimal(10,2),arbalance) as Amount from adenroll where (syschoolstatusid = 5 or syschoolstatusid = 7 or syschoolstatusid = 13 or syschoolstatusid = 14 or syschoolstatusid = 20  or syschoolstatusid = 62 or syschoolstatusid = 63 or syschoolstatusid = 67 or syschoolstatusid = 76 or syschoolstatusid = 80) and arbalance != 0 and adshiftid = 1 and systudentid = "/>

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        Session["EnrollID"] = (int)reader["adenrollid"];

                        //Check to see if payment for this EnrollID is waiting to be processed by Controller's Office.
                        //Query .101/.26 TuitionProcessing db
                        if (!IsPmtReceived())
                        {
                            //if NOT waiting to be processed, display
                            dt.Columns.Add(new DataColumn("Date Billed", typeof(System.String)));
                            dt.Columns.Add(new DataColumn("Program", typeof(System.String)));
                            dt.Columns.Add(new DataColumn("Amount Due", typeof(System.Decimal)));

                            if (decimal.Parse(reader["Amount"].ToString()) > 0)
                            {
                                //Session["EnrollID"] = reader["adEnrollID"].ToString();
                                DataRow datarow;
                                datarow = dt.NewRow();
                                //if (reader["Date"] != DBNull.Value)
                                //    datarow[0] = reader.GetString(1);
                                //else
                                //    datarow[0] = "Unknown";
                                datarow[0] = DateBilled;

                                datarow[1] = reader.GetString(2);
                                datarow[2] = reader.GetDecimal(3);
                                dt.Rows.Add(datarow);

                                PanelOutstanding.Visible = true;
                                GridViewBalanceDue.DataSource = dt;
                                GridViewBalanceDue.DataBind();

                                GridViewBalanceDue.HeaderRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                                GridViewBalanceDue.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                GridViewBalanceDue.Rows[0].Cells[2].HorizontalAlign = HorizontalAlign.Center;

                            }
                            else
                                PanelMsg4.Visible = true;
                        }
                        else
                            PanelMsg4.Visible = true;
                    }

                    reader.Close();
                }
                else
                    PanelMsg4.Visible = true;  // Zero balance found.  No tuition charges due
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: GetEnrollRecs - " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }  //GetadEnrollRec

        private GetList.ListFilteredInMsg CreateInMsgFiltered(GetList.EntityTypeFiltered type, bool activeOnly, int[] locationIds, GetList.CustomAttributeMsg[] msgCustomAttributes)
        {
            GetList.ListFilteredInMsg inMsg = new GetList.ListFilteredInMsg();
            inMsg.ItemType = type;
            inMsg.ActiveOnly = activeOnly;
            inMsg.CampusIds = locationIds;
            if (msgCustomAttributes != null)
                inMsg.CustomAttributes = msgCustomAttributes;
            return inMsg;
        }  //GetList Filtered

        private static GetList.CustomAttributeMsg WsCreateCustomAttribute(string name, string value)
        {
            GetList.CustomAttributeMsg custAttrib = new GetList.CustomAttributeMsg();
            custAttrib.Name = name.Trim();
            custAttrib.Value = value.Trim();
            return custAttrib;
        }  //GetList CustomAttrib

        private void GetStudentName()
        {
            //See Web.config for query.
                //<add key="SqlName" value="SELECT FirstName + ' ' + LastName as Name from systudent where systudentid = "/>

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionStringCV"]);

            //////SqlCommand cmd = new SqlCommand(System.Configuration.ConfigurationManager.AppSettings["SqlName"] + int.Parse(Session["StudentID"].ToString()), conn);
            SqlCommand cmd = new SqlCommand(System.Configuration.ConfigurationManager.AppSettings["SqlName"] + int.Parse(Session["StudentID"].ToString()), conn);
            cmd.CommandType = CommandType.Text;

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["Name"] != DBNull.Value)
                        {
                            Session["Name"] = (string)reader["Name"];
                        }
                    }

                    reader.Close();
                }
                else
                    Session["Name"] = "";
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: GetStudentName - " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }

        private void GetStudentNumber()
        {

            //Initialize GetStudent Request
            Student.GetStudentInfoRequest requestGetStudent = new Student.GetStudentInfoRequest();
            requestGetStudent.TokenId = tokenId;

            //Create the GetStudentInMsg
            Student.GetStudentInMsg inMsgStudent = new Student.GetStudentInMsg();
            inMsgStudent.CorrelationId = correlationID;
            inMsgStudent.StudentId = int.Parse(Session["StudentID"].ToString()); // StudentID;

            //Bind GetStudentInMsg to the request
            requestGetStudent.Students = new Student.GetStudentInMsg[1];
            requestGetStudent.Students[0] = inMsgStudent;

            //Instantiate a Student service and url.
            Student.StudentWebService serviceStudent = new Student.StudentWebService();
            string urlStudent = System.Configuration.ConfigurationManager.AppSettings["StudentURL"];
            serviceStudent.Url = urlStudent;

            //Create GetStudent Response and pass in the request through the Student service
            Student.GetStudentInfoResponse responseGetStudent = serviceStudent.GetStudentInfo(requestGetStudent);

            //TODO - Check GetStudentInfoResponse - If not "OK" then handle somehow somewhere.  
            if (responseGetStudent.TrxResult == "OK")
                StudentNumber = responseGetStudent.Students[0].StudentNumber.Trim();
            else
                StudentNumber = "0";  //TODO - is necessary??

        }  //GetStudentNumber

        private bool IsPmtReceived()
        {
            bool returnVal = false;

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionStringProcessing"]);

            SqlCommand cmd = new SqlCommand(System.Configuration.ConfigurationManager.AppSettings["SqlProcessedInfo"] + Session["EnrollID"].ToString(), conn);
            cmd.CommandType = CommandType.Text;

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["PostDate"] == DBNull.Value)
                            returnVal = true;                        
                    }
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: GetProcessedInfo - " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return returnVal;
        }  //IsPmtReceived

        private void PostActivity(string ActivityCode)
        {
            GetActivityID(ActivityCode);

            //Initialize Activity Request
            Activity.PostActivityRequest requestActivity = new Activity.PostActivityRequest();
            requestActivity.TokenId = tokenId;

            //Create the AddNewActivityInMsg
            Activity.AddNewActivityInMsg inMsgActivity = new Activity.AddNewActivityInMsg();
            inMsgActivity.CorrelationId = correlationID;
            inMsgActivity.StudentId = int.Parse(Session["StudentID"].ToString());
            inMsgActivity.ActivityStatusId = 1;                  //CmEventStatusId  Pending=P=1  Queued=Q=5

            //inMsgActivity.AssignedStaffId = 11454;            //11454=FinancialAid=financialaidonline@atsu.edu
                                                                //11455=Controller=controllers@atsu.edu
                                                                //56-CLaw   10481-KChamblee  10465-MKRobison ADMISS-10499

            if (EventType == 8)   
            {
                //TO STAFF
                switch (PmtType)
                {
                    case "FA":
                        inMsgActivity.AssignedStaffId = 11454;     //11454=FinancialAid=financialaidonline@atsu.edu
                        inMsgActivity.ActivityTemplateId = 1036;   //PAYFA    ActivityID;     //cmTemplateID
                        break;
                    case "Check":
                        inMsgActivity.AssignedStaffId = 11455;     //11455=controllers@atsu.edu
                        inMsgActivity.ActivityTemplateId = 1037;   //PAYCHECK  ActivityID;     //cmTemplateID
                        break;
                    case "ThirdParty":
                        inMsgActivity.AssignedStaffId = 11455;     //11455=controllers@atsu.edu
                        inMsgActivity.ActivityTemplateId = 1039;   //PAY3RD   ActivityID;     //cmTemplateID
                        break;
                    case "PPlan":
                        inMsgActivity.AssignedStaffId = 11455;     //11455=controllers@atsu.edu
                        inMsgActivity.ActivityTemplateId = 1041;   //PAYPLAN  ActivityID;     //cmTemplateID
                        break;
                } //end TO STAFF
            }
            else
            {
                //else Event Type == 9
                //TO STUDENT
                switch (PmtType)
                {
                    case "FA":
                        inMsgActivity.ActivityTemplateId = 1030;        //PAYFAS
                        inMsgActivity.AssignedStaffId = 11454;          //TO STUDENT from syStaff  systaffid=11454=financialAid
                        break;
                    case "Check":
                        inMsgActivity.AssignedStaffId = 11455;     //11455=controllers@atsu.edu
                        inMsgActivity.ActivityTemplateId = 1038;   //PAYCHECK  ActivityID;     //cmTemplateID
                        break;
                    case "ThirdParty":
                        inMsgActivity.AssignedStaffId = 11455;     //11455=controllers@atsu.edu
                        inMsgActivity.ActivityTemplateId = 1040;   //PAY3RD   ActivityID;     //cmTemplateID
                        break;
                    case "PPlan":
                        inMsgActivity.AssignedStaffId = 11455;     //11455=controllers@atsu.edu
                        inMsgActivity.ActivityTemplateId = 1042;   //PAYPLAN  ActivityID;     //cmTemplateID
                        break;
                } //end TO STUDENT
            }                
            inMsgActivity.DueDate = DateTime.Today;
            inMsgActivity.Priority = Activity.ActivityPriorityType.Normal;

            //Bind ActivityInMsg to the request
            requestActivity.AddNewActivities = new Activity.AddNewActivityInMsg[1];
            requestActivity.AddNewActivities[0] = inMsgActivity;

            //Instantiate an Activity service and url.
            Activity.ActivityWebService serviceActivity = new Activity.ActivityWebService();
            string urlActivity = System.Configuration.ConfigurationManager.AppSettings["ActivityURL"];
            serviceActivity.Url = urlActivity;

            //Create Activity Response and pass in the request through the Activity service
            Activity.PostActivityResponse responseActivity = serviceActivity.PostActivity(requestActivity);

            if (responseActivity.TrxResult == "OK")
            {
                //Success
                //Response.Redirect("Results.aspx?Re=" + PmtType);
            }
            else
            {
                Label1.Text += ":  Activity Failed.";
                Label1.Visible = true;
                //Failed;
            }

        }  //PostActivity(ActivityCode)

        private void SendEmails()
        {
            //1030 PAYFAS FA to Student
            //1036 PAYFA  FA to FA Staff  systaffid 11454  "financialaidonline@atsu.edu"

            //1037 PAYCHECK SA to Controller Staff
            //1038 PAYCHKS SA to Student

            //1039 PAY3RD SA to Controller Staff
            //1040 PAY3RDS SA to Student

            //1041 PAYPLAN SA to Controller Staff
            //1042 PAYPLANS SA to Student

            switch (PmtType)
            {
                case "FA":
                    EventType = 8;
                    PostActivity("PAYFA");    //to FA systaffid 11454
                    EventType = 9;            //toStudent
                    PostActivity("PAYFAS");
                    break;
                case "Check":
                    EventType = 8;
                    PostActivity("PAYCHECK");  //to Controller systaffid 11455
                    EventType = 9;
                    PostActivity("PAYCHKS");   //toStudent
                    break;
                case "ThirdParty":
                    EventType = 8;
                    PostActivity("PAY3RD");  //to Controller systaffid 11455
                    EventType = 9;
                    PostActivity("PAY3RDS");   //toStudent
                    break;
                case "PPlan":
                    EventType = 8;
                    PostActivity("PAYPLAN");  //to Controller systaffid 11455
                    EventType = 9;
                    PostActivity("PAYPLANS");   //toStudent
                    break;
            } //end switch
        }  //SendEmails(PmtType)


        private bool TermIdOkay()
        {
            //get student's most recent date added 'TUIT' saTrans record adtermid for posting
            bool returnVal = false;

            //See Web.config for query.
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionStringCV"]);

            SqlCommand cmd = new SqlCommand(System.Configuration.ConfigurationManager.AppSettings["SqlTermID"] + Session["StudentID"].ToString() + System.Configuration.ConfigurationManager.AppSettings["SqlTermID2"] + Session["StudentID"].ToString() + ")", conn);
            cmd.CommandType = CommandType.Text;
//web.config
        //OLD UNUSED
        //add key="SqlTermID" value="select adtermid from satrans where syStudentID ="/>
        //<add key="SqlTermID2" value=" and dateadded IN (Select MAX(dateadded) from satrans where sabillcode = 'TUIT' and syStudentID ="/>
    //NEW
    //<add key="SqlTermID" value="select adtermid, Convert(varchar,date,101) as Date from satrans where syStudentID ="/>
    //<add key="SqlTermID2" value=" and dateadded IN (Select MAX(dateadded) from satrans where ( sabillcode = 'TUIT' or sabillcode = 'ACTFEE' or sabillcode = 'DISFEE') ) and syStudentID ="/>

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if ((int)reader["adtermid"] > 0)
                        {
                            DateBilled = (string)reader["Date"];
                            returnVal = true;
                        }
                    }

                    reader.Close();

                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: TermIdOkay - " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return returnVal;


        }  //TermIdOkay()

        protected void Button2_Click(object sender, EventArgs e)
        {
            //"Go Back" Testing Button
            Response.Redirect("ViewTestOnly.aspx");
        }

        protected void ButtonMakePmt_Click(object sender, EventArgs e)
        {
            //NO LONGER USING MAKE PAYMENT BUTTON....
            //SEE CCMakePayment Method for CreditCard payment redirect.  /kc 5/22/2014

            //Check to see if payment for this EnrollID is waiting to be processed by Controller's Office 
            //     (or if pmt has just been made).
            //Query .101/.26 TuitionProcessing db
            if (!IsPmtReceived())
            {
                //Redirect to Secure server - see web.config
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["SSLRef"] + "?ID=" + Session["StudentID"].ToString());
            }
            else
                PanelMsg4.Visible = true;
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

        protected void ButtonRelogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", true);
        }

        protected void ButtonContinue_Click(object sender, EventArgs e)
        {
            PmtType = RadioButtonList1.SelectedValue.ToString();

            //do some error checking on radiobuttonlist HERE
            //if PmtType ok and not credit card...
            if (PmtType != "CC")
            {
                SendEmails();
                EmailsToIT();
                Response.Redirect("Results.aspx?Re=" + PmtType);
            }
            else
                CCMakePmt();

        }  //ButtonContinue_Click()

        protected void ButtonTryAgain_Click(object sender, EventArgs e)
        {
            Page_Init();
        }
        //ButtonTryAgain_Click()

        protected void btnTestEmail_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient();

                message.From = new MailAddress(ConfigurationManager.AppSettings["doNotReplyEmail"].ToString(),
                    "Distance Online Learning Tuition Payment System ");

                message.To.Add("pkharade@atsu.edu");

                message.Subject = "Distance Online Learning Tuition Payment System - CampusVue Activities posted notification.";

                message.Body = "IT Staff Notification:\n\r\n\rCampusVue Activities have been posted for Student \n\r\n\rStudent Number: " + StudentNumber + 
                    "\n\r Student Name: " + Session["Name"].ToString() + "\n\r\n\r";
                switch (PmtType)
                {
                    case "FA":
                        message.Body += "FA - Pay by FA to Student\n\rFA - Pay by FA to Staff\n\r\n\r";
                        break;
                    case "Check":
                        message.Body += "SA - Pay by Check to Student\n\rSA - Pay by Check to Staff\n\r\n\r";
                        break;
                    case "ThirdParty":
                        message.Body += "SA - Pay by 3rd Party to Student\n\rSA - Pay by 3rd Party to Staff\n\r\n\r";
                        break;
                    case "PPlan:":
                        message.Body += "SA - Pay by Payment Plan to Student\n\rSA - Pay by Payment Plan to Staff\n\r\n\r";
                        break;
                }

                message.Body += "This message is notification to IT Staff only.\n\r\n\rThank you!";

                client.Port = 25;  // 25 default smtp port; //587 gmail port
                client.Host = "appmail.atsu.edu";  //from Randy:10.1.1.121     Hotvedt:10.1.92.21
                //client.Host = "smtp.gmail.com";  //gmail
                //client.EnableSsl = true;         //gmail
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(message);   

                Response.Write(message.Body);
            }
            catch (Exception ex)
            {
                Label1.Text += ": IT Notifications Failed.  Please report this error message to helpdesk@atsu.edu - " + ex.Message;
            }
        }
    }
}

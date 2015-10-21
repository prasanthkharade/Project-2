<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Unauthorized.aspx.cs" Inherits="VS2010_TuitionPayment.Unauthorized" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Unauthorized User</title>
            <style type="text/css">
        .style1
        {
            font-family: Arial, Helvetica, sans-serif;
            font-weight: bold;
            font-size: x-large;
            font-style: italic;
        }
        .style2
        {
            width: 100%;
            background-color: #4b70a5;
        }   
        p.MsoNormal
	{margin-bottom:.0001pt;
	font-size:12.0pt;
	font-family:"Times New Roman","serif";
	        margin-left: 0in;
            margin-right: 0in;
            margin-top: 0in;
        }
        </style>

</head>
<body>
    <form id="Unauthorized" runat="server">
        <table class="style2" style="position: static" width="100%">
        <tr>
            <td>
            <img src="images/ATSU_logo.jpg" />
            </td>
        </tr>
    </table>

    <div>
        <asp:Panel ID="Panel1" runat="server" style="position: static" Width="850px" ><span class="style1">Tuition Processing System</span>
        </asp:Panel>
        <br />
        <table style="position: static" width="850">
            <tr>
                <td colspan="1" style="width: 30px">
                </td>
                <td colspan="3">
                    <asp:Label ID="Label1" runat="server" Style="position: static"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="1" style="width: 30px; text-align: left">
                    &nbsp;</td>
                <td colspan="3" style="text-align: left">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="1" style="width: 30px; text-align: left">
                    &nbsp;</td>
                <td colspan="3" style="text-align: left">
                    If you continue to experience problems with login, please contact
                    <a href="mailto:helpdesk@atsu.edu">helpdesk@atsu.edu</a>.&nbsp; </td>
            </tr>
            <tr>
                <td colspan="1" style="width: 30px; text-align: left">
                </td>
                <td colspan="3" style="text-align: left">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 30px; height: 21px">
                </td>
                <td style="width: 388px; height: 21px">
                    &nbsp;</td>
                <td style="width: 100px; height: 21px; text-align: center">
                </td>
                <td style="width: 393px; height: 21px">
                </td>
            </tr>
            <tr>
                <td style="width: 30px">
                </td>
                <td style="width: 388px">
                    </td>
                <td style="width: 100px; text-align: center">
                    </td>
                <td style="width: 393px">
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>

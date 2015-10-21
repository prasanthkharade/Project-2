<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VS2010_TuitionPayment.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Login</title>
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
    <form id="form1" runat="server">
    <div>
        <table class="style2" style="position: static" width="100%">
            <tr>
                <td>
                    <img src="images/ATSU_logo.jpg" /></td>
            </tr>
        </table>
    
    </div>
        <asp:Panel ID="PanelHeader" runat="server" Style="position: static" Width="100%">
            <span class="style1">Tuition Payment System</span>
            <br />
        </asp:Panel>
        <br />
        <asp:Panel ID="Panel1" runat="server" Style="position: static" Width="100%" DefaultButton="Button1">
            <table style="position: static" width="850">
                <tr>
                    <td style="width: 80px; height: 21px; text-align: right">
                        <asp:Label ID="Label2" runat="server" BackColor="White" BorderColor="White" BorderStyle="None"
                            Font-Size="Medium" ForeColor="Black" Style="position: static; text-align: right"
                            Text="Username: " Width="80px"></asp:Label></td>
                    <td style="width: 2px; height: 21px">
                        </td>
                    <td colspan="2" style="height: 21px; text-align: left">
                        <asp:TextBox ID="Username" runat="server" Style="position: static" Width="150px"></asp:TextBox>&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Username"
                            ErrorMessage="*Username Required" Font-Bold="True" Style="position: static" ValidationGroup="Login"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 80px; height: 21px; text-align: right">
                        <asp:Label ID="Label3" runat="server" BackColor="White" BorderColor="White" BorderStyle="None"
                            Font-Size="Medium" ForeColor="Black" Style="position: static; text-align: right"
                            Text="Password:" Width="80px"></asp:Label></td>
                    <td style="width: 2px; height: 21px">
                        </td>
                    <td colspan="2" style="height: 21px; text-align: left">
                        <asp:TextBox ID="Password" runat="server" Style="position: static" TextMode="Password"
                            Width="150px"></asp:TextBox>&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Password"
                            ErrorMessage="*Password Required" Font-Bold="True" Style="position: static" ValidationGroup="Login"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 80px; height: 21px">
                    </td>
                    <td style="width: 2px; height: 21px">
                    </td>
                    <td style="width: 340px; height: 21px; text-align: left">
                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Style="position: static"
                            Text="Login" Width="70px" /></td>
                    <td style="width: 393px; height: 21px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 80px; text-align: center">
                        &nbsp;</td>
                    <td style="width: 2px">
                        </td>
                    <td style="width: 340px; text-align: left">
                    </td>
                    <td style="width: 393px">
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>

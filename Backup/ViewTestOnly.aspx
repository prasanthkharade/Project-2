<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewTestOnly.aspx.cs" Inherits="VS2010_TuitionPayment.ViewTestOnly" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>View/Test Only</title>
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
            <span class="style1">Tuition Payment System - View/Test Only</span>&nbsp;<br />
        </asp:Panel>
        <asp:Panel ID="PanelWelcome" runat="server" Style="position: static" Width="100%">
            <table style="position: static" width="100%">
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                    </td>
                    <td style="text-align: right">
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td style="text-align: right">
                        <asp:Button ID="ButtonLogout" runat="server" OnClick="ButtonLogout_Click" Style="position: static"
                            Text="Logout" /></td>
                </tr>
                <tr>
                    <td>
                        Hello
                        <asp:Label ID="LabelName" runat="server" Style="position: static"></asp:Label>
                    </td>
                    <td>
                    </td>
                    <td style="text-align: right">
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr style="position: static" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="PanelTestingOnly" runat="server" DefaultButton="ButtonShow" Style="position: static"
            Width="100%">
            <table style="position: static" width="850">
                <tr>
                    <td colspan="3">
                        This is for Viewing/Testing purposes only.&nbsp; The pages ahead will replace your
                        name above with the Student Name..&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        If you have questions or concerns, contact kchamblee@atsu.edu or call (480)219.6199.</td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        &nbsp;</td>
                    <td style="width: 2px">
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px; text-align: right" nowrap="noWrap">
                        &nbsp;Student Number:</td>
                    <td style="width: 2px">
                    </td>
                    <td>
                        <asp:TextBox ID="tbStuNum" runat="server" Style="position: static" Width="65px"></asp:TextBox>
                        <asp:Button ID="ButtonShow" runat="server" OnClick="ButtonShow_Click" Style="position: static"
                            Text="Show Me" ValidationGroup="ShowMe" />&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbStuNum"
                            ErrorMessage="RequiredFieldValidator" Font-Bold="True" SetFocusOnError="True"
                            Style="position: static" ValidationGroup="ShowMe">*Student Number Required</asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 22px">
                    </td>
                    <td style="width: 2px; height: 22px">
                    </td>
                    <td style="height: 22px">
                        <asp:Label ID="Label1" runat="server" ForeColor="Red" Style="position: static" Visible="False"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 2px">
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>

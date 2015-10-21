<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="VS2010_TuitionPayment.Results" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">

        .style2
        {
            width: 100%;
            background-color: #4b70a5;
        }   
        .style1
        {
            font-family: Arial, Helvetica, sans-serif;
            font-weight: bold;
            font-size: x-large;
            font-style: italic;
        }
        .style3
        {
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
        <table class="style2" style="position: static" width="100%">
            <tr>
                <td>
                    <img src="images/ATSU_logo.jpg" /></td>
            </tr>
        </table>
        <asp:Panel ID="PanelHeader" runat="server" style="position: static" Width="100%" ><span class="style1">
            Tuition Payment System</span>
        <br /><br />
            <table width="100%">
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td style="text-align: right">
                        <asp:Button ID="Button1" runat="server" OnClick="ButtonLogout_Click" Style="position: static"
                            Text="Logout" /></td>
                </tr>
                <tr>
                    <td>
                        Hello&nbsp;<asp:Label ID="LabelName" runat="server" Style="position: static"></asp:Label>
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
        <asp:Panel ID="Panel1" runat="server" Visible="False">
            <asp:Label ID="Label1" runat="server" Text="MsgHere" 
    Visible="False"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server" Visible="False">
            <table style="width:100%;">
                <tr>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        ATSU</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        Controller&#39;s Office</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        800 W. Jefferson</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        Kirksville, MO 63501</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style3" colspan="4">
                        Thank you for using the ATSU Distance Programs Online Tuition Payment system.</td>
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>

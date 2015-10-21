<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VS2010_TuitionPayment._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Tuition Payment</title>
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
        .style3
        {
            width: 602px;
        }
        .style5
        {
            width: 135px;
        }
        .style8
        {
            width: 44px;
        }
        .style9
        {
            height: 23px;
        }
    </style>
<script type= "text/javascript">
    function ChangeCheckBoxState(id, checkState) {

        var cb = document.getElementById(id);
        if (cb != null)
            cb.checked = checkState;
    }

    function ChangeAllCheckBoxStates(checkState) {

        // Toggles through all of the checkboxes defined in the CheckBoxIDs array
        // and updates their value to the checkState input parameter
        if (cbIDs != null) {
            for (var i = 0; i < cbIDs.length; i++)
                ChangeCheckBoxState(cbIDs[i], checkState);
        }
    }

    function ChangeHeaderAsNeeded() {
        // Whenever a checkbox in the GridView is toggled, we need to
        // check the Header checkbox if ALL of the GridView checkboxes are
        // checked, and uncheck it otherwise
        if (cbIDs != null) {
            // check to see if all other checkboxes are checked
            for (var i = 1; i < cbIDs.length; i++) {
                var cb = document.getElementById(cbIDs[i]);
                if (!cb.checked) {
                    // Whoops, there is an unchecked checkbox, make sure
                    // that the header checkbox is unchecked
                    ChangeCheckBoxState(cbIDs[0], false);
                    return;
                }
            }

            // If we reach here, ALL GridView checkboxes are checked
            ChangeCheckBoxState(cbIDs[0], true);
        }
    }
</script>

</head>
<body>
    <form id="form1" runat="server">
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
                        <asp:Label ID="Label1" runat="server" ForeColor="Red" 
                            Text="Report Error" Visible="False"></asp:Label>
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
        <asp:Panel ID="PanelOutstanding" runat="server" Style="position: static" Width="100%" Visible="False"><table width="100%" style="position: static">
            <tr>
                <td colspan="3">
            Your outstanding tuition charge is listed below.&nbsp; 
                    <br />
                    </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;<asp:GridView ID="GridViewBalanceDue" runat="server" Style="position: static" Width="700px">
                <HeaderStyle BackColor="LightSteelBlue" Font-Bold=true Font-Underline=true HorizontalAlign=center />
            </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="style8">
                </td>
                <td class="style5">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="style8">
                    &nbsp;</td>
                <td class="style5">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style8">
                    &nbsp;</td>
                <td colspan="2">
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" 
                        style="margin-left: 0px" Width="647px">
                        <asp:ListItem Value="FA">   I will be funding my class using Financial Aid. &lt;br&gt;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&lt;b&gt;(FAFSA required and completion of all Award Letter Steps, which will come via email)&lt;/b&gt;</asp:ListItem>
                        <asp:ListItem Value="Check">   I will be paying by check.</asp:ListItem>
                        <asp:ListItem Value="ThirdParty">   I will be using other 3rd party, i.e. VA, Employer, etc.</asp:ListItem>
                        <asp:ListItem Value="PPlan">   I will be using a payment plan set up through the Controller&#39;s office.</asp:ListItem>
                        <asp:ListItem Value="CC">   I will be paying by credit card.</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="style8">
                    &nbsp;</td>
                <td class="style5">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style8">
                    &nbsp;</td>
                <td class="style5">
                    <asp:Button ID="ButtonContinue" runat="server" onclick="ButtonContinue_Click" 
                        Text="Continue" Width="212px" />
                    <asp:Button ID="ButtonMakePmt" runat="server" OnClick="ButtonMakePmt_Click" 
                        Style="position: static" Text="Make Payment" Visible="False" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
            &nbsp;&nbsp;
        </asp:Panel>
        <asp:Panel ID="PanelMsg1" runat="server" Style="position: static" Width="100%" Visible="False"><table width="100%" style="position: static">
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="height: 21px">
            Online tuition payments cannot be made for your program.&nbsp;&nbsp;<br /> <br />
                    Please contact <a href="mailto:controllers@atsu.edu">controllers@atsu.edu</a> or 
                    660.626.2495 if you have questions.</td>
                <td style="height: 21px">
                </td>
                <td style="height: 21px">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
        </asp:Panel>
        <asp:Panel ID="PanelMsg2" runat="server" Style="position: static" Width="100%" Visible="False"><table width="100%" style="position: static">
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="style9">
                    You have charges that are currently being posted to your account.&nbsp; Please wait 
                    3-5 minutes and try again.<br /> </td>
                <td class="style9">
                </td>
                <td class="style9">
                </td>
            </tr>
            <tr>
                <td class="style9">
                </td>
                <td class="style9">
                </td>
                <td class="style9">
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ButtonTryAgain" runat="server" onclick="ButtonTryAgain_Click" 
                        Text="Try again" Width="212px" />
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    Please contact <a href="mailto:controllers@atsu.edu">controllers@atsu.edu</a> or 
                    660.626.2495 if you have questions or continue to experience a delay.</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
        </asp:Panel>
        <asp:Panel ID="PanelMsg3" runat="server" Style="position: static" Width="100%" 
            Visible="False"><table width="100%" style="position: static">
            <tr>
                <td class="style3">
                </td>
                <td class="style5">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    Your login has expired!&nbsp; Please login again.</td>
                <td class="style5">
                    &nbsp;</td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    <asp:Button ID="ButtonRelogin" runat="server" onclick="ButtonRelogin_Click" 
                        Text="Login" Width="135px" />
                </td>
                <td class="style5">
                </td>
                <td>
                </td>
            </tr>
        </table>
        </asp:Panel><asp:Panel ID="PanelTest" runat="server" Style="position: static" Width="100%" Visible="False">
            <asp:Panel ID="PanelMsg4" runat="server" Style="position: static" 
                Visible="False" Width="100%">
                <table style="position: static" width="100%">
                    <tr>
                        <td class="style9">
                        </td>
                        <td class="style9">
                        </td>
                        <td class="style9">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 21px">
                            You have no tuition charges due at this time.&nbsp;&nbsp;<br /> 
                            <br />
                            If you have questions or concerns, please contact
                            <a href="mailto:controllers@atsu.edu">controllers@atsu.edu</a>&nbsp; or 660.626.2495.</td>
                        <td style="height: 21px">
                        </td>
                        <td style="height: 21px">
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            Thank you for using the ATSU Distance Programs Online Tuition Payment system.</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="PanelMsg5" runat="server" Style="position: static" 
                Visible="False" Width="100%">
                <table style="position: static" width="100%">
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="style9">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Underline="False" 
                                ForeColor="Red" Text="ALERT:  Pending charges have been found."></asp:Label>
                            &nbsp;
                        </td>
                        <td class="style9">
                        </td>
                        <td class="style9">
                        </td>
                    </tr>
                    <tr>
                        <td class="style9">
                            <br />
                            Please contact 660.626.2495 and report this Alert message.<br />
                            <br />
                            Thank you.<br />
                        </td>
                        <td class="style9">
                        </td>
                        <td class="style9">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <br />
            <br />
            <hr style="position: static" />
            <hr style="position: static" />
            <hr style="position: static" />
            <table style="position: static" width="100%">
                <tr>
                    <td style="width: 600px">
                        <strong><span style="text-decoration: underline">View/Test Area Only</span></strong></td>
                    <td style="width: 100px">
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="width: 600px">
                        &nbsp;</td>
                    <td style="width: 100px">
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" Caption="<b>For Controller Testing/Viewing Purposes Only</b>"
                DataSourceID="SqlDataSourceTest" Style="position: static" Width="700px">
                <Columns>
                    <asp:BoundField DataField="adprogramdescrip" HeaderText="Program Descrip" SortExpression="adprogramdescrip" />
                    <asp:BoundField DataField="datebilled" HeaderText="Date Billed" ReadOnly="True" SortExpression="datebilled" />
                    <asp:BoundField DataField="arbalance" HeaderText="AR Balance" ReadOnly="True" SortExpression="arbalance">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SchoolStatusID" HeaderText="SchoolStatusID" SortExpression="SchoolStatusID">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Code" SortExpression="Status Code">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Descrip" HeaderText="Descrip" SortExpression="Descrip" />
                </Columns>
                <HeaderStyle BackColor="LightSteelBlue" Font-Underline="True" />
                <AlternatingRowStyle BackColor="WhiteSmoke" />
            </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="width: 600px; height: 21px;">
                    </td>
                    <td style="width: 100px; height: 21px;">
                    </td>
                    <td style="height: 21px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 600px">
                        <asp:Button ID="btnTestEmail" runat="server" OnClick="btnTestEmail_Click" Text="Send Test Email" />
                    </td>
                    <td style="width: 100px; text-align: right">
                        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Style="position: static"
                            Text="Go Back" /></td>
                    <td>
                    </td>
                </tr>
            </table>
            <br />
            &nbsp;<asp:SqlDataSource ID="SqlDataSourceTest" runat="server" ConnectionString="<%$ ConnectionStrings:CampusVue_Live_ConnectionString %>"
                
                SelectCommand="select &#13;&#10;a.adprogramdescrip,&#13;&#10;Convert(varchar,a.datebilled,101) as datebilled,&#13;&#10;Convert(decimal(10,2),a.arbalance) as arbalance, &#13;&#10;a.syschoolstatusid as SchoolStatusID, &#13;&#10;b.code as Status, &#13;&#10;b.descrip as Descrip&#13;&#10;from adenroll a&#13;&#10;inner join syschoolstatus b&#13;&#10;on a.syschoolstatusid = b.syschoolstatusID&#13;&#10;where &#13;&#10;systudentid = @systudentid">
                <SelectParameters>
                    <asp:SessionParameter DbType="Int32" Name="systudentid" SessionField="StudentID" />
                </SelectParameters>
            </asp:SqlDataSource>
            &nbsp;&nbsp;<br />
        </asp:Panel>
        <br />
        <br /><br />
    <div>
    
    </div>
    </form>
</body>
</html>
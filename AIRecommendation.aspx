<%@ Page Title="AI Room Recommendation" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    Async="true"
    CodeBehind="AIRecommendation.aspx.cs"
    Inherits="HotelBookingSystem_New.AIRecommendation" %>
<asp:Content ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div style="max-width:800px; margin:40px auto;">

        <h1 style="text-align:center;">
            AI-Based Room Recommendation
        </h1>

        <p style="text-align:center;">
            Enter your requirements and the system will recommend
            a suitable hotel room.
        </p>

        <hr />

        <h3>Customer Requirements</h3>

        <p>
            <asp:Label ID="lblBudget"
                runat="server"
                Text="Maximum Budget Per Day (₹):">
            </asp:Label>

            <br />

            <asp:TextBox ID="txtBudget"
                runat="server"
                Width="250px">
            </asp:TextBox>
        </p>

        <p>
            <asp:Label ID="lblGuests"
                runat="server"
                Text="Number of Guests:">
            </asp:Label>

            <br />

            <asp:TextBox ID="txtGuests"
                runat="server"
                Width="250px">
            </asp:TextBox>
        </p>

        <p>
            <asp:Label ID="lblRoomType"
                runat="server"
                Text="Preferred Room Type:">
            </asp:Label>

            <br />

            <asp:DropDownList ID="ddlRoomType"
                runat="server"
                Width="260px">

                <asp:ListItem
                    Text="-- Select Room Type --"
                    Value="">
                </asp:ListItem>

                <asp:ListItem
                    Text="Single"
                    Value="Single">
                </asp:ListItem>

                <asp:ListItem
                    Text="Double"
                    Value="Double">
                </asp:ListItem>

                <asp:ListItem
                    Text="Deluxe"
                    Value="Deluxe">
                </asp:ListItem>

                <asp:ListItem
                    Text="Suite"
                    Value="Suite">
                </asp:ListItem>

            </asp:DropDownList>
        </p>

        <p>
            <asp:Label ID="lblFacilities"
                runat="server"
                Text="Required Facilities:">
            </asp:Label>

            <br />

            <asp:TextBox ID="txtFacilities"
                runat="server"
                Width="250px"
                placeholder="Example: AC, Wi-Fi">
            </asp:TextBox>
        </p>

        <br />

        <asp:Button ID="btnRecommend"
            runat="server"
            Text="Get AI Recommendation"
            OnClick="btnRecommend_Click" />

        <br /><br />

        <asp:Panel ID="pnlRecommendation"
            runat="server"
            Visible="false">

            <hr />

            <h3>AI Recommendation</h3>

            <asp:Label ID="lblRecommendation"
                runat="server"
                Font-Bold="true"
                Font-Size="Large">
            </asp:Label>

            <br /><br />

            <asp:Button ID="btnBookRecommended"
                runat="server"
                Text="Book Recommended Room"
                OnClick="btnBookRecommended_Click" />

        </asp:Panel>

        <br />

        <asp:Label ID="lblMessage"
            runat="server"
            Font-Bold="true">
        </asp:Label>

    </div>

</asp:Content>
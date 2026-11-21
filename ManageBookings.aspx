<%@ Page Title="Manage Bookings" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="ManageBookings.aspx.cs"
    Inherits="HotelBookingSystem_New.ManageBookings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div style="max-width:1100px; margin:30px auto;">

        <h1 style="text-align:center;">Manage Bookings</h1>

        <hr />

        <h3>All Bookings</h3>

        <asp:GridView ID="gvBookings"
            runat="server"
            AutoGenerateColumns="False"
            Width="100%"
            BorderWidth="1"
            CellPadding="8"
            AutoGenerateSelectButton="True"
            OnSelectedIndexChanged="gvBookings_SelectedIndexChanged">

            <Columns>

                <asp:BoundField DataField="BookingID"
                    HeaderText="Booking ID" />

                <asp:BoundField DataField="FullName"
                    HeaderText="Customer Name" />

                <asp:BoundField DataField="Mobile"
                    HeaderText="Mobile" />

                <asp:BoundField DataField="RoomNumber"
                    HeaderText="Room Number" />

                <asp:BoundField DataField="RoomType"
                    HeaderText="Room Type" />

                <asp:BoundField DataField="CheckInDate"
                    HeaderText="Check-In" />

                <asp:BoundField DataField="CheckOutDate"
                    HeaderText="Check-Out" />

                <asp:BoundField DataField="TotalCost"
                    HeaderText="Total Cost" />

            </Columns>

        </asp:GridView>

        <br />

        <h3>Update Booking</h3>

        <asp:Label ID="lblBookingID"
            runat="server"
            Text="Booking ID:">
        </asp:Label>

        <br />

        <asp:TextBox ID="txtBookingID"
            runat="server"
            Width="200px"
            ReadOnly="true">
        </asp:TextBox>

        <br /><br />

        <asp:Label ID="lblCheckIn"
            runat="server"
            Text="Check-In Date:">
        </asp:Label>

        <br />

        <asp:TextBox ID="txtNewCheckIn"
            runat="server"
            TextMode="Date"
            Width="200px">
        </asp:TextBox>

        <br /><br />

        <asp:Label ID="lblCheckOut"
            runat="server"
            Text="Check-Out Date:">
        </asp:Label>

        <br />

        <asp:TextBox ID="txtNewCheckOut"
            runat="server"
            TextMode="Date"
            Width="200px">
        </asp:TextBox>

        <br /><br />

        <asp:Button ID="btnUpdate"
            runat="server"
            Text="Update Booking"
            OnClick="btnUpdate_Click" />

        <br /><br />

        <h3>Cancel Booking</h3>

        <asp:Label ID="lblDeleteID"
            runat="server"
            Text="Booking ID:">
        </asp:Label>

        <br />

        <asp:TextBox ID="txtDeleteID"
            runat="server"
            Width="200px">
        </asp:TextBox>

        <br /><br />

        <asp:Button ID="btnDelete"
            runat="server"
            Text="Cancel / Delete Booking"
            OnClick="btnDelete_Click"
            OnClientClick="return confirm('Are you sure you want to cancel this booking?');" />

        <br /><br />

        <asp:Label ID="lblMessage"
            runat="server"
            Font-Bold="true">
        </asp:Label>

    </div>

</asp:Content>
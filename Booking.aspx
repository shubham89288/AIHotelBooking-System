<%@ Page Title="Hotel Booking" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Booking.aspx.cs"
    Inherits="HotelBookingSystem_New.Booking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div style="max-width:900px; margin:30px auto;">

        <h1 style="text-align:center;">Hotel Room Booking</h1>

        <hr />

        <h3>Customer Details</h3>

        <p>
            <asp:Label ID="lblName" runat="server" Text="Full Name:"></asp:Label>
            <br />
            <asp:TextBox ID="txtName" runat="server" Width="300px"></asp:TextBox>
        </p>

        <p>
            <asp:Label ID="lblMobile" runat="server" Text="Mobile:"></asp:Label>
            <br />
            <asp:TextBox ID="txtMobile" runat="server" Width="300px"></asp:TextBox>
        </p>

        <p>
            <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
            <br />
            <asp:TextBox ID="txtEmail" runat="server" Width="300px"></asp:TextBox>
        </p>

        <hr />

        <h3>Available Rooms</h3>

        <asp:GridView ID="gvRooms" runat="server"
            AutoGenerateColumns="False"
            Width="100%"
            BorderWidth="1"
            CellPadding="8">

            <Columns>

                <asp:BoundField DataField="RoomNumber"
                    HeaderText="Room Number" />

                <asp:BoundField DataField="RoomType"
                    HeaderText="Room Type" />

                <asp:BoundField DataField="Price"
                    HeaderText="Price Per Day" />

                <asp:BoundField DataField="Capacity"
                    HeaderText="Capacity" />

                <asp:BoundField DataField="Facilities"
                    HeaderText="Facilities" />

            </Columns>

        </asp:GridView>

        <br />

        <h3>Select Room</h3>

        <asp:DropDownList ID="ddlRoom" runat="server"
            Width="300px">
        </asp:DropDownList>

        <br /><br />

        <h3>Booking Dates</h3>

        <p>
            Check-In Date:
            <br />
            <asp:TextBox ID="txtCheckIn"
                runat="server"
                TextMode="Date"
                Width="200px">
            </asp:TextBox>
        </p>

        <p>
            Check-Out Date:
            <br />
            <asp:TextBox ID="txtCheckOut"
                runat="server"
                TextMode="Date"
                Width="200px">
            </asp:TextBox>
        </p>

        <br />

        <asp:Button ID="btnCalculate"
            runat="server"
            Text="Calculate Total Cost"
            OnClick="btnCalculate_Click" />

        <br /><br />

        <asp:Label ID="lblTotal"
            runat="server"
            Font-Bold="true"
            Font-Size="Large">
        </asp:Label>

        <br /><br />

        <asp:Button ID="btnBook"
    runat="server"
    Text="Book Room"
    OnClick="btnBook_Click"
    OnClientClick="return confirm('Are you sure you want to book this room?');" />

        <br /><br />

        <asp:Label ID="lblMessage"
            runat="server"
            Font-Bold="true">
        </asp:Label>

    </div>

</asp:Content>
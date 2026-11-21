using System;
using System.Data;
using HotelBookingSystem_New.Data;
using BookingModel = HotelBookingSystem_New.Models.Booking;
using CustomerModel = HotelBookingSystem_New.Models.Customer;

namespace HotelBookingSystem_New
{
    public partial class Booking : System.Web.UI.Page
    {
        private readonly BookingRepository repository =
            new BookingRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRooms();

                string recommendedRoomID =
                    Request.QueryString["RoomID"];

                if (!string.IsNullOrEmpty(recommendedRoomID))
                {
                    if (ddlRoom.Items.FindByValue(
                        recommendedRoomID) != null)
                    {
                        ddlRoom.SelectedValue =
                            recommendedRoomID;
                    }
                }
            }
        }

        private void LoadRooms()
        {
            DataTable rooms = repository.GetAvailableRooms();

            gvRooms.DataSource = rooms;
            gvRooms.DataBind();

            ddlRoom.Items.Clear();

            ddlRoom.Items.Add(
                new System.Web.UI.WebControls.ListItem(
                    "-- Select Room --", "0"));

            foreach (DataRow row in rooms.Rows)
            {
                ddlRoom.Items.Add(
                    new System.Web.UI.WebControls.ListItem(
                        row["RoomNumber"].ToString()
                        + " - "
                        + row["RoomType"].ToString()
                        + " - ₹"
                        + row["Price"].ToString(),
                        row["RoomID"].ToString()));
            }
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            if (ddlRoom.SelectedValue == "0")
            {
                lblTotal.Text = "Please select a room.";
                return;
            }

            if (string.IsNullOrEmpty(txtCheckIn.Text) ||
                string.IsNullOrEmpty(txtCheckOut.Text))
            {
                lblTotal.Text = "Please select check-in and check-out dates.";
                return;
            }

            DateTime checkIn;
            DateTime checkOut;

            if (!DateTime.TryParse(txtCheckIn.Text, out checkIn) ||
                !DateTime.TryParse(txtCheckOut.Text, out checkOut))
            {
                lblTotal.Text = "Please enter valid dates.";
                return;
            }

            if (checkOut <= checkIn)
            {
                lblTotal.Text = "Check-out date must be after check-in date.";
                return;
            }

            DataTable rooms = repository.GetAvailableRooms();

            DataRow[] selectedRoom =
                rooms.Select("RoomID = " + ddlRoom.SelectedValue);

            if (selectedRoom.Length == 0)
            {
                lblTotal.Text = "Selected room is not available.";
                return;
            }

            decimal price =
                Convert.ToDecimal(selectedRoom[0]["Price"]);

            int numberOfDays =
                (checkOut - checkIn).Days;

            decimal totalCost =
                price * numberOfDays;

            lblTotal.Text =
                "Total Cost: ₹" + totalCost.ToString("0.00")
                + " for " + numberOfDays + " day(s).";
        }
        protected void btnBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) ||
                    string.IsNullOrWhiteSpace(txtMobile.Text))
                {
                    lblMessage.Text =
                        "Please enter customer name and mobile number.";
                    return;
                }

                if (ddlRoom.SelectedValue == "0")
                {
                    lblMessage.Text = "Please select a room.";
                    return;
                }

                DateTime checkIn;
                DateTime checkOut;

                if (!DateTime.TryParse(txtCheckIn.Text, out checkIn) ||
                    !DateTime.TryParse(txtCheckOut.Text, out checkOut))
                {
                    lblMessage.Text = "Please enter valid booking dates.";
                    return;
                }

                if (checkOut <= checkIn)
                {
                    lblMessage.Text =
                        "Check-out date must be after check-in date.";
                    return;
                }

                DataTable rooms = repository.GetAvailableRooms();

                DataRow[] selectedRoom =
                    rooms.Select("RoomID = " + ddlRoom.SelectedValue);

                if (selectedRoom.Length == 0)
                {
                    lblMessage.Text =
                        "Selected room is not available.";
                    return;
                }

                decimal price =
                    Convert.ToDecimal(selectedRoom[0]["Price"]);

                int numberOfDays =
                    (checkOut - checkIn).Days;

                decimal totalCost =
                    price * numberOfDays;

                CustomerModel customer =
                    new CustomerModel(
                        txtName.Text.Trim(),
                        txtMobile.Text.Trim(),
                        txtEmail.Text.Trim());

                int customerID =
                    repository.AddCustomer(customer);

                BookingModel booking =
                    new BookingModel(
                        customerID,
                        Convert.ToInt32(ddlRoom.SelectedValue),
                        checkIn,
                        checkOut,
                        totalCost);

                int bookingID =
                    repository.AddBooking(booking);

                repository.UpdateRoomAvailability(
                    booking.RoomID);

                lblMessage.Text =
                    "Booking successful! Booking ID: "
                    + bookingID
                    + " | Total Cost: ₹"
                    + totalCost.ToString("0.00");

                LoadRooms();
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Error: " + ex.Message;
            }
        }
    }
}
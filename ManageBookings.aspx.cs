using System;
using System.Data;
using HotelBookingSystem_New.Data;
using BookingModel = HotelBookingSystem_New.Models.Booking;

namespace HotelBookingSystem_New
{
    public partial class ManageBookings : System.Web.UI.Page
    {
        private readonly BookingRepository repository =
            new BookingRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBookings();
            }
        }

        private void LoadBookings()
        {
            try
            {
                DataTable bookings =
                    repository.GetAllBookings();

                gvBookings.DataSource = bookings;
                gvBookings.DataBind();

                if (bookings.Rows.Count == 0)
                {
                    lblMessage.Text = "No bookings found.";
                }
                else
                {
                    lblMessage.Text =
                        "Total Bookings: " + bookings.Rows.Count;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Error: " + ex.Message;
            }
        }

        protected void gvBookings_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            try
            {
                int rowIndex = gvBookings.SelectedIndex;

                string bookingID =
                    gvBookings.Rows[rowIndex].Cells[1].Text;

                txtBookingID.Text = bookingID;
                txtDeleteID.Text = bookingID;

                DataTable booking =
                    repository.GetBookingByID(
                        Convert.ToInt32(bookingID));

                if (booking.Rows.Count > 0)
                {
                    txtNewCheckIn.Text =
                        Convert.ToDateTime(
                            booking.Rows[0]["CheckInDate"])
                        .ToString("yyyy-MM-dd");

                    txtNewCheckOut.Text =
                        Convert.ToDateTime(
                            booking.Rows[0]["CheckOutDate"])
                        .ToString("yyyy-MM-dd");

                    lblMessage.Text =
                        "Booking " + bookingID +
                        " selected for update.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Error: " + ex.Message;
            }
        }

        protected void btnUpdate_Click(
            object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBookingID.Text))
                {
                    lblMessage.Text =
                        "Please select a booking first.";
                    return;
                }

                DateTime checkIn;
                DateTime checkOut;

                if (!DateTime.TryParse(
                        txtNewCheckIn.Text, out checkIn) ||
                    !DateTime.TryParse(
                        txtNewCheckOut.Text, out checkOut))
                {
                    lblMessage.Text =
                        "Please enter valid dates.";
                    return;
                }

                if (checkOut <= checkIn)
                {
                    lblMessage.Text =
                        "Check-out date must be after check-in date.";
                    return;
                }

                DataTable booking =
                    repository.GetBookingByID(
                        Convert.ToInt32(txtBookingID.Text));

                if (booking.Rows.Count == 0)
                {
                    lblMessage.Text =
                        "Booking not found.";
                    return;
                }

                int roomID =
                    Convert.ToInt32(
                        booking.Rows[0]["RoomID"]);

                DataTable rooms =
                    repository.GetAvailableRooms();

                decimal price = 0;

                DataRow[] selectedRoom =
                    rooms.Select(
                        "RoomID = " + roomID);

                if (selectedRoom.Length > 0)
                {
                    price =
                        Convert.ToDecimal(
                            selectedRoom[0]["Price"]);
                }
                else
                {
                    // Get room price directly from database
                    // because the booked room is unavailable.
                    price =
                        GetRoomPrice(roomID);
                }

                int numberOfDays =
                    (checkOut - checkIn).Days;

                decimal totalCost =
                    price * numberOfDays;

                BookingModel updatedBooking =
                    new BookingModel();

                updatedBooking.BookingID =
                    Convert.ToInt32(txtBookingID.Text);

                updatedBooking.CheckInDate = checkIn;
                updatedBooking.CheckOutDate = checkOut;
                updatedBooking.TotalCost = totalCost;

                repository.UpdateBooking(
                    updatedBooking);

                lblMessage.Text =
                    "Booking updated successfully. " +
                    "New Total Cost: ₹" +
                    totalCost.ToString("0.00");

                LoadBookings();
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Error: " + ex.Message;
            }
        }

        private decimal GetRoomPrice(int roomID)
        {
            DataTable rooms =
                repository.GetAllRooms();

            DataRow[] selectedRoom =
                rooms.Select(
                    "RoomID = " + roomID);

            if (selectedRoom.Length > 0)
            {
                return Convert.ToDecimal(
                    selectedRoom[0]["Price"]);
            }

            return 0;
        }

        protected void btnDelete_Click(
            object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDeleteID.Text))
                {
                    lblMessage.Text =
                        "Please enter a Booking ID.";
                    return;
                }

                int bookingID =
                    Convert.ToInt32(txtDeleteID.Text);

                repository.DeleteBooking(bookingID);

                lblMessage.Text =
                    "Booking cancelled successfully.";

                txtBookingID.Text = "";
                txtDeleteID.Text = "";
                txtNewCheckIn.Text = "";
                txtNewCheckOut.Text = "";

                LoadBookings();
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Error: " + ex.Message;
            }
        }
    }
}
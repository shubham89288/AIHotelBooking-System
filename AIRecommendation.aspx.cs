
using System;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using HotelBookingSystem_New.Data;

namespace HotelBookingSystem_New
{
    public partial class AIRecommendation : System.Web.UI.Page
    {
        private readonly BookingRepository repository =
            new BookingRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected async void btnRecommend_Click(
            object sender, EventArgs e)
        {
            try
            {
                pnlRecommendation.Visible = false;
                lblMessage.Text = "";

                // ---------------------------------------
                // 1. READ CUSTOMER REQUIREMENTS
                // ---------------------------------------

                decimal budget;

                if (!decimal.TryParse(
                    txtBudget.Text.Trim(),
                    out budget) ||
                    budget <= 0)
                {
                    lblMessage.Text =
                        "Please enter a valid budget.";

                    return;
                }

                int guests;

                if (!int.TryParse(
                    txtGuests.Text.Trim(),
                    out guests) ||
                    guests <= 0)
                {
                    lblMessage.Text =
                        "Please enter a valid number of guests.";

                    return;
                }

                string preferredRoomType =
                    ddlRoomType.SelectedValue.Trim();

                string requiredFacilities =
                    txtFacilities.Text.Trim();


                // ---------------------------------------
                // 2. GET CURRENT AVAILABLE ROOMS
                // ---------------------------------------

                DataTable rooms =
                    repository.GetAvailableRooms();

                if (rooms.Rows.Count == 0)
                {
                    lblMessage.Text =
                        "No rooms are currently available.";

                    return;
                }


                // ---------------------------------------
                // 3. FILTER ONLY HARD CONSTRAINTS
                // ---------------------------------------
                // C# handles rules that must NEVER be broken:
                //
                // Price <= Budget
                // Capacity >= Guests
                //
                // AI will decide the best room afterwards.

                DataTable validRooms =
                    rooms.Clone();

                foreach (DataRow room in rooms.Rows)
                {
                    decimal price =
                        Convert.ToDecimal(
                            room["Price"]);

                    int roomCapacity =
     Convert.ToInt32(
         room["Capacity"]);

                    // Hard budget rule
                    if (price > budget)
                    {
                        continue;
                    }

                    // Hard guest capacity rule
                    if (roomCapacity < guests)
                    {
                        continue;
                    }

                    validRooms.ImportRow(room);
                }


                // ---------------------------------------
                // 4. CHECK IF ANY ROOM IS POSSIBLE
                // ---------------------------------------

                if (validRooms.Rows.Count == 0)
                {
                    lblMessage.Text =
                        "No available room can satisfy your " +
                        "budget and number of guests.";

                    return;
                }


                // ---------------------------------------
                // 5. CREATE AI PROMPT
                // ---------------------------------------

                StringBuilder prompt =
                    new StringBuilder();

                prompt.AppendLine(
                    "You are an AI hotel room recommendation assistant.");

                prompt.AppendLine();

                prompt.AppendLine(
                    "Your task is to choose the most suitable room " +
                    "from the valid available rooms.");

                prompt.AppendLine(
                    "The rooms listed below have already passed " +
                    "the basic budget and guest-capacity checks.");

                prompt.AppendLine(
                    "You must choose exactly ONE room from the list.");

                prompt.AppendLine(
                    "Do not invent a room.");

                prompt.AppendLine();

                prompt.AppendLine(
                    "CUSTOMER REQUIREMENTS:");

                prompt.AppendLine(
                    "Maximum budget per day: ₹"
                    + budget.ToString("0.00"));

                prompt.AppendLine(
                    "Number of guests: "
                    + guests);

                prompt.AppendLine(
                    "Preferred room type: "
                    + (string.IsNullOrEmpty(
                        preferredRoomType)
                        ? "No preference"
                        : preferredRoomType));

                prompt.AppendLine(
                    "Required facilities: "
                    + (string.IsNullOrEmpty(
                        requiredFacilities)
                        ? "No specific preference"
                        : requiredFacilities));

                prompt.AppendLine();

                prompt.AppendLine(
                    "VALID AVAILABLE ROOMS:");

                foreach (DataRow room
                    in validRooms.Rows)
                {
                    prompt.AppendLine(
                        "RoomID: "
                        + room["RoomID"]);

                    prompt.AppendLine(
                        "Room Number: "
                        + room["RoomNumber"]);

                    prompt.AppendLine(
                        "Room Type: "
                        + room["RoomType"]);

                    prompt.AppendLine(
                        "Price Per Day: ₹"
                        + room["Price"]);

                    prompt.AppendLine(
                        "Capacity: "
                        + room["Capacity"]);

                    prompt.AppendLine(
                        "Facilities: "
                        + room["Facilities"]);

                    prompt.AppendLine();
                }


                // ---------------------------------------
                // 6. AI DECISION RULES
                // ---------------------------------------

                prompt.AppendLine(
                    "AI DECISION RULES:");

                prompt.AppendLine(
                    "1. Choose exactly ONE room from the list.");

                prompt.AppendLine(
                    "2. Never invent a room.");

                prompt.AppendLine(
                    "3. All listed rooms already satisfy the budget " +
                    "and guest-capacity requirements.");

                prompt.AppendLine(
                    "4. Consider the required facilities.");

                prompt.AppendLine(
                    "5. Consider the preferred room type.");

                prompt.AppendLine(
                    "6. Preferred room type is only a preference, " +
                    "not a mandatory requirement.");

                prompt.AppendLine(
                    "7. If another room has better facilities or " +
                    "is more suitable, you may choose that room.");

                prompt.AppendLine(
                    "8. Prefer a room that provides a good balance " +
                    "between price, capacity, room type and facilities.");

                prompt.AppendLine();

                prompt.AppendLine(
                    "Return the result using EXACTLY this format:");

                prompt.AppendLine(
                    "RoomID: [database RoomID]");

                prompt.AppendLine(
                    "Room Number: [hotel room number]");

                prompt.AppendLine(
                    "Room Type: [room type]");

                prompt.AppendLine(
                    "Reason: [short explanation]");


                // ---------------------------------------
                // 7. SEND DATA TO OLLAMA
                // ---------------------------------------

                string aiResponse =
                    await GetOllamaResponse(
                        prompt.ToString());

                if (string.IsNullOrWhiteSpace(
                    aiResponse))
                {
                    lblMessage.Text =
                        "No response received from Ollama.";

                    return;
                }


                // ---------------------------------------
                // 8. READ ROOMID FROM AI RESPONSE
                // ---------------------------------------

                Match roomIDMatch =
                    Regex.Match(
                        aiResponse,
                        @"RoomID\s*:\s*(\d+)",
                        RegexOptions.IgnoreCase);

                if (!roomIDMatch.Success)
                {
                    lblMessage.Text =
                        "AI response received, but the room " +
                        "could not be identified.";

                    lblRecommendation.Text =
                        aiResponse.Replace(
                            Environment.NewLine,
                            "<br />");

                    pnlRecommendation.Visible = true;

                    return;
                }

                int recommendedRoomID =
                    Convert.ToInt32(
                        roomIDMatch.Groups[1].Value);


                // ---------------------------------------
                // 9. VERIFY AI ROOM
                // ---------------------------------------

                DataRow[] selectedRooms =
                    validRooms.Select(
                        "RoomID = " +
                        recommendedRoomID);

                if (selectedRooms.Length == 0)
                {
                    lblMessage.Text =
                        "AI selected an invalid room. " +
                        "Please try again.";

                    return;
                }

                DataRow selectedRoom =
                    selectedRooms[0];


                // ---------------------------------------
                // 10. GET ACTUAL ROOM INFORMATION
                // ---------------------------------------

                int roomNumber =
                    Convert.ToInt32(
                        selectedRoom["RoomNumber"]);

                string roomType =
                    selectedRoom["RoomType"].ToString();

                decimal roomPrice =
                    Convert.ToDecimal(
                        selectedRoom["Price"]);

                int capacity =
                    Convert.ToInt32(
                        selectedRoom["Capacity"]);

                string facilities =
                    selectedRoom["Facilities"].ToString();


                // ---------------------------------------
                // 11. STORE ROOM FOR BOOKING
                // ---------------------------------------

                ViewState["RecommendedRoomID"] =
                    recommendedRoomID.ToString();


                // ---------------------------------------
                // 12. DISPLAY AI RESPONSE
                // ---------------------------------------

                lblRecommendation.Text =
                    aiResponse.Replace(
                        Environment.NewLine,
                        "<br />")
                    + "<br /><br />"
                    + "<b>Verified Available Room</b>"
                    + "<br />"
                    + "Room Number: "
                    + roomNumber
                    + "<br />"
                    + "Room Type: "
                    + roomType
                    + "<br />"
                    + "Price: ₹"
                    + roomPrice.ToString("0.00")
                    + " per day"
                    + "<br />"
                    + "Capacity: "
                    + capacity
                    + " guests"
                    + "<br />"
                    + "Facilities: "
                    + facilities;

                pnlRecommendation.Visible = true;

                lblMessage.Text =
                    "AI recommendation generated successfully.";
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "AI Error: " + ex.Message;
            }
        }


        // ---------------------------------------
        // OLLAMA API
        // ---------------------------------------

        private async System.Threading.Tasks.Task<string>
            GetOllamaResponse(string prompt)
        {
            using (HttpClient client =
                new HttpClient())
            {
                client.Timeout =
                    TimeSpan.FromMinutes(2);

                var requestData =
                    new
                    {
                        model = "llama3.2:3b",
                        prompt = prompt,
                        stream = false
                    };

                JavaScriptSerializer serializer =
                    new JavaScriptSerializer();

                string json =
                    serializer.Serialize(
                        requestData);

                StringContent content =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json");

                HttpResponseMessage response =
                    await client.PostAsync(
                        "http://localhost:11434/api/generate",
                        content);

                response.EnsureSuccessStatusCode();

                string responseText =
                    await response.Content.ReadAsStringAsync();

                var result =
                    serializer.Deserialize<
                        System.Collections.Generic.Dictionary<
                            string,
                            object>>(responseText);

                if (result.ContainsKey("response"))
                {
                    return result["response"].ToString();
                }

                return "";
            }
        }


        // ---------------------------------------
        // BOOK RECOMMENDED ROOM
        // ---------------------------------------

        protected void btnBookRecommended_Click(
            object sender, EventArgs e)
        {
            try
            {
                if (ViewState["RecommendedRoomID"] == null)
                {
                    lblMessage.Text =
                        "Please generate an AI recommendation first.";

                    return;
                }

                string roomID =
                    ViewState["RecommendedRoomID"]
                    .ToString();

                Response.Redirect(
                    "Booking.aspx?RoomID="
                    + roomID);
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Error: " + ex.Message;
            }
        }
    }
}
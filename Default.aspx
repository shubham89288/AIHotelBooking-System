<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="HotelBookingSystem_New._Default" %>

<asp:Content ID="BodyContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <main>

        <!-- Welcome Section -->

        <section style="
            text-align:center;
            padding:60px 20px;
            background:#f5f7fa;
            border-radius:10px;
            margin-top:25px;">

            <h1 style="
                font-size:42px;
                font-weight:bold;
                margin-bottom:15px;">

                Welcome to Hotel Booking System

            </h1>

            <p style="
                font-size:20px;
                margin-bottom:25px;">

                Find a suitable room and book your stay easily.

            </p>

            <a href="AIRecommendation.aspx"
               class="btn btn-primary btn-lg">

                Get AI Room Recommendation

            </a>

            <a href="Booking.aspx"
               class="btn btn-default btn-lg"
               style="margin-left:10px;">

                Book a Room

            </a>

        </section>


        <!-- Features -->

        <section style="margin-top:40px;">

            <h2 style="
                text-align:center;
                margin-bottom:30px;">

                System Features

            </h2>


            <div class="row">

                <!-- AI Recommendation -->

                <div class="col-md-4">

                    <div style="
                        border:1px solid #ddd;
                        border-radius:10px;
                        padding:25px;
                        min-height:210px;
                        text-align:center;
                        margin-bottom:20px;">

                        <h3>🤖 AI Recommendation</h3>

                        <p>
                            Enter your budget, number of guests,
                            preferred room type and required
                            facilities to get a suitable room
                            recommendation.
                        </p>

                        <a href="AIRecommendation.aspx">
                            Try AI Recommendation →
                        </a>

                    </div>

                </div>


                <!-- Easy Booking -->

                <div class="col-md-4">

                    <div style="
                        border:1px solid #ddd;
                        border-radius:10px;
                        padding:25px;
                        min-height:210px;
                        text-align:center;
                        margin-bottom:20px;">

                        <h3>🏨 Easy Booking</h3>

                        <p>
                            Select a suitable room, enter customer
                            details and booking dates, and
                            automatically calculate the total cost.
                        </p>

                        <a href="Booking.aspx">
                            Book a Room →
                        </a>

                    </div>

                </div>


                <!-- Manage Bookings -->

                <div class="col-md-4">

                    <div style="
                        border:1px solid #ddd;
                        border-radius:10px;
                        padding:25px;
                        min-height:210px;
                        text-align:center;
                        margin-bottom:20px;">

                        <h3>📋 Manage Bookings</h3>

                        <p>
                            View existing bookings and perform
                            update or cancellation operations
                            when required.
                        </p>

                        <a href="ManageBookings.aspx">
                            Manage Bookings →
                        </a>

                    </div>

                </div>

            </div>

        </section>


        <!-- How It Works -->

        <section style="
            margin-top:35px;
            margin-bottom:30px;
            text-align:center;">

            <h2>How It Works</h2>

            <p style="font-size:18px; margin-top:20px;">

                Enter Requirements
                &nbsp; → &nbsp;
                Get AI Recommendation
                &nbsp; → &nbsp;
                Select Dates
                &nbsp; → &nbsp;
                Book Room

            </p>

        </section>

    </main>

</asp:Content>
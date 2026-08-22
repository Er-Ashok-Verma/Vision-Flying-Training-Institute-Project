<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentReceipt.aspx.cs" Inherits="Flying_Hour_PaymentReceipt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Payment Receipt</title>

    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <style>
        * {
            box-sizing: border-box;
        }

        body {
            margin: 0;
            padding: 0;
            background: #f5f5f5;
            font-family: Arial, Helvetica, sans-serif;
            color: #222;
        }

        .receipt-page {
            width: 100%;
            max-width: 650px;
            margin: 0 auto;
            background: #fff;
            min-height: 100vh;
        }

        .top-section {
            padding: 28px 30px 22px;
            background: #fff;
        }

        .success-row {
            display: flex;
            align-items: center;
            gap: 18px;
        }

        .success-icon {
            width: 55px;
            height: 55px;
            border-radius: 50%;
            background: #6d22c9;
            color: #fff;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 30px;
            font-weight: bold;
        }

        .success-title {
            font-size: 30px;
            font-weight: 600;
            margin-bottom: 5px;
        }

        .success-date {
            font-size: 18px;
            color: #555;
        }

        .receipt-card {
            margin: 0 20px 25px;
            background: #fff;
            border-radius: 28px;
            padding: 28px 28px 32px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.08);
        }

        .section-title {
            font-size: 23px;
            font-weight: 600;
            margin-bottom: 25px;
        }

        .paid-row {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            gap: 15px;
        }

        .institute-box {
            display: flex;
            align-items: center;
            gap: 18px;
        }

        .institute-icon {
            width: 72px;
            height: 72px;
            border-radius: 18px;
            background: #6d22c9;
            color: #fff;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 30px;
            font-weight: bold;
        }

        .institute-name {
            font-size: 23px;
            font-weight: 500;
            line-height: 1.3;
        }

        .payment-amount {
            font-size: 25px;
            font-weight: 700;
            white-space: nowrap;
        }

        .divider {
            border: 0;
            border-top: 1px solid #ddd;
            margin: 28px 0;
        }

        .details-title {
            font-size: 23px;
            font-weight: 500;
            margin-bottom: 25px;
        }

        .detail-item {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 20px;
            margin-bottom: 18px;
            padding-bottom: 10px;
            border-bottom: 1px solid #eee;
        }

        .detail-label {
            display: block;
            color: #777;
            font-size: 16px;
            font-weight: 500;
            min-width: 200px;
        }

        .detail-value {
            display: block;
            font-size: 17px;
            font-weight: 500;
            text-align: right;
            word-break: break-word;
            flex: 1;
        }

        @media (max-width: 600px) {
            .detail-item {
                align-items: flex-start;
                gap: 10px;
            }

            .detail-label {
                min-width: 130px;
                font-size: 14px;
            }

            .detail-value {
                font-size: 15px;
                text-align: right;
            }
        }

        .amount-value {
            font-size: 23px;
            font-weight: 600;
        }

        .success-status {
            color: #16803a;
            font-weight: 600;
        }

        .footer {
            text-align: center;
            padding: 35px 20px 50px;
            color: #777;
        }

        .powered {
            font-size: 17px;
            margin-bottom: 10px;
        }

        .razorpay-text {
            font-size: 23px;
            font-weight: 700;
            color: #2867c7;
        }

        .print-area {
            text-align: center;
            padding: 0 20px 35px;
        }

        .print-btn {
            border: 0;
            background: #6d22c9;
            color: #fff;
            padding: 13px 35px;
            border-radius: 8px;
            font-size: 16px;
            cursor: pointer;
        }

        @media (max-width: 600px) {

            .top-section {
                padding: 22px 18px;
            }

            .success-title {
                font-size: 24px;
            }

            .success-date {
                font-size: 16px;
            }

            .success-icon {
                width: 48px;
                height: 48px;
                font-size: 25px;
            }

            .receipt-card {
                margin: 0 10px 20px;
                padding: 22px 18px 25px;
                border-radius: 22px;
            }

            .section-title,
            .details-title {
                font-size: 20px;
            }

            .institute-icon {
                width: 25px !important;
                height: 45px !important;
                overflow: hidden;
                flex-shrink: 0;
            }

                .institute-icon img {
                    width: 25px !important;
                    height: 25px !important;
                    max-width: 25px !important;
                    max-height: 25px !important;
                    object-fit: contain !important;
                    display: block;
                }


            .institute-name {
                font-size: 18px;
            }

            .payment-amount {
                font-size: 21px;
            }
        }

        @media print {

            body {
                background: #fff;
            }

            .receipt-page {
                max-width: 100%;
            }

            .print-area {
                display: none;
            }

            .receipt-card {
                box-shadow: none;
                margin: 0;
            }
        }
    </style>
</head>

<body>

    <form id="form1" runat="server">

        <div class="receipt-page">

            <div class="top-section">

                <div class="success-row">

                    <div class="success-icon">
                        ✓
                    </div>

                    <div>
                        <div class="success-title">
                            Transaction Successful
                        </div>

                        <div class="success-date">
                            <asp:Label ID="lblPaymentDate" runat="server"></asp:Label>
                        </div>
                    </div>

                </div>

            </div>


            <div class="receipt-card">

                <div class="section-title">
                    Paid to
                </div>

                <div class="paid-row">

                    <div class="institute-box">

                        <div class="institute-icon">
                            <img src="Vision.png" style="width: 72px; height: 72px; object-fit: contain;" />
                        </div>

                        <div class="institute-name">
                            <asp:Label ID="lblInstituteName" runat="server">
                            </asp:Label>
                        </div>

                    </div>

                    <div class="payment-amount">
                        ₹<asp:Label ID="lblAmount" runat="server"></asp:Label>
                    </div>

                </div>


                <hr class="divider" />


                <div class="details-title">
                    ▣ &nbsp; Payment Details
                </div>


                <div class="detail-item">

                    <span class="detail-label">Registration No.
                    </span>

                    <span class="detail-value">
                        <asp:Label ID="lblRegNo" runat="server"></asp:Label>
                    </span>

                </div>


                <div class="detail-item">

                    <span class="detail-label">Student Name
                    </span>

                    <span class="detail-value">
                        <asp:Label ID="lblStudentName" runat="server"></asp:Label>
                    </span>

                </div>

                <div class="detail-item">
                    <span class="detail-label">Course
                    </span>

                    <span class="detail-value">
                        <asp:Label ID="lblCourseName" runat="server"></asp:Label>
                    </span>
                </div>


                <div class="detail-item">

                    <span class="detail-label">Receipt No.
                    </span>

                    <span class="detail-value">
                        <asp:Label ID="lblReceiptNo" runat="server"></asp:Label>
                    </span>

                </div>


                <div class="detail-item">

                    <span class="detail-label">Razorpay Payment ID
                    </span>

                    <span class="detail-value">
                        <asp:Label ID="lblPaymentId" runat="server"></asp:Label>
                    </span>

                </div>


                <div class="detail-item">

                    <span class="detail-label">Payment Status
                    </span>

                    <span class="detail-value success-status">
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </span>

                </div>


                <div class="detail-item">

                    <span class="detail-label">Amount Paid
                    </span>

                    <span class="detail-value amount-value">₹<asp:Label ID="lblPaidAmount" runat="server"></asp:Label>
                    </span>

                </div>

            </div>


            <div class="print-area">

                <button type="button" class="print-btn" onclick="window.print();">
                    Print Receipt
                </button>

            </div>
 
        </div>

    </form>

</body>
</html>

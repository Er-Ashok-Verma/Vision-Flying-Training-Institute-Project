<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Entries_Receipt.aspx.cs" Inherits="Flying_Hour_Entries_Receipt" %>

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
            background: #e9e9e9;
            font-family: Arial, Helvetica, sans-serif;
            color: #111;
        }

        .receipt-page {
            width: 100%;
            max-width: 850px;
            margin: 20px auto;
            background: #fff;
            padding: 25px 35px 35px;
        }

        /* Header */
        .header {
            text-align: center;
            margin-bottom: 10px;
        }

        .header-logo-row {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 16px;
            margin-bottom: 6px;
        }

            .header-logo-row img {
                height: 55px;
                width: auto;
            }

        .uni-name {
            font-family: Georgia, 'Times New Roman', serif;
            font-size: 26px;
            font-weight: 700;
            color: #2b2f77;
            line-height: 1.1;
            text-align: left;
        }

        .uni-full-name {
            font-size: 22px;
            font-weight: 700;
            margin: 8px 0 4px;
        }

        .uni-address {
            font-size: 14px;
            margin-bottom: 2px;
        }

        .uni-phone {
            font-size: 14px;
            margin-bottom: 10px;
        }

        /* Receipt No / Date */
        .meta-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            font-size: 15px;
            font-weight: 600;
            margin-bottom: 10px;
        }

        /* Outer Border */
        .receipt-box {
            border: 2px solid #000;
        }

        /* Student Information */
        .student-info {
            padding: 14px 18px;
            font-size: 15px;
        }

            .student-info div {
                margin-bottom: 6px;
            }

            .student-info .label {
                display: inline-block;
                min-width: 160px;
                font-weight: 700;
            }

        /* Particulars Table */
        table.particulars {
            width: 100%;
            border-collapse: collapse;
            font-size: 15px;
        }

            table.particulars th,
            table.particulars td {
                border: 1px solid #000;
                padding: 8px 12px;
            }

            table.particulars th {
                font-weight: 700;
                text-align: left;
                background: #fff;
            }

                table.particulars th.sno,
                table.particulars td.sno {
                    text-align: center;
                    width: 60px;
                }

                table.particulars th.amount,
                table.particulars td.amount {
                    text-align: right;
                    width: 160px;
                }

            table.particulars tr.min-row td {
                height: 90px;
                vertical-align: top;
            }

        /* Payment Mode */
        .pay-mode-row {
            display: flex;
            border-top: 1px solid #000;
        }

        .pay-mode-left {
            flex: 1.4;
            padding: 12px 18px;
            border-right: 1px solid #000;
            font-size: 15px;
        }

            .pay-mode-left div {
                margin-bottom: 8px;
            }

        .pay-mode-right {
            flex: 1;
        }

        /* Deposited Amount */
        .deposited-amount {
            display: flex;
            border-bottom: 1px solid #000;
        }

            .deposited-amount .label {
                padding: 10px 12px;
                font-weight: 700;
                font-size: 15px;
            }

            .deposited-amount .value {
                padding: 10px 12px;
                font-size: 15px;
                border-left: 1px solid #000;
                flex: 1;
                text-align: right;
            }

        /* Amount Words */
        .amount-words {
            padding: 10px 12px;
            font-size: 15px;
        }

            .amount-words .label {
                font-weight: 700;
                margin-bottom: 4px;
            }

            .amount-words .words {
                margin-top: 4px;
                line-height: 1.4;
            }

        /* Status */
        .status-row {
            display: flex;
            justify-content: space-between;
            padding: 10px 18px;
            border-top: 1px solid #000;
            font-size: 14px;
        }

            .status-row .success-status {
                color: #16803a;
                font-weight: 700;
            }

        /* Footer */
        .footer-row {
            display: flex;
            justify-content: space-between;
            align-items: flex-end;
            padding: 14px 4px 0;
            font-size: 13px;
        }

            .footer-row .note {
                max-width: 65%;
            }

                .footer-row .note strong {
                    display: block;
                    margin-bottom: 4px;
                }

                .footer-row .note ol {
                    margin: 0;
                    padding-left: 18px;
                }

            .footer-row .generated {
                text-align: right;
                font-weight: 600;
            }

        /* Print */
        .print-area {
            text-align: center;
            padding: 25px 0 0;
        }

        .print-btn {
            border: 0;
            background: #2b2f77;
            color: #fff;
            padding: 12px 32px;
            border-radius: 6px;
            font-size: 15px;
            cursor: pointer;
        }

        @media print {
            body {
                background: #fff;
            }

            .receipt-page {
                max-width: 100%;
                margin: 0;
            }

            .print-area {
                display: none;
            }
        }

        @media (max-width: 600px) {

            .receipt-page {
                padding: 16px;
            }

            .header-logo-row {
                gap: 8px;
            }

                .header-logo-row img {
                    height: 40px;
                }

            .uni-name {
                font-size: 18px;
            }

            .uni-full-name {
                font-size: 16px;
            }

            .uni-address,
            .uni-phone {
                font-size: 11px;
            }

            .meta-row,
            .student-info,
            table.particulars,
            .pay-mode-left,
            .deposited-amount .label,
            .deposited-amount .value,
            .amount-words {
                font-size: 12px;
            }

                .student-info .label {
                    min-width: 110px;
                }

            .pay-mode-row {
                flex-direction: column;
            }

            .pay-mode-left {
                border-right: 0;
                border-bottom: 1px solid #000;
            }

            .footer-row {
                flex-direction: column;
                align-items: flex-start;
                gap: 15px;
            }

                .footer-row .note {
                    max-width: 100%;
                }

                .footer-row .generated {
                    text-align: left;
                }
        }
    </style>
</head>

<body>

    <form id="form1" runat="server">
        <div class="receipt-page">

            <!-- Header -->
            <div class="header">
                <div class="header-logo-row">
                    <img src="Vision.png" alt="Institute Logo" />
                    <div>
                        <div class="uni-name">VISION FLYING TRAINING INSTITUTE</div>
                    </div>
                </div>

                <div class="uni-full-name">
                    <asp:Label ID="lblInstituteName" runat="server"></asp:Label>
                </div>
                <div class="uni-address">D, SEC 07, Ramphal Chowk Rd, Block D, Palam Extension, Dwarka, NEW DELHI, Delhi, 110077</div>
                <div class="uni-phone">Ph- 8462875138</div>
            </div>

            <!-- Receipt No / Date -->
            <div class="meta-row">
                <div>
                    Receipt No. :
                    <asp:Label ID="lblReceiptNo" runat="server"></asp:Label>
                </div>
                <div>
                    Date :
                    <asp:Label ID="lblPaymentDate" runat="server"></asp:Label>
                </div>
            </div>

            <!-- Receipt Box -->
            <div class="receipt-box">

                <!-- Student Information -->
                <div class="student-info">
                    <div>
                        <span class="label">Student Name</span> :
                        <asp:Label ID="lblStudentName" runat="server"></asp:Label>
                    </div>
                    <div>
                        <span class="label">Father Name</span> :
                        <asp:Label ID="lblFatherName" runat="server"></asp:Label>
                    </div>
                    <div>
                        <span class="label">Name of the Course</span> :
                        <asp:Label ID="lblCourseName" runat="server"></asp:Label>
                    </div>
                </div>

                <!-- Particulars -->
                <table class="particulars">
                    <tr>
                        <th class="sno">Sr.No.</th>
                        <th>Particulars</th>
                        <th class="amount">Received Amount</th>
                    </tr>
                    <tr class="min-row">
                        <td class="sno">1</td>
                        <td>
                            <asp:Label ID="lblFeeHeadName" runat="server"></asp:Label></td>
                        <td class="amount">₹
                            <asp:Label ID="lblAmount" runat="server"></asp:Label></td>
                    </tr>
                </table>

                <!-- Payment Details -->
                <div class="pay-mode-row">

                    <div class="pay-mode-left">
                        <div>
                            <strong>By :</strong>
                            <asp:Label ID="lblPaymentMode" runat="server"></asp:Label>
                        </div>
                        <div>
                            <strong>DD/Chq No. :</strong><br />
                            <asp:Label ID="lblPaymentId" runat="server"></asp:Label>
                        </div>
                        <div>
                            <strong>Date :</strong><br />
                            <asp:Label ID="lblDate" runat="server"></asp:Label>
                        </div>
                        <div>
                            <strong>Bank Name :</strong><br />
                            <asp:Label ID="lblBankName" runat="server"></asp:Label>
                        </div>
                    </div>

                    <!-- Amount -->
                    <div class="pay-mode-right">
                        <div class="deposited-amount">
                            <div class="label">Deposited Amount</div>
                            <div class="value">
                                ₹
                                <asp:Label ID="lblPaidAmount" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="amount-words">
                            <div class="label">Amount in Words</div>
                            <div class="words">
                                <asp:Label ID="lblAmountWords" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>

                </div>

                <!-- Payment Status -->
                <div class="status-row">
                    <div>Payment Status</div>
                    <div class="success-status">
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </div>
                </div>

            </div>

            <!-- Footer -->
            <div class="footer-row">

                <div class="note">
                    <strong>Note</strong>
                    <ol>
                        <li>Cheques/DD subject to clearance.</li>
                        <li>Please bring original first receipt if fees is deposited in installments.</li>
                        <li>Receipt will be valid only after realisation of amount in our bank.</li>
                    </ol>
                </div>

                <div class="generated">
                    Generated By :<br />
                    Student
                </div>

            </div>

            <!-- Print Button -->
            <div class="print-area">
                <button type="button" class="print-btn" onclick="window.print();">Print Receipt</button>
            </div>

        </div>
    </form>

</body>
</html>


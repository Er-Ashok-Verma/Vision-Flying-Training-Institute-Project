<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Student_Payment.aspx.cs" Inherits="Flying_Hour_Student_Payment" %>
 <!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Student Payment</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css" />
    <script src="https://checkout.razorpay.com/v1/checkout.js"></script>
    <style>

        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            background: #f5f7fc;
            min-height: 500px;
            font-family: 'Segoe UI', Roboto, system-ui, sans-serif;
            padding: 16px;
        }

        .main-card {
            background: #ffffff;
            border-radius: 24px;
            padding: 28px 24px;
            box-shadow: 0 12px 40px rgba(0, 0, 0, 0.08);
            border: 1px solid #eaedf2;
            margin-top: 12px;
            transition: all 0.2s ease;
        }

        .title {
            background: #1e2f4a;
            padding: 12px 16px;
            border-radius: 6px;
            text-align: center;
            font-size: 22px;
            font-weight: 600;
            color: #ffffff;
            margin-bottom: 28px;
            letter-spacing: 0.3px;
            box-shadow: 0 4px 10px rgba(30, 47, 74, 0.15);
        }

        .title i {
            color: #f9c74f;
            margin-right: 10px;
        }

        .form-label {
            font-weight: 600;
            color: #1e2f4a;
            margin-bottom: 6px;
            font-size: 14px;
            letter-spacing: 0.2px;
        }

        .input-icon {
            position: relative;
        }

        .input-icon i {
            position: absolute;
            left: 14px;
            top: 50%;
            transform: translateY(-50%);
            color: #1e2f4a;
            font-size: 16px;
            opacity: 0.7;
        }

        .form-control {
            height: 46px;
            border: 1px solid #d0d7e2;
            border-radius: 12px;
            padding: 10px 16px 10px 44px;
            font-size: 15px;
            background: #ffffff;
            box-shadow: none;
            transition: border 0.2s, box-shadow 0.2s;
        }

        .form-control:focus {
            border-color: #1e2f4a;
            box-shadow: 0 0 0 3px rgba(30, 47, 74, 0.08);
        }

        .student-info {
            margin-top: 20px;
            padding: 20px 18px;
            background: #f8faff;
            border-radius: 20px;
            border: 1px solid #e6ecf3;
        }

        .info-box {
            background: #ffffff;
            border-radius: 14px;
            padding: 12px 14px;
            border: 1px solid #e6ecf3;
        }

        .info-box .form-label {
            margin-bottom: 4px;
            font-size: 13px;
            color: #4a5b6f;
        }

        .info-box .form-control {
            background: #f9fbfe;
            border: 1px solid #dce2ea;
            padding-left: 16px;
            height: 40px;
            font-weight: 500;
            color: #1e2f4a;
        }

        .amount-box {
            margin-top: 18px;
        }

        .amount-box .form-control {
            padding-left: 16px;
            font-weight: 600;
            font-size: 16px;
            background: #ffffff;
            border-color: #d0d7e2;
        }

        .button-area {
            text-align: center;
            margin-top: 22px;
            display: flex;
            flex-wrap: wrap;
            align-items: center;
            justify-content: center;
            gap: 12px;
        }

        .btn-search {
            height: 46px;
            min-width: 120px;
            background: #1e2f4a;
            color: #ffffff;
            border: none;
            border-radius: 60px;
            font-weight: 600;
            padding: 0 28px;
            transition: 0.2s;
            box-shadow: 0 4px 8px rgba(30, 47, 74, 0.12);
        }

        .btn-search:hover {
            background: #0f1e30;
            color: #ffffff;
            transform: translateY(-2px);
            box-shadow: 0 8px 14px rgba(30, 47, 74, 0.18);
        }

        .btn-login {
            height: 46px;
            min-width: 140px;
            background: #1a7e4b;
            color: #ffffff;
            border: none;
            border-radius: 60px;
            font-weight: 600;
            padding: 0 28px;
            transition: 0.2s;
            box-shadow: 0 4px 8px rgba(26, 126, 75, 0.15);
        }

        .btn-login:hover {
            background: #0f673a;
            color: #ffffff;
            transform: translateY(-2px);
            box-shadow: 0 8px 14px rgba(26, 126, 75, 0.2);
        }

        .btn-login i {
            margin-right: 6px;
        }

        @media (max-width: 768px) {
            body {
                padding: 8px;
            }

            .main-card {
                padding: 18px 14px;
                border-radius: 20px;
                margin-top: 6px;
            }

            .title {
                font-size: 19px;
                padding: 10px 12px;
                border-radius: 40px;
            }

            .student-info {
                padding: 14px 12px;
            }

            .info-box {
                padding: 10px 12px;
            }

            .btn-search,
            .btn-login {
                width: 100%;
                min-width: unset;
                margin: 4px 0;
            }

            .button-area {
                flex-direction: column;
                gap: 8px;
            }

            .form-control {
                height: 44px;
                font-size: 15px;
                padding-left: 40px;
            }

            .input-icon i {
                left: 12px;
                font-size: 15px;
            }
        }

        @media (max-width: 480px) {
            .main-card {
                padding: 14px 10px;
            }

            .title {
                font-size: 17px;
                padding: 8px 10px;
            }

            .info-box .form-control {
                height: 38px;
                font-size: 14px;
            }

            .btn-search,
            .btn-login {
                height: 44px;
                font-size: 15px;
            }
        }

        /* loading overlay */
        .update-progress-overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(255, 255, 255, 0.6);
            backdrop-filter: blur(2px);
            display: flex;
            align-items: center;
            justify-content: center;
            z-index: 9999;
        }

        .progress-card {
            background: white;
            padding: 24px 40px;
            border-radius: 40px;
            box-shadow: 0 20px 50px rgba(0, 0, 0, 0.15);
            text-align: center;
        }

        .progress-card img {
            max-width: 60px;
            height: auto;
        }

        .hidden-btn {
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableViewState="true" EnablePageMethods="true">
            <Services>
                <asp:ServiceReference Path="~/EducationService.asmx" />
            </Services>
        </asp:ScriptManager>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="container">
                    <div class="main-card">
                        <div class="title">
                            <i class="fa-regular fa-credit-card"></i> Student Payment
                        </div>

                        <div class="row justify-content-center">
                            <div class="col-lg-8 col-md-10 col-12">
                                <div class="mb-3">
                                    <label class="form-label"><i class="fa-regular fa-id-card me-2"></i>Registration No.</label>
                                    <div class="input-icon">
                                        <i class="fa-regular fa-address-card"></i>
                                   <asp:TextBox ID="txtRegNo" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtRegNo_TextChanged"></asp:TextBox>                                    </div>
                                </div>

                                <div class="button-area">
 
                                </div>

                                <asp:HiddenField ID="hfOrderId" runat="server" />
                                <asp:HiddenField ID="hfAmount" runat="server" />
                                <asp:HiddenField ID="hfPublicToken" runat="server" />
                                <asp:HiddenField ID="hfPaymentId" runat="server" />
                                <asp:HiddenField ID="hfPaymentOrderId" runat="server" />
                                <asp:HiddenField ID="hfPaymentSignature" runat="server" />

                               <div class="student-info" id="div" Visible="false" runat="server">
                                    <div class="row g-3">
                                        <div class="col-md-6 col-12">
                                            <div class="info-box">
                                                <label class="form-label"><i class="fa-regular fa-user me-2"></i>Student Name</label>
                                                <asp:Label ID="lblStuName" runat="server" CssClass="form-control"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-6 col-12">
                                            <div class="info-box">
                                                <label class="form-label"><i class="fa-regular fa-building-columns me-2"></i>Course</label>
                                                <asp:Label ID="txt_course_name" runat="server" CssClass="form-control"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="amount-box">
                                        <label class="form-label"><i class="fa-solid fa-indian-rupee-sign me-2"></i>Amount</label>
                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" placeholder="Enter amount"></asp:TextBox>
                                    </div>

                                    <div class="button-area">
                                        <asp:Button ID="btnVerifyPayment" runat="server" Text="Verify Payment" Style="display: none;" OnClick="btnVerifyPayment_Click" />
                                        <asp:Button ID="btnPayment" runat="server" Text="Pay Now" CssClass="btn-login" OnClick="btnPayment_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
            <ProgressTemplate>
                <div class="update-progress-overlay">
                    <div class="progress-card">
                        <img src="../images/wait.gif" alt="Processing" />
                        <br /><br />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
</body>
</html>
 


 <%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentFlyingHours_View.aspx.cs" Inherits="Flying_Hour_StudentFlyingHours_View" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Student Flying Hours View</title>

    <!-- Bootstrap 5 + Icons (Student Registration Style) -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css" />

    <!-- DataTables CSS (for advanced table) -->
    <link href="https://cdn.datatables.net/1.13.7/css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/buttons/2.4.2/css/buttons.dataTables.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/responsive/2.5.0/css/responsive.dataTables.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/fixedheader/3.4.0/css/fixedHeader.dataTables.min.css" rel="stylesheet" />

    <style>
        /* ----- GLOBAL RESET (matches student registration) ----- */
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            background: white;
            min-height: 100vh;
            font-family: 'Segoe UI', sans-serif;
            padding: 20px;
        }

        /* MAIN CARD (exact style from registration) */
        .main-card {
            background: white;
            border-radius: 25px;
            padding: 35px;
            box-shadow: 0 10px 35px rgba(0, 0, 0, 0.25);
            border: 1px solid rgba(255, 255, 255, 0.2);
            margin-top: 20px;
            animation: fadeIn 0.8s ease-in-out;
        }

        /* TITLE (exact style) */
        .title {
            background-color: #243b55;
            padding: 10px;
            border-radius: 2px;
            text-align: center;
            font-size: 24px;
            font-weight: 500;
            color: white;
            margin-bottom: 35px;
            text-shadow: 2px 2px 8px rgba(0, 0, 0, 0.3);
        }

        .title i {
            color: #ffd43b;
            margin-right: 10px;
             font-size:24px;
        }

        /* INFO ROWS – label + value (clean, bold labels) */
        .info-row {
            display: flex;
            align-items: baseline;
            flex-wrap: wrap;
            padding: 6px 0;
            border-bottom: 1px dashed #e9ecef;
        }

        .info-row .info-label {
            font-weight: 700;
            color: #1e2a3a;
            min-width: 160px;
        }

        .info-row .info-value {
            font-weight: 500;
            color: #0b1a33;
            word-break: break-word;
        }

        /* STAT CARDS - styled like registration badges but with colors */
        .stat-card {
            background: #f8f9fa;
            border: 1px solid #e9ecef;
            border-radius: 16px;
            padding: 8px 10px;
            text-align: center;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
            transition: 0.3s;
            height: 100%;
            width:75%;
            margin-left:25px;
        }

        .stat-card:hover {
            transform: translateY(-4px);
            box-shadow: 0 8px 20px rgba(0, 0, 0, 0.12);
        }

        .stat-card .stat-label {
            font-weight: 600;
            color: #1e2a3a;
            font-size: 14px;
            letter-spacing: 0.5px;
        }

        .stat-card .stat-value {
            font-weight: 700;
            font-size: 18px;
            margin-top: 6px;
            display: block;
        }

        .stat-card .stat-icon {
            font-size: 12px;
            margin-right: 8px;
        }

        /* individual stat colors */
        .stat-alloted .stat-value { color: #0d6efd; }
        .stat-eligible .stat-value { color: #198754; }
        .stat-flying .stat-value { color: #ff6a00; }
        .stat-balance .stat-value { color: #dc3545; }

        /* GRID BOX (same as registration) */
        .grid-box {
            background: rgba(255, 255, 255, 0.12);
            border-radius: 25px;
            padding: 25px;
            margin-top: 30px;
            overflow-x: auto;
            box-shadow: 0px 10px 30px rgba(0, 0, 0, 0.25);
            animation: fadeIn 0.8s ease-in-out;
        }

        /* TABLE (matching registration table style) */
        .table {
            min-width: 900px;
            border-radius: 15px;
            overflow: hidden;
            background: white;
            width: 100% !important;
            border-collapse: collapse;
        }

        .table th {
            background: linear-gradient(45deg, #141e30, #243b55);
            color: white;
            text-align: center;
            padding: 14px;
            font-size: 15px;
            white-space: nowrap;
        }

        .table td {
            text-align: center;
            vertical-align: middle;
            padding: 12px;
            white-space: nowrap;
        }

        .table tr:hover {
            background-color: #f1f5ff;
            transition: 0.3s;
        }

        /* DataTables override to keep consistency */
        .dataTables_wrapper .dataTables_filter input {
            border: 2px solid #ced4da;
            border-radius: 30px;
            padding: 6px 16px;
            outline: none;
        }

        .dataTables_wrapper .dataTables_filter input:focus {
            border-color: #243b55;
            box-shadow: 0 0 0 0.2rem rgba(36, 59, 85, 0.25);
        }

        .dataTables_wrapper .dataTables_paginate .paginate_button.current {
            background: #243b55 !important;
            color: white !important;
            border-radius: 30px;
            border: none;
        }

        .dataTables_wrapper .dataTables_paginate .paginate_button {
            border-radius: 30px;
        }

        .dataTables_wrapper .dt-buttons .btn {
            background: #f8f9fa;
            border: 1px solid #ced4da;
            border-radius: 30px;
            padding: 5px 14px;
            margin: 0 4px;
            transition: 0.2s;
        }

        .dataTables_wrapper .dt-buttons .btn:hover {
            background: #e2e6ea;
            transform: scale(1.02);
        }

        /* mobile responsive */
        @media (max-width: 768px) {
            body {
                padding: 10px;
            }

            .main-card {
                padding: 20px;
                border-radius: 20px;
            }

            .title {
                font-size: 26px;
                line-height: 40px;
            }

            .info-row .info-label {
                min-width: 120px;
            }

            .table {
                min-width: 700px;
            }

            .stat-card .stat-value {
                font-size: 20px;
            }
        }

        @media (max-width: 576px) {
            .stat-card {
                padding: 12px 16px;
            }
            .stat-card .stat-value {
                font-size: 18px;
            }
        }

        /* animation */
        @keyframes fadeIn {
            from {
                opacity: 0;
                transform: translateY(20px);
            }
            to {
                opacity: 1;
                transform: translateY(0px);
            }
        }

        /* utility */
        .mt-2 {
            margin-top: 8px;
        }
        .mb-2 {
            margin-bottom: 8px;
        }
        .gap-3 {
            gap: 16px;
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

                    <!-- MAIN CARD (same style as registration) -->
                    <div class="main-card">

                        <div class="title">
                            <i class="fa-solid fa-user-graduate"></i>
                            Student Flying Hours View
                        </div>

                        <!-- Student Info (row layout like registration) -->
                        <div class="row g-3">

                            <!-- Registration Number -->
                            <div class="col-lg-6 col-md-6 col-12">
                                <div class="info-row">
                                    <span class="info-label"><i class="fa-regular fa-id-card me-2" style="color:#243b55;"></i>Registration Number :</span>
                                    <span class="info-value"><asp:Label ID="lblRegNumber" runat="server" Text="--" /></span>
                                </div>
                            </div>
 

                            <!-- Student Name -->
                            <div class="col-lg-6 col-md-6 col-12">
                                <div class="info-row">
                                    <span class="info-label"><i class="fa-regular fa-user me-2" style="color:#243b55;"></i>Student Name :</span>
                                    <span class="info-value"><asp:Label ID="lblStudentName" runat="server" Text="--" /></span>
                                </div>
                            </div>

                            <!-- Father Name -->
                            <div class="col-lg-6 col-md-6 col-12">
                                <div class="info-row">
                                    <span class="info-label"><i class="fa-regular fa-user me-2" style="color:#243b55;"></i>Father Name :</span>
                                    <span class="info-value"><asp:Label ID="lblFatherName" runat="server" Text="--" /></span>
                                </div>
                            </div>

                            <!-- Course Name -->
                            <div class="col-lg-6 col-md-6 col-12">
                                <div class="info-row">
                                    <span class="info-label"><i class="fa-solid fa-graduation-cap me-2" style="color:#243b55;"></i>Course Name :</span>
                                    <span class="info-value"><asp:Label ID="lblCourseName" runat="server" Text="--" /></span>
                                </div>
                            </div>

                        </div> <!-- /row -->

                        <!-- STAT CARDS (styled like registration badges) -->
                        <div class="row g-3 mt-2">

                            <div class="col-lg-3 col-md-6 col-12">
                                <div class="stat-card stat-alloted">
                                    <div>
                                        <span class="stat-icon"><i class="fa-regular fa-clock" style="color:#0d6efd;"></i></span>
                                        <span class="stat-label">Alloted Hours</span>
                                    </div>
                                    <span class="stat-value"><asp:Label ID="lblAllotedHours" runat="server" Text="0" /></span>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-6 col-12">
                                <div class="stat-card stat-eligible">
                                    <div>
                                        <span class="stat-icon"><i class="fa-regular fa-circle-check" style="color:#198754;"></i></span>
                                        <span class="stat-label">Eligible Hours</span>
                                    </div>
                                    <span class="stat-value"><asp:Label ID="lblEligibleHours" runat="server" Text="0" /></span>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-6 col-12">
                                <div class="stat-card stat-flying" >
                                    <div>
                                        <span class="stat-icon"><i class="fa-solid fa-plane" style="color:#ff6a00;"></i></span>
                                        <span class="stat-label">Flying Hours</span>
                                    </div>
                                    <span class="stat-value"><asp:Label ID="lblFlyingHours" runat="server" Text="0" /></span>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-6 col-12">
                                <div class="stat-card stat-balance">
                                    <div>
                                        <span class="stat-icon"><i class="fa-regular fa-hourglass-half" style="color:#dc3545;"></i></span>
                                        <span class="stat-label">Balance Hours</span>
                                    </div>
                                    <span class="stat-value"><asp:Label ID="lblBalanceHours" runat="server" Text="0" /></span>
                                </div>
                            </div>

                        </div> <!-- /stat cards row -->

                    </div> <!-- /main-card -->

                    <!-- GRIDVIEW (same style as registration grid-box) -->
                    <div class="grid-box">

                        <div class="grid-scroll">
                            <asp:GridView ID="gridview" runat="server"
                                AutoGenerateColumns="false"
                                CssClass="table table-bordered table-hover"
                                >
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr.No">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     
                                    <asp:BoundField HeaderText="Instructor Name" DataField="Name" />
                                    <asp:BoundField HeaderText="Flying Hours" DataField="FlyingHours" />
                                     <asp:BoundField HeaderText="Flying Date" DataField="DailyFlyingHours" />
                                    <asp:BoundField HeaderText="Entry Date" DataField="EntryDate" />
                                    <asp:BoundField HeaderText="EntryBy" DataField="EmployeeName" />
                                </Columns>
                            </asp:GridView>
                        </div>

                    </div> <!-- /grid-box -->

                </div> <!-- /container -->
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- UpdateProgress (same as original) -->
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
            <ProgressTemplate>
                <div class="ProgressMsg" style="position:fixed; top:40%; left:45%; background:white; padding:20px 30px; border-radius:30px; box-shadow:0 10px 30px rgba(0,0,0,0.3); z-index:9999;">
                    <img src="../images/wait.gif" alt="Wait" />
                    <br /><br />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

    </form>

</html>
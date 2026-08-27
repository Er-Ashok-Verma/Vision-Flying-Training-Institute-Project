 <%@ Page Language="C#" AutoEventWireup="true" CodeFile="Unsettled_Registration.aspx.cs" Inherits="Flying_Hour_Unsettled_Registration" %>

<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Unsettled Registration</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link href="../assets/stylesheets/bootstrap/bootstrap.css" rel="stylesheet" />
    <link href="../assets/stylesheets/light-theme.css" rel="stylesheet" />
    <link href="../assets/stylesheets/theme-colors.css" rel="stylesheet" />
    <link href="../assets/stylesheets/demo.css" rel="stylesheet" />
    <link href="../font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/1.13.7/css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/buttons/2.4.2/css/buttons.dataTables.min.css" rel="stylesheet" />

    <script src="../JQuery/MessageBox_Function.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@9"></script>
    <script src="../js/notify.min.js"></script>

    <style type="text/css">
        /* Global Styles */
        body {
            background: #f0f4f9;
            font-family: 'Segoe UI', 'Helvetica Neue', Arial, sans-serif;
        }

        .box {
            background: #ffffff;
            border-radius: 12px;
            box-shadow: 0 5px 25px rgba(0, 0, 0, 0.08);
            margin-bottom: 30px;
            overflow: hidden;
            border: 1px solid #e8edf4;
        }

        .box-header {
            margin-top: 14px;
            padding: 18px 25px;
            background: #333389;
            border-bottom: none;
        }

        .box-header .title h4 {
            margin: 0;
            color: #ffffff;
            font-weight: 500;
            font-size: 20px;
            letter-spacing: 0.5px;
            padding: 12px;
        }

        .box-header .title h4 i {
            margin-right: 12px;
            opacity: 0.9;
            font-size: 20px;
        }

        .box-content {
             
            margin:12px;
        }

        /* GridView Styling */
        .gridview-style {
            width: 100%;
            border-collapse: separate;
            border-spacing: 0;
            border: 1px solid #e2e8f0;
            border-radius: 8px;
            overflow: hidden;
            font-size: 14px;
        }

        .gridview-style th {
            background: #2c4058 !important;
            color: #ffffff !important;
            font-weight: 400;
            padding: 6px ;
            border: none;
            text-align: center;
            font-size: 13px;
            letter-spacing: 0.3px;
            text-transform: uppercase;

        }

        .gridview-style td {
            padding: 6px;
            border-bottom: 1px solid #edf2f7;
            vertical-align: middle;
            background: #ffffff;
            color: #2d3748;
        }

        .gridview-style tr:last-child td {
            border-bottom: none;
        }

        .gridview-style tr:hover td {
            background: #f7fafc;
        }

        .gridview-style tr:nth-child(even) td {
            background: #fafbfc;
        }

        .gridview-style tr:nth-child(even):hover td {
            background: #f7fafc;
        }

        /* Button Styles */
        .btn-settle {
            background: #2ed0ba;
            color: white !important;
            border: none;
            border-radius: 4px;
            padding: 6px 8px;
            font-size: 12px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            box-shadow: 0 2px 8px rgba(56, 161, 105, 0.3);
            text-decoration: none;
            display: inline-block;
        }

        .btn-settle:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(56, 161, 105, 0.4);
            background: linear-gradient(135deg, #38a169 0%, #2f855a 100%);
            color: white !important;
            text-decoration: none;
        }

        .btn-settle:active {
            transform: translateY(0px);
        }

        .btn-settle i {
            margin-right: 2px;
        }

        /* DataTables Overrides */
        .dataTables_wrapper .dt-button {
            background: #ffffff !important;
            border: 1px solid #e2e8f0 !important;
            border-radius: 8px !important;
            padding: 7px 12px !important;
            margin-right: 6px;
            font-size: 13px;
            color: #2d3748 !important;
            transition: all 0.2s ease;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
        }

        .dataTables_wrapper .dt-button:hover {
            background: #f7fafc !important;
            border-color: #cbd5e0 !important;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
            transform: translateY(-1px);
        }

        .dataTables_wrapper .dt-button i {
            margin-right: 10px;
        }

        .dataTables_wrapper .dataTables_filter input {
            border: 2px solid #e2e8f0;
            border-radius: 25px;
            padding: 8px 20px;
            outline: none;
            background: #ffffff;
            transition: all 0.3s ease;
            font-size: 14px;
            width: 250px;
        }

        .dataTables_wrapper .dataTables_filter input:focus {
            border-color: #2c4058;
            box-shadow: 0 0 0 3px rgba(44, 64, 88, 0.1);
        }

        .dataTables_wrapper .dataTables_info {
            font-size: 13px;
            color: #4a5568;
            padding-top: 10px;
        }

        .dataTables_wrapper .dataTables_paginate .paginate_button {
            border-radius: 25px !important;
            background: #ffffff;
            border: 1px solid #e2e8f0 !important;
            color: #2d3748 !important;
            margin: 0 3px;
            padding: 6px 14px !important;
            transition: all 0.2s ease;
        }

        .dataTables_wrapper .dataTables_paginate .paginate_button:hover {
            background: #edf2f7 !important;
            border-color: #cbd5e0 !important;
        }

        .dataTables_wrapper .dataTables_paginate .paginate_button.current {
            background: #2c4058 !important;
            border-color: #2c4058 !important;
            color: white !important;
        }

        .dataTables_wrapper .dataTables_paginate .paginate_button.disabled {
            opacity: 0.5;
        }

        /* Scroll Container */
        .grid-scroll {
            width: 100%;
            overflow-x: auto;
            border-radius: 8px;
            background: #ffffff;
        }

        /* ========== MODAL DESIGN ========== */
        .modal-content {
            border-radius: 20px;
            border: none;
            box-shadow: 0 25px 60px rgba(0, 0, 0, 0.25);
            overflow: hidden;
        }

        .modal-header {
            background: linear-gradient(135deg, #1a2a3f 0%, #2c4058 100%);
            padding: 20px 28px;
            border-bottom: none;
        }

        .modal-header .modal-title {
            color: #ffffff;
            font-weight: 600;
            font-size: 20px;
            letter-spacing: 0.3px;
        }

        .modal-header .modal-title i {
            margin-right: 12px;
            color: #68d391;
        }

        .modal-header .close {
            color: #ffffff;
            opacity: 0.8;
            text-shadow: none;
            font-size: 28px;
            font-weight: 300;
            transition: all 0.2s ease;
        }

        .modal-header .close:hover {
            opacity: 1;
            transform: rotate(90deg);
        }

        .modal-body {
            padding: 30px 28px 20px 28px;
            background: #f8fafc;
        }

        .modal-footer {
            background: #ffffff;
            border-top: 1px solid #edf2f7;
            padding: 18px 28px 25px 28px;
            display: flex;
            justify-content: center;
            gap: 15px;
        }

        /* ===== NEW: Two Column Layout for Transaction & Amount ===== */
        .info-row {
            display: flex;
            gap: 20px;
            margin-bottom: 20px;
        }

        .info-col {
            flex: 1;
            background: #ffffff;
            border-radius: 10px;
            padding: 15px 18px;
            box-shadow: 0 1px 4px rgba(0, 0, 0, 0.05);
            border-left: 4px solid #2c4058;
        }

        .info-col.amount-col {
            border-left-color: #38a169;
        }

        .info-col .label-text {
            font-size: 11px;
            text-transform: uppercase;
            color: #718096;
            font-weight: 600;
            letter-spacing: 0.5px;
        }

        .info-col .value-text {
            font-size: 14px;
            font-weight: 600;
            color: #1a202c;
            margin-top: 4px;
        }

        .info-col.amount-col .value-text {
            color: #38a169;
        }

        /* Registration No - Full Width */
        .reg-row {
            margin-bottom: 20px;
        }

        .reg-row .form-group {
            margin-bottom: 0;
        }

        .reg-row label {
            font-weight: 600;
            color: #2d3748;
            margin-bottom: 6px;
            font-size: 13px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        .reg-row .form-control {
            border-radius: 10px;
            border: 2px solid #e2e8f0;
            padding: 12px 18px;
            background: #ffffff;
            transition: all 0.3s ease;
            font-size: 16px;
            color: #2d3748;
        }

        .reg-row .form-control:focus {
            border-color: #2c4058;
            box-shadow: 0 0 0 3px rgba(44, 64, 88, 0.12);
        }

        /* Student Details - 3 Column Grid */
        .student-details-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 15px;
            margin-top: 5px;
        }

        .student-detail-item {
            background: #ffffff;
            border-radius: 10px;
            padding: 12px 15px;
            box-shadow: 0 1px 4px rgba(0, 0, 0, 0.05);
            border-left: 4px solid #4299e1;
        }

        .student-detail-item:nth-child(2) {
            border-left-color: #ed8936;
        }

        .student-detail-item:nth-child(3) {
            border-left-color: #9f7aea;
        }

        .student-detail-item .label-text {
            font-size: 10px;
            text-transform: uppercase;
            color: #718096;
            font-weight: 600;
            letter-spacing: 0.5px;
        }

        .student-detail-item .value-text {
            font-size: 14px;
            font-weight: 600;
            color: #1a202c;
            margin-top: 3px;
        }

        /* Buttons */
        .btn-cancel-modal {
            background: #edf2f7 !important;
            border: 2px solid #e2e8f0 !important;
            border-radius: 25px !important;
            padding: 8px 30px !important;
            font-weight: 600;
            color: #4a5568 !important;
            transition: all 0.2s ease;
            
        }

        .btn-cancel-modal:hover {
            background: #e2e8f0 !important;
            border-color: #cbd5e0 !important;
            transform: translateY(-2px);
        }

        .btn-settle-modal {
            background: linear-gradient(135deg, #48bb78 0%, #38a169 100%) !important;
            border: none !important;
            border-radius: 25px !important;
            padding: 8px 35px !important;
            font-weight: 600;
            color: white !important;
            box-shadow: 0 3px 12px rgba(56, 161, 105, 0.35);
            transition: all 0.3s ease;
        }

        .btn-settle-modal:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 20px rgba(56, 161, 105, 0.45);
            background: linear-gradient(135deg, #38a169 0%, #2f855a 100%) !important;
        }

        .btn-settle-modal:active {
            transform: translateY(0px);
        }

        /* Progress */
        .ProgressMsg {
            text-align: center;
            padding: 40px 20px;
            background: rgba(255, 255, 255, 0.95);
            border-radius: 12px;
        }

        .ProgressMsg img {
            display: block;
            margin: 0 auto 16px auto;
            width: 50px;
        }

        .ProgressMsg span {
            font-size: 16px;
            font-weight: 500;
            color: #2c4058;
        }

        /* Responsive */
        @media (max-width: 768px) {
            .box-content {
                padding: 10px 10px;
            }
            .modal-body {
                padding: 20px 15px;
            }
            .modal-dialog {
                margin: 10px;
            }
            .modal-content {
                width: 100% !important;
            }
            .dataTables_wrapper .dataTables_filter input {
                width: 180px;
            }
            .info-row {
                flex-direction: column;
                gap: 10px;
            }
            .student-details-grid {
                grid-template-columns: 1fr;
            }
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

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <section>
                    <div class="col-sm-12 col-lg-12">
                        <div class="box">

                            <div class="box-header">
                                <div class="title">
                                    <h4><i class="fa fa-credit-card"></i> Unsettled Payment</h4>
                                </div>
                            </div>

                            <div class="box-content">
                                <div class="row" style="padding-left: 6px; padding-right: 6px;">

                                    <div class="grid-scroll">
                                        <asp:GridView ID="gridview" runat="server"
                                            AutoGenerateColumns="false"
                                            CssClass="gridview-style"
                                            Style="width: 100%;"
                                            UseAccessibleHeader="true">
                                            <Columns>

                                                <asp:TemplateField HeaderText="Sr.No" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px">
                                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField HeaderText="Student Name" DataField="StudentName" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Father Name" DataField="FatherName" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Mobile No" DataField="MobileNo" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField HeaderText="Email" DataField="Email_ID" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Course Name" DataField="CourseName" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Amount" DataField="Amount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                                <asp:BoundField HeaderText="Entry Date" DataField="EntryDate" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd-MMM-yyyy}" />

                                                <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkFillData" runat="server" 
                                                            Text="Settle Payment" 
                                                            CssClass="btn-settle"
                                                            CommandArgument='<%# Eval("ID") %>' 
                                                            OnClick="lnkFillData_Click">
                                                            Settle
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                </div>

                                <!-- ========== BEAUTIFUL MODAL WITH NEW LAYOUT ========== -->
                                <div class="modal fade" id="verifyModal" tabindex="-1" role="dialog" aria-labelledby="verifyModalLabel" aria-hidden="true">
                                    <div class="modal-dialog modal-dialog-centered" role="document">
                                        <div class="modal-content" style="width: 750px; max-width: 95%;">

                                            <div class="modal-header">
                                                <h5 class="modal-title" id="verifyModalLabel">
                                                    <i class="fa fa-check-circle"></i> Settle Payment
                                                </h5>
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                    <span aria-hidden="true">&times;</span>
                                                </button>
                                            </div>

                                            <div class="modal-body">

                                                <asp:HiddenField ID="hdnUnsettledID" runat="server" />

                                                <!-- ===== ROW 1: Transaction ID (Left) | Amount (Right) ===== -->
                                                <div class="info-row">
                                                    <div class="info-col">
                                                        <div class="label-text"><i class="fa fa-hashtag"></i> Transaction ID</div>
                                                        <div class="value-text">
                                                            <asp:Label ID="lblTransID" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="info-col amount-col">
                                                        <div class="label-text"><i class="fa fa-money"></i> Amount</div>
                                                        <div class="value-text">
                                                            <asp:Label ID="lblAmount" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <!-- ===== ROW 2: Registration No (Full Width) ===== -->
                                                <div class="reg-row">
                                                    <div class="form-group">
                                                        <label><i class="fa fa-id-card"></i> Enter Registration Number</label>
                                                        <asp:TextBox ID="txtRegNo"
                                                            runat="server"
                                                            CssClass="form-control"
                                                            placeholder="Type registration number and press Enter"
                                                            AutoPostBack="true"
                                                            OnTextChanged="txtRegNo_TextChanged">
                                                        </asp:TextBox>
                                                    </div>
                                                </div>

                                                <!-- ===== ROW 3: Student Details (3 Columns) ===== -->
                                                <div id="div1" runat="server" style="display:none;">
                                                    <div class="student-details-grid">
                                                        <div class="student-detail-item">
                                                            <div class="label-text"><i class="fa fa-user"></i> Student Name</div>
                                                            <div class="value-text">
                                                                <asp:Label ID="lblStudentName" runat="server"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="student-detail-item">
                                                            <div class="label-text"><i class="fa fa-user"></i> Father Name</div>
                                                            <div class="value-text">
                                                                <asp:Label ID="lblFatherName" runat="server"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="student-detail-item">
                                                            <div class="label-text"><i class="fa fa-graduation-cap"></i> Course Name</div>
                                                            <div class="value-text">
                                                                <asp:Label ID="lblCourseName" runat="server"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div> <!-- modal-body -->

                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-cancel-modal" data-dismiss="modal">
                                                    <i class="fa fa-times"></i> Cancel
                                                </button>
                                                <asp:Button ID="btnVerifyPayment" runat="server" 
                                                    Text="Confirm Settlement" 
                                                    CssClass="btn-settle-modal"
                                                    OnClick="btnVerifyPayment_Click" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <!-- ========== END MODAL ========== -->

                            </div> <!-- box-content -->
                        </div> <!-- box -->
                    </div> <!-- col -->
                </section>

            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
            <ProgressTemplate>
                <div class="ProgressMsg">
                    <img src="../images/wait.gif" alt="Loading" />
                    <span>Processing your request...</span>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <cc2:UpdateProgressOverlayExtender ID="UpdateProgressOverlayExtender3" runat="server" 
            TargetControlID="UpdateProgress3" CssClass="updateProgress" OverlayType="Browser" />

    </form>

    <!-- ========== SCRIPTS ========== -->
    <script src="../js_admin/jquery/jquery-2.0.3.min.js"></script>
    <script src="../js_admin/jquery-ui-1.10.3.custom/js/jquery-ui-1.10.3.custom.min.js"></script>
    <script src="../bootstrap-dist/js/bootstrap.min.js"></script>
    <script src="../js_admin/bootstrap-daterangepicker/moment.min.js"></script>
    <script src="../js_admin/bootstrap-daterangepicker/daterangepicker.min.js"></script>
    <script src="../js_admin/jQuery-slimScroll-1.3.0/jquery.slimscroll.min.js"></script>
    <script src="../js_admin/jQuery-slimScroll-1.3.0/slimScrollHorizontal.min.js"></script>
    <script src="../js_admin/jQuery-BlockUI/jquery.blockUI.min.js"></script>
    <script src="../js_admin/jQuery-Cookie/jquery.cookie.min.js"></script>
    <script src="../js_admin/script.js"></script>

    <script src="https://cdn.datatables.net/1.13.7/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.html5.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.print.min.js"></script>

    <script type="text/javascript">
        var unsettledDataTable = null;

        function initializeUnsettledDataTable() {
            var table = $('#<%= gridview.ClientID %>');

            if (table.length === 0) return;

            if ($.fn.DataTable.isDataTable(table)) {
                table.DataTable().destroy();
            }

            unsettledDataTable = table.DataTable({
                destroy: true,
                searching: true,
                ordering: false,
                paging: true,
                info: true,
                autoWidth: false,
                scrollX: true,
                scrollY: '450px',
                scrollCollapse: true,
                pageLength: 10,
                lengthMenu: [[10, 25, 50, -1], ['10 rows', '25 rows', '50 rows', 'Show all']],
                dom: 'Bfrtip',
                columnDefs: [
                    { targets: 0, orderable: false, searchable: false },
                    { targets: 8, orderable: false, searchable: false }
                ],
                buttons: [
                    {
                        extend: 'pageLength',
                        text: '<i class="fa fa-list"></i> Show'
                    },
                    {
                        extend: 'excelHtml5',
                        text: '<i class="fa fa-file-excel-o"></i>',
                        titleAttr: 'Excel',
                        title: 'Unsettled Registration Report'
                    },
                    {
                        extend: 'pdfHtml5',
                        text: '<i class="fa fa-file-pdf-o"></i>',
                        titleAttr: 'PDF',
                        title: 'Unsettled Registration Report',
                        orientation: 'landscape',
                        pageSize: 'A4'
                    },
                    {
                        extend: 'print',
                        text: '<i class="fa fa-print"></i>',
                        titleAttr: 'Print',
                        title: 'Unsettled Registration Report'
                    }
                ],
                language: {
                    search: "Search:",
                    lengthMenu: "Show _MENU_ entries",
                    info: "Showing _START_ to _END_ of _TOTAL_ entries",
                    infoEmpty: "No entries available",
                    infoFiltered: "(filtered from _MAX_ total entries)",
                    zeroRecords: "No matching records found"
                }
            });
        }

        function pageLoad(sender, args) {
            initializeUnsettledDataTable();
        }

        $(document).ready(function () {
            initializeUnsettledDataTable();
        });
    </script>

</body>
</html>
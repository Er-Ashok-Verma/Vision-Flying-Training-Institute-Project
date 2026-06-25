<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Student_DailyFlyingHourse.aspx.cs" Inherits="Academic_Student_DailyFlyingHourse" %>

<%@ Register Assembly="CommonClassLibrary" Namespace="CommonClassLibrary" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Namespace="MSS" TagPrefix="Custom" %>

<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server"></script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Fee Deposition</title>
    <%--<link href="../assets/stylesheets/light-theme.css" media="all" rel="stylesheet" type="text/css" />--%>

      <%--  DATATABLE CSS START--%>
    <link href="https://cdn.datatables.net/1.13.7/css/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
    <link href="https://cdn.datatables.net/buttons/2.4.2/css/buttons.dataTables.min.css" rel="stylesheet" type="text/css" />
    <link href="https://cdn.datatables.net/fixedheader/3.4.0/css/fixedHeader.dataTables.min.css" rel="stylesheet" type="text/css" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />

    <link href="../assets/stylesheets/light-theme.css" rel="stylesheet" />

    <link href="../assets/stylesheets/bootstrap/bootstrap.css" media="all" rel="stylesheet" type="text/css" />

    <link type="text/css" rel="Stylesheet" href="../Account.css" />
    <link href="../PagingGridView.css" rel="stylesheet" type="text/css" />
    <%--   <link rel="stylesheet" type="text/css" href="../css_Admin/cloud-admin.css" />--%>
    <link rel="stylesheet" type="text/css" href="../css_Admin/themes/default.css" />
    <link href="../font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="../css/custom.css" rel="stylesheet" type="text/css" />
    <!-- DATE RANGE PICKER -->
    <link rel="stylesheet" type="text/css" href="../js_admin/bootstrap-daterangepicker/daterangepicker-bs3.css" />
    <!-- UNIFORM -->
    <link rel="stylesheet" type="text/css" href="../js_admin/uniform/css/uniform.default.min.css" />
    <!-- ANIMATE -->
    <link rel="stylesheet" type="text/css" href="../css_Admin/animatecss/animate.min.css" />
    <script src="../js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.core.js" type="text/javascript"></script>
    <link href="../js/jquery.ui.theme.css" rel="stylesheet" type="text/css" />
    <link href="../js/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-ui-1.8.6.custom.js" type="text/javascript"></script>
    <script src="../js/JScript.js" type="text/javascript"></script>
    <script language="javascript" src="../datetimepicker.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function disableBtn(btnID, newText) {
            var btn = document.getElementById(btnID);

            btn.disabled = true;
            btn.value = newText;
        }

        function FinishConfirmation() {
            var answer = confirm("Are you sure?")
            if (answer) {
                document.getElementById("btnSave").click();
                //evt.preventDefault();
            }
            else {
                return false;
            }
        }

        function ClearSearch() {
            var txt_ID = document.getElementById("txtSearchName");
            txt_ID.value = "";
        }

        function onASMSelectedMenuItem() {//debugger

            $get('btnSearch').click();
        }

        function GetSuggestions(keyword, usePaging, pageIndex, pageSize, callbackMethod) {

            TRACE("GetSuggestions");
            var Condition = '';
            var SearchOption = 1;
            var tab_name = '';
            var txtField = '';
            var valField = '';
            var Insti_id = 115;



            var SearchOption_ID = document.getElementById("rdbSearchOpt");
            var inputs2 = SearchOption_ID.getElementsByTagName("input");
            var SearchOptionSelectedValue;
            for (var i = 0; i < inputs2.length; i++) {
                if (inputs2[i].checked) {
                    SearchOptionSelectedValue = inputs2[i];
                    break;
                }
            }

            Condition = ' InstituteID = ' + Insti_id;
            SearchOption = SearchOptionSelectedValue.value;


            if (SearchOption == 3) {
                tab_name = "(SELECT StudentReg.StudentID, StudentReg.StudentName +' ('+StudentReg.RegNo+') -'+ StudentReg.FatherName AS StudentName, StudentReg.InstituteID FROM StudentReg INNER JOIN StudentStatus ON StudentReg.StudentID = StudentStatus.StudentID where  StudentStatus.Status != 'ENQ' and  StudentStatus.InstituteID = '115') as StudentSearch";
                //tab_name = "(SELECT StudentID, StudentName + ' (' + RegNo + ') -' + FatherName AS StudentName, InstituteID FROM  StudentReg where InstituteID = '" + rblSelectedValue.value + "') as StudentSearch";

                txtField = 'StudentName';
                txtField = txtField.trim();
                valField = 'StudentID';
            }
            else if (SearchOption == 2) {
                tab_name = "StudentReg";
                txtField = 'RegNo';
                valField = 'StudentID';
            }
            else if (SearchOption == 4) {
                tab_name = "(SELECT StudentReg.StudentID, StudentReg.ACTN_No, StudentReg.InstituteID FROM StudentReg INNER JOIN StudentStatus ON StudentReg.StudentID = StudentStatus.StudentID where  StudentStatus.Status = 'ENQ' and  StudentStatus.InstituteID = '115') as StudentSearch";
                // tab_name = "StudentReg";
                txtField = 'ACTN_No';
                valField = 'StudentID';

            }

            else {
                tab_name = "StudentRollNo";
                txtField = 'UniversityRollNo';
                valField = 'StudentID';
            }

            RCSService.GetSuggestion_StudentSearch(keyword, usePaging, pageIndex, pageSize, true, tab_name, txtField, valField, Condition, callbackMethod);

        }


        function OnupdateTextBoxValue(evt) {//debugger;
            var textBoxID = evt.source.textBoxID;
            $get(textBoxID).value = evt.selMenuItem.label;
            evt.preventDefault();
        }

        function IntegerNumber() {

            if ((window.event.keyCode < 48) || (window.event.keyCode > 57)) {
                window.event.keyCode = 0;
            }
        }

        function comboValidation(sender, args) {
            if ((args.Value == "---Select---") || (args.Value == "0")) {
                args.IsValid = false;
                return;
            }
            else {
                args.IsValid = true;
            }
        }


    </script>
    <style type="text/css">

       
    #gridview {
        width: 100% !important;
    }

    #gridview th {
        width: auto;
        text-align: center;
        white-space: nowrap;
    }

    #gridview thead,
    #gridview tbody {
        width: 100%;
    }
 


        .DivVerticalScroll {
            overflow-y: auto;
            max-height: 300px;
        }


        .DivVerticalScroll_g {
            overflow-y: auto;
            max-height: 150px;
        }

        .modalPopup {
            position: fixed;
            z-index: 10000;
            background: white;
            border: 2px solid #4361ee;
            border-radius: 8px;
            box-shadow: 0 8px 20px rgba(0,0,0,0.2);
            top: 5%;
            left: 50%;
            transform: translate(-50%, -50%);
        }

        .modalMask {
            position: fixed;
            z-index: 9999;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
        }

        .aqua-row {
            background-color: Aqua !important;
        }

        .normal-row {
            /* your normal row styles */
        }
    </style>

</head>
<body>

    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">

            <Services>
                <asp:ServiceReference Path="~/RCSService.asmx" />
            </Services>
        </asp:ToolkitScriptManager>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div class="box">
                    <div class="box-header blue-background">
                        <div class="box-title">
                            <h4 style="color: #fff"><i class="fa fa-bars"></i>
                                <div id="lbltitle" runat="server" style="margin-left: 22px; margin-top: -18px;"></div>
                                <h4></h4>
                            </h4>
                        </div>
                    </div>




                    <div class="box-content" style="margin: 5px">
                        <div class="row">
                            <div class="col-md-3" style="text-align: center;">
                                <div style="height: 300px;; background: linear-gradient(135deg, #ffffff 0%, #f8f9fa 100%); border-radius: 15px; box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08); border: 1px solid rgba(67, 97, 238, 0.1); padding: 20px; display: flex; flex-direction: column; align-items: center; justify-content: space-between;">
                                    <div style="margin-bottom: 15px;">
                                        <div style="width: 150px; height: 150px; margin: 0 auto; position: relative;">
                                            <asp:Image ID="imgPhoto" runat="server"
                                                Style="width: 100%; height: 100%; object-fit: cover; border-radius: 50%; border: 4px solid #4361ee; box-shadow: 0 4px 8px rgba(67, 97, 238, 0.2); cursor: pointer; transition: all 0.3s ease;"
                                                onmouseover="this.style.transform='scale(1.05)'; this.style.boxShadow='0 6px 12px rgba(67, 97, 238, 0.3)';"
                                                onmouseout="this.style.transform='scale(1)'; this.style.boxShadow='0 4px 8px rgba(67, 97, 238, 0.2)';"
                                                ImageUrl="~/assets/images/User.jpg" />

                                            <!-- Photo Status Indicator -->
                                            <div style="position: absolute; bottom: 5px; right: 5px; width: 20px; height: 20px; background: #28a745; border-radius: 50%; border: 2px solid white;">
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Student Name -->
                                    <div style="margin-bottom: 15px; text-align: center; width: 100%;">
                                        <div style="display: flex; justify-content: center;">
                                            <asp:Label ID="lblStuName" runat="server"
                                                Style="font-weight: 700; font-size: 1.2rem; color: #4361ee; padding: 8px 20px; border-radius: 10px; display: inline-block; max-width: 260px; background: rgba(67, 97, 238, 0.08); border: 1px solid rgba(67, 97, 238, 0.2); text-align: center; line-height: 1.3; height: auto; overflow: visible; text-overflow: clip; white-space: normal; word-wrap: break-word; word-break: break-word; display: -webkit-box; -webkit-line-clamp: 2; -webkit-box-orient: vertical; overflow: hidden;"></asp:Label>


                                        </div>
                                    </div>






                                </div>
                            </div>
                            <div class="col-md-9">
                                <div style="height: 300px; background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%); border-radius: 15px; box-shadow: 0 6px 18px rgba(0, 0, 0, 0.06); border: 1px solid rgba(67, 97, 238, 0.12); padding: 22px; transition: all 0.3s ease;" class="box-content">
                                    <div class="row" style="margin-bottom: 10px; padding-bottom: 5px; border-bottom: 1px dashed #e0e7ff;">
                                        <div class="col-md-4">
                                            <div class="form-group" style="margin-bottom: 20px; height:100px; background: linear-gradient(145deg, #ffffff, #f8faff); border-radius: 16px; box-shadow: 0 8px 20px rgba(67, 97, 238, 0.08); border: 1px solid rgba(67, 97, 238, 0.15); transition: all 0.3s ease;"
                                                onmouseover="this.style.boxShadow='0 12px 28px rgba(67, 97, 238, 0.12)'; this.style.borderColor='rgba(67, 97, 238, 0.3)';"
                                                onmouseout="this.style.boxShadow='0 8px 20px rgba(67, 97, 238, 0.08)'; this.style.borderColor='rgba(67, 97, 238, 0.15)';">
                                                <br /> <label for='inputText' style="display: block; font-weight: 700; color: #1e3a5f; font-size: 0.95rem; text-transform: uppercase; letter-spacing: 1px; margin-bottom: 15px; position: relative; padding-left: 12px; border-left: 4px solid #4361ee;">
                                                    <span style="background: white; padding-right: 10px;">🔍 Search Option</span>
                                                </label>
                                                <asp:RadioButtonList ID="rdbSearchOpt" runat="server"
                                                    RepeatDirection="Horizontal" onchange="ClearSearch();" Width="100%"
                                                    Style="display: flex; gap: 25px; flex-wrap: wrap; background: white; padding: 12px 16px; border-radius: 12px; margin: 0;">
                                                    <%--<asp:ListItem Value="1" style="color: #2d3e50; font-size: 0.95rem; padding: 8px 16px; background: #f0f4ff; border-radius: 30px; transition: all 0.2s ease; font-weight: 500; border: 1px solid #e0e7ff; display: inline-block; box-shadow: 0 2px 6px rgba(0,0,0,0.02);">&nbsp;&nbsp;Roll No.</asp:ListItem>--%>
                                                    <asp:ListItem Selected="True" Value="2" style="color: #2d3e50; font-size: 1.25rem; padding: 8px 16px; background: #f0f4ff; border-radius: 30px; transition: all 0.2s ease; font-weight: 500; border: 1px solid #e0e7ff; display: inline-block; box-shadow: 0 2px 6px rgba(0,0,0,0.02);">&nbsp;&nbsp;Reg No.</asp:ListItem>
                                                    <asp:ListItem Value="3" style="color: #2d3e50; font-size: 1.25rem; padding: 8px 16px; background: #f0f4ff; border-radius: 30px; transition: all 0.2s ease; font-weight: 500; border: 1px solid #e0e7ff; display: inline-block; box-shadow: 0 2px 6px rgba(0,0,0,0.02);">&nbsp;&nbsp;Name</asp:ListItem>
                                                    <%--<asp:ListItem Value="4" style="color: #2d3e50; font-size: 0.95rem; padding: 8px 16px; background: #f0f4ff; border-radius: 30px; transition: all 0.2s ease; font-weight: 500; border: 1px solid #e0e7ff; display: inline-block; box-shadow: 0 2px 6px rgba(0,0,0,0.02);">&nbsp;&nbsp;Enquiry No.</asp:ListItem>--%>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="form-group" style="margin-bottom: 12px;">
                                                <asp:Label ID="Label77" runat="server" Text="Search Here" Style="display: block; font-weight: 600; color: #2c3e50; font-size: 0.9rem; text-transform: uppercase; letter-spacing: 0.3px; margin-bottom: 8px;"></asp:Label>
                                                <span class="mandatoryfields" style="color: #dc3545; font-weight: 700; margin-left: 5px;">&nbsp;*</span>
                                                <Custom:AutoSuggestMenu ID="txtNameStudent" runat="server" IsFlexibleQuery="false"
                                                    KeyPressDelay="10" OnClientTextBoxUpdate="OnupdateTextBoxValue" OnGetSuggestions="GetSuggestions"
                                                    OnSelectedValueEvent="onASMSelectedMenuItem" TargetControlID="txtSearchName"
                                                    UseIFrame="false" UsePageMethods="false" />
                                                <asp:Button ID="btnSearch" runat="server" CausesValidation="False" OnClick="btnSearchStu_Click"
                                                    Style="display: none;" Text="Button" />
                                                <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control" Width="100%"
                                                    Style="border-radius: 8px; border: 1.5px solid #e0e7ff; padding: 10px 14px; font-size: 1.25rem; transition: all 0.2s ease; box-shadow: 0 2px 4px rgba(0,0,0,0.02);"
                                                    onfocus="this.style.borderColor='#4361ee'; this.style.boxShadow='0 0 0 3px rgba(67, 97, 238, 0.1)';"
                                                    onblur="this.style.borderColor='#e0e7ff'; this.style.boxShadow='0 2px 4px rgba(0,0,0,0.02)';"></asp:TextBox>
                                            </div>
                                           
                                        </div>
                                    </div>
                                     <div class="col-md-12" style="padding-left:12px; padding-right:12px">
                                                <div class='form-group-2' style="margin-bottom: 4px;">
                                                    <asp:Label ID="Label16" runat="server" Text="Course Name" Font-Bold="true"
                                                        Style="display: block; font-weight: 700; color: #1a2d5e; font-size: 0.75rem; text-transform: uppercase; letter-spacing: 0.5px; margin-bottom: 6px; font-family: 'Inter', -apple-system, sans-serif;">
                                                    </asp:Label>
                                                </div>
                                                <div class='form-group-2' style="margin-bottom: 8px;">
                                                    <asp:Label ID="txt_course_name" runat="server" ReadOnly="true" CssClass="form-control"
                                                        Style="width: 100%; border-radius: 12px; border: 2px solid #e2e8f0; padding: 12px 18px; background: linear-gradient(135deg, #f8faff 0%, #eef4ff 100%); color: #0f1a3c; font-weight: 800; font-size: 1.25rem; font-family: 'Inter', -apple-system, sans-serif; box-shadow: 0 2px 8px rgba(26, 45, 94, 0.06); transition: all 0.2s ease; display: flex; align-items: center; gap: 10px; min-height: 35px; letter-spacing: 0.2px; border-left: 4px solid #1a2d5e;">
                                                        <i class="fas fa-graduation-cap" style="color: #1a2d5e; font-size: 16px; margin-right: 8px;"></i>
                                                        <asp:Label ID="lblCourseValue" runat="server"
                                                            Style="background: transparent; border: none; padding: 0; font-weight: 600; flex: 1;"></asp:Label>
                                                        <span style="background: #1a2d5e; color: white; padding: 2px 12px; border-radius: 40px; font-size: 0.65rem; font-weight: 700; letter-spacing: 0.5px; text-transform: uppercase;">Active</span>
                                                    </asp:Label>
                                                </div>
                                            </div>

                                    <div class="row" style="margin-bottom: 5px; display: flex; flex-wrap: wrap; gap: 10px; padding-left:18px">

                                        <div style="width: 23%; border: 1px solid #ccc; padding: 10px; border-radius: 5px; background: #f8f9fa;">
                                            <b>Alloted Hours</b><br />
                                            <asp:Label ID="lblAllotedHours" runat="server" Text="0"></asp:Label>
                                        </div>

                                        <div style="width: 23%; border: 1px solid #ccc; padding: 10px; border-radius: 5px; background: #f8f9fa;">
                                            <b>Eligible Hours</b><br />
                                            <asp:Label ID="lblEligibleHours" runat="server" Text="0"></asp:Label>
                                        </div>

                                        <div style="width: 23%; border: 1px solid #ccc; padding: 10px; border-radius: 5px; background: #f8f9fa;">
                                            <b>Flying Hours</b><br />
                                            <asp:Label ID="lblFlyingHours" runat="server" Text="0"></asp:Label>
                                        </div>

                                        <div style="width: 23%; border: 1px solid #ccc; padding: 10px; border-radius: 5px; background: #f8f9fa;">
                                            <b>Balance Hours</b><br />
                                            <asp:Label ID="lblBalanceHours" runat="server" Text="0"></asp:Label>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="divSubmit" runat="server" visible="false" class="box-content" style="margin: 5px; margin-top: 15px;">

                        <div class="row">

                            <div class="col-md-3">
                                <label>Instructor <span style="color: red">*</span></label>
                                <asp:DropDownList ID="ddlInstructor" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>

                            <div class="col-md-3">
                                <label>Flying Date <span style="color: red">*</span></label>
                                <asp:TextBox ID="txtFlyingDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>

                            <div class="col-md-3">
                                <label>Flying Hours <span style="color: red">*</span></label>
                                <asp:TextBox ID="txtFlyingHours" runat="server" CssClass="form-control"  placeholder="HH:mm" MaxLength="5" OnTextChanged="txtFlyingHours_TextChanged" ></asp:TextBox>
                            </div>

                            <div class="col-md-3">
                                <label>Remark</label>
                                <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                        </div>

                        <br />
                        <br />

                        <center>
                            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click"
                                Text="Submit" CssClass="btn btn-primary" />
                        </center>

                    </div>


                    <div class="box-content" style="margin: 5px; margin-top: 15px;">
                        <div class="col-md-12">
                            <div>
                                <asp:GridView ID="gridview" runat="server" AutoGenerateColumns="false"
                                    class="scrollable-area table data-table table table-bordered table-striped"
                                    DataKeyNames=" ID">
                                    <Columns>

                                        <asp:BoundField HeaderText="Student Name" DataField="StudentName" />
                                        <asp:BoundField HeaderText="Instructor Name" DataField="Name" />
                                        <asp:BoundField HeaderText="Flying Date" DataField="FlyingDate" />
                                        <asp:BoundField HeaderText="FlyingHours" DataField="FlyingHours" />
                                        <asp:BoundField HeaderText="Remark" DataField="Remark" />
                                        <asp:BoundField HeaderText="EntryBy" DataField="EmployeeName" />
                                        <asp:BoundField HeaderText="EntryDate" DataField="EntryDate" />

                                    </Columns>
                                </asp:GridView>
                            </div>

                        </div>

                    </div>





                </div>

            </ContentTemplate>
            <Triggers>
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
            <ProgressTemplate>
                <div class="ProgressMsg" style="height: auto; position: fixed; z-index: 10000">
                    <img src="../images/wait.gif" alt="Wait" />
                    <span class="rdgrowalt">Please Wait...</span>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <cc1:UpdateProgressOverlayExtender ID="UpdateProgressOverlayExtender3" runat="server"
            CssClass="updateProgress" TargetControlID="UpdateProgress3" OverlayType="Browser" />
    </form>
      <%--  DATATABLE JS START--%>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
<script type="text/javascript" src="https://code.jquery.com/jquery-3.7.0.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.13.7/js/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/fixedheader/3.4.0/js/dataTables.fixedHeader.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/2.4.2/js/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.html5.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.print.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/rowgroup/1.4.1/js/dataTables.rowGroup.min.js"></script>

<script type="text/javascript">
    function pageLoad(sender, args) {
        $(function () {
            $("#gridview").prepend($("<thead></thead>").append($("#gridview").find("tbody tr:first"))).DataTable({
                fixedHeader: true,
                bFilter: true,
                bSort: true,
                bPaginate: true,
                scrollCollapse: true,
                scrollY: true,
                scrollX: true,
                dom: 'Bfrtip',
                lengthMenu: [
                    [10, 25, 50, -1],
                    ['10 rows', '25 rows', '50 rows', 'Show all']
                ],
                buttons: [
                    'pageLength',
                    {
                        extend: 'excelHtml5',
                        text: '<i class="fa fa-file-excel-o" style="color:green"></i>',
                        titleAttr: 'Excel',
                        title: 'Student Daily Flying Hours Report',
                    },
                    {
                        extend: 'pdfHtml5',
                        text: '<i class="fa fa-file-pdf-o" style="color:red"></i>',
                        titleAttr: 'PDF',
                        title: 'Student Daily Flying Hours Report',
                    },
                    {
                        extend: 'print',
                        text: '<i class="fa fa-print" style="color:#007"></i> ',
                        titleAttr: 'Print',
                        title: 'Student Daily Flying Hours Report',
                    },
                ],

            });

        });

    }

</script>

<%--  DATATABLE JS START--%>
</body>
</html>

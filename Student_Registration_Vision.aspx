<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Student_Registration_Vision.aspx.cs" Inherits="Academic_Student_Registration_Vision" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc1" %>
<%@ Register Namespace="MSS" TagPrefix="Custom" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Student Registration</title>
    <link href="../assets/stylesheets/bootstrap/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
    <link href='../assets/images/meta_icons/favicon.ico' rel='shortcut icon' type='image/x-icon' />
    <link href='../assets/images/meta_icons/apple-touch-icon.png' rel='apple-touch-icon-precomposed' />
    <link href='../assets/images/meta_icons/apple-touch-icon-57x57.png' rel='apple-touch-icon-precomposed' sizes='57x57' />
    <link href='../assets/images/meta_icons/apple-touch-icon-72x72.png' rel='apple-touch-icon-precomposed' sizes='72x72' />
    <link href='../assets/images/meta_icons/apple-touch-icon-114x114.png' rel='apple-touch-icon-precomposed' sizes='114x114' />
    <link href='../assets/images/meta_icons/apple-touch-icon-144x144.png' rel='apple-touch-icon-precomposed' sizes='144x144' />
    <link href="../assets/stylesheets/bootstrap/bootstrap.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../assets/stylesheets/light-theme.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../assets/stylesheets/theme-colors.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../assets/stylesheets/demo.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../assets/stylesheets/plugins/select2/select2.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../assets/stylesheets/plugins/bootstrap_colorpicker/bootstrap-colorpicker.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../assets/stylesheets/plugins/bootstrap_daterangepicker/bootstrap-daterangepicker.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../assets/stylesheets/plugins/bootstrap_datetimepicker/bootstrap-datetimepicker.min.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../assets/stylesheets/plugins/bootstrap_switch/bootstrap-switch.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../assets/stylesheets/plugins/common/bootstrap-wysihtml5.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../assets/stylesheets/light-theme.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../assets/stylesheets/theme-colors.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../font-awesome/css/font-awesome.css" rel="stylesheet" />
    <script src="../assets/javascripts/ie/html5shiv.js" type="text/javascript"></script>
    <script src="../assets/javascripts/ie/respond.min.js" type="text/javascript"></script>
    <script src="../js/JScript.js" type="text/javascript"></script>
    <script src="../Scripts/Global.js" type="text/javascript"></script>
    <script language="javascript" src="../datetimepicker.js" type="text/javascript"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/webcamjs/1.0.26/webcam.js"></script>

    <script>

        function validatePAN() {
            var pan = document.getElementById("txtPan").value.trim().toUpperCase();

            var panRegex = /^[A-Z]{5}[0-9]{4}[A-Z]$/;

            if (!panRegex.test(pan)) {
                alert("Please enter a valid PAN Number (e.g. ABCDE1234F)");
                return false;
            }

            return true;
        }
    </script>

    <script type="text/javascript">
        function startCamera() {
            Webcam.set({
                width: 120,
                height: 120,
                image_format: 'jpeg',
                jpeg_quality: 90
            });
            Webcam.attach('#webcam');
            $("#webcam").show();
            $("#<%= startCam.ClientID %>").hide();
            $("#<%= btnCapture.ClientID %>").show();
            $("#<%= defaultimg.ClientID %>").hide();
            $("#<%= defaultimg1.ClientID %>").show();
        }

        function stopCamera() {
            Webcam.reset();
            $("#wizReg_imgCapture").attr("src", "");
            $("#<%= defaultimg.ClientID %>").show();
            $("#<%= defaultimg1.ClientID %>").show();
            $("#webcam").hide();
            $("#<%= startCam.ClientID %>").show();
            $("#<%= btnCapture.ClientID %>").hide();
        }

        function captureImage() {
            Webcam.snap(function (data_uri) {
                $("#<%= defaultimg1.ClientID %>").hide();
                $("#wizReg_imgCapture").attr("src", data_uri);
                $("#<%= hfImageData.ClientID %>").val(data_uri);
            });
        }
    
    
        function validatePAN() {
            var pan = document.getElementById("<%= txtStudentPANNumber.ClientID %>").value.trim().toUpperCase();
        
            var panRegex = /^[A-Z]{5}[0-9]{4}[A-Z]$/;
        
            if (pan != "" && !panRegex.test(pan)) {
                alert("Please enter a valid PAN Number (e.g. ABCDE1234F)");
                document.getElementById("<%= txtStudentPANNumber.ClientID %>").value = "";
            document.getElementById("<%= txtStudentPANNumber.ClientID %>").focus();
            return false;
        }
        
        return true;
    }
    
    function validateFatherPAN() {
        var pan = document.getElementById("<%= FatherPANNumber.ClientID %>").value.trim().toUpperCase();
        
        var panRegex = /^[A-Z]{5}[0-9]{4}[A-Z]$/;
        
        if (pan != "" && !panRegex.test(pan)) {
            alert("Please enter a valid Father PAN Number (e.g. ABCDE1234F)");
            document.getElementById("<%= FatherPANNumber.ClientID %>").value = "";
            document.getElementById("<%= FatherPANNumber.ClientID %>").focus();
            return false;
        }
        
        return true;
    }

        
      
    // Student Aadhaar Validation
    function validateAadhaar() {
        var aadhaar = document.getElementById("<%= txtAadhaar.ClientID %>").value.trim();
        
        var aadhaarRegex = /^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/;
        
        if (aadhaar != "" && !aadhaarRegex.test(aadhaar)) {
            alert("Please enter a valid 12-digit Aadhaar Number (Should not start with 0 or 1)");
            document.getElementById("<%= txtAadhaar.ClientID %>").value = "";
            document.getElementById("<%= txtAadhaar.ClientID %>").focus();
            return false;
        }
        
        return true;
    }
    
    // Father Aadhaar Validation
    function validateFatherAadhaar() {
        var aadhaar = document.getElementById("<%= txtFatherAadharNo.ClientID %>").value.trim();
        
        var aadhaarRegex = /^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/;
        
        if (aadhaar != "" && !aadhaarRegex.test(aadhaar)) {
            alert("Please enter a valid 12-digit Father Aadhaar Number (Should not start with 0 or 1)");
            document.getElementById("<%= txtFatherAadharNo.ClientID %>").value = "";
            document.getElementById("<%= txtFatherAadharNo.ClientID %>").focus();
            return false;
        }
        
        return true;
    }
    
    // Mother Aadhaar Validation
    function validateMotherAadhaar() {
        var aadhaar = document.getElementById("<%= txtMotherAadharNo.ClientID %>").value.trim();
        
        var aadhaarRegex = /^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/;
        
        if (aadhaar != "" && !aadhaarRegex.test(aadhaar)) {
            alert("Please enter a valid 12-digit Mother Aadhaar Number (Should not start with 0 or 1)");
            document.getElementById("<%= txtMotherAadharNo.ClientID %>").value = "";
            document.getElementById("<%= txtMotherAadharNo.ClientID %>").focus();
            return false;
        }
        
        return true;
    }
 
        // Email Validation
        function validateEmail(emailField) {
            var email = emailField.value.trim();
            var emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
        
            if (email != "" && !emailRegex.test(email)) {
                alert("Please enter a valid Email Address (e.g., example@company.com)");
                emailField.value = "";
                emailField.focus();
                return false;
            }
            return true;
        }


    </script>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/css/select2.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.full.min.js"></script>

    <script type="text/javascript">
        function initializeSelect2() {
         
            //$('#<%=wizReg_ddlCourse.ClientID%>').select2();

        }

        $(document).ready(function () {
            initializeSelect2();
        });
    </script>

    <script type="text/javascript" language="javascript">

        function disableBtn(btnID, newText) {
            var btn = document.getElementById(btnID);
            btn.disabled = true;
            btn.value = newText;
        }
        function disAbleMe() {
            $("wizReg$FinishNavigationTemplateContainerID$FinishButton").disabled = true;

        }

        function ClearMOTSelection() {

            $(document).ready(function () {

                var radiolist = $('#wizReg_wizReg_RBLMOT').find('input:radio');
                radiolist.removeAttr('checked');
            });
        }

        function ClearHostelSelection() {
            $(document).ready(function () {
                $('#wizReg_wizReg_chkHostel').attr('checked', false);
            });
        }

        function FinishConfirmation() {

            var answer = confirm("Are you sure? You want to finish this transaction..!!")
            if (answer) {

            }
            else {

                return false;
            }
        }
        function ClearSearch() {
            var txt_ID = document.getElementById("txtSearchName");
            txt_ID.value = "";
        }

        function onASMSelectedMenuItem() {

            $get('btnSearch').click();
        }

        function GetSuggestions(keyword, usePaging, pageIndex, pageSize, callbackMethod) {

            TRACE("GetSuggestions");
            var Condition = '';
            var SearchOption = 1;
            var tab_name = '';
            var txtField = '';
            var valField = '';
            var ddl_ID = document.getElementById("wizReg_ddlInstitute");

            var SearchOption_ID = document.getElementById("wizReg_rdbSearchOpt_0");

            var SearchOptionSelectedValue;
            for (var i = 0; i < SearchOption_ID.length; i++) {
                if (SearchOption_ID[i].checked) {
                    SearchOptionSelectedValue = SearchOption_ID[i];
                    break;
                }
            }

            Condition = ' InstituteID = ' + ddl_ID.value;

            SearchOption = SearchOptionSelectedValue.value;

            if (SearchOption == 3) {
                tab_name = "(select StudentID, StudentName+' - '+FatherName as StudentName,InstituteID from StudentReg where InstituteID = '" + ddl_ID.value + "') as StudentSearch";

                txtField = 'StudentName';
                valField = 'StudentID';
            }
            else if (SearchOption == 2) {
                tab_name = "StudentReg";
                txtField = 'RegNo';
                valField = 'StudentID';
            }

            else {
                tab_name = "StudentRollNo";
                txtField = 'UniversityRollNo';
                valField = 'StudentID';
            }

            RCSService.GetSuggestion_StudentSearch(keyword, usePaging, pageIndex, pageSize, true, tab_name, txtField, valField, Condition, callbackMethod);

        }

        function OnupdateTextBoxValue(evt) {
            var textBoxID = evt.source.textBoxID;
            $get(textBoxID).value = evt.selMenuItem.label;
            evt.preventDefault();
        }

    </script>

    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.4/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

    <style type="text/css">
        .input-group {
            display: flex;
            align-items: center;
        }

        .input-group-text {
            background-color: #fff;
            padding: 0.77rem 1rem;
            border: 1px solid #d1d5db;
            border-left: none;
            display: flex;
            align-items: center;
        }

        .custom-textbox {
            border: 1px solid #d1d5db;
            border-right: none;
            padding: 0.5rem;
            height: 100%;
            flex: 1;
        }

        .custom-dropdown {
    width: 100%;
    min-height: 34px;
}

.dd_chk_drop {
    position: absolute !important;
    z-index: 99999 !important;
    background: #fff !important;
    border: 1px solid #ccc !important;
    max-height: 300px;
    overflow-y: auto;
}
    </style>

    <script type="text/javascript">
           
             
        function PreviewImage(input) {

            if (input.files && input.files[0]) {

                var reader = new FileReader();

                reader.onload = function (e) {
                    document.getElementById('<%= imgPhoto.ClientID %>').src = e.target.result;
                     };

                     reader.readAsDataURL(input.files[0]);
                     }
                 }


                 function previewSign(input) {
                     if (input.files && input.files[0]) {

                         var reader = new FileReader();

                         reader.onload = function (e) {

                             var img = document.getElementById('<%= imgSign.ClientID %>');

                    img.src = e.target.result;
                    img.style.display = "block";
                };

                reader.readAsDataURL(input.files[0]);
                }
            }
 
</script>
    <script type="text/javascript">
        $(document).ready(function () {

            $(".dd_chk_select #caption").click(function () {
                $(this).siblings(".dd_chk_drop").toggle();
            });

        });
</script>

    <script type="text/javascript">
        document.addEventListener("change", function (e) {

            if (e.target.name.indexOf("_sll") > -1) {

                var isChecked = e.target.checked;

                var container = e.target.closest("#checks");

                var checkboxes = container.querySelectorAll(
                    "input[type='checkbox']:not([name*='_sll'])"
                );

                checkboxes.forEach(function (cb) {
                    cb.checked = isChecked;
                });
            }

        });
</script>
</head>
<body>
    <form id="form1" runat="server">

        <asp:Label ID="hindiOutput" runat="server"></asp:Label>

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            <Services>
                <asp:ServiceReference Path="~/RCSService.asmx" />
            </Services>
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <section id=''>
        <div class=''>
          <div id='Div1'>
               <div class='col-sm-12 col-lg-12'>               
                  <div class='box'>
                    <div class='box-header blue-background'>
                      <div class='title'>
                       <h4>
                       <i class="fa fa-bars"></i>
                        <div id="lbltitle"  runat="server" style="display:inline-block;">
                        </div>
                           <h4></h4>
                           <h4></h4>
                           <h4></h4>
                           <h4></h4>
                          </h4>                      
                       </div>                     
                    </div>           
                                   
                   <div class='col-sm-12 col-lg-12'>
                    <div class='box-content'>                                      
                   <asp:Wizard ID="wizReg" runat="server" ActiveStepIndex="0" 
                            DisplaySideBar="False"   
                             Width="100%" onactivestepchanged="wizReg_ActiveStepChanged"            
                                 style="font-family: Arial, Helvetica, sans-serif; font-size: x-small; text-align: left;">
                           
                            <FinishNavigationTemplate>
                               
                            </FinishNavigationTemplate>
                           
                            <HeaderStyle Height="20px" />
                            <StartNavigationTemplate>
                                <asp:Button ID="StartNextButton" runat="server" CommandName="MoveNext" 
                                    Text="Next &gt;&gt;" Font-Names="Verdana"  
                                    CssClass="btn btn-primary" TabIndex="15" />
                            </StartNavigationTemplate>                           
                            <WizardSteps>
                       <asp:WizardStep ID="step1" runat="server" StepType="Start" title="">
                    
                   <div class='col-sm-12 form-group-2' style="display:none;">
                   <asp:RadioButtonList ID="rdbSearchOpt" runat="server" 
                                RepeatDirection="Horizontal" Font-Size="Small" onchange="ClearSearch();" Width="70%">
                                <asp:ListItem Selected="True" Value="1">Search By Roll No.</asp:ListItem>
                                <asp:ListItem Value="2">Search By Registration No.</asp:ListItem>
                                <asp:ListItem Value="3">Search By Name</asp:ListItem>
                            </asp:RadioButtonList>   
                   </div>
 <div class='col-sm-12 form-group-2' id="Div_msg" runat="server" style="background-color:#edc1be;display:none;margin-bottom:18px;">
                               <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Larger" Text=""></asp:Label>
                              </div>
                           <div class="row col-sm-12 col-md-12">
                               <div class='col-sm-12 form-group-2 ' runat="server" id="divInst">
                    <div class='col-sm-2 form-group-2'>
                     <asp:Label ID="Label15" runat="server" CssClass="Label_form" Text="Institute Name"></asp:Label>
                       <span class="mandatoryfields">*</span>
                        </div>
                      <div class='col-sm-9 form-group-2 '>
                       <asp:DropDownList ID="ddlInstitute" runat="server" OnSelectedIndexChanged="ddlInstitute_SelectedIndexChanged" 
                        AutoPostBack="true" class="form-control">
                        </asp:DropDownList>
                          </div>
                     </div>
                                </div>
                           <div class="row col-sm-12 col-md-12">
                                <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="lblStuedent" runat="server" CssClass="Label_form" Text="Student Name"></asp:Label>
                        <span class="mandatoryfields">*</span>
                          </div>
                           <div class='col-sm-6 form-group-2 '>
                           <asp:TextBox ID="wizReg_txtStudentName" runat="server" TabIndex="1" class="form-control"></asp:TextBox>
                          </div>
                        </div>  
                                 <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="lbl" runat="server" CssClass="Label_form" Text="Father Name"></asp:Label>
                        <span class="mandatoryfields">*</span>
                          </div>
                           <div class='col-sm-6 form-group-2 '>
                          <asp:TextBox ID="wizReg_txtFatherName" runat="server" TabIndex="2" class="form-control"></asp:TextBox>
                           </div>
                        </div> 
                                </div>  
  

                             <div class="row col-sm-12 col-md-12">
                                 <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                           <asp:Label ID="lblmother" runat="server" CssClass="Label_form" Text="Mother Name"></asp:Label>
                            <span class="mandatoryfields">*</span>
                           </div>
                                    <div class='col-sm-6 form-group-2 '>
                          <asp:TextBox ID="txtmotherName" runat="server" TabIndex="3" class="form-control"></asp:TextBox>
                           </div>
                        </div> 
                                 <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label3" runat="server" CssClass="Label_form" Text="Date of Birth"></asp:Label>
                         <span class="mandatoryfields">*</span>
                          </div>
                          <div class='col-sm-6 form-group-2 '>
                          <asp:TextBox ID="wizReg_txtDOB" runat="server"  
                           EnableTheming="True" onblur="checkdate1(this,'en-US','dd-MMM-yyyy');" MaxLength="8" class="form-control" AutoPostBack="true" OnTextChanged="wizReg_txtDOB_TextChanged" placeholder="DDMMYYYY" TabIndex="4"></asp:TextBox>
                           </div>
                        </div> 
                         
                                </div>  

                            <div class="row col-sm-12 col-md-12" >

                                 <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label2" runat="server" CssClass="Label_form" Text="Gender"></asp:Label>
                       <span class="mandatoryfields">*</span>
                        </div>
                         <div class='col-sm-6 form-group-2 '>
                         <asp:DropDownList ID="wizReg_ddlGender" runat="server" class="form-control" TabIndex="5">
                              <asp:ListItem>---Select---</asp:ListItem>
                              <asp:ListItem>Male</asp:ListItem>
                             <asp:ListItem>Female</asp:ListItem>
                             <asp:ListItem>TransGender</asp:ListItem>
                           </asp:DropDownList>     

                         </div>
                        </div>

                                 <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label4" runat="server" CssClass="Label_form" Text="Student Mob No."></asp:Label>
                         <span class="mandatoryfields">*</span>
                          </div>
                         <div class='col-sm-6 form-group-2 '>
                         <asp:TextBox ID="wizReg_txtContact" runat="server" 
                           MaxLength="10" onkeypress="return isNumberKey(event);" class="form-control" TabIndex="6" OnTextChanged="wizReg_txtContact_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div> 
                                
                               
 
                            </div>

                           <div class="row col-sm-12 col-md-12" >
                            <div class='col-sm-6 form-group-2 '>
                              <div class='col-sm-4 form-group-2'>
                               <asp:Label ID="lblstudentEmail" runat="server" CssClass="Label_form" Text="Student Email ID"></asp:Label>
                                     <span class="mandatoryfields">*</span>
                                  </div>
                                    <div class='col-sm-6 form-group-2 '>
                            <asp:TextBox ID="txtemail" runat="server" CssClass="form-control" placeholder="example@company.com" inputmode="email" TabIndex="6" onblur="return validateEmail(this);"></asp:TextBox>
                                   </div>
                                    </div> 

                                <div class='col-sm-6 form-group-2 ' id="div_Aadhaar" runat="server">
                    
                                     <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label27" runat="server" CssClass="Label_form" Text="Aadhaar No."></asp:Label>
                       <span class="mandatoryfields">*</span>
                       </div>
                       <div class='col-sm-6 form-group-2 '>

                           <div class="input-group mb-3">
                       <asp:TextBox ID="txtAadhaar" class="form-control custom-textbox" TabIndex="8" aria-describedby="addon1" runat="server" OnTextChanged="txtAadhaar_TextChanged" MaxLength="12" AutoPostBack="true" CssClass="form-control" onkeypress="return isNumberKey(event);" onblur="return validateAadhaar();"></asp:TextBox>    
                                        <span  class="input-group-text form-control" runat="server"  id="Span1" style="color:green;display:none">✔</span>
                                   </div>
                        </div>

                           </div>
                               </div>

                            <div class="row col-sm-12 col-md-12" >

                                 <div class='col-sm-6 form-group-2 '>
                      <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label30" runat="server" CssClass="Label_form" Text="Student PAN Number"></asp:Label>
                         <span class="mandatoryfields">*</span>
                          </div>
                         <div class='col-sm-6 form-group-2 '>
                        <asp:TextBox ID="txtStudentPANNumber" runat="server" onblur="return validatePAN();" class="form-control" OnTextChanged="txtStudentPANNumber_TextChanged" AutoPostBack="true" TabIndex="9"></asp:TextBox>
                            </div>
                        </div> 

                                <div class='col-sm-6 form-group-2 '>
                      <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label28" runat="server" CssClass="Label_form" Text="Father Mob No."></asp:Label>
                          <span class="mandatoryfields">*</span>
                          </div>
                         <div class='col-sm-6 form-group-2 '>
                         <asp:TextBox ID="txtFatherMobNo" runat="server"  MaxLength="10" onkeypress="return isNumberKey(event);" class="form-control"  AutoPostBack="true" TabIndex="10"></asp:TextBox>
                            </div>
                        </div> 
                           
                                    </div>

                                <div class="row col-sm-12 col-md-12" >

                                       <div class='col-sm-6 form-group-2 '>
                              <div class='col-sm-4 form-group-2'>
                               <asp:Label ID="Label29" runat="server" CssClass="Label_form" Text="Father Email ID"></asp:Label>
                                   <span class="mandatoryfields">*</span>
                                  </div>
                            <div class='col-sm-6 form-group-2 '>
                               
                               <asp:TextBox ID="txtFemail" runat="server" CssClass="form-control" TabIndex="11" placeholder="example@company.com" inputmode="email" onblur="return validateEmail(this);"></asp:TextBox>                        </div>

                               </div>

                                <div class='col-sm-6 form-group-2 '>
                      <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label51" runat="server" CssClass="Label_form" Text="Father Aadhar No."></asp:Label>
                          <span class="mandatoryfields">*</span>
                          </div>
                         <div class='col-sm-6 form-group-2 '>
                     <asp:TextBox ID="txtFatherAadharNo" runat="server" TabIndex="12" MaxLength="12" onkeypress="return isNumberKey(event);" class="form-control" AutoPostBack="true" onblur="return validateFatherAadhaar();"></asp:TextBox>                            </div>
                        </div> 
                             
                                    </div>

                            <div class="row col-sm-12 col-md-12" >

                                 <div class='col-sm-6 form-group-2 '>
                      <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label38" runat="server" CssClass="Label_form" Text="Father PAN Number"></asp:Label>
                          <span class="mandatoryfields">*</span>
                          </div>
                         <div class='col-sm-6 form-group-2 '>
                        <asp:TextBox ID="FatherPANNumber" runat="server" onblur="return validateFatherPAN();" TabIndex="13" class="form-control"></asp:TextBox>
                            </div>
                        </div> 

                                 <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                         <asp:Label ID="Label1" runat="server" CssClass="Label_form" Text="Mother Mob. No."></asp:Label>
                         <span class="mandatoryfields">*</span>
                       </div>
                       <div class='col-sm-6 form-group-2 '>
                         <asp:TextBox ID="txtMotherMobNo" runat="server"  MaxLength="10" TabIndex="14" onkeypress="return isNumberKey(event);" class="form-control"  AutoPostBack="true"></asp:TextBox>

                        </div>
                        </div>

                                 
                           </div> 
                           
                            
                            <div class="row col-sm-12 col-md-12" >

                                 <div class='col-sm-6 form-group-2 '>
                                <div class='col-sm-4 form-group-2'>
                               <asp:Label ID="Label37" runat="server" CssClass="Label_form" Text="Mother Email ID"></asp:Label> 
                                     <span class="mandatoryfields">*</span>  
                                    </div>      
                                    <div class='col-sm-6 form-group-2 '>  
                            <asp:TextBox ID="txtMemail" runat="server" CssClass="form-control" TabIndex="15" placeholder="example@company.com" inputmode="email" onblur="return validateEmail(this);"></asp:TextBox>                                   
                                    </div>
                        </div>

                                 <div class='col-sm-6 form-group-2 '>
                              <div class='col-sm-4 form-group-2'>
                               <asp:Label ID="Label52" runat="server" CssClass="Label_form" Text="Mother Aadhar No"></asp:Label>
                                   <span class="mandatoryfields">*</span>
                                  </div>
                            <div class='col-sm-6 form-group-2 '>
                               
                         <asp:TextBox ID="txtMotherAadharNo" runat="server" TabIndex="16" MaxLength="12" onkeypress="return isNumberKey(event);" class="form-control" AutoPostBack="true" onblur="return validateMotherAadhaar();"></asp:TextBox>  

                            </div>

                               </div>
                                 
                            </div>    
                                  
                                 <div class="row col-sm-12 col-md-12" >
                                      <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="lbl_Course" runat="server" CssClass="Label_form" Text="Class"></asp:Label>
                       <span class="mandatoryfields">*</span>
                       </div>
                       <div class='col-sm-6 form-group-2 '>
                           <asp:DropDownCheckBoxes ID="wizReg_ddlCourse"  runat="server"  CssClass="custom-dropdown" TabIndex="17"></asp:DropDownCheckBoxes>
                        <%--<asp:DropDownList ID="wizReg_ddlCourse" runat="server" AutoPostBack="True" 
                         OnSelectedIndexChanged="wizReg_ddlCourse_SelectedIndexChanged" 
                         class="form-control" TabIndex="21"></asp:DropDownList>--%>
                        </div>
                        </div>     

                      <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label39" runat="server" CssClass="Label_form" Text="SPL Number"></asp:Label>
                         
                          </div>
                         <div class='col-sm-6 form-group-2 '>
                         <asp:TextBox ID="txtSPLNumber" runat="server" TabIndex="18" class="form-control"></asp:TextBox>
                            </div>
                        </div> 

                                    </div>  
                           
                           <div class="row col-sm-12 col-md-12" >
                               
                           <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label42" runat="server" CssClass="Label_form" Text="Computer Number"></asp:Label>
                       
                          </div>
                         <div class='col-sm-6 form-group-2 '>
                         <asp:TextBox ID="txtComputerNumber" runat="server" TabIndex="19" class="form-control"></asp:TextBox>
                            </div>
                        </div> 
                        

                               </div> 
                           <div>
                                 <asp:Label ID="Label41" 
    runat="server" 
    Text="Permanent Address"
    Style="font-weight:bold; font-size:18px; color:skyblue; margin-left:30px;">
</asp:Label> 
                         
                                <div class="row col-sm-12 col-md-12" >
                                <div class='col-sm-6 form-group-2 ' runat="server" id="Div4">
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label44" runat="server" CssClass="Label_form" Text="City" ></asp:Label>
                       <span class="mandatoryfields">*</span>
                        </div>
                        <div class='col-sm-6 form-group-2 '>
                       <asp:DropDownList ID="ddlPcity"  OnSelectedIndexChanged="ddlcity_SelectedIndexChanged" AutoPostBack="true" runat="server" class="form-control" TabIndex="20">
                        </asp:DropDownList>      

                        </div>
                                    <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label45" runat="server" CssClass="Label_form" Text="State"></asp:Label>
                       <span class="mandatoryfields">*</span>
                        </div>
                                  
                        <div class='col-sm-6 form-group-2 '>
                       <asp:DropDownList ID="ddlPState" runat="server" class="form-control" TabIndex="21">
                        </asp:DropDownList>     

                        </div>

                                     
                    <div class='col-sm-4 form-group-2'>
                     <asp:Label ID="Label46" runat="server" CssClass="Label_form" Text="Country"></asp:Label>
                       <span class="mandatoryfields">*</span>
                        </div>
                      <div class='col-sm-6 form-group-2 '>
                       <asp:DropDownList ID="ddlPCountry" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true" runat="server" TabIndex="22" 
                         class="form-control">
                        </asp:DropDownList>
                         
                     </div>
                                     </div>
                                      <div class='col-sm-6 form-group-2 '>
                                     
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label47" runat="server" CssClass="Label_form" Text="Parmanent Address"></asp:Label>
                       <span class="mandatoryfields">*</span>
                       </div>
                       <div class='col-sm-6 form-group-2 '>
                         <asp:TextBox ID="txtPAddress" placeholder="(Max 300 Characters are allowed)" TextMode="MultiLine" Height="60px" MaxLength="300" Width="225px" TabIndex="14" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                                         
                       

                                     <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label50" runat="server" CssClass="Label_form" Text="PIN Code"></asp:Label>
                       <span class="mandatoryfields">*</span>
                        </div>
                                  
                        <div class='col-sm-6 form-group-2 '>
                       <asp:TextBox ID="txtPZip" runat="server" class="form-control"></asp:TextBox>    

                        </div>
                                          </div>
                             

 </div>
                        </div>
                          
                           <div>
                               <asp:Label ID="Label40" runat="server" Text="Current Address" Style="font-weight:bold; font-size:18px; color:skyblue; margin-left:30px;"></asp:Label>
                               
                                <div class="row col-sm-12 col-md-12" >
                                <div class='col-sm-6 form-group-2 ' runat="server" id="Div3">
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label24" runat="server" CssClass="Label_form" Text="City"></asp:Label>
                       <span class="mandatoryfields">*</span>
                        </div>
                        <div class='col-sm-6 form-group-2 '>
                       <asp:DropDownList ID="ddlcity" OnSelectedIndexChanged="ddlcity_SelectedIndexChanged" AutoPostBack="true" runat="server" class="form-control" TabIndex="11">
                        </asp:DropDownList>     

                        </div>
                                    <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label25" runat="server" CssClass="Label_form" Text="State"></asp:Label>
                       <span class="mandatoryfields">*</span>
                        </div>
                                  
                        <div class='col-sm-6 form-group-2 '>
                       <asp:DropDownList ID="ddlState" runat="server" class="form-control" TabIndex="12">
                        </asp:DropDownList>     

                        </div>

                                     
                    <div class='col-sm-4 form-group-2'>
                     <asp:Label ID="Label26" runat="server" CssClass="Label_form" Text="Country"></asp:Label>
                       <span class="mandatoryfields">*</span>
                        </div>
                      <div class='col-sm-6 form-group-2 '>
                       <asp:DropDownList ID="ddlCountry" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true" runat="server" TabIndex="13" 
                         class="form-control">
                        </asp:DropDownList>
                         
                     </div>
                                     

                        </div>
                                 <div class='col-sm-6 form-group-2 '>
                                     
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label23" runat="server" CssClass="Label_form" Text="Current Address"></asp:Label>
                       <span class="mandatoryfields">*</span>
                       </div>
                       <div class='col-sm-6 form-group-2 '>
                         <asp:TextBox ID="txtCAddress" placeholder="(Max 300 Characters are allowed)" TextMode="MultiLine" Height="60px" MaxLength="300" Width="225px" TabIndex="14" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                          <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label49" runat="server" CssClass="Label_form" Text="PIN Code"></asp:Label>
                       <span class="mandatoryfields">*</span>
                        </div>
                                  
                        <div class='col-sm-6 form-group-2 '>
                       <asp:TextBox ID="txtCZip" runat="server" class="form-control"></asp:TextBox>    

                        </div>               

                        </div>

                            </div>
                               </div>


                           <div>
                               <asp:Label ID="Label43" runat="server" Text="Upload Photo/Document" Style="font-weight:bold; font-size:18px; color:skyblue; margin-left:30px;"></asp:Label> 
                      <div class="row">
                                       
<div class="col-sm-6">

    <div class="row">

        <div class="col-sm-4 text-center">
            <asp:Image ID="imgPhoto" runat="server"
                ImageUrl="~/assets/images/User.jpg"
                Width="120px" Height="120px" />
        </div>

        <label class="Label_form">
            Attach Student Photo
        </label>
        <span class="mandatoryfields">*</span>

        <div class="col-sm-6">

            <div class="row">

                <div class="col-sm-12" style="color:red;font-size:small;">
                    Supported formats : png, jpeg, jpg
                </div>

                <div class="col-sm-12" style="color:red;font-size:small;">
                    Maximum file size : 200 KB
                </div>

                <div class="col-sm-12 form-group" style="margin-top:10px;">
                    <asp:FileUpload ID="filePhoto" runat="server"
                        CssClass="form-control"
                        TabIndex="27"
                        onchange="PreviewImage(this);" />
                </div>

            </div>

        </div>

    </div>

</div>

       
                    
    <div class="col-sm-6" >
         
        <div class="row">
            
         <div class="col-sm-6 form-group">

              <label class="Label_form">
        Attach Student Final Signed Quotation
    </label>
    <span class="mandatoryfields">*</span>

    <div class="col-sm-12" style="color:red;font-size:small;">
        Maximum file size : 1 MB
    </div>
            
    <div class="col-sm-12 text-center">
        <asp:Image ID="imgSign"
            runat="server"
            Width="200px"
            Height="80px"
            Style="display:none;border:1px solid #ccc;padding:2px;" />
    </div>
               <%--<small class="text-muted">
        Allowed: JPG, JPEG, PNG
    </small>--%>
             <asp:Label ID="lblsign"
        runat="server"
        ForeColor="Green"
        Font-Bold="true">
    </asp:Label>
               <asp:FileUpload ID="_StudentSign"
        runat="server"
        CssClass="form-control"
        onchange="previewSign(this);" />


    

</div>

           <div class="col-sm-6 form-group">
    
    <label class="Label_form">
        Attach 10th Marksheet
    </label>
    <span class="mandatoryfields">*</span>

    <div class="col-sm-12" style="color:red;font-size:small;">
        Upload only pdf format
    </div>

    <asp:FileUpload ID="fu10thMarksheet"
        runat="server"
        CssClass="form-control" />

    <small class="text-muted">
        Allowed: PDF
    </small>

    <br />

    <asp:Label ID="lbl10thFileName"
        runat="server"
        ForeColor="Green"
        Font-Bold="true">
    </asp:Label>

            </div>
        </div>
    </div>

</div>
                               </div>
                           <center> <div class="col-sm-12 form-group">
                        <asp:Button ID="btnUpload" runat="server"
                            CssClass="btn btn-primary"
                            OnClick="btnUpload_Click"
                            TabIndex="28"
                            Text="Upload All Documents"
                            Width="25%" />
                    </div></center>

                              
                           <div class="row col-sm-12 col-md-12" style="display:none" >

                   
                                <div class='col-sm-6 form-group-2 ' style="display:none" id="Div_Passport" runat="server">
                    
                                     <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label31" runat="server" CssClass="Label_form" Text="Passport No."></asp:Label>
                       <span class="mandatoryfields">*</span>
                       </div>
                       <div class='col-sm-6 form-group-2 '>

                           <div class="input-group mb-3">
   <asp:TextBox ID="txtpassport"  class="form-control custom-textbox" TabIndex="16" runat="server"  AutoPostBack="true"  CssClass="form-control"></asp:TextBox>
    
</div>
                       </div>
                        </div>
                                <div class='col-sm-6 form-group-2 ' style="display:none" id="div_passortdate" runat="server">
                    
                                     <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label32" runat="server" CssClass="Label_form" Text="Passport Valid Till Date"></asp:Label>
                       <span class="mandatoryfields">*</span>
                       </div>
                       <div class='col-sm-6 form-group-2 '>

                           <div class="input-group mb-3">
   <asp:TextBox ID="txtpassportdate"  class="form-control custom-textbox" TabIndex="16" runat="server" AutoPostBack="true" onblur="checkdate1(this,'en-US','dd-MMM-yyyy');" MaxLength="8" placeholder="DDMMYYYY"  CssClass="form-control"></asp:TextBox>
    
</div>
                       </div>
                        </div>
                                <div class='col-sm-6 form-group-2 ' style="display:none" id="Div_visa" runat="server">
                    
                                     <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label33" runat="server" CssClass="Label_form" Text="Visa No."></asp:Label>
                       <span class="mandatoryfields">*</span>
                       </div>
                       <div class='col-sm-6 form-group-2 '>

                           <div class="input-group mb-3">
   <asp:TextBox ID="txtvisa"  class="form-control custom-textbox" TabIndex="16"   runat="server"   AutoPostBack="true"  CssClass="form-control"></asp:TextBox>
    
</div>
                       </div>
                        </div>

                                <div class='col-sm-6 form-group-2 ' style="display:none" id="Div_Visadate" runat="server">
                    
                                     <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label34" runat="server" CssClass="Label_form" Text="Visa Valid Till Date"></asp:Label>
                       <span class="mandatoryfields">*</span>
                       </div>
                       <div class='col-sm-6 form-group-2 '>

                           <div class="input-group mb-3">
   <asp:TextBox ID="txtvisadate"  class="form-control custom-textbox" TabIndex="16"  runat="server" onblur="checkdate1(this,'en-US','dd-MMM-yyyy');" MaxLength="8" placeholder="DDMMYYYY"  AutoPostBack="true"  CssClass="form-control"></asp:TextBox>
    
</div>
                       </div>
                        </div>
                                <div class='col-sm-6 form-group-2 ' style="display:none" id="Div_frro" runat="server">
                    
                                     <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label35" runat="server" CssClass="Label_form" Text="FRRO No."></asp:Label>
                        
                       </div>
                       <div class='col-sm-6 form-group-2 '>

                           <div class="input-group mb-3">
   <asp:TextBox ID="txtfrro"  class="form-control custom-textbox" TabIndex="16"  runat="server"  AutoPostBack="true"  CssClass="form-control"></asp:TextBox>
    
</div>
                       </div>
                        </div>
                                <div class='col-sm-6 form-group-2 ' style="display:none" id="div_frrodate" runat="server">
                    
                                     <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label36" runat="server" CssClass="Label_form" Text="FRRO Valid Till Date"></asp:Label>
                        
                       </div>
                       <div class='col-sm-6 form-group-2 '>

                           <div class="input-group mb-3">
   <asp:TextBox ID="txtfrrodate"  class="form-control custom-textbox" TabIndex="16"  runat="server" onblur="checkdate1(this,'en-US','dd-MMM-yyyy');" MaxLength="8" placeholder="DDMMYYYY" AutoPostBack="true"  CssClass="form-control"></asp:TextBox>
    
</div>
                       </div>
                        </div>             

                              <div id="div_none" runat="server" style="display:none">    

 <div class='col-sm-6 form-group-2 ' runat="server" id="Div_AdmissionSource">
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label5" runat="server" CssClass="Label_form" Text="Admission Source"></asp:Label>
                       <span class="mandatoryfields">*</span>
                        </div>
                        <div class='col-sm-6 form-group-2 '>
                       <asp:DropDownList ID="wizReg_ddlAdmSource" runat="server" class="form-control" TabIndex="17">
                        </asp:DropDownList>     

                        </div>
                        </div>
                           

                      <div class='col-sm-6 form-group-2 ' style="display:none;">
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label16" runat="server" CssClass="Label_form" Text="Search Student"></asp:Label>
                                  <span class="mandatoryfields">*</span>
                             </div>
                              <div class='col-sm-6 form-group-2 '>
                           <asp:TextBox ID="txtSearchName"  CssClass="search form-control" Height="34" Width="100%" placeholder="Search Student to Update Detail"  runat="server"></asp:TextBox>
                          <Custom:AutoSuggestMenu ID="txtNameStudent" runat="server" IsFlexibleQuery="false" KeyPressDelay="10" OnClientTextBoxUpdate="OnupdateTextBoxValue" OnGetSuggestions="GetSuggestions" onselectedvalueevent="onASMSelectedMenuItem"  TargetControlID="txtSearchName" UseIFrame="false" UsePageMethods="false" />

                            </div>
                        </div>  

                      
                             <div class="row col-sm-12 col-md-12">
                                  <div class='col-sm-6 form-group-2 ' runat="server" id="Div_AdmType">
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label6" runat="server" CssClass="Label_form" Text="Admission Type"></asp:Label>
                       <span class="mandatoryfields">*</span>
                        </div>
                        <div class='col-sm-6 form-group-2 '>
                            <asp:DropDownList ID="wizReg_ddlAdmType" runat="server" AutoPostBack="True" 
                            OnSelectedIndexChanged="wizReg_ddlAdmType_SelectedIndexChanged" 
                           class="form-control" TabIndex="18"></asp:DropDownList> 

                        </div>
                        </div>

                        <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label7" runat="server" CssClass="Label_form" Text="Hostel Facility Required"></asp:Label>
                       <span class="mandatoryfields">*</span>
                        </div>
                       <div class='col-sm-6 form-group-2 '>
                      <asp:CheckBox ID="wizReg_chkHostel" runat="server" onclick="ClearMOTSelection();" 
                      class="form-control"  TabIndex="19"/>

                       </div>
                       </div>

                             </div>
                      
                           <div class="row col-sm-12 col-md-12">
                                <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label8" runat="server" CssClass="Label_form" Text="Mode of Transport"></asp:Label>
                       <span class="mandatoryfields">*</span>
                       </div>
                       <div class='col-sm-6 form-group-2 '>
                         <asp:RadioButtonList ID="wizReg_RBLMOT" runat="server" TabIndex="20"  onclick="ClearHostelSelection();" class="form-control" RepeatDirection="Horizontal">
                             <asp:ListItem Value="Bus">Bus</asp:ListItem>
                             <asp:ListItem Value="Picked Up">Picked Up</asp:ListItem>
                             <asp:ListItem Value="Walker" Selected="True">Walker</asp:ListItem>
                           </asp:RadioButtonList>
                             
                       
                        </div>
                        
                        </div>

                     

                           </div>
                      
                               <div class="row col-sm-12 col-md-12">

                                   <div class='col-sm-6 form-group-2 ' runat="server" id="Div_Spec">
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label10" runat="server" CssClass="Label_form" Text="Specialization"></asp:Label>
                        <span class="mandatoryfields">*</span>
                        </div>
                        <div class='col-sm-6 form-group-2 '>
                       <asp:DropDownList ID="wizReg_ddlSpec" runat="server" class="form-control" TabIndex="22">
                          </asp:DropDownList>
                           </div>
                        </div>

                       <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label11" runat="server" CssClass="Label_form" Text="Session"></asp:Label>
                       <span class="mandatoryfields">*</span>
                       </div>
                        <div class='col-sm-6 form-group-2 '>
                       <asp:DropDownList ID="wizReg_ddlSession" runat="server" AutoPostBack="True" 
                             OnSelectedIndexChanged="wizReg_ddlSession_SelectedIndexChanged" 
                                class="form-control" TabIndex="23"></asp:DropDownList>
                            </div>
                        </div>
                               </div>   

                      
                        <div class="row col-sm-12 col-md-12">
                              <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label21" runat="server" CssClass="Label_form" text="Year"></asp:Label>
                       <span class="mandatoryfields">*</span>
                       </div>
                       <div class='col-sm-6 form-group-2 '>
                        <asp:DropDownList ID="ddlyear" runat="server" AutoPostBack="True" 
                         OnSelectedIndexChanged="wizReg_ddlCourse_SelectedIndexChanged" 
                         class="form-control" TabIndex="24"></asp:DropDownList>
                        </div>
                        </div>
                           <div class='col-sm-6 form-group-2 ' runat="server" id="Div2">
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label18" runat="server" CssClass="Label_form" Text="Fee Setup"></asp:Label>
                         <span class="mandatoryfields">*</span>
                             </div>
                              <div class='col-sm-6 form-group-2 '>
                                  <asp:DropDownList ID="ddlModule" runat="server" AutoPostBack="True" 
                           OnSelectedIndexChanged="ddlModule_SelectedIndexChanged" class="form-control" 
                                            TabIndex="25"></asp:DropDownList>
                            </div>
                        </div>
                        

                        </div>

                       
                         <div class="row col-sm-12 col-md-12">
                               <div class='col-sm-6 form-group-2 ' runat="server" id="Div_YrSem">
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="lblY" runat="server" CssClass="Label_form" Text="Semester"></asp:Label>
                         <span class="mandatoryfields">*</span>
                             </div>
                              <div class='col-sm-6 form-group-2 '>
                                  <asp:DropDownList ID="wizReg_ddlYrSem" runat="server" AutoPostBack="True" 
                                     OnSelectedIndexChanged="wizReg_ddlYrSem_SelectedIndexChanged" class="form-control" 
                                            TabIndex="26"></asp:DropDownList>
                            </div>
                        </div>

             
                        
                         </div>
                                   </div>

                            
                           
<div class='col-sm-12 form-group-2'>

<asp:Button runat="server" ID="startCam" Text="Start Camera 📷" CssClass="btn btn-danger" OnClientClick="startCamera(); return false;"></asp:Button>
<asp:Button runat="server" ID="stopCam" Text="Stop Camera 🚫" CssClass="btn btn-danger" OnClientClick="stopCamera(); return false;" Style="display: none"></asp:Button>
<asp:Button runat="server" ID="btnCapture" Text="Take Photo ✨" CssClass="btn btn-primary" OnClientClick="captureImage(); return false;" Style="display: none"></asp:Button>
</div>

<div class='col-sm-12 form-group-2'>

    <div class='col-sm-2 form-group-2' style="text-align:center">
        <asp:Image runat="server" ID="defaultimg" ImageUrl="~/assets/images/User.jpg" Width="120px" Height="120px" />
        <center>
        <div id="webcam" style="display:none;"></div>
        </center>
    </div>

    <div class='col-sm-2 form-group-2' style="text-align:center">
        <asp:Image runat="server" ID="defaultimg1" ImageUrl="~/assets/images/User.jpg" Width="120px" Height="120px" />
        <asp:Image runat="server" ID="imgCapture" />
    </div>

      
</div>
<asp:HiddenField ID="hfImageData" runat="server" />
<asp:HiddenField ID="hdn_ImageID" runat="server" />
                               <asp:HiddenField ID="hdnSignData" runat="server" />
                                                              <asp:HiddenField ID="hdn10thMarksheetPdf" runat="server" />

                           
                            </asp:WizardStep>
                      <asp:WizardStep ID="step2" runat="server" StepType="Finish" Title="">

                       <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label9" runat="server" CssClass="Label_form" Text="Registration No."></asp:Label>                       
                             </div>
                              <div class='col-sm-6 form-group-2 '>
                            <asp:Label ID="lblRegNo" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#FF3300"></asp:Label>
                            </div>
                        </div>

                       <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label12" runat="server" CssClass="Label_form" Text="Student Name"></asp:Label>
                       
                             </div>
                              <div class='col-sm-6 form-group-2 '>
                              <asp:Label ID="lblStudentName" runat="server" CssClass="Label_form"  Font-Bold="True"></asp:Label>
                            </div>
                        </div>

                          <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label13" runat="server" CssClass="Label_form" Text="Cast Category"></asp:Label>
                       
                             </div>
                              <div class='col-sm-6 form-group-2 '>
                          <asp:Label ID="lblCategory" CssClass="Label_form" runat="server"></asp:Label>
                            </div>
                        </div>

                          <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label14" runat="server" CssClass="Label_form" Text="Father Name"></asp:Label>
                       
                             </div>
                              <div class='col-sm-6 form-group-2 '>
                        <asp:Label ID="lblFatherName" CssClass="Label_form" runat="server"></asp:Label>
                            </div>
                        </div>

                          <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label17" runat="server" CssClass="Label_form" Text="Student Mob. No."></asp:Label>
                        
                             </div>
                              <div class='col-sm-6 form-group-2 '>
                                 <asp:Label ID="lblContact" CssClass="Label_form" runat="server" ></asp:Label>
                            </div>
                        </div>

                       <div class='col-sm-6 form-group-2 '>

                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="lbl_CourseCaption" runat="server" CssClass="Label_form" Text=""></asp:Label>                      
                             </div>
                              <div class='col-sm-6 form-group-2 '>
                                  <asp:Label ID="lblCourse" CssClass="Label_form" runat="server"></asp:Label>
                            </div>
                        </div>

                          <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label19" runat="server" CssClass="Label_form" Text="Admission Type"></asp:Label>
                     
                             </div>
                              <div class='col-sm-6 form-group-2 '>
                                 <asp:Label ID="lblStType" CssClass="Label_form" runat="server"></asp:Label>
                            </div>
                        </div>

                          <div class='col-sm-6 form-group-2 '>
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label20" runat="server" CssClass="Label_form" Text="Session"></asp:Label>
                        
                             </div>
                              <div class='col-sm-6 form-group-2 '>
                                <asp:Label ID="lblSession" CssClass="Label_form" runat="server"></asp:Label>
                            </div>
                        </div>

                       <div class='col-sm-6 form-group-2 ' runat="server" id="Div_YrSem2">
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="lbl_Yr_Caption" runat="server" CssClass="Label_form" Text=""></asp:Label>
                       
                             </div>
                              <div class='col-sm-6 form-group-2 '>
                                 <asp:Label ID="lblYrSem" CssClass="Label_form" runat="server"></asp:Label>
                            </div>
                        </div>

                       <div class='col-sm-6 form-group-2 ' runat="server" id="Div_SpecSem2">
                       <div class='col-sm-4 form-group-2'>
                       <asp:Label ID="Label22" runat="server" CssClass="Label_form" Text="Batch"></asp:Label>                       
                             </div>
                              <div class='col-sm-6 form-group-2 '>
                                <asp:Label ID="lblBatch" CssClass="Label_form" runat="server" ></asp:Label>
                            </div>
                        </div>

                        <div>
                            <asp:GridView ID="gvFee" runat="server" AutoGenerateColumns="False" 
                                                    DataKeyNames="FeeDetailID,courseFeeId,FeeHeadCode,FeeClass,CID,CourseCode"
                                                    ShowFooter="True" class="scrollable-area table table-bordered table-striped">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TextBox1" runat="server" Enabled="False"  Width="250px" ></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkF" runat="server" Checked="True" Enabled="False"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                                                        <asp:BoundField DataField="FeeHeadName" HeaderText="Fee Head" />
                                                        <asp:TemplateField HeaderText="Total Amount">
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("TotalAmount") %>' 
                                                                    Width="250px" CssClass="form-control" Enabled="False"></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TextBox2" CssClass="form-control" Enabled="False"
                                                                     runat="server" Text='<%# Bind("TotalAmount")%>' 
                                                                  ></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="BreakoffTime" HeaderText="Break Off" 
                                                            Visible="False" />
                                                        <asp:BoundField DataField="LastDate" HeaderText="Due Date" 
                                                            DataFormatString="{0:dd-MMM-yyyy}" Visible="False"/>
                                                    </Columns>
                                                    <FooterStyle Font-Bold="True" />
                                                </asp:GridView>
                        </div>
                             </asp:WizardStep>
                            </WizardSteps>
                        </asp:Wizard>
                         
                      <div class="col-sm-12 col-lg-12 form-actions form-actions-padding-sm form-actions-padding-md form-actions-padding-lg"  style='margin-bottom: 0;text-align:right'>  
                        <asp:Button 
                            ID="btnView" runat="server"   
                            CssClass="btn btn-primary" Text="View"  TabIndex="29" Visible="False" />
                        <asp:Button  ID="btnfinish" runat="server"  
                            CssClass="btn btn-primary" Text="Finish" TabIndex="30" Visible="False"  OnClick="btnfinish_Click" />
                                              
                      </div>  
                     </div>            
                            
                  </div>   
              </div>
             </div>
              </div>
              </div>           
        </section>
                <asp:HiddenField ID="hdnCType" runat="server" />
                <asp:HiddenField ID="hdnStuID" runat="server" />
                <asp:HiddenField ID="hdnRegID" runat="server" />
                <asp:HiddenField ID="hdnStatus" runat="server" />
                <asp:HiddenField ID="hdnbatch" runat="server" />
                <asp:HiddenField ID="hdnChange" runat="server" />
                <asp:HiddenField ID="hdnFeeS" runat="server" />
                <asp:HiddenField ID="hdnAmt" runat="server" />
                <asp:Button ID="btnSearch" runat="server" CausesValidation="False" OnClick="btnSearchStu_Click" Style="display: none;" Text="Button" />

                <input type="hidden" id="hdnVoucherNo" runat="server" />
                <input type="hidden" id="hdnCompanyName" runat="server" />
                <input type="hidden" id="hdnStudentLedgerName" runat="server" />
                <input type="hidden" id="hdnAdditionalName" runat="server" />
                <input type="hidden" id="hdnStudentLedgerParent" runat="server" />
                <input type="hidden" id="hdnMainGroupName" runat="server" />
                <input type="hidden" id="hdnGroupParent" runat="server" />
                <input type="hidden" id="hdnMsg" runat="server" />
                <input type="hidden" id="hdnSubGroupName" runat="server" />
                <input type="hidden" id="hdnFeeHeadLedgerName" runat="server" />
                <input type="hidden" id="hdnFeeHeadLedgerAdditionalName" runat="server" />
                <input type="hidden" id="hdnFeeHeadLedgerParentName" runat="server" />
                <input type="hidden" id="hdnTransDate" runat="server" />
                <input type="hidden" id="hdnDueAmount" runat="server" />
                <input type="hidden" id="hdnVoucherType" runat="server" />
                <input type="hidden" id="hdnTransMode" runat="server" />
                <input type="hidden" id="hdnNarration" runat="server" />
            </ContentTemplate>

            <Triggers>
                <asp:PostBackTrigger ControlID="wizReg" />

            </Triggers>

        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
            <ProgressTemplate>
                <div class="ProgressMsg" style="height: auto">
                    <img src="../images/wait.gif" alt="Wait" />Please wait...        
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <cc1:UpdateProgressOverlayExtender ID="UpdateProgressOverlayExtender3" runat="server"
            CssClass="updateProgress" TargetControlID="UpdateProgress3" OverlayType="Browser" />



        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js" type="text/javascript"></script>

        <script type="text/javascript">
            function previewSign(input) {
                if (input.files && input.files[0]) {

                    var reader = new FileReader();

                    reader.onload = function (e) {
                        var img = document.getElementById('<%= imgSign.ClientID %>');
                img.src = e.target.result;
                img.style.display = "none";
            };

            reader.readAsDataURL(input.files[0]);
            }
        }
</script>

        <script type="text/javascript">


            $('#wizReg_wizReg_txtStudentName').keypress(function (e) {


                var text = document.getElementById("wizReg_TextBox3");
          
                var key = (e.keyCode ? e.keyCode : e.which);

         

                if (key == 13)
                {

             
                    $('#wizReg_wizReg_txtStudentName').val($('#wizReg_TextBox3').text());
                    $('#wizReg_TextBox3').text('')
                    $('.select-option').hide();
                }
                else {
            
                    $('.select-option').show();

                }
                var englishText = $('#wizReg_wizReg_txtStudentName').val();

                $.ajax({
                    url: `https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=hi&dt=t&q=${encodeURI(englishText)}`,
                success: function (result) {

             
                    if (result[0] != undefined && result[0] != null) {
                        var hindiText = result[0][0][0];

                        text.value=hindiText;
                    }
                    else {
                        $('#wizReg_TextBox3').text('');
                    }

                }
            });
            });

            $(document).on("click", ".select-option", function () {
                $('#wizReg_wizReg_txtStudentName').val($('#wizReg_TextBox3').text());
                $('#wizReg_TextBox3').text('')
            })

        </script>

        <script type="text/javascript">

            $('#wizReg_wizReg_txtFatherName').keypress(function (e) {
                 

                var text = document.getElementById("wizReg_txtFHindi");
          
                var key = (e.keyCode ? e.keyCode : e.which);

         

                if (key == 13)
                {

             
                    $('#wizReg_wizReg_txtFatherName').val($('#wizReg_txtFHindi').text());
                    $('#wizReg_txtFHindi').text('')
                    $('.select-option').hide();
                }
                else {
                   
                    $('.select-option').show();

                }
                var englishText = $('#wizReg_wizReg_txtFatherName').val();

                $.ajax({
                    url: `https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=hi&dt=t&q=${encodeURI(englishText)}`,
                success: function (result) {

             
                    if (result[0] != undefined && result[0] != null) {
                        var hindiText = result[0][0][0];

                        text.value=hindiText;
                    }
                    else {
                        $('#wizReg_txtFHindi').text('');
                    }

                }
            });
            });

            $(document).on("click", ".select-option", function () {
                $('#wizReg_wizReg_txtFatherName').val($('#wizReg_txtFHindi').text());
                $('#wizReg_txtFHindi').text('')
            })

        </script>

        <script type="text/javascript">

            $('#wizReg_txtmotherName').keypress(function (e) {
                 

                var text = document.getElementById("wizReg_TxtMhindi");
          
                var key = (e.keyCode ? e.keyCode : e.which);

         

                if (key == 13)
                {

             
                    $('#wizReg_txtmotherName').val($('#wizReg_TxtMhindi').text());
                    $('#wizReg_TxtMhindi').text('')
                    $('.select-option').hide();
                }
                else {
                   
                    $('.select-option').show();

                }
                var englishText = $('#wizReg_txtmotherName').val();

                $.ajax({
                    url: `https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=hi&dt=t&q=${encodeURI(englishText)}`,
                success: function (result) {

             
                    if (result[0] != undefined && result[0] != null) {
                        var hindiText = result[0][0][0];

                        text.value=hindiText;
                    }
                    else {
                        $('#wizReg_TxtMhindi').text('');
                    }

                }
            });
            });

            $(document).on("click", ".select-option", function () {
                $('#wizReg_txtmotherName').val($('#wizReg_TxtMhindi').text());
                $('#wizReg_TxtMhindi').text('')
            })

        </script>

    </form>
</body>
<script src="../assets/javascripts/jquery/jquery.min.js" type="text/javascript"></script>
<script src="../assets/javascripts/jquery/jquery.mobile.custom.min.js" type="text/javascript"></script>

<script src="../assets/javascripts/jquery/jquery-migrate.min.js" type="text/javascript"></script>
<script src="../assets/javascripts/jquery/jquery-ui.min.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/jquery_ui_touch_punch/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
<script src="../assets/javascripts/bootstrap/bootstrap.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/modernizr/modernizr.min.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/retina/retina.js" type="text/javascript"></script>
<script src="../assets/javascripts/theme.js" type="text/javascript"></script>
<script src="../assets/javascripts/demo.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/fileinput/bootstrap-fileinput.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/select2/select2.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/bootstrap_colorpicker/bootstrap-colorpicker.min.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/common/moment.min.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/bootstrap_daterangepicker/bootstrap-daterangepicker.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/bootstrap_datetimepicker/bootstrap-datetimepicker.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/input_mask/bootstrap-inputmask.min.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/bootstrap_maxlength/bootstrap-maxlength.min.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/charCount/charCount.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/autosize/jquery.autosize-min.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/bootstrap_switch/bootstrapSwitch.min.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/naked_password/naked_password-0.2.4.min.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/mention/mention.min.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/typeahead/typeahead.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/common/wysihtml5.min.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/common/bootstrap-wysihtml5.js" type="text/javascript"></script>
<script src="../assets/javascripts/plugins/pwstrength/pwstrength.js" type="text/javascript"></script>
</html>

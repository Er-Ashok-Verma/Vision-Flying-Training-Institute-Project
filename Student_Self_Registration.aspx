<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Student_Self_Registration.aspx.cs" Inherits="Flying_Hour_Student_Self_Registration" %>

<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
 
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Student Self Registration</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript">
        function PreviewImage(input) { if (input.files && input.files[0]) { var reader = new FileReader(); reader.onload = function (e) { document.getElementById('<%= imgPhoto.ClientID %>').src = e.target.result; }; reader.readAsDataURL(input.files[0]); } }
        function previewSign(input) { if (input.files && input.files[0]) { var reader = new FileReader(); reader.onload = function (e) { }; reader.readAsDataURL(input.files[0]); } }
        function validchar(e) { e = e || window.event; var char = e.keyCode || e.which; return ((char >= 65 && char <= 90) || (char >= 97 && char <= 122) || char == 32); }
        function validnum(e) { e = e || window.event; var num = e.keyCode || e.which; return (num >= 48 && num <= 57); }
        function validateEmail() { var email = document.getElementById("<%= txtEmail.ClientID %>").value.trim(); if (email == "") return true; var reg = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[A-Za-z]{2,}$/; if (!reg.test(email)) { alert("Please enter a valid email address."); document.getElementById("<%= txtEmail.ClientID %>").focus(); return false; } return true; }
        function validateFatherEmail() { var email = document.getElementById("<%= txtFatherEmail.ClientID %>").value.trim(); if (email == "") return true; var reg = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[A-Za-z]{2,}$/; if (!reg.test(email)) { alert("Please enter a valid father email address."); document.getElementById("<%= txtFatherEmail.ClientID %>").focus(); return false; } return true; }
        function validateMotherEmail() { var email = document.getElementById("<%= txtMotherEmail.ClientID %>").value.trim(); if (email == "") return true; var reg = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[A-Za-z]{2,}$/; if (!reg.test(email)) { alert("Please enter a valid mother email address."); document.getElementById("<%= txtMotherEmail.ClientID %>").focus(); return false; } return true; }
        function validateForm() { if (!validateEmail()) return false; if (!validateFatherEmail()) return false; if (!validateMotherEmail()) return false; return true; }
    </script>
    <style>
    * { margin: 0; padding: 0; box-sizing: border-box; }

    body {
        background: white;
        min-height: 100vh;
        font-family: 'Segoe UI', sans-serif;
        padding: 20px;
    }

    .main-card {
        background: white;
        border-radius: 25px;
        padding: 35px 35px 4px 35px;
        box-shadow: 0 10px 35px rgba(0,0,0,0.25);
        border: 1px solid rgba(255,255,255,0.2);
        margin-top: 20px;
        animation: fadeIn 0.8s ease-in-out;
    }

    .title {
        background-color: #243b55;
        padding: 14px;
        border-radius: 5px;
        text-align: center;
        font-size: 24px;
        font-weight: 500;
        color: white;
        margin-bottom: 30px;
        text-shadow: 2px 2px 8px rgba(0,0,0,0.3);
    }

    .title i {
        color: #ffd43b;
        margin-right: 10px;
        font-size: 24px;
    }

    .section-title {
        background: linear-gradient(45deg,#141e30,#243b55);
        color: white;
        padding: 10px 18px;
        border-radius: 8px;
        margin: 25px 0 20px 0;
        font-size: 18px;
        font-weight: 600;
        box-shadow: 0 4px 10px rgba(0,0,0,0.15);
    }

    .section-title i {
        color: #ffd43b;
        margin-right: 8px;
    }

    .form-label-custom {
        font-weight: 450;
        color: #1e2a3a;
        margin-bottom: 6px;
        display: block;
    }

    .required {
        color: red;
        margin-left: 3px;
    }

    .form-control,
    .form-select {
        height: 36px;
        border: 1px solid #ced4da;
        border-radius: 8px;
        padding: 8px 12px;
        font-size: 14px;
        transition: all 0.2s ease;
    }

    textarea.form-control {
        height: 80px;
        resize: vertical;
    }

    .form-control:focus,
    .form-select:focus {
        border-color: #243b55;
        box-shadow: 0 0 0 0.2rem rgba(36,59,85,0.18);
    }

    .form-group-custom {
        margin-bottom: 18px;
    }

    .custom-dropdown {
        width: 100% !important;
        min-height: 36px !important;
        border-radius: 8px !important;
    }

    .custom-dropdown select {
        width: 100% !important;
        min-height: 42px !important;
        border: 1px solid #ced4da;
        border-radius: 8px;
    }

    .upload-box {
        background: #f8f9fa;
        border: 1px solid #dee2e6;
        border-radius: 10px;
        padding: 8px;
        height: 140px;
        min-height: 140px;
        box-shadow: 0 3px 8px rgba(0,0,0,0.08);
        transition: all 0.3s ease;
    }

    .upload-box:hover {
        transform: translateY(-2px);
        box-shadow: 0 5px 12px rgba(0,0,0,0.12);
    }

    .upload-title {
        font-size: 13px;
        font-weight: 700;
        color: #243b55;
        margin-bottom: 5px;
    }

    .upload-help {
        color: #dc3545;
        font-size: 10px;
        display: block;
        margin: 3px 0;
        line-height: 13px;
    }

    .img-preview {
        width: 50px;
        height: 50px;
        border: 1px solid #ced4da;
        border-radius: 6px;
        object-fit: cover;
        background: white;
    }

    .upload-box .text-center {
        margin-bottom: 3px !important;
    }

    .upload-box .text-center i {
        font-size: 40px !important;
    }

    .upload-box .form-control {
        height: 30px;
        padding: 4px 6px;
        font-size: 11px;
    }

    .btn-custom {
        min-width: 140px;
        padding: 10px 25px;
        border-radius: 18px;
        font-weight: 600;
        margin: 5px;
        transition: all 0.2s ease;
    }

    
    .btn-submit {
        background: #198754;
        border-color: #198754;
        color: white;
    }

    .btn-reset {
        background: #dc3545;
        border-color: #dc3545;
        color: white;
    }

    .required-note {
        font-size: 13px;
        color: #dc3545;
        margin-bottom: 15px;
    }

    @keyframes fadeIn {
        from {
            opacity: 0;
            transform: translateY(20px);
        }
        to {
            opacity: 1;
            transform: translateY(0);
        }
    }

    @media (max-width:768px) {
        body {
            padding: 10px;
        }

        .main-card {
            padding: 18px 18px 4px 18px;
            border-radius: 20px;
            margin-top: 10px;
        }

        .title {
            font-size: 20px;
            line-height: 30px;
        }

        .section-title {
            font-size: 16px;
        }

        .upload-box {
            height: 140px;
            /*min-height: 140px;*/
            margin-bottom: 10px;
        }

        .img-preview {
            width: 50px;
            height: 50px;
            margin-bottom: 5px;
        }

        .btn-custom {
            width: 100%;
            margin: 5px 0;
        }
    }
</style>
       
</head>
<body>
<form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableViewState="true" EnablePageMethods="true"><Services><asp:ServiceReference Path="~/EducationService.asmx" /></Services></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="main-card">
                    <div class="title"><i class="fa-solid fa-user-graduate"></i>Student Self Registration</div>
                    <div class="required-note"><i class="fa-solid fa-circle-info"></i> Fields marked with <span class="required">*</span> are mandatory.</div>

                    <div class="section-title"><i class="fa-solid fa-user"></i>Student Information</div>
                    <div class="row">
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Student Name <span class="required">*</span></label><asp:TextBox ID="txtStudentName" runat="server" CssClass="form-control" placeholder="Enter Student Name" onkeypress="return validchar(event);"></asp:TextBox></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Mobile No. <span class="required">*</span></label><asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control" MaxLength="10" onkeypress="return validnum(event);" placeholder="Enter Mobile Number"></asp:TextBox></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Email <span class="required">*</span></label><asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="example@company.com" onblur="return validateEmail();"></asp:TextBox></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Gender <span class="required">*</span></label><asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select"><asp:ListItem Value="">--Select Gender--</asp:ListItem><asp:ListItem Value="Male">Male</asp:ListItem><asp:ListItem Value="Female">Female</asp:ListItem><asp:ListItem Value="Other">Other</asp:ListItem></asp:DropDownList></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Date of Birth <span class="required">*</span></label><asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Aadhaar No. <span class="required">*</span></label><asp:TextBox ID="txtAadhaarNo" runat="server" CssClass="form-control" MaxLength="12" onkeypress="return validnum(event);"></asp:TextBox></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Student PAN Number <span class="required">*</span></label><asp:TextBox ID="txtStudentPAN" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Class <span class="required">*</span></label><asp:DropDownCheckBoxes ID="wizReg_ddlCourse" runat="server" CssClass="custom-dropdown" TabIndex="17"></asp:DropDownCheckBoxes></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">SPL Number</label><asp:TextBox ID="txtSPLNumber" runat="server" CssClass="form-control" placeholder="Enter SPL Number"></asp:TextBox></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Computer Number</label><asp:TextBox ID="txtComputerNumber" runat="server" CssClass="form-control" placeholder="Enter Computer Number"></asp:TextBox></div>
                    </div>

                    <div class="section-title"><i class="fa-solid fa-person"></i>Father Information</div>
                    <div class="row">
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Father Name <span class="required">*</span></label><asp:TextBox ID="txtFatherName" runat="server" CssClass="form-control" placeholder="Enter Father Name" onkeypress="return validchar(event);"></asp:TextBox></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Father Mob No. <span class="required">*</span></label><asp:TextBox ID="txtFatherMobNo" runat="server" CssClass="form-control" MaxLength="10" onkeypress="return validnum(event);"></asp:TextBox></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Father Email ID <span class="required">*</span></label><asp:TextBox ID="txtFatherEmail" runat="server" CssClass="form-control" placeholder="Father Email" onblur="return validateFatherEmail();"></asp:TextBox></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Father Aadhaar No. <span class="required">*</span></label><asp:TextBox ID="txtFatherAadhaar" runat="server" CssClass="form-control" MaxLength="12" onkeypress="return validnum(event);"></asp:TextBox></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Father PAN Number <span class="required">*</span></label><asp:TextBox ID="txtFatherPAN" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox></div>
                    </div>

                    <div class="section-title"><i class="fa-solid fa-person-dress"></i>Mother Information</div>
                    <div class="row">
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Mother Name <span class="required">*</span></label><asp:TextBox ID="txtMotherName" runat="server" CssClass="form-control" placeholder="Enter Mother Name" onkeypress="return validchar(event);"></asp:TextBox></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Mother Mob. No. <span class="required">*</span></label><asp:TextBox ID="txtMotherMobNo" runat="server" CssClass="form-control" MaxLength="10" onkeypress="return validnum(event);"></asp:TextBox></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Mother Email ID <span class="required">*</span></label><asp:TextBox ID="txtMotherEmail" runat="server" CssClass="form-control" placeholder="Mother Email" onblur="return validateMotherEmail();"></asp:TextBox></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Mother Aadhaar No. <span class="required">*</span></label><asp:TextBox ID="txtMotherAadhaar" runat="server" CssClass="form-control" MaxLength="12" onkeypress="return validnum(event);"></asp:TextBox></div>
                    </div>

                    <div class="section-title"><i class="fa-solid fa-house"></i>Permanent Address</div>
                    <div class="row">
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">City <span class="required">*</span></label><asp:DropDownList ID="ddlPCity" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlPCity_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">State <span class="required">*</span></label><asp:DropDownList ID="ddlPState" runat="server" CssClass="form-select"></asp:DropDownList></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Country <span class="required">*</span></label><asp:DropDownList ID="ddlPCountry" runat="server" CssClass="form-select"></asp:DropDownList></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Address <span class="required">*</span></label><asp:TextBox ID="txtPAddress" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Enter Permanent Address"></asp:TextBox></div>
                         <div class="col-lg-4 col-md-4 col-12 form-group-custom"><label class="form-label-custom">PIN Code <span class="required">*</span></label><asp:TextBox ID="txtPZip" runat="server" CssClass="form-control" MaxLength="6" onkeypress="return validnum(event);" placeholder="Enter PIN Code"></asp:TextBox></div>
                    </div>

                    <div class="section-title"><i class="fa-solid fa-location-dot"></i>Current Address</div>
                    <div class="row">
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">City <span class="required">*</span></label><asp:DropDownList ID="ddlCity" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlPCity_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">State <span class="required">*</span></label><asp:DropDownList ID="ddlState" runat="server" CssClass="form-select"></asp:DropDownList></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Country <span class="required">*</span></label><asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-select"></asp:DropDownList></div>
                        <div class="col-lg-4 col-md-6 col-12 form-group-custom"><label class="form-label-custom">Address <span class="required">*</span></label><asp:TextBox ID="txtCAddress" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Enter Current Address"></asp:TextBox></div>
                         <div class="col-lg-4 col-md-4 col-12 form-group-custom"><label class="form-label-custom">PIN Code <span class="required">*</span></label><asp:TextBox ID="txtCZip" runat="server" CssClass="form-control" MaxLength="6" onkeypress="return validnum(event);" placeholder="Enter PIN Code"></asp:TextBox></div>
                    </div>

                    <div class="section-title"><i class="fa-solid fa-file-arrow-up"></i>Upload Photo / Documents</div>
                    <div class="row">
                        <div class="col-lg-4 col-md-6 col-12"><div class="upload-box" style="position:relative;"><div class="upload-title"><i class="fa-solid fa-camera"></i> Student Photo <span class="required">*</span></div><asp:Image ID="imgPhoto" runat="server" ImageUrl="~/assets/images/User.jpg" style="position:absolute;top:10px;right:10px;width:120px;height:120px;object-fit:cover;border:2px solid #ced4da;border-radius:8px;" /><small class="upload-help" style="position:absolute;left:10px;top:66px;width:calc(100% - 140px);">Maximum Size : 200 KB<br />JPG / JPEG / PNG only</small><asp:Label ID="lblPhotoName" runat="server" ForeColor="Green" Font-Bold="true" style="display:block;margin-top:65px;width:calc(100% - 140px);"></asp:Label><asp:FileUpload ID="filePhoto" runat="server" CssClass="form-control" onchange="PreviewImage(this);" style="position:absolute;left:10px;bottom:10px;width:calc(100% - 150px);" /></div></div>
                         <div class="col-lg-4 col-md-6 col-12"><div class="upload-box"><div class="upload-title"><i class="fa-solid fa-file-pdf"></i> 10th Marksheet <span class="required">*</span></div><div class="text-center mb-3"><i class="fa-solid fa-file-pdf" style="font-size:70px;color:#dc3545;"></i></div><asp:Label ID="lbl10thFileName" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label><small class="upload-help">Upload PDF Only</small><asp:FileUpload ID="fu10thMarksheet" runat="server" CssClass="form-control" /></div></div>
                        <div class="col-lg-4 col-md-6 col-12"><div class="upload-box"><div class="upload-title"><i class="fa-solid fa-signature"></i> Student Sign <span class="required">*</span></div><div class="text-center mb-3"><i class="fa-solid fa-file-signature" style="font-size:70px;color:#243b55;"></i></div><asp:Label ID="lblSignFileName" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label><small class="upload-help">JPG / JPEG / PNG only</small><asp:FileUpload ID="fuStudentSign" runat="server" CssClass="form-control" onchange="previewSign(this);" /></div></div>
                    </div>

                    <div class="row mt-2"><div class="col-12 text-center"><asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-custom btn-submit" OnClick="btnSubmit_Click" OnClientClick="return validateForm();" /><asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-custom btn-reset" CausesValidation="false" OnClick="btnReset_Click" /></div></div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers><asp:PostBackTrigger ControlID="btnSubmit" /></Triggers>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
        <ProgressTemplate><div class="ProgressMsg" style="position:fixed;top:40%;left:45%;background:white;padding:20px 30px;border-radius:30px;box-shadow:0 10px 30px rgba(0,0,0,0.3);z-index:9999;text-align:center;"><img src="../images/wait.gif" alt="Wait" /><br /><br />Please wait...</div></ProgressTemplate>
    </asp:UpdateProgress>

    <cc2:UpdateProgressOverlayExtender ID="UpdateProgressOverlayExtender3" runat="server" TargetControlID="UpdateProgress3" CssClass="updateProgress" OverlayType="Browser" />

</form>
</body>
</html>
 

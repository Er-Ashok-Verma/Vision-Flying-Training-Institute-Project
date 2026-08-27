 <%@ Page Language="C#" AutoEventWireup="true" CodeFile="Link_and_QR_Generate.aspx.cs" Inherits="Flying_Hour_Link_and_QR_Generate" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Student Self Registration - Link & QR</title>  
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css" />  
   <style>
    * {
        margin: 0;
        padding: 0;
        box-sizing: border-box;
    }

    body {
        background: white;
        min-height: 100%;
        font-family: 'Segoe UI', sans-serif;
        padding: 20px;
        width: 100%;
    }

    .main-card {
        background: white;
        box-shadow: 0 10px 35px rgba(0,0,0,0.25);
        border: 1px solid rgba(255,255,255,0.2);
        margin-top: 20px;
        animation: fadeIn 0.8s ease-in-out;
        max-width: 1150px;
        width: 100%;
        border-radius:4px;
    }

    .title {
        background-color: #153d77;
        padding: 4px;
        border-radius: 2px;
        font-size: 24px;
        font-weight: 400;
        color: white;
        margin-bottom: 35px;
        text-shadow: 2px 2px 8px rgba(0,0,0,0.3);
    }

    .title i {
        color: #ffd43b;
        margin-right: 10px;
        font-size: 24px;
        padding-left:8px;
    }

    /* ROW */
    .row {
        display: flex;
        flex-wrap: wrap;
        width: 100%;
        margin: 0;
    }

    /* BOTH SECTION SAME ROW */
    .col-lg-6,
    .col-md-6 {
        width: 50%;
        padding: 0 12px;
    }

    .form-label {
        font-weight: 600;
        color: #1e2a3a;
        margin-bottom: 8px;
        letter-spacing: 0.3px;
        display: block;
    }

    /* LINK SECTION */
    .link-section {
        width: 100%;
    }

    .link-box {
        display: flex;
        width: 100%;
        margin-bottom: 6px;
        align-items: center;
        border-radius: 12px;
        overflow: hidden;
        border: 1px solid #ced4da;
        background: white;
        transition: 0.2s;
    }

    .link-box:focus-within {
        border-color: #243b55;
        box-shadow: 0 0 0 0.2rem rgba(36,59,85,0.2);
    }

    .link-input {
        flex: 1;
        height: 48px;
        padding: 0 18px;
        border: none;
        outline: none;
        font-size: 14px;.copy-icon
        background: transparent;
        color: #1e2a3a;
        min-width: 0;
    }

    /* COPY BUTTON */
    .copy-icon-btn {
        width: 56px;
        min-width: 56px;
        height: 48px;
        border: none;
        background: white;
        cursor: pointer;
        display: flex;
        align-items: center;
        justify-content: center;
        transition: 0.2s;
        border-left: 1px solid #ced4da;
    }


    .copy-icon-btn:active {
        transform: scale(0.94);
    }

    .copy-icon {
        width: 40px;
        height: 40px;
        object-fit: contain;
         
    }

    /* COPY MESSAGE */
    .copy-message {
        display: none;
        width: 100%;
        text-align: center;
        margin-top: 8px;
        padding: 8px 12px;
        background: #28a745;
        color: white;
        border-radius: 40px;
        font-size: 14px;
        font-weight: 600;
        letter-spacing: 0.5px;
    }

    /* QR SECTION */
    .qr-section {
        text-align: center;
        width: 100%;
        margin-top: 0;
        padding-top: 0;
        border-top: none;
    }

    .qr-section .form-label {
        margin-bottom: 18px;
    }

    /* QR IMAGE */
    .qr-section img {
        max-width: 180px;
        height: auto;
        display: block;
        margin: 10px auto 20px auto;
    }

    /* DOWNLOAD BUTTON */
    .download-btn {
        display: inline-block;
        padding: 12px 38px;
        background: linear-gradient(145deg, #28a745, #1e7e34);
        color: white;
        border: none;
        border-radius: 60px;
        font-weight: 600;
        font-size: 16px;
        cursor: pointer;
        transition: 0.2s;
        box-shadow: 0 4px 12px rgba(40,167,69,0.25);
        letter-spacing: 0.5px;
    }

    .download-btn:hover {
        transform: translateY(-2px);
        box-shadow: 0 8px 18px rgba(40,167,69,0.3);
        background: #1e7e34;
    }

    .download-btn:active {
        transform: scale(0.96);
    }

    /* MESSAGE */
    .message {
        display: block;
        text-align: center;
        margin-top: 18px;
        font-weight: 600;
        color: #dc3545;
    }

    /* MOBILE */
    @media (max-width: 767px) {

        .title {
            font-size: 22px;
            padding: 12px 8px;
        }

        .col-lg-6,
        .col-md-6 {
            width: 100%;
            padding: 0;
        }

        .qr-section {
            margin-top: 30px;
            padding-top: 25px;
            border-top: 2px solid #e9ecef;
        }

        .link-input {
            font-size: 13px;
        }

        .copy-icon-btn {
            width: 50px;
            min-width: 50px;
        }

        .download-btn {
            width: 100%;
            padding: 14px;
        }
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

    .fa-regular,
    .fa-solid {
        margin-right: 6px;
    }
</style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="main-card">

        <div class="title">
            <i class="fa-solid fa-link"></i> Student Self Registration Link & QR
        </div>

        <div class="row">

            <!-- Registration Link -->
            <div class="col-lg-6 col-md-6 col-12">
                <div class="link-section">
                    <label class="form-label">
                        <i class="fa-regular fa-copy"></i>
                        Student Registration Form Link
                    </label>

                    <div class="link-box">
                        <asp:TextBox 
                            ID="txtRegistrationLink" 
                            runat="server" 
                            CssClass="link-input" 
                            ReadOnly="true">
                        </asp:TextBox>

                        <button type="button" 
                                class="copy-icon-btn" 
                                onclick="copyLink()" 
                                title="Copy Link">
                            <img src="copy.png" 
                                 alt="Copy" 
                                 class="copy-icon" />
                        </button>
                    </div>

                    <div id="copyMessage" class="copy-message">
                        <i class="fa-regular fa-check-circle"></i>
                        Link Copied!
                    </div>
                </div>
            </div>

            <!-- QR Code -->
            <div class="col-lg-6 col-md-6 col-12">
                <div class="qr-section">
                    <label class="form-label">
                        <i class="fa-regular fa-qrcode"></i>
                        Download Registration QR Code
                    </label>

                    <asp:Image 
                        ID="imgQRCode" 
                        runat="server" 
                        Visible="false" />

                    <asp:Button 
                        ID="btnDownloadQR" 
                        runat="server" 
                        Text="Download QR Code" 
                        CssClass="download-btn" 
                        OnClick="btnDownloadQR_Click" />

                    <asp:Label 
                        ID="lblMessage" 
                        runat="server" 
                        CssClass="message">
                    </asp:Label>
                </div>
            </div>

        </div>
    </div>
</form>

    <script>
        function copyLink() {
            var textbox = document.getElementById('<%= txtRegistrationLink.ClientID %>');
            var link = textbox.value;
            if (!link) return;
            if (navigator.clipboard && window.isSecureContext) {
                navigator.clipboard.writeText(link).then(function () { showCopyMessage(); }).catch(function () { fallbackCopy(link); });
            } else { fallbackCopy(link); }
        }
        function fallbackCopy(text) {
            var textarea = document.createElement("textarea");
            textarea.value = text;
            textarea.style.position = "fixed";
            textarea.style.left = "-999999px";
            textarea.style.top = "0";
            document.body.appendChild(textarea);
            textarea.focus();
            textarea.select();
            try {
                var successful = document.execCommand("copy");
                if (successful) { showCopyMessage(); } else { alert("Unable to copy link."); }
            } catch (err) { alert("Unable to copy link."); }
            document.body.removeChild(textarea);
        }
        function showCopyMessage() {
            var message = document.getElementById("copyMessage");
            message.style.display = "block";
            setTimeout(function () { message.style.display = "none"; }, 1200);
        }
    </script>
</body>
</html>
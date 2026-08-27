using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;

public partial class Flying_Hour_Payment_Entries : System.Web.UI.Page  
{
    DbFunctions objfun = new DbFunctions();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            objfun.FillDropdownlist(wizReg_ddlCourse, "CourseName", "CourseId", "SELECT CourseId, CourseName FROM Course", "---Select---");
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtStudentName.Text.Trim() == "")
            {
                objfun.MsgBox("Please Enter Student Name.!", this);
                txtStudentName.Focus();
                return;
            }

            if (txtFatherName.Text.Trim() == "")
            {
                objfun.MsgBox("Please Enter Father Name.!", this);
                txtFatherName.Focus();
                return;
            }

            if (txtMobileNo.Text.Trim() == "")
            {
                objfun.MsgBox("Please Enter Student Mobile No.!", this);
                txtMobileNo.Focus();
                return;
            }

            if (txtEmail.Text.Trim() == "")
            {
                objfun.MsgBox("Please Enter Student Email.!", this);
                txtEmail.Focus();
                return;
            }

            if (txtAadhaarNo.Text.Trim() == "")
            {
                objfun.MsgBox("Please Enter Aadhaar Number.!", this);
                txtAadhaarNo.Focus();
                return;
            }
            if (wizReg_ddlCourse.SelectedIndex <= 0)
            {
                objfun.MsgBox("Please Select Class.!", this);
                wizReg_ddlCourse.Focus();
                return;
            }

            if (txtAmount.Text.Trim() == "")
            {
                objfun.MsgBox("Please Enter Amount.!", this);
                txtAmount.Focus();
                return;
            }

            decimal amount;

            if (!decimal.TryParse(txtAmount.Text.Trim(), out amount) || amount <= 0)
            {
                objfun.MsgBox("Please Enter Valid Amount.!", this);
                txtAmount.Focus();
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["FeesManagementConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string mobileQuery = "SELECT COUNT(*) FROM Unsettled_Payment_Entries WHERE MobileNo = @MobileNo";

                using (SqlCommand cmdMobile = new SqlCommand(mobileQuery, con))
                {
                    cmdMobile.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text.Trim());

                    int mobileCount = Convert.ToInt32(cmdMobile.ExecuteScalar());

                    if (mobileCount > 0)
                    {
                        objfun.MsgBox("Mobile No. already exists.", this);
                        txtMobileNo.Focus();
                        return;
                    }
                }

                string emailQuery = "SELECT COUNT(*) FROM Unsettled_Payment_Entries WHERE Email_ID = @Email";

                using (SqlCommand cmdEmail = new SqlCommand(emailQuery, con))
                {
                    cmdEmail.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                    int emailCount = Convert.ToInt32(cmdEmail.ExecuteScalar());

                    if (emailCount > 0)
                    {
                        objfun.MsgBox("Email already exists.", this);
                        txtEmail.Focus();
                        return;
                    }
                }

                string aadhar = "SELECT COUNT(*) FROM Unsettled_Payment_Entries WHERE Aadhar_No = @AadharNo";

                using (SqlCommand cmdAadharNo = new SqlCommand(aadhar, con))
                {
                    cmdAadharNo.Parameters.AddWithValue("@AadharNo", txtAadhaarNo.Text.Trim());

                    int aadharCount = Convert.ToInt32(cmdAadharNo.ExecuteScalar());

                    if (aadharCount > 0)
                    {
                        objfun.MsgBox("Aadhaar Number already exists.", this);
                        txtAadhaarNo.Focus();
                        return;
                    }
                }

                string receiptNo = "STU-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                RazorpayHelper razorpay = new RazorpayHelper();
                string response = razorpay.CreateOrder(amount, receiptNo);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> orderData = serializer.Deserialize<Dictionary<string, object>>(response);
                if (orderData == null || !orderData.ContainsKey("id"))
                {
                    objfun.MsgBox("Razorpay order could not be created.", this);
                    return;
                }

                string orderId = orderData["id"].ToString();
                string insertUnsettled = "INSERT INTO Unsettled_Payment_Entries (StudentName, FatherName, MobileNo, Email_ID, Aadhar_No, Course_ID, TransID, PaymentMode, PaymentDate, Amount, Razorpay_OrderID, EntryDateTime, Status) VALUES (@StudentName, @FatherName, @MobileNo, @Email_ID, @Aadhar_No, @Course_ID, NULL, NULL, NULL, @Amount, @Razorpay_OrderID, GETDATE(), 'Unsettled'); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                int unsettledID = 0;

                using (SqlCommand cmd = new SqlCommand(insertUnsettled, con))
                {
                    cmd.Parameters.AddWithValue("@StudentName", txtStudentName.Text.Trim());
                    cmd.Parameters.AddWithValue("@FatherName", txtFatherName.Text.Trim());
                    cmd.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email_ID", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Aadhar_No", txtAadhaarNo.Text.Trim());
                    cmd.Parameters.AddWithValue("@Course_ID", wizReg_ddlCourse.SelectedValue);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@Razorpay_OrderID", orderId);

                    unsettledID = Convert.ToInt32(cmd.ExecuteScalar());
                }

                if (unsettledID <= 0)
                {
                    objfun.MsgBox("Unsettled payment entry could not be saved.", this);
                    return;
                }

                string insertStudentPayment = "INSERT INTO StudentPayment (StudentID, Amount, ReceiptNo, RazorpayOrderID, PaymentStatus, CreatedDate) VALUES (@StudentID, @Amount, @ReceiptNo, @RazorpayOrderID, 'CREATED', GETDATE())";

                using (SqlCommand cmdPayment = new SqlCommand(insertStudentPayment, con))
                {
                    cmdPayment.Parameters.AddWithValue("@StudentID", unsettledID);
                    cmdPayment.Parameters.AddWithValue("@Amount", amount);
                    cmdPayment.Parameters.AddWithValue("@ReceiptNo", receiptNo);
                    cmdPayment.Parameters.AddWithValue("@RazorpayOrderID", orderId);

                    int paymentResult = cmdPayment.ExecuteNonQuery();

                    if (paymentResult <= 0)
                    {
                        objfun.MsgBox("StudentPayment entry could not be saved.", this);
                        return;
                    }
                }

                hfOrderId.Value = orderId;
                hfAmount.Value = (amount * 100).ToString("0");
                hfPublicToken.Value = ConfigurationManager.AppSettings["RazorpayKeyId"];

                string script = @"
setTimeout(function () {

    var options = {

        key: '" + hfPublicToken.Value + @"',

        amount: '" + hfAmount.Value + @"',

        currency: 'INR',

        name: 'Vision Flying Training Institute',

        description: 'Course Registration Payment',

        order_id: '" + hfOrderId.Value + @"',

        handler: function (response) {

            document.getElementById('" + hfPaymentId.ClientID + @"').value =
                response.razorpay_payment_id;

            document.getElementById('" + hfPaymentOrderId.ClientID + @"').value =
                response.razorpay_order_id;

            document.getElementById('" + hfPaymentSignature.ClientID + @"').value =
                response.razorpay_signature;

            document.getElementById('" + btnVerifyPayment.ClientID + @"').click();

        },

        modal: {
            ondismiss: function () {
                console.log('Razorpay Checkout Closed');
            }
        },

        theme: {
            color: '#7C3AED'
        }
    };

    var rzp1 = new Razorpay(options);

    rzp1.open();

}, 100);";

                ScriptManager.RegisterStartupScript(this, GetType(), "RazorpayCheckout", script, true);
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message.Replace("'", "\\'");

            ScriptManager.RegisterStartupScript(this, GetType(), "RazorpayError", "alert('Razorpay Error: " + message + "');", true);
        }
    }

    protected void btnVerifyPayment_Click(object sender, EventArgs e)
    {
        try
        {
            string paymentId = hfPaymentId.Value.Trim();
            string orderId = hfPaymentOrderId.Value.Trim();
            string razorpaySignature = hfPaymentSignature.Value.Trim();

            if (string.IsNullOrEmpty(paymentId) || string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(razorpaySignature))
            {
                objfun.MsgBox("Payment information is incomplete.", this);
                return;
            }

            string secret = ConfigurationManager.AppSettings["RazorpayKeySecret"];

            if (string.IsNullOrEmpty(secret))
            {
                objfun.MsgBox("Razorpay Secret Key is not configured.", this);
                return;
            }

            string generatedSignature = GenerateRazorpaySignature(orderId, paymentId, secret);

            if (!string.Equals(generatedSignature, razorpaySignature, StringComparison.OrdinalIgnoreCase))
            {
                objfun.MsgBox("Payment verification failed. Invalid signature.", this);
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["FeesManagementConn"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string updateStudentPayment = "UPDATE StudentPayment SET RazorpayPaymentID = @PaymentID, RazorpaySignature = @Signature, PaymentStatus = 'SUCCESS', PaymentMethod = 'Razorpay', PaymentDate = GETDATE(), Remarks = 'Payment verified successfully' WHERE RazorpayOrderID = @OrderID AND PaymentStatus = 'CREATED'";

                using (SqlCommand cmd = new SqlCommand(updateStudentPayment, con))
                {
                    cmd.Parameters.AddWithValue("@PaymentID", paymentId);
                    cmd.Parameters.AddWithValue("@Signature", razorpaySignature);
                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    int result = cmd.ExecuteNonQuery();

                    if (result <= 0)
                    {
                        objfun.MsgBox("StudentPayment record could not be updated.", this);
                        return;
                    }
                }

                string updateUnsettled = "UPDATE Unsettled_Payment_Entries SET TransID = @TransID, PaymentMode = 'Razorpay', PaymentDate = GETDATE(), Status = 'Unsettled' WHERE Razorpay_OrderID = @OrderID AND Status = 'Unsettled'";

                using (SqlCommand cmd = new SqlCommand(updateUnsettled, con))
                {
                    cmd.Parameters.AddWithValue("@TransID", paymentId);
                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    int result = cmd.ExecuteNonQuery();

                    if (result <= 0)
                    {
                        objfun.MsgBox("Unsettled payment record could not be updated.", this);
                        return;
                    }
                }
            }

            Response.Redirect("Entries_Receipt.aspx?orderId=" + Server.UrlEncode(orderId), false);
            Context.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            objfun.MsgBox("Payment verification error: " + ex.Message, this);
        }
    }

    private string GenerateRazorpaySignature(string orderId, string paymentId, string secret)
    {
        string payload = orderId + "|" + paymentId;

        byte[] keyBytes = Encoding.UTF8.GetBytes(secret);
        byte[] messageBytes = Encoding.UTF8.GetBytes(payload);

        using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
        {
            byte[] hash = hmac.ComputeHash(messageBytes);

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("x2"));
            }

            return result.ToString();
        }
    }

    public void Reset()
    {
        txtStudentName.Text = "";
        txtFatherName.Text = "";
        txtMobileNo.Text = "";
        txtEmail.Text = "";
        txtAadhaarNo.Text = "";
        txtAmount.Text = "";

        if (wizReg_ddlCourse.Items.Count > 0)
        {
            wizReg_ddlCourse.SelectedIndex = 0;
        }

        hfOrderId.Value = "";
        hfAmount.Value = "";
        hfPublicToken.Value = "";
        hfPaymentId.Value = "";
        hfPaymentOrderId.Value = "";
        hfPaymentSignature.Value = "";

        txtStudentName.Focus();
    }
}
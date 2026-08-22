using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;



public partial class Flying_Hour_Student_Payment : System.Web.UI.Page
{
    DbFunctions objfun = new DbFunctions();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void txtRegNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string regNo = txtRegNo.Text.Trim();

            if (string.IsNullOrWhiteSpace(regNo))
            {
                div.Visible = false;
                lblStuName.Text = "";
                txt_course_name.Text = "";
                return;
            }

            string instituteID = Convert.ToString(115);

            string studentName = objfun.Get_details("SELECT StudentName FROM StudentReg WHERE RegNo='" + regNo.Replace("'", "''") + "' AND InstituteID='" + instituteID.Replace("'", "''") + "'");

            if (string.IsNullOrWhiteSpace(studentName))
            {
                objfun.MsgBox("Student not found.", this);
                lblStuName.Text = "";
                txt_course_name.Text = "";
                div.Visible = false;
                return;
            }

            string[] nameParts = studentName.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder maskedName = new StringBuilder();

            foreach (string name in nameParts)
            {
                if (name.Length <= 2)
                {
                    maskedName.Append(name);
                }
                else
                {
                    maskedName.Append(name.Substring(0, 1));
                    maskedName.Append(new string('*', name.Length - 2));
                    maskedName.Append(name.Substring(name.Length - 1, 1));
                }

                maskedName.Append(" ");
            }

            lblStuName.Text = maskedName.ToString().Trim();

            txt_course_name.Text = objfun.Get_details("SELECT DISTINCT Course.CourseName FROM StudentStatus INNER JOIN Course ON StudentStatus.CourseID=Course.CourseId INNER JOIN SchoolMaster ON Course.School_ID=SchoolMaster.ID WHERE StudentStatus.Status IN ('C','ENQ') AND StudentStatus.StudentID=(SELECT StudentID FROM StudentReg WHERE RegNo='" + regNo.Replace("'", "''") + "' AND InstituteID='" + instituteID.Replace("'", "''") + "') AND StudentStatus.InstituteID='" + instituteID.Replace("'", "''") + "'");

            txt_course_name.Text = txt_course_name.Text.Trim();

            div.Visible = true;
        }
        catch (Exception ex)
        {
            objfun.MsgBox(ex.Message, this);
        }
    }

   

    protected void btnVerifyPayment_Click(object sender, EventArgs e)
    {
        try
        {
            string paymentId = hfPaymentId.Value.Trim();
            string orderId = hfPaymentOrderId.Value.Trim();
            string razorpaySignature = hfPaymentSignature.Value.Trim();

            if (string.IsNullOrWhiteSpace(paymentId) || string.IsNullOrWhiteSpace(orderId) || string.IsNullOrWhiteSpace(razorpaySignature))
            {
                objfun.MsgBox("Payment information is incomplete.", this);
                return;
            }

            string secret = System.Configuration.ConfigurationManager.AppSettings["RazorpayKeySecret"];

            if (string.IsNullOrWhiteSpace(secret))
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

            string paymentAmountText = objfun.Get_details("SELECT TOP 1 Amount FROM StudentPayment WHERE RazorpayOrderID='" + orderId.Replace("'", "''") + "' AND PaymentStatus='CREATED' ORDER BY ID DESC");

            decimal paymentAmount;

            if (!decimal.TryParse(paymentAmountText, out paymentAmount) || paymentAmount <= 0)
            {
                objfun.MsgBox("Payment amount not found.", this);
                return;
            }

            string studentIDText = objfun.Get_details("SELECT TOP 1 StudentID FROM StudentPayment WHERE RazorpayOrderID='" + orderId.Replace("'", "''") + "' AND PaymentStatus='CREATED' ORDER BY ID DESC");

            int studentID;

            if (!int.TryParse(studentIDText, out studentID) || studentID <= 0)
            {
                objfun.MsgBox("Student ID not found.", this);
                return;
            }

            string voucherNo = objfun.Get_details("SELECT TOP 1 VoucherNo FROM StudentRegDetail WHERE StudentID=" + studentID + " AND VoucherNo IS NOT NULL AND LTRIM(RTRIM(VoucherNo))<>'' ORDER BY StudentDetailID DESC");

            if (string.IsNullOrWhiteSpace(voucherNo))
            {
                objfun.MsgBox("Voucher No. not found for this student.", this);
                return;
            }

            string updatePayment = "UPDATE StudentPayment SET RazorpayPaymentID='" + paymentId.Replace("'", "''") + "', RazorpaySignature='" + razorpaySignature.Replace("'", "''") + "', PaymentStatus='SUCCESS', PaymentMethod='Razorpay', PaymentDate=GETDATE(), Remarks='Payment verified successfully' WHERE RazorpayOrderID='" + orderId.Replace("'", "''") + "' AND PaymentStatus='CREATED'";

            int paymentUpdateResult = objfun.ExecuteDML(updatePayment);

            if (paymentUpdateResult <= 0)
            {
                objfun.MsgBox("Payment verified but StudentPayment could not be updated.", this);
                return;
            }

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["FeesManagementConn"].ConnectionString;

            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Sp_OnlineFeeSubmission", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PaymentID", paymentId);
                    cmd.Parameters.AddWithValue("@StudentID", studentID);
                    cmd.Parameters.AddWithValue("@VoucherNo", voucherNo);
                    cmd.Parameters.AddWithValue("@TaxnAmount", paymentAmount);

                    con.Open();
                    cmd.ExecuteNonQuery(); 
                }
            }

            Response.Redirect("PaymentReceipt.aspx?orderId=" + Server.UrlEncode(orderId), false);
            Context.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            objfun.MsgBox("Payment verification error: " + ex.Message, this);
        }
    }

    protected void btnPayment_Click(object sender, EventArgs e)
    {
        try
        {
            string regNo = txtRegNo.Text.Trim();

            if (string.IsNullOrEmpty(regNo))
            {
                objfun.MsgBox("Please enter Registration No.", this);
                return;
            }

            string instituteID = Convert.ToString(115);

            string studentIDText = objfun.Get_details(
                "SELECT StudentID FROM StudentReg WHERE RegNo='" +
                regNo.Replace("'", "''") +
                "' AND InstituteID='" +
                instituteID.Replace("'", "''") +
                "'"
            );

            if (string.IsNullOrEmpty(studentIDText) ||
                studentIDText == "0")
            {
                objfun.MsgBox(
                    "Student not found for this Registration No.",
                    this
                );
                return;
            }

            int RegistrationID;

            if (!int.TryParse(studentIDText, out RegistrationID) ||
                RegistrationID <= 0)
            {
                objfun.MsgBox("Invalid Student ID.", this);
                return;
            }

            decimal amount;

            if (!decimal.TryParse(txtAmount.Text.Trim(), out amount))
            {
                objfun.MsgBox("Please enter a valid amount.", this);
                return;
            }

            if (amount <= 0)
            {
                objfun.MsgBox("Please enter a valid amount.", this);
                return;
            }

            string receiptNo =
                "STU-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            RazorpayHelper razorpay = new RazorpayHelper();

            string response =
                razorpay.CreateOrder(amount, receiptNo);

            JavaScriptSerializer serializer =
                new JavaScriptSerializer();

            Dictionary<string, object> orderData =
                serializer.Deserialize<Dictionary<string, object>>(response);

            if (orderData == null || !orderData.ContainsKey("id"))
            {
                objfun.MsgBox(
                    "Razorpay Order ID was not generated.",
                    this
                );
                return;
            }

            string orderId = orderData["id"].ToString();

            string insertPayment = @"INSERT INTO StudentPayment
(
    StudentID,
    Amount,
    ReceiptNo,
    RazorpayOrderID,
    PaymentStatus,
    CreatedDate
)
VALUES
(
    " + RegistrationID + @",
    " + amount.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",
    '" + receiptNo.Replace("'", "''") + @"',
    '" + orderId.Replace("'", "''") + @"',
    'CREATED',
    GETDATE()
)";

            int paymentResult =
                objfun.ExecuteDML(insertPayment);

            if (paymentResult <= 0)
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "PaymentError",
                    "alert('Payment order could not be saved. Please try again.');",
                    true
                );
                return;
            }

            hfOrderId.Value = orderId;

            hfAmount.Value =
                (amount * 100).ToString(
                    System.Globalization.CultureInfo.InvariantCulture
                );

            hfPublicToken.Value =
                System.Configuration.ConfigurationManager
                .AppSettings["RazorpayKeyId"];

            string script = @"
setTimeout(function () {

    var options = {
        key: '" + HttpUtility.JavaScriptStringEncode(hfPublicToken.Value) + @"',
        amount: '" + hfAmount.Value + @"',
        currency: 'INR',
        name: 'Vision Flying Training Institute',
        description: 'Course Registration Payment',
        order_id: '" + HttpUtility.JavaScriptStringEncode(hfOrderId.Value) + @"',

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

}, 100);
";

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "RazorpayCheckout",
                script,
                true
            );
        }
        catch (Exception ex)
        {
            string errorMessage =
                HttpUtility.JavaScriptStringEncode(ex.Message);

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "RazorpayError",
                "alert('Razorpay Error: " + errorMessage + "');",
                true
            );
        }
    }

    private string GenerateRazorpaySignature(string orderId,string paymentId,string secret)
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
}
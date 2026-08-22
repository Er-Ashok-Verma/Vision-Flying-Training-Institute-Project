using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Web;

public partial class Flying_Hour_PaymentReceipt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadReceipt();
        }
    }
    private void LoadReceipt()
    {
        try
        {
            string orderId = Convert.ToString(Request.QueryString["orderId"]);

            if (string.IsNullOrWhiteSpace(orderId))
            {
                Response.Write("Order ID not received.");
                return;
            }

            orderId = orderId.Trim();

            string connectionString = ConfigurationManager.ConnectionStrings["FeesManagementConn"].ConnectionString;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Response.Write("Database connection string not found.");
                return;
            }

            string sql = "SELECT TOP 1 SP.ReceiptNo, SP.StudentID, SP.Amount, SP.RazorpayPaymentID, SP.PaymentStatus, SP.PaymentDate, SR.RegNo, SR.StudentName, (SELECT TOP 1 C.CourseName FROM StudentStatus SS INNER JOIN Course C ON SS.CourseID=C.CourseId WHERE SS.Status IN ('C','ENQ') AND SS.StudentID=SR.StudentID AND SS.InstituteID=115) AS CourseName FROM StudentPayment SP LEFT JOIN StudentReg SR ON SP.StudentID=SR.StudentID WHERE SP.RazorpayOrderID=@OrderId AND SP.PaymentStatus='SUCCESS' ORDER BY SP.PaymentDate DESC";

            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                Response.Write("Payment receipt not found.");
                return;
            }

            DataRow dr = dt.Rows[0];

            string receiptNo = Convert.ToString(dr["ReceiptNo"]);
            string razorpayPaymentId = Convert.ToString(dr["RazorpayPaymentID"]);
            string paymentStatus = Convert.ToString(dr["PaymentStatus"]);
            string paymentDateText = Convert.ToString(dr["PaymentDate"]);
            string regNo = Convert.ToString(dr["RegNo"]);
            string studentName = Convert.ToString(dr["StudentName"]);
            string courseName = Convert.ToString(dr["CourseName"]);

            if (string.IsNullOrWhiteSpace(studentName))
            {
                studentName = "Student";
            }

            if (string.IsNullOrWhiteSpace(courseName))
            {
                courseName = "Course Not Available";
            }

            decimal amount = 0;

            if (dr["Amount"] != DBNull.Value)
            {
                decimal.TryParse(
                    Convert.ToString(dr["Amount"]),
                    out amount
                );
            }

            lblInstituteName.Text = "Vision Flying Training Institute";

            lblStudentName.Text = studentName.Trim();

            lblRegNo.Text = regNo.Trim();

            lblCourseName.Text = courseName.Trim();

            lblReceiptNo.Text = receiptNo.Trim();

            lblPaymentId.Text = razorpayPaymentId.Trim();

            lblStatus.Text = paymentStatus.Trim();

            lblAmount.Text = amount.ToString("N2");

            lblPaidAmount.Text = amount.ToString("N2");

            DateTime paymentDate;

            if (DateTime.TryParse(paymentDateText, out paymentDate))
            {
                lblPaymentDate.Text =
                    paymentDate.ToString("hh:mm tt") +
                    " on " +
                    paymentDate.ToString("dd MMM yyyy");
            }
            else
            {
                lblPaymentDate.Text = "";
            }
        }
        catch (Exception ex)
        {
            Response.Write(
                "Receipt Error: " +
                HttpUtility.HtmlEncode(ex.ToString())
            );
        }
    }

    private string MaskStudentName(string studentName)
    {
        if (string.IsNullOrWhiteSpace(studentName))
        {
            return "";
        }

        string[] nameParts = studentName.Trim().Split(
            new char[] { ' ' },
            StringSplitOptions.RemoveEmptyEntries
        );

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

        return maskedName.ToString().Trim();
    }
}
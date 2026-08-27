using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

public partial class Flying_Hour_Entries_Receipt : System.Web.UI.Page
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
            string orderId = Convert.ToString(Request.QueryString["orderId"]).Trim();

            if (string.IsNullOrEmpty(orderId))
            {
                Response.Write("Invalid Payment Order ID.");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["FeesManagementConn"].ConnectionString;

            string query = "SELECT TOP (1) Unsettled_Payment_Entries.StudentName, Unsettled_Payment_Entries.FatherName, Unsettled_Payment_Entries.Amount, StudentPayment.ReceiptNo, Course.CourseName, StudentPayment.PaymentMethod, StudentPayment.RazorpayPaymentID, StudentPayment.PaymentDate, StudentPayment.RazorpayOrderID, StudentPayment.PaymentStatus FROM Unsettled_Payment_Entries INNER JOIN StudentPayment ON Unsettled_Payment_Entries.ID = StudentPayment.StudentID INNER JOIN Course ON Unsettled_Payment_Entries.Course_ID = Course.CourseId WHERE Unsettled_Payment_Entries.Razorpay_OrderID = @OrderID";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblInstituteName.Text = "VISION FLYING TRAINING INSTITUTE";
                            lblReceiptNo.Text = Convert.ToString(dr["ReceiptNo"]);
                            lblStudentName.Text = Convert.ToString(dr["StudentName"]);
                            lblFatherName.Text = Convert.ToString(dr["FatherName"]);
                            lblCourseName.Text = Convert.ToString(dr["CourseName"]);
                            lblPaymentMode.Text = Convert.ToString(dr["PaymentMethod"]);
                            lblPaymentId.Text = Convert.ToString(dr["RazorpayPaymentID"]);
                            lblBankName.Text = Convert.ToString(dr["PaymentMethod"]);
                            lblStatus.Text = Convert.ToString(dr["PaymentStatus"]);
                            decimal amount = 0;

                            if (dr["Amount"] != DBNull.Value)
                            {
                                amount = Convert.ToDecimal(dr["Amount"]);
                            }

                            lblAmount.Text = amount.ToString("N2");

                            lblPaidAmount.Text = amount.ToString("N2");

                            if (dr["PaymentDate"] != DBNull.Value)
                            {
                                DateTime paymentDate = Convert.ToDateTime(dr["PaymentDate"]);
                                lblPaymentDate.Text = paymentDate.ToString("dd MMM yyyy");
                                lblDate.Text = paymentDate.ToString("dd MMM yyyy");
                            }
                            else
                            {
                                lblPaymentDate.Text = "-";

                                lblDate.Text = "-";
                            }

                            lblFeeHeadName.Text = Convert.ToString(dr["CourseName"]);
                            lblAmountWords.Text = NumberToWords(amount) + " Only";
                        }
                        else
                        {
                            Response.Write("Payment receipt not found.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Error: " + ex.Message);
        }
    }

    private string NumberToWords(decimal number)
    {
        long rupees = Convert.ToInt64(Math.Floor(number));

        int paise =
            Convert.ToInt32(
                Math.Round(
                    (number - rupees) * 100,
                    MidpointRounding.AwayFromZero));

        string result = ConvertNumberToWords(rupees) + " Rupees";

        if (paise > 0)
        {
            result += " and " +
                      ConvertNumberToWords(paise) +
                      " Paise";
        }

        return result;
    }

    private string ConvertNumberToWords(long number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " +
                   ConvertNumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 10000000) > 0)
        {
            words +=
                ConvertNumberToWords(number / 10000000) +
                " Crore ";

            number %= 10000000;
        }

        if ((number / 100000) > 0)
        {
            words +=
                ConvertNumberToWords(number / 100000) +
                " Lakh ";

            number %= 100000;
        }

        if ((number / 1000) > 0)
        {
            words +=
                ConvertNumberToWords(number / 1000) +
                " Thousand ";

            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words +=
                ConvertNumberToWords(number / 100) +
                " Hundred ";

            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            if (number < 20)
            {
                words +=
                    UnitsMap[number];
            }
            else
            {
                words +=
                    TensMap[number / 10];

                if ((number % 10) > 0)
                {
                    words += " " +
                             UnitsMap[number % 10];
                }
            }
        }

        return words.Trim();
    }

    private static readonly string[] UnitsMap =
    {
        "Zero",
        "One",
        "Two",
        "Three",
        "Four",
        "Five",
        "Six",
        "Seven",
        "Eight",
        "Nine",
        "Ten",
        "Eleven",
        "Twelve",
        "Thirteen",
        "Fourteen",
        "Fifteen",
        "Sixteen",
        "Seventeen",
        "Eighteen",
        "Nineteen"
    };

    private static readonly string[] TensMap =
    {
        "",
        "",
        "Twenty",
        "Thirty",
        "Forty",
        "Fifty",
        "Sixty",
        "Seventy",
        "Eighty",
        "Ninety"
    };
}

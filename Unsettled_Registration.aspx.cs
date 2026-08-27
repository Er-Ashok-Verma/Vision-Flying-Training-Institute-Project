using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Flying_Hour_Unsettled_Registration : System.Web.UI.Page
{
    DbFunctions objFunc = new DbFunctions();
    string connectionString = ConfigurationManager.ConnectionStrings["FeesManagementConn"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fillgrid();
        }

        if (gridview.Rows.Count > 0)
        {
            gridview.UseAccessibleHeader = true;
            gridview.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    public void fillgrid()
    {
        try
        {
            string query = "SELECT U.ID,U.StudentName,U.FatherName,U.MobileNo,U.Email_ID,U.Amount,CONVERT(VARCHAR(11),U.EntryDateTime,106) AS EntryDate,C.CourseName,U.TransID,U.Aadhar_No,U.Course_ID,U.PaymentMode,U.PaymentDate,U.Razorpay_OrderID,U.EntryDateTime,U.Status FROM Unsettled_Payment_Entries U INNER JOIN Course C ON U.Course_ID=C.CourseId WHERE ISNULL(U.Status,'')<>'Settled' ORDER BY U.EntryDateTime DESC";

            DataTable dt = objFunc.FillDataTable(query);

            gridview.DataSource = dt;
            gridview.DataBind();
        }
        catch (Exception ex)
        {
            objFunc.MsgBox("Error: " + ex.Message, this);
        }
    }

    protected void lnkFillData_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = sender as LinkButton;

            if (btn == null)
            {
                objFunc.MsgBox("Invalid payment button!", this);
                return;
            }

            int unsettledID;

            if (!int.TryParse(btn.CommandArgument.Trim(), out unsettledID))
            {
                objFunc.MsgBox("Invalid payment ID!", this);
                return;
            }

            string query = "SELECT U.ID,ISNULL(U.TransID,'') AS TransID,ISNULL(U.Amount,0) AS Amount,ISNULL(U.Status,'') AS Status,ISNULL(U.StudentName,'') AS StudentName,ISNULL(U.FatherName,'') AS FatherName,C.CourseName FROM Unsettled_Payment_Entries U INNER JOIN Course C ON U.Course_ID=C.CourseId WHERE U.ID=@ID";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = unsettledID;

                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                    {
                        objFunc.MsgBox("Payment record not found!", this);
                        return;
                    }

                    string status = Convert.ToString(dr["Status"]);

                    if (status.Equals("Settled", StringComparison.OrdinalIgnoreCase))
                    {
                        objFunc.MsgBox("This payment is already Settled!", this);
                        return;
                    }

                    hdnUnsettledID.Value = Convert.ToString(dr["ID"]);
                     
                    lblTransID.Text = Convert.ToString(dr["TransID"]);
                    lblAmount.Text = Convert.ToDecimal(dr["Amount"]).ToString("0.00");
                    txtRegNo.Text = "";

                    ShowVerifyModal();
                }
            }
        }
        catch (Exception ex)
        {
            objFunc.MsgBox("Error: " + ex.Message, this);
        }
    }

    protected void btnVerifyPayment_Click(object sender, EventArgs e)
    {
        string regNo = txtRegNo.Text.Trim();

        if (string.IsNullOrEmpty(regNo))
        {
            objFunc.MsgBox("Please enter Registration No!", this);
            ShowVerifyModal();
            return;
        }

        int unsettledID;

        if (!int.TryParse(hdnUnsettledID.Value, out unsettledID))
        {
            objFunc.MsgBox("Invalid payment record!", this);
            return;
        }

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            using (SqlTransaction transaction = con.BeginTransaction())
            {
                try
                {
                    int studentID = 0;
                    decimal amount = 0;
                    string paymentID = "";
                    string status = "";
                    string voucherNo = "";

                    string studentQuery = "SELECT TOP 1 StudentID FROM StudentReg WHERE LTRIM(RTRIM(RegNo))=@RegNo ORDER BY StudentID DESC";

                    using (SqlCommand cmdStudent = new SqlCommand(studentQuery, con, transaction))
                    {
                        cmdStudent.Parameters.Add("@RegNo", SqlDbType.VarChar, 100).Value = regNo;

                        object result = cmdStudent.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                        {
                            transaction.Rollback();
                            objFunc.MsgBox("Registration No not found in StudentReg!", this);
                            ShowVerifyModal();
                            return;
                        }

                        studentID = Convert.ToInt32(result);
                    }

                    string paymentQuery = "SELECT ISNULL(Amount,0) AS Amount,ISNULL(TransID,'') AS TransID,ISNULL(Status,'') AS Status FROM Unsettled_Payment_Entries WHERE ID=@ID";

                    using (SqlCommand cmdPayment = new SqlCommand(paymentQuery, con, transaction))
                    {
                        cmdPayment.Parameters.Add("@ID", SqlDbType.Int).Value = unsettledID;

                        using (SqlDataReader dr = cmdPayment.ExecuteReader())
                        {
                            if (!dr.Read())
                            {
                                transaction.Rollback();
                                objFunc.MsgBox("Unsettled payment record not found!", this);
                                ShowVerifyModal();
                                return;
                            }

                            amount = Convert.ToDecimal(dr["Amount"]);
                            paymentID = Convert.ToString(dr["TransID"]).Trim();
                            status = Convert.ToString(dr["Status"]);
                        }
                    }

                    if (status.Equals("Settled", StringComparison.OrdinalIgnoreCase))
                    {
                        transaction.Rollback();
                        objFunc.MsgBox("This payment is already Settled!", this);
                        return;
                    }

                    if (amount <= 0)
                    {
                        transaction.Rollback();
                        objFunc.MsgBox("Invalid payment amount!", this);
                        ShowVerifyModal();
                        return;
                    }

                    if (string.IsNullOrEmpty(paymentID))
                    {
                        transaction.Rollback();
                        objFunc.MsgBox("Razorpay Payment ID is missing!", this);
                        ShowVerifyModal();
                        return;
                    }

                    string voucherQuery = "SELECT TOP 1 ISNULL(VoucherNo,'') AS VoucherNo FROM StudentRegDetail WHERE StudentID=@StudentID AND ISNULL(BalanceAmount,0)>0 AND ISNULL(VoucherNo,'')<>'' ORDER BY StudentDetailID DESC";

                    using (SqlCommand cmdVoucher = new SqlCommand(voucherQuery, con, transaction))
                    {
                        cmdVoucher.Parameters.Add("@StudentID", SqlDbType.Int).Value = studentID;

                        object result = cmdVoucher.ExecuteScalar();

                        if (result == null || result == DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(result)))
                        {
                            transaction.Rollback();
                            objFunc.MsgBox("Voucher No not found for this student!", this);
                            ShowVerifyModal();
                            return;
                        }

                        voucherNo = Convert.ToString(result).Trim();
                    }

                    using (SqlCommand cmdSP = new SqlCommand("Sp_OnlineFeeSubmission", con, transaction))
                    {
                        cmdSP.CommandType = CommandType.StoredProcedure;
                        cmdSP.CommandTimeout = 120;

                        cmdSP.Parameters.Add("@PaymentID", SqlDbType.NVarChar, -1).Value = paymentID;
                        cmdSP.Parameters.Add("@StudentID", SqlDbType.Int).Value = studentID;
                        cmdSP.Parameters.Add("@VoucherNo", SqlDbType.NVarChar, -1).Value = voucherNo;

                        SqlParameter amountParameter = cmdSP.Parameters.Add("@TaxnAmount", SqlDbType.Decimal);
                        amountParameter.Precision = 18;
                        amountParameter.Scale = 0;
                        amountParameter.Value = Math.Round(amount, 0);

                        cmdSP.ExecuteNonQuery();
                    }

                    string updateUnsettledQuery = "UPDATE Unsettled_Payment_Entries SET Status='Settled' WHERE ID=@ID AND ISNULL(Status,'')<>'Settled'";

                    using (SqlCommand cmdUpdateUnsettled = new SqlCommand(updateUnsettledQuery, con, transaction))
                    {
                        cmdUpdateUnsettled.Parameters.Add("@ID", SqlDbType.Int).Value = unsettledID;

                        if (cmdUpdateUnsettled.ExecuteNonQuery() == 0)
                        {
                            transaction.Rollback();
                            objFunc.MsgBox("Payment status update failed!", this);
                            return;
                        }
                    }

                    transaction.Commit();

                    hdnUnsettledID.Value = "";
                    lblStudentName.Text = "";
                    lblFatherName.Text = "";
                    lblCourseName.Text = "";
                    lblTransID.Text = "";
                    lblAmount.Text = "";
                    txtRegNo.Text = "";

                    fillgrid();

                    objFunc.MsgBox("Payment verified and settled successfully!", this);
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                    }

                    objFunc.MsgBox("Error while verifying payment: " + ex.Message, this);
                    ShowVerifyModal();
                }
            }
        }
    }

    private void ShowVerifyModal()
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "ShowVerifyModal", "setTimeout(function(){ $('#verifyModal').modal('show'); },100);", true);
    }

    protected void txtRegNo_TextChanged(object sender, EventArgs e)
    {
        string regNo = txtRegNo.Text.Trim();

        if (string.IsNullOrEmpty(regNo))
        {
            return;
        }

        string query = "SELECT StudentReg.StudentID, StudentReg.StudentName, StudentReg.FatherName, StudentReg.RegNo, Course.CourseName FROM StudentReg INNER JOIN Course  ON StudentReg.CourseID = Course.CourseId WHERE StudentReg.RegNo = @RegNo";

        using (SqlConnection con = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@RegNo", regNo);

            con.Open();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    hdnUnsettledID.Value = Convert.ToString(dr["StudentID"]);

                    lblStudentName.Text = Convert.ToString(dr["StudentName"]);
                    lblFatherName.Text = Convert.ToString(dr["FatherName"]);
                    lblCourseName.Text = Convert.ToString(dr["CourseName"]);
                    div1.Style.Add("display", "block");

                    ShowVerifyModal();
                }
                else
                {
                    hdnUnsettledID.Value = "";

                    lblStudentName.Text = "";
                    lblFatherName.Text = "";
                    lblCourseName.Text = "";

                    objFunc.MsgBox("Reg No not found.", this);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Flying_Hour_EligibleHours : System.Web.UI.Page
{
    DbFunctions objfun = new DbFunctions();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UID"] == null)
        {
            Response.Redirect("../error_404_2.html");
        }

        if (Session["InstID"].ToString() == null)
        {
            Response.Redirect("../error_404_2.html");
        }

        if (Session["SesnID"].ToString() == null)
        {
            Response.Redirect("../error_404_2.html");
        }

        if (!IsPostBack)
        {
            objfun.FillActivityLog(Convert.ToInt32(Session["UID"]), "Eligible Hours", "Visit Eligible Hours Page", Convert.ToInt32(Session["InstID"]));

            DataFill();
           fillgrid();

        }
    }

    public void DataFill()
    {
        DataTable Header = new DataTable();

        Header = objfun.FillDataTable("SELECT DISTINCT SchoolMaster.SchoolName, Course.CourseName, StudentStatus.StudentID, StudentReg.StudentName, StudentReg.FatherName, StudentStatus.SessionID, StudentReg.RegNo FROM Course INNER JOIN StudentStatus INNER JOIN StudentReg ON StudentStatus.StudentID = StudentReg.StudentID ON Course.CourseId = StudentStatus.CourseID INNER JOIN SchoolMaster ON Course.School_ID = SchoolMaster.ID where (StudentStatus.Status='C') AND (StudentStatus.StudentID = '" + Session["UID"] + "')");
        if (Header.Rows.Count > 0)
        {

           // lblSchoolName.Text = Header.Rows[0]["SchoolName"].ToString().ToUpper();
          //  lblSession.Text = Header.Rows[0]["SessionID"].ToString().ToUpper();
            lblRegNumber.Text = Header.Rows[0]["RegNo"].ToString().ToUpper();
            lblStudentName.Text = Header.Rows[0]["StudentName"].ToString().ToUpper();
            lblFatherName.Text = Header.Rows[0]["FatherName"].ToString().ToUpper();
            lblCourseName.Text = Header.Rows[0]["CourseName"].ToString().ToUpper();

        }
    }

    //public void fillgrid()
    //{
    //    DataTable dt = new DataTable();

    //    dt = objfun.FillDataTable("SELECT StudentRegDetail.StudentDetailID, StudentRegDetail.StudentID, FeeHead.FeeHeadName, StudentRegDetail.FeeID, StudentRegDetail.TotalAmount, StudentRegDetail.PaidAmount, StudentRegDetail.BalanceAmount, StudentRegDetail.InstituteID, StudentRegDetail.VoucherNo, StudentRegDetail.VNo, StudentWise_FlyingHrs.Flying_Hrs, CASE WHEN StudentWise_FlyingHrs.StudentID IS NULL THEN 0 ELSE 1 END AS IsUpdated, Employee_Master.empName + ' ' + Employee_Master.MiddelName + ' ' + Employee_Master.lastName AS EntryName, CONVERT(VARCHAR, StudentWise_FlyingHrs.EntryDate, 106) AS EntryDate FROM Employee_Master INNER JOIN StudentWise_FlyingHrs ON Employee_Master.empId = StudentWise_FlyingHrs.EntryBy RIGHT OUTER JOIN StudentRegDetail INNER JOIN FeeHead ON StudentRegDetail.FeeID = FeeHead.FeeHeadId ON StudentWise_FlyingHrs.StudentID = StudentRegDetail.StudentID AND StudentWise_FlyingHrs.VoucherNo = StudentRegDetail.VoucherNo AND StudentWise_FlyingHrs.FeeHeadID = StudentRegDetail.FeeID AND StudentWise_FlyingHrs.InstID = StudentRegDetail.InstituteID  WHERE StudentRegDetail.StudentID='" + Session["UID"] + "'  AND (StudentRegDetail.BalanceAmount = 0) AND (StudentWise_FlyingHrs.Flying_Hrs IS NOT NULL)");

    //    if (dt.Rows.Count > 0)
    //    {
    //        gridview.DataSource = dt;
    //        gridview.DataBind();
    //    }
    //    else
    //    {
    //        gridview.DataSource = null;
    //        gridview.DataBind();
    //        objfun.MsgBox("Record Not Found", this);
    //    }
    //}
    public void fillgrid()
    {
        DataTable dt = new DataTable();

        dt = objfun.FillDataTable("SELECT StudentRegDetail.StudentDetailID, StudentRegDetail.StudentID, FeeHead.FeeHeadName, StudentRegDetail.FeeID, StudentRegDetail.TotalAmount, StudentRegDetail.PaidAmount, StudentRegDetail.BalanceAmount, StudentRegDetail.InstituteID, StudentRegDetail.VoucherNo, StudentRegDetail.VNo, StudentWise_FlyingHrs.Flying_Hrs, CASE WHEN StudentWise_FlyingHrs.StudentID IS NULL THEN 0 ELSE 1 END AS IsUpdated, Employee_Master.empName + ' ' + Employee_Master.MiddelName + ' ' + Employee_Master.lastName AS EntryName, CONVERT(VARCHAR, StudentWise_FlyingHrs.EntryDate, 106) AS EntryDate FROM Employee_Master INNER JOIN StudentWise_FlyingHrs ON Employee_Master.empId = StudentWise_FlyingHrs.EntryBy RIGHT OUTER JOIN StudentRegDetail INNER JOIN FeeHead ON StudentRegDetail.FeeID = FeeHead.FeeHeadId ON StudentWise_FlyingHrs.StudentID = StudentRegDetail.StudentID AND StudentWise_FlyingHrs.VoucherNo = StudentRegDetail.VoucherNo AND StudentWise_FlyingHrs.FeeHeadID = StudentRegDetail.FeeID AND StudentWise_FlyingHrs.InstID = StudentRegDetail.InstituteID WHERE StudentRegDetail.StudentID='" + Session["UID"] + "' AND StudentRegDetail.BalanceAmount=0 AND StudentWise_FlyingHrs.Flying_Hrs IS NOT NULL");

        if (dt.Rows.Count > 0)
        {
            decimal total = 0;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Flying_Hrs"] != DBNull.Value)
                    total += Convert.ToDecimal(dr["Flying_Hrs"]);
            }

            gridview.DataSource = dt;
            gridview.DataBind();

            lblTotalHours.Text = total.ToString("0.00");
        }
        else
        {
            gridview.DataSource = null;
            gridview.DataBind();
            lblTotalHours.Text = "0.00";
            objfun.MsgBox("Record Not Found", this);
        }
    }
}
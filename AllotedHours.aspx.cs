using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Flying_Hour_Alloted_Hours : System.Web.UI.Page
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
            objfun.FillActivityLog(Convert.ToInt32(Session["UID"]), "Alloted Hours ", "Visit Flying Hours Page", Convert.ToInt32(Session["InstID"]));

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

          //  lblSchoolName.Text = Header.Rows[0]["SchoolName"].ToString().ToUpper();
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

    //    dt = objfun.FillDataTable("SELECT StudentWise_FlyingHrs.ID, StudentWise_FlyingHrs.StudentID, StudentWise_FlyingHrs.VoucherNo, StudentWise_FlyingHrs.VNNo, StudentWise_FlyingHrs.FeeHeadID, StudentWise_FlyingHrs.Flying_Hrs, StudentWise_FlyingHrs.EntryBy, CONVERT(VARCHAR(11), StudentWise_FlyingHrs.EntryDate,106) AS EntryDate, StudentWise_FlyingHrs.InstID, StudentWise_FlyingHrs.SessionID, FeeHead.FeeHeadName, Employee_Master.empName+' '+Employee_Master.MiddelName+' '+Employee_Master.lastName AS EntryName FROM StudentWise_FlyingHrs INNER JOIN StudentReg ON StudentWise_FlyingHrs.StudentID=StudentReg.StudentID INNER JOIN FeeHead ON StudentWise_FlyingHrs.FeeHeadID=FeeHead.FeeHeadId INNER JOIN Employee_Master ON StudentWise_FlyingHrs.EntryBy=Employee_Master.empId WHERE StudentReg.StudentID='" + Session["UID"] + "'");

    //    if (dt.Rows.Count > 0)
    //    {
    //        decimal total = 0;

    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            total += Convert.ToDecimal(dr["Flying_Hrs"]);
    //        }

    //        ViewState["TotalFlying"] = total;

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
    //protected void gridview_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        e.Row.Cells[2].Text = "Total Alloted Hours";
    //        e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
    //        e.Row.Cells[2].Font.Bold = true;

    //        e.Row.Cells[3].Text = ViewState["TotalFlying"].ToString();
    //        e.Row.Cells[3].ForeColor = System.Drawing.Color.Green;
    //        e.Row.Cells[3].Font.Bold = true;
    //    }
    //}

    public void fillgrid()
    {
        DataTable dt = objfun.FillDataTable("SELECT StudentWise_FlyingHrs.ID, StudentWise_FlyingHrs.StudentID, StudentWise_FlyingHrs.VoucherNo, StudentWise_FlyingHrs.VNNo, StudentWise_FlyingHrs.FeeHeadID, StudentWise_FlyingHrs.Flying_Hrs, StudentWise_FlyingHrs.EntryBy, CONVERT(VARCHAR(11), StudentWise_FlyingHrs.EntryDate,106) AS EntryDate, StudentWise_FlyingHrs.InstID, StudentWise_FlyingHrs.SessionID, FeeHead.FeeHeadName, Employee_Master.empName+' '+Employee_Master.MiddelName+' '+Employee_Master.lastName AS EntryName FROM StudentWise_FlyingHrs INNER JOIN StudentReg ON StudentWise_FlyingHrs.StudentID=StudentReg.StudentID INNER JOIN FeeHead ON StudentWise_FlyingHrs.FeeHeadID=FeeHead.FeeHeadId INNER JOIN Employee_Master ON StudentWise_FlyingHrs.EntryBy=Employee_Master.empId WHERE StudentReg.StudentID='" + Session["UID"] + "'");

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
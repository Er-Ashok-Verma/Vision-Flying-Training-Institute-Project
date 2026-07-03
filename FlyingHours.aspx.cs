using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Flying_Hour_FlyingHours : System.Web.UI.Page
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
            objfun.FillActivityLog(Convert.ToInt32(Session["UID"]), "Flying Hours ", "Visit Flying Hours Page", Convert.ToInt32(Session["InstID"]));

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
           // lblSession.Text = Header.Rows[0]["SessionID"].ToString().ToUpper();
            lblRegNumber.Text = Header.Rows[0]["RegNo"].ToString().ToUpper();
            lblStudentName.Text = Header.Rows[0]["StudentName"].ToString().ToUpper();
            lblFatherName.Text = Header.Rows[0]["FatherName"].ToString().ToUpper();
            lblCourseName.Text = Header.Rows[0]["CourseName"].ToString().ToUpper();

        }
    }

         //public void fillgrid()
         //{
         //    DataTable dt = new DataTable();

         //    dt = objfun.FillDataTable("SELECT DISTINCT Student_DailyFlyingHours.ID, Student_DailyFlyingHours.StudentID, CONVERT(VARCHAR, Student_DailyFlyingHours.FlyingDate, 106) AS FlyingDate, CONVERT(VARCHAR(5), Student_DailyFlyingHours.FlyingHours, 108)  AS FlyingHours, Student_DailyFlyingHours.Remark, Student_DailyFlyingHours.EntryBy, CONVERT(VARCHAR(11), Student_DailyFlyingHours.EntryDate, 106) AS EntryDate, Student_DailyFlyingHours.InstructorID, Instructor_Master.Name, Employee_Master.empName + ' ' + Employee_Master.MiddelName + ' ' + Employee_Master.lastName AS EmployeeName, StudentRegDetail.VoucherNo FROM Student_DailyFlyingHours INNER JOIN Instructor_Master ON Student_DailyFlyingHours.InstructorID = Instructor_Master.ID INNER JOIN Employee_Master ON Student_DailyFlyingHours.EntryBy = Employee_Master.empId INNER JOIN StudentRegDetail ON Student_DailyFlyingHours.StudentID = StudentRegDetail.StudentID WHERE StudentRegDetail.StudentID='" + Session["UID"] + "'");

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

         //public void fillgrid()
         //{
         //    DataTable dt = new DataTable();
         //    dt = objfun.FillDataTable("SELECT  Student_DailyFlyingHours.ID, Student_DailyFlyingHours.StudentID, CONVERT(VARCHAR, Student_DailyFlyingHours.FlyingDate, 106) AS FlyingDate, CONVERT(VARCHAR(5), Student_DailyFlyingHours.FlyingHours, 108) AS FlyingHours, Student_DailyFlyingHours.Remark, Student_DailyFlyingHours.EntryBy, CONVERT(VARCHAR(11), Student_DailyFlyingHours.EntryDate, 106) AS EntryDate, Student_DailyFlyingHours.InstructorID,  Instructor_Master.Name, Employee_Master.empName + ' ' + Employee_Master.MiddelName + ' ' + Employee_Master.lastName AS EmployeeName,  CONVERT(VARCHAR(11), Student_DailyFlyingHours.FlyingDate,106) AS DailyFlyingHours FROM Student_DailyFlyingHours INNER JOIN Instructor_Master ON Student_DailyFlyingHours.InstructorID = Instructor_Master.ID INNER JOIN Employee_Master ON Student_DailyFlyingHours.EntryBy = Employee_Master.empId WHERE Student_DailyFlyingHours.StudentID='" + Session["UID"] + "'");
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

         //    // Total Flying Hours
         //    DataTable dtTotal = objfun.FillDataTable("SELECT CAST(SUM(DATEDIFF(MINUTE,'00:00:00',FlyingHours))/60 AS VARCHAR)+':'+RIGHT('00'+CAST(SUM(DATEDIFF(MINUTE,'00:00:00',FlyingHours))%60 AS VARCHAR),2) AS TotalHours FROM Student_DailyFlyingHours WHERE StudentID='" + Session["UID"] + "'");

         //    if (dtTotal.Rows.Count > 0 && dtTotal.Rows[0]["TotalHours"].ToString() != "")
         //    {
         //        lblTotalHours.Text = dtTotal.Rows[0]["TotalHours"].ToString();
         //    }
         //    else
         //    {
         //        lblTotalHours.Text = "00:00";
         //    }
         //}

         public void fillgrid()
         {
             //==================== Grid Data ====================//
             DataTable dt = objfun.FillDataTable(" SELECT Student_DailyFlyingHours.ID, Student_DailyFlyingHours.StudentID, CONVERT(VARCHAR, Student_DailyFlyingHours.FlyingDate,106) AS FlyingDate, CONVERT(VARCHAR(5),Student_DailyFlyingHours.FlyingHours,108) AS FlyingHours, Student_DailyFlyingHours.Remark, Student_DailyFlyingHours.EntryBy, CONVERT(VARCHAR(11),Student_DailyFlyingHours.EntryDate,106) AS EntryDate, Student_DailyFlyingHours.InstructorID, Instructor_Master.Name, Employee_Master.empName + ' ' + Employee_Master.MiddelName + ' ' + Employee_Master.lastName AS EmployeeName, CONVERT(VARCHAR(11),Student_DailyFlyingHours.FlyingDate,106) AS DailyFlyingHours FROM Student_DailyFlyingHours INNER JOIN Instructor_Master ON Student_DailyFlyingHours.InstructorID = Instructor_Master.ID INNER JOIN Employee_Master ON Student_DailyFlyingHours.EntryBy = Employee_Master.empId WHERE Student_DailyFlyingHours.StudentID='" + Session["UID"] + "'");

             if (dt.Rows.Count > 0)
             {
                 gridview.DataSource = dt;
                 gridview.DataBind();
             }
             else
             {
                 gridview.DataSource = null;
                 gridview.DataBind();
                 objfun.MsgBox("Record Not Found", this);
             }

             //==================== Total Flying Hours ====================//

             DataTable dtTotal = objfun.FillDataTable("SELECT CAST(SUM(DATEDIFF(MINUTE,'00:00:00',FlyingHours))/60 AS VARCHAR) + ':' + RIGHT('00'+CAST(SUM(DATEDIFF(MINUTE,'00:00:00',FlyingHours))%60 AS VARCHAR),2) AS TotalHours FROM Student_DailyFlyingHours WHERE StudentID='" + Session["UID"] + "'");

             // Eligible Hours
             string eligibleHoursStr = objfun.Get_details(" SELECT ISNULL( SUM(TRY_CAST(StudentWise_FlyingHrs.Flying_Hrs AS DECIMAL(18,2))),0) FROM StudentWise_FlyingHrs INNER JOIN StudentRegDetail ON StudentWise_FlyingHrs.StudentID=StudentRegDetail.StudentID AND StudentWise_FlyingHrs.FeeHeadID=StudentRegDetail.FeeID AND StudentWise_FlyingHrs.InstID=StudentRegDetail.InstituteID WHERE StudentRegDetail.StudentID='" + Session["UID"] + "' AND StudentRegDetail.BalanceAmount=0");

             // Used Flying Hours (Decimal)
             decimal flyingHoursDecimal = 0;

             decimal.TryParse(objfun.Get_details("SELECT ISNULL(CAST(SUM(DATEDIFF(MINUTE,'00:00:00',FlyingHours))/60.0 AS DECIMAL(18,2)),0) FROM Student_DailyFlyingHours WHERE StudentID='" + Session["UID"] + "'"), out flyingHoursDecimal);

             // Convert Eligible Hours
             decimal eligibleHours = 0;
             decimal.TryParse(eligibleHoursStr, out eligibleHours);

             // Balance Hours
             decimal balanceHours = eligibleHours - flyingHoursDecimal;

             if (balanceHours < 0)
                 balanceHours = 0;

             int balanceMinutes = (int)Math.Round(balanceHours * 60);

             TimeSpan balanceTime = TimeSpan.FromMinutes(balanceMinutes);

             lblBalanceHours.Text = string.Format("{0:00}:{1:00}",
                                                  (int)balanceTime.TotalHours,
                                                  balanceTime.Minutes);

             // Total Hours Label
             if (dtTotal.Rows.Count > 0 &&
                 dtTotal.Rows[0]["TotalHours"].ToString() != "")
             {
                 lblTotalHours.Text = dtTotal.Rows[0]["TotalHours"].ToString();
             }
             else
             {
                 lblTotalHours.Text = "00:00";
             }
         }
}
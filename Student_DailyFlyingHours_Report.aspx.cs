using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Flying_Hour_Student_DailyFlyingHours_Report : System.Web.UI.Page
{
    DbFunctions objfun = new DbFunctions();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UID"] == null)
        {
            Response.Redirect("../error_404_2.html");
        }

        if (Session["instID"].ToString() == null)
        {
            Response.Redirect("../error_404_2.html");
        }

        if (Session["SesnID"].ToString() == null)
        {
            Response.Redirect("../error_404_2.html");
        }

        if (!IsPostBack)
        {
            objfun.FillDropdownlist(ddlCourse, "CourseName", "CourseId", "SELECT CourseId, CourseName FROM Course", "--Select--");
            objfun.FillDropdownlist(ddlInstructor, "Name", "ID", "SELECT ID, Name FROM Instructor_Master", "--Select--");
           
        } 
    }

    

    public void Reset()
    {
        ddlCourse.SelectedValue = "0";
        ddlInstructor.SelectedValue = "0";
        ddlReportType.SelectedValue = "";
       
    }



    public void fillgridSum()
    {
        DataTable dt = new DataTable();

      //  dt = objfun.FillDataTable("SELECT StudentReg.StudentID,StudentReg.StudentName,ISNULL(Instructor_Master.Name,'N/A') AS InstructorName,ISNULL(CONVERT(VARCHAR,LastFlying.FlyingDate,106),'N/A') AS FlyingDate,ISNULL(RIGHT('00'+CAST(DATEPART(HOUR,LastFlying.FlyingHours) AS VARCHAR(2)),2)+':'+RIGHT('00'+CAST(DATEPART(MINUTE,LastFlying.FlyingHours) AS VARCHAR(2)),2),'N/A') AS FlyingHours,ISNULL(NULLIF(LTRIM(RTRIM(LastFlying.Remark)),''),'N/A') AS Remark,ISNULL(ISNULL(Employee_Master.empName,'')+' '+ISNULL(Employee_Master.MiddelName,'')+' '+ISNULL(Employee_Master.lastName,''),'N/A') AS EmployeeName,ISNULL(CONVERT(VARCHAR,LastFlying.EntryDate,106),'N/A') AS EntryDate,ISNULL(AllotedHoursTable.AllotedHours,0) AS AllotedHours,CASE WHEN ISNULL(EligibleHoursTable.EligibleHours,0)=0 THEN 'N/A' ELSE CAST(EligibleHoursTable.EligibleHours AS VARCHAR(20)) END AS EligibleHours,CASE WHEN ISNULL(TotalHoursTable.TotalMinutes,0)=0 THEN 'N/A' ELSE CAST(ISNULL(TotalHoursTable.TotalMinutes,0)/60 AS VARCHAR(10))+':'+RIGHT('00'+CAST(ISNULL(TotalHoursTable.TotalMinutes,0)%60 AS VARCHAR(2)),2) END AS TotalFlyingHours,CASE WHEN ISNULL(EligibleHoursTable.EligibleHours,0)=0 OR ISNULL(TotalHoursTable.FlyingHoursDecimal,0)=0 THEN 'N/A' ELSE CAST(ABS(CAST(ROUND((ISNULL(EligibleHoursTable.EligibleHours,0)-ISNULL(TotalHoursTable.FlyingHoursDecimal,0))*60,0) AS INT))/60 AS VARCHAR(10))+':'+RIGHT('00'+CAST(ABS(CAST(ROUND((ISNULL(EligibleHoursTable.EligibleHours,0)-ISNULL(TotalHoursTable.FlyingHoursDecimal,0))*60,0) AS INT))%60 AS VARCHAR(2)),2) END AS BalanceHours FROM (SELECT DISTINCT StudentID FROM StudentWise_FlyingHrs) SWF INNER JOIN StudentReg ON SWF.StudentID=StudentReg.StudentID OUTER APPLY (SELECT TOP 1 * FROM Student_DailyFlyingHours WHERE StudentID=StudentReg.StudentID ORDER BY FlyingDate DESC,ID DESC) LastFlying LEFT JOIN Instructor_Master ON LastFlying.InstructorID=Instructor_Master.ID LEFT JOIN Employee_Master ON LastFlying.EntryBy=Employee_Master.empId LEFT JOIN (SELECT StudentID,SUM(CAST(Flying_Hrs AS DECIMAL(18,2))) AS AllotedHours FROM StudentWise_FlyingHrs GROUP BY StudentID) AllotedHoursTable ON StudentReg.StudentID=AllotedHoursTable.StudentID LEFT JOIN (SELECT StudentRegDetail.StudentID,SUM(CAST(StudentWise_FlyingHrs.Flying_Hrs AS DECIMAL(18,2))) AS EligibleHours FROM StudentWise_FlyingHrs INNER JOIN StudentRegDetail ON StudentWise_FlyingHrs.StudentID=StudentRegDetail.StudentID AND StudentWise_FlyingHrs.FeeHeadID=StudentRegDetail.FeeID AND StudentWise_FlyingHrs.InstID=StudentRegDetail.InstituteID WHERE StudentRegDetail.BalanceAmount=0 GROUP BY StudentRegDetail.StudentID) EligibleHoursTable ON StudentReg.StudentID=EligibleHoursTable.StudentID LEFT JOIN (SELECT StudentID,SUM(DATEDIFF(MINUTE,'00:00:00',FlyingHours)) AS TotalMinutes,CAST(SUM(DATEDIFF(MINUTE,'00:00:00',FlyingHours))/60.0 AS DECIMAL(18,2)) AS FlyingHoursDecimal FROM Student_DailyFlyingHours GROUP BY StudentID) TotalHoursTable ON StudentReg.StudentID=TotalHoursTable.StudentID ORDER BY StudentReg.StudentName");
     //   dt = objfun.FillDataTable("SELECT StudentReg.StudentID,StudentReg.StudentName,ISNULL(Instructor_Master.Name,'N/A') AS InstructorName,ISNULL(CONVERT(VARCHAR,LastFlying.FlyingDate,106),'N/A') AS FlyingDate,ISNULL(RIGHT('00'+CAST(DATEPART(HOUR,LastFlying.FlyingHours) AS VARCHAR(2)),2)+':'+RIGHT('00'+CAST(DATEPART(MINUTE,LastFlying.FlyingHours) AS VARCHAR(2)),2),'N/A') AS FlyingHours,ISNULL(NULLIF(LTRIM(RTRIM(LastFlying.Remark)),''),'N/A') AS Remark,ISNULL(ISNULL(Employee_Master.empName,'')+' '+ISNULL(Employee_Master.MiddelName,'')+' '+ISNULL(Employee_Master.lastName,''),'N/A') AS EmployeeName,ISNULL(CONVERT(VARCHAR,LastFlying.EntryDate,106),'N/A') AS EntryDate,CASE WHEN ISNULL(AllotedHoursTable.AllotedHours,0)=0 THEN 'N/A' ELSE CAST(AllotedHoursTable.AllotedHours AS VARCHAR(20)) END AS AllotedHours,CASE WHEN ISNULL(EligibleHoursTable.EligibleHours,0)=0 THEN 'N/A' ELSE CAST(EligibleHoursTable.EligibleHours AS VARCHAR(20)) END AS EligibleHours,CASE WHEN ISNULL(TotalHoursTable.TotalMinutes,0)=0 THEN 'N/A' ELSE CAST(ISNULL(TotalHoursTable.TotalMinutes,0)/60 AS VARCHAR(10))+':'+RIGHT('00'+CAST(ISNULL(TotalHoursTable.TotalMinutes,0)%60 AS VARCHAR(2)),2) END AS TotalFlyingHours,CASE WHEN ISNULL(EligibleHoursTable.EligibleHours,0)=0 OR ISNULL(TotalHoursTable.FlyingHoursDecimal,0)=0 THEN 'N/A' ELSE CAST(ABS(CAST(ROUND((ISNULL(EligibleHoursTable.EligibleHours,0)-ISNULL(TotalHoursTable.FlyingHoursDecimal,0))*60,0) AS INT))/60 AS VARCHAR(10))+':'+RIGHT('00'+CAST(ABS(CAST(ROUND((ISNULL(EligibleHoursTable.EligibleHours,0)-ISNULL(TotalHoursTable.FlyingHoursDecimal,0))*60,0) AS INT))%60 AS VARCHAR(2)),2) END AS BalanceHours FROM StudentReg OUTER APPLY (SELECT TOP 1 * FROM Student_DailyFlyingHours WHERE StudentID=StudentReg.StudentID ORDER BY FlyingDate DESC,ID DESC) LastFlying LEFT JOIN Instructor_Master ON LastFlying.InstructorID=Instructor_Master.ID LEFT JOIN Employee_Master ON LastFlying.EntryBy=Employee_Master.empId LEFT JOIN (SELECT StudentID,SUM(CAST(Flying_Hrs AS DECIMAL(18,2))) AS AllotedHours FROM StudentWise_FlyingHrs GROUP BY StudentID) AllotedHoursTable ON StudentReg.StudentID=AllotedHoursTable.StudentID LEFT JOIN (SELECT StudentRegDetail.StudentID,SUM(CAST(StudentWise_FlyingHrs.Flying_Hrs AS DECIMAL(18,2))) AS EligibleHours FROM StudentWise_FlyingHrs INNER JOIN StudentRegDetail ON StudentWise_FlyingHrs.StudentID=StudentRegDetail.StudentID AND StudentWise_FlyingHrs.FeeHeadID=StudentRegDetail.FeeID AND StudentWise_FlyingHrs.InstID=StudentRegDetail.InstituteID WHERE StudentRegDetail.BalanceAmount=0 GROUP BY StudentRegDetail.StudentID) EligibleHoursTable ON StudentReg.StudentID=EligibleHoursTable.StudentID LEFT JOIN (SELECT StudentID,SUM(DATEDIFF(MINUTE,'00:00:00',FlyingHours)) AS TotalMinutes,CAST(SUM(DATEDIFF(MINUTE,'00:00:00',FlyingHours))/60.0 AS DECIMAL(18,2)) AS FlyingHoursDecimal FROM Student_DailyFlyingHours GROUP BY StudentID) TotalHoursTable ON StudentReg.StudentID=TotalHoursTable.StudentID " + condition() + " ORDER BY StudentReg.StudentName");
        dt = objfun.FillDataTable("SELECT StudentReg.StudentID,StudentReg.RegNo,StudentReg.StudentName,ISNULL(Instructor_Master.Name,'N/A') AS InstructorName,ISNULL(CONVERT(VARCHAR,LastFlying.FlyingDate,106),'N/A') AS FlyingDate,ISNULL(RIGHT('00'+CAST(DATEPART(HOUR,LastFlying.FlyingHours) AS VARCHAR(2)),2)+':'+RIGHT('00'+CAST(DATEPART(MINUTE,LastFlying.FlyingHours) AS VARCHAR(2)),2),'N/A') AS FlyingHours,ISNULL(NULLIF(LTRIM(RTRIM(LastFlying.Remark)),''),'N/A') AS Remark,ISNULL(ISNULL(Employee_Master.empName,'')+' '+ISNULL(Employee_Master.MiddelName,'')+' '+ISNULL(Employee_Master.lastName,''),'N/A') AS EmployeeName,ISNULL(CONVERT(VARCHAR,LastFlying.EntryDate,106),'N/A') AS EntryDate,CASE WHEN ISNULL(AllotedHoursTable.AllotedHours,0)=0 THEN 'N/A' ELSE CAST(AllotedHoursTable.AllotedHours AS VARCHAR(20)) END AS AllotedHours,CASE WHEN ISNULL(EligibleHoursTable.EligibleHours,0)=0 THEN 'N/A' ELSE CAST(EligibleHoursTable.EligibleHours AS VARCHAR(20)) END AS EligibleHours,CASE WHEN ISNULL(TotalHoursTable.TotalMinutes,0)=0 THEN 'N/A' ELSE CAST(ISNULL(TotalHoursTable.TotalMinutes,0)/60 AS VARCHAR(10))+':'+RIGHT('00'+CAST(ISNULL(TotalHoursTable.TotalMinutes,0)%60 AS VARCHAR(2)),2) END AS TotalFlyingHours,CASE WHEN ISNULL(EligibleHoursTable.EligibleHours,0)=0 OR ISNULL(TotalHoursTable.FlyingHoursDecimal,0)=0 THEN 'N/A' ELSE CAST(ABS(CAST(ROUND((ISNULL(EligibleHoursTable.EligibleHours,0)-ISNULL(TotalHoursTable.FlyingHoursDecimal,0))*60,0) AS INT))/60 AS VARCHAR(10))+':'+RIGHT('00'+CAST(ABS(CAST(ROUND((ISNULL(EligibleHoursTable.EligibleHours,0)-ISNULL(TotalHoursTable.FlyingHoursDecimal,0))*60,0) AS INT))%60 AS VARCHAR(2)),2) END AS BalanceHours FROM StudentReg OUTER APPLY (SELECT TOP 1 * FROM Student_DailyFlyingHours WHERE StudentID=StudentReg.StudentID ORDER BY FlyingDate DESC,ID DESC) LastFlying LEFT JOIN Instructor_Master ON LastFlying.InstructorID=Instructor_Master.ID LEFT JOIN Employee_Master ON LastFlying.EntryBy=Employee_Master.empId LEFT JOIN (SELECT StudentID,SUM(CAST(Flying_Hrs AS DECIMAL(18,2))) AS AllotedHours FROM StudentWise_FlyingHrs GROUP BY StudentID) AllotedHoursTable ON StudentReg.StudentID=AllotedHoursTable.StudentID LEFT JOIN (SELECT StudentRegDetail.StudentID,SUM(CAST(StudentWise_FlyingHrs.Flying_Hrs AS DECIMAL(18,2))) AS EligibleHours FROM StudentWise_FlyingHrs INNER JOIN StudentRegDetail ON StudentWise_FlyingHrs.StudentID=StudentRegDetail.StudentID AND StudentWise_FlyingHrs.FeeHeadID=StudentRegDetail.FeeID AND StudentWise_FlyingHrs.InstID=StudentRegDetail.InstituteID WHERE StudentRegDetail.BalanceAmount=0 GROUP BY StudentRegDetail.StudentID) EligibleHoursTable ON StudentReg.StudentID=EligibleHoursTable.StudentID LEFT JOIN (SELECT StudentID,SUM(DATEDIFF(MINUTE,'00:00:00',FlyingHours)) AS TotalMinutes,CAST(SUM(DATEDIFF(MINUTE,'00:00:00',FlyingHours))/60.0 AS DECIMAL(18,2)) AS FlyingHoursDecimal FROM Student_DailyFlyingHours GROUP BY StudentID) TotalHoursTable ON StudentReg.StudentID=TotalHoursTable.StudentID " + condition() + " ORDER BY StudentReg.StudentName");    
        
        if (dt.Rows.Count > 0)
        {
            gridview.DataSource = dt;
            gridview.DataBind();
        }
        else
        {
            gridview.DataSource = null;
            gridview.DataBind();
            objfun.MsgBox("Record not found!", this);
        }
    }

    public void fillgridDet()
    {
        DataTable dt1 = new DataTable();

        dt1 = objfun.FillDataTable("SELECT Student_DailyFlyingHours.ID, Student_DailyFlyingHours.StudentID, CONVERT(VARCHAR, Student_DailyFlyingHours.FlyingDate, 106) AS FlyingDate, Student_DailyFlyingHours.FlyingHours, Student_DailyFlyingHours.Remark,  Student_DailyFlyingHours.EntryBy, CONVERT(VARCHAR, Student_DailyFlyingHours.EntryDate, 106) AS EntryDate, Student_DailyFlyingHours.InstructorID, Instructor_Master.Name, StudentReg.StudentName,  Employee_Master.empName+' '+ Employee_Master.MiddelName+' '+ Employee_Master.lastName as EmployeeName FROM Student_DailyFlyingHours INNER JOIN Instructor_Master ON Student_DailyFlyingHours.InstructorID = Instructor_Master.ID INNER JOIN StudentReg ON Student_DailyFlyingHours.StudentID = StudentReg.StudentID INNER JOIN Employee_Master ON Student_DailyFlyingHours.EntryBy = Employee_Master.empId " + condition() + " ");
        if (dt1.Rows.Count > 0)
        {
            gridview1.DataSource = dt1;
            gridview1.DataBind();
        }
        else
        {
            gridview1.DataSource = null;
            gridview1.DataBind();
            objfun.MsgBox("Record not found!", this);
        }
    }

    private string condition()
    {
        string Where = "";

        if (ddlCourse.SelectedValue != "" && ddlCourse.SelectedValue != "0")
        {
            Where = " StudentReg.CourseID='" + ddlCourse.SelectedValue + "'";
        }

        if (ddlInstructor.SelectedValue != "" && ddlInstructor.SelectedValue != "0")
        {
            if (Where == "")
            {
                Where = " Instructor_Master.ID='" + ddlInstructor.SelectedValue + "'";
            }
            else
            {
                Where += " AND Instructor_Master.ID='" + ddlInstructor.SelectedValue + "'";
            }
        }

        if (Where != "")
        {
            Where = " WHERE " + Where;
        }

        return Where;
    }

    protected void btnreset_Click(object sender, EventArgs e)
    {
        Reset();
    }

    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReportType.SelectedValue == "Summary")
        {
            div3.Visible = false;

            gridview.Visible = false;
            gridview1.Visible = false;
        }
        else if (ddlReportType.SelectedValue == "Detail")
        {
            div3.Visible = true;

            gridview.Visible = false;
            gridview1.Visible = false;
        }
    }
    protected void ddlInstructor_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlReportType.SelectedValue == "Detail")
        //{
        //    fillgridDet();
        //}
    }
    protected void btnview_Click(object sender, EventArgs e)
    {

        if (ddlCourse.SelectedValue == "0")
        {
            objfun.MsgBox("Please Select Course !", this);
            return;
        }

        if (ddlReportType.SelectedValue == "Summary")
        {
            div3.Visible = false;

            gridview.Visible = true;
            gridview1.Visible = false;

            fillgridSum();
        }
        else if (ddlReportType.SelectedValue == "Detail")
        {
            div3.Visible = true;

            gridview.Visible = false;
            gridview1.Visible = true;

            fillgridDet();
        }
        else
        {
            objfun.MsgBox("Please Select Report Type!", this);
        }
    }
}
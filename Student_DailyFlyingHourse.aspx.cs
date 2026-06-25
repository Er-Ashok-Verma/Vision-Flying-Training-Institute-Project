using NewDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class Academic_Student_DailyFlyingHourse : System.Web.UI.Page
{
    DbFunctions objFun = new DbFunctions();
    protected void Page_Load(object sender, EventArgs e)
    {
        
         if (Session["UID"] == null)
        {
            Response.Redirect("../error_404_2.html");
            return;
        }

        if (Session["InstID"].ToString() == null)
        {
            Response.Redirect("../error_404_2.html");
        }

        if (Session["sesnID"].ToString() == null)
        {
            Response.Redirect("../error_404_2.html");
        }

        if (Context.Request.QueryString["title"] != null)
        {
            lbltitle.InnerText = Context.Request.QueryString["title"].ToString();
        }
        if (!IsPostBack)
        {
            try
            {
                objFun.FillDropdownlist(ddlInstructor, "Name", "ID", "SELECT ID, Name, Eamil, MobileNo FROM Instructor_Master", "--Select--");
                fillgrid();
                divSubmit.Visible = false;
            }
            catch (Exception ex)
            {
                objFun.MsgBox1(ex.Message, UpdatePanel1);
                return;
            }
        }
    }

    protected void btnSearchStu_Click(object sender, EventArgs e)
    {
        try
        {
            lblStuName.Text = objFun.Get_details("Select StudentName +' - '+ FatherName as Student From StudentReg where  StudentID='" + txtNameStudent.SelectedValue + "' and InstituteID = '" + Session["InstID"].ToString() + "'");
            lblStuName.Text = lblStuName.Text.Trim();
            divSubmit.Visible = true;

            txt_course_name.Text = objFun.Get_details("SELECT distinct Course.CourseName+' ('+ SchoolMaster.ShortName +')' as CourseName FROM StudentStatus INNER JOIN Course ON StudentStatus.CourseID = Course.CourseId INNER JOIN SchoolMaster ON Course.School_ID = SchoolMaster.ID WHERE (StudentStatus.Status In ('C','ENQ')) and StudentStatus.StudentID='" + txtNameStudent.SelectedValue + "' and StudentStatus.InstituteID = '" + Session["InstID"].ToString() + "'");
         

            DataTable dt = new DataTable();
            dt = objFun.FillDataTable("select Photo from StudentdetailEntry where StudentID='" + txtNameStudent.SelectedValue + "' and InstituteID = '" + Session["InstID"].ToString() + "'");
            if (dt.Rows.Count > 0)
            {
                byte[] imgbin = { 1, 1, 0, 0 };
                if (dt.Rows[0]["Photo"].ToString() != DBNull.Value.ToString())
                {
                    imgbin = (Byte[])dt.Rows[0]["Photo"];
                    this.imgPhoto.ImageUrl = "~/ThumbNail1.ashx?uid=" + txtNameStudent.SelectedValue + "&InstituteID=" + Session["InstID"].ToString();

                }
            }

          //  FillVouchers();
            LoadHours();
            fillgrid();
          
        }
        catch (Exception ex)
        {
            objFun.MsgBox(ex.Message, this);
            return;
        }

    }

    //public void FillVouchers()
    //{
    //    try
    //    {
    //        FeeServices objF = new FeeServices();
    //        DataSet set = new DataSet();

    //        set = FillFeeVoucher_New(Convert.ToInt32(Session["InstID"].ToString()), Convert.ToInt32(txtNameStudent.SelectedValue), 1, Convert.ToInt32(0));
            
    //        DataTable fill_Voucher = new DataTable();
    //        DataTable fill_Voucher1 = new DataTable();
    //        if (set.Tables[0].Rows.Count > 0)
    //        {
    //            fill_Voucher = set.Tables[0];
    //        }
    //        if (set.Tables[1].Rows.Count > 0)
    //        {
    //            fill_Voucher1 = set.Tables[1];
    //        }
    //        ddlVoucher.DataSource = fill_Voucher;
    //        ddlVoucher.DataTextField = "VNo";
    //        ddlVoucher.DataValueField = "VoucherNo";
    //        ddlVoucher.DataBind();
    //        ddlVoucher.Items.Insert(0, "--Select--");

    //        if (fill_Voucher1.Rows.Count > 0)
    //        {
    //            // ddlVoucher.Visible = false;
    //            chkVou.DataSource = fill_Voucher1;
    //            chkVou.DataTextField = "VNo";
    //            chkVou.DataValueField = "VoucherNo";
    //            chkVou.DataBind();
    //        }
    //        else
    //        {
    //            ddlVoucher.Visible = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        objFun.MsgBox1(ex.Message, UpdatePanel1);
    //        return;
    //    }
    //}

    //private void LoadHours()
    //{
    //    string studentId = txtNameStudent.SelectedValue;

         
    //    lblAllotedHours.Text = objFun.Get_details("SELECT ISNULL(SUM(Flying_Hrs),0) FROM StudentWise_FlyingHrs WHERE StudentID='" + txtNameStudent.SelectedValue + "' AND InstID='" + Session["InstID"].ToString() + "'");

    //    lblEligibleHours.Text = objFun.Get_details("SELECT ISNULL(SUM(SWFH.Flying_Hrs),0) FROM StudentWise_FlyingHrs SWFH INNER JOIN StudentRegDetail SRD ON SWFH.StudentID=SRD.StudentID AND SWFH.FeeHeadID=SRD.FeeID AND SWFH.InstID=SRD.InstituteID WHERE SRD.StudentID='" + txtNameStudent.SelectedValue + "' AND SRD.BalanceAmount=0");

    //    lblFlyingHours.Text = objFun.Get_details("SELECT ISNULL(SUM(FlyingHours),0) FROM Student_DailyFlyingHours WHERE ID='" + txtNameStudent.SelectedValue + "' AND InstID='" + Session["InstID"].ToString() + "'");
    //    lblBalanceHours.Text = "0";
    //}
 
    private void LoadHours()
    {
        
        lblAllotedHours.Text = objFun.Get_details("SELECT ISNULL(SUM(TRY_CAST(Flying_Hrs AS DECIMAL(18,2))),0) FROM StudentWise_FlyingHrs WHERE StudentID='" + txtNameStudent.SelectedValue + "' AND InstID='" + Session["InstID"].ToString() + "'");
         
        lblEligibleHours.Text = objFun.Get_details("SELECT ISNULL(SUM(TRY_CAST(StudentWise_FlyingHrs.Flying_Hrs AS DECIMAL(18,2))),0) FROM StudentWise_FlyingHrs INNER JOIN StudentRegDetail ON StudentWise_FlyingHrs.StudentID=StudentRegDetail.StudentID AND StudentWise_FlyingHrs.FeeHeadID=StudentRegDetail.FeeID AND StudentWise_FlyingHrs.InstID=StudentRegDetail.InstituteID WHERE StudentRegDetail.StudentID='" + txtNameStudent.SelectedValue + "' AND StudentRegDetail.BalanceAmount=0");
       
        lblFlyingHours.Text = objFun.Get_details("SELECT ISNULL(RIGHT('00' + CAST(SUM(DATEDIFF(MINUTE,'00:00:00',FlyingHours))/60 AS VARCHAR(10)),2) + ':' + RIGHT('00' + CAST(SUM(DATEDIFF(MINUTE,'00:00:00',FlyingHours))%60 AS VARCHAR(2)),2),'00:00') FROM Student_DailyFlyingHours WHERE StudentID='" + txtNameStudent.SelectedValue + "'");
      
        decimal FlyingHoursDecimal = 0;

        decimal.TryParse(objFun.Get_details("SELECT ISNULL(CAST(SUM(DATEDIFF(MINUTE,'00:00:00',FlyingHours))/60.0 AS DECIMAL(18,2)),0) FROM Student_DailyFlyingHours WHERE StudentID='" + txtNameStudent.SelectedValue + "'"), out FlyingHoursDecimal);
 
        decimal EligibleHours = 0;
        decimal.TryParse(lblEligibleHours.Text, out EligibleHours);

        int BalanceMinutes = (int)Math.Round((EligibleHours - FlyingHoursDecimal) * 60);

        lblBalanceHours.Text = (BalanceMinutes / 60).ToString("00") + ":" + (BalanceMinutes % 60).ToString("00");
    }

    public DataSet FillFeeVoucher_New(int instid, int studentid, int flag, int Sid)
    {

        NewDAL.DBManager objDB = new DBManager();
        objDB.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["FeesManagementConn"].ConnectionString;
        objDB.DBManager(DataAccessLayer.DataProvider.SqlServer, objDB.ConnectionString);
        objDB.Open();
        objDB.CreateParameters(4);
        objDB.AddParameters(0, "InstituteID", instid, DbType.Int32);
        objDB.AddParameters(1, "studentid", studentid, DbType.Int32);
        objDB.AddParameters(2, "Sid", Sid, DbType.Int32);
        objDB.AddParameters(3, "flag", flag, DbType.Int32);
        DataSet ds = new DataSet();
        try
        {
            ds = objDB.ExecuteDataSet(CommandType.StoredProcedure, "FeeDepositionSelect");
        }
        finally
        {
            objDB.Connection.Close();
            objDB.Dispose();
        }
        return ds;
    }
    protected void chkVou_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            gridview.DataSource = null;
            gridview.DataBind();
            string voucher = "";

           // int selectedCount = 0;
            //foreach (System.Web.UI.WebControls.ListItem item in chkVou.Items)
            //{
            //    if (item.Selected)
            //    {
            //        selectedCount++;
            //        if (selectedCount > 1)
            //        {
            //            objFun.MsgBox("Please select only one voucher at a time...", this);
            //            foreach (System.Web.UI.WebControls.ListItem li in chkVou.Items)
            //            {
            //                li.Selected = false;
            //            }
            //            return; // Stop execution
            //        }
            //        voucher = item.Value; // Store first selected value
            //    }
            //}


            if (txtSearchName.Text == String.Empty)
            {
                objFun.MsgBox1("Select Student Name", UpdatePanel1);
                txtSearchName.Focus();
                return;
            }

            DataTable Feedetail = new DataTable();
            Feedetail = objFun.FillDataTable("SELECT StudentRegDetail.StudentDetailID, StudentRegDetail.StudentID,FeeHead.FeeHeadName, StudentRegDetail.FeeID, StudentRegDetail.TotalAmount,StudentRegDetail.PaidAmount, StudentRegDetail.BalanceAmount,StudentRegDetail.InstituteID,StudentRegDetail.VoucherNo, StudentRegDetail.VNo FROM StudentRegDetail INNER JOIN FeeHead ON StudentRegDetail.FeeID = FeeHead.FeeHeadId WHERE (StudentRegDetail.StudentID ='" + txtNameStudent.SelectedValue + "') and StudentRegDetail.VoucherNo='" + voucher + "' ");
            if (Feedetail.Rows.Count > 0)
            {
                gridview.DataSource = Feedetail;
                gridview.DataBind();
            }
            else
            {
                gridview.DataSource = null;
                gridview.DataBind();
            }
         


        }
        catch (Exception ex)
        {
            objFun.MsgBox1(ex.Message, UpdatePanel1);
            return;
        }
    }

    public void fillgrid()
    {
        if (txtNameStudent.SelectedValue == "")
            return;

        DataTable dt = objFun.FillDataTable("SELECT Student_DailyFlyingHours.ID, Student_DailyFlyingHours.StudentID, CONVERT(VARCHAR, Student_DailyFlyingHours.FlyingDate, 106) AS FlyingDate, Student_DailyFlyingHours.FlyingHours, Student_DailyFlyingHours.Remark,  Student_DailyFlyingHours.EntryBy, CONVERT(VARCHAR, Student_DailyFlyingHours.EntryDate, 106) AS EntryDate, Student_DailyFlyingHours.InstructorID, Instructor_Master.Name, StudentReg.StudentName,  Employee_Master.empName+' '+ Employee_Master.MiddelName+' '+ Employee_Master.lastName as EmployeeName FROM Student_DailyFlyingHours INNER JOIN Instructor_Master ON Student_DailyFlyingHours.InstructorID = Instructor_Master.ID INNER JOIN StudentReg ON Student_DailyFlyingHours.StudentID = StudentReg.StudentID INNER JOIN Employee_Master ON Student_DailyFlyingHours.EntryBy = Employee_Master.empId WHERE Student_DailyFlyingHours.StudentID='" + txtNameStudent.SelectedValue + "'");
        gridview.DataSource = dt;
        gridview.DataBind();
    }
    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btn = sender as ImageButton;
        GridViewRow row = btn.NamingContainer as GridViewRow;
        string FeeHeadID = gridview.DataKeys[row.RowIndex].Values[1].ToString();
        string VoucherNo = gridview.DataKeys[row.RowIndex].Values[2].ToString();
        string VN_No = gridview.DataKeys[row.RowIndex].Values[3].ToString();

        if (txtSearchName.Text == String.Empty || txtNameStudent.SelectedValue=="")
        {
            objFun.MsgBox1("Select Student Name", UpdatePanel1);
            txtSearchName.Focus();
            return;
        }
    }
    public void  Reset()
    {
        ddlInstructor.SelectedIndex = 0;
        txtFlyingDate.Text = "";
        txtFlyingHours.Text = "";
        txtRemark.Text = "";
    }

    protected void btnSave_Click(object sender, EventArgs e)


    {
        if (ddlInstructor.SelectedValue == "0")
        {
            objFun.MsgBox("Please Select Instructor !", this);
            ddlInstructor.Focus();
            return;
        }

        if (txtFlyingDate.Text.Trim() == "")
        {
            objFun.MsgBox("Please Select Flying Date !", this);
            txtFlyingDate.Focus();
            return;
        }



        //if (txtFlyingHours.Text.Trim() == "")
        //{
        //    objFun.MsgBox("Please Enter Flying Hours !", this);
        //    txtFlyingHours.Focus();
        //    return;
        //}

        TimeSpan time;

        if (!TimeSpan.TryParseExact(txtFlyingHours.Text.Trim(),
                                    @"hh\:mm",
                                    null,
                                    out time))
        {
            objFun.MsgBox("Please enter valid time in HH:mm format!", this);
            txtFlyingHours.Focus();
            return;
        }
  
        int insert = objFun.ExecuteDML("INSERT INTO Student_DailyFlyingHours(StudentID, InstructorID, FlyingDate, FlyingHours, Remark, EntryBy, EntryDate) VALUES ('" + txtNameStudent.SelectedValue + "','" + ddlInstructor.SelectedValue + "','" + txtFlyingDate.Text + "','" + txtFlyingHours.Text + "', '" + txtRemark.Text + "', '" + Session["UID"] + "', '" + DateTime.Now + "' )");
       
        
        if (insert > 0)
        {

            objFun.MsgBox("Data Submited Successfully...", this);
            Reset();
            LoadHours();
            fillgrid();
            return;
        }
    }
    protected void txtFlyingHours_TextChanged(object sender, EventArgs e)
    {
        string value = txtFlyingHours.Text.Trim();

        TimeSpan time;
        if (TimeSpan.TryParseExact(value, @"hh\:mm", null, out time))
        {
            txtFlyingHours.Text = time.ToString(@"hh\:mm");
        }
        
    }
}
   

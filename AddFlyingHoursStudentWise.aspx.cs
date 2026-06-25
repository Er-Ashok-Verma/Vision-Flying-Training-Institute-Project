using NewDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Flying_Hour_AddFlyingHoursStudentWise : System.Web.UI.Page
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

            FillVouchers();
          
        }
        catch (Exception ex)
        {
            objFun.MsgBox(ex.Message, this);
            return;
        }

    }

    private void Reset()
    {
        txtSearchName.Text = "";
        txtNameStudent.SelectedValue = "";

        lblStuName.Text = "";
        txt_course_name.Text = "";

        ddlVoucher.Items.Clear();
        chkVou.Items.Clear();

        grdFee.DataSource = null;
        grdFee.DataBind();

        imgPhoto.ImageUrl = "";

        txtSearchName.Focus();
    }

    

    private void BindGrid(string voucher)
    {
        DataTable Feedetail = objFun.FillDataTable("SELECT StudentRegDetail.StudentDetailID,StudentRegDetail.StudentID,FeeHead.FeeHeadName,StudentRegDetail.FeeID,StudentRegDetail.TotalAmount,StudentRegDetail.PaidAmount,StudentRegDetail.BalanceAmount,StudentRegDetail.InstituteID,StudentRegDetail.VoucherNo,StudentRegDetail.VNo,StudentWise_FlyingHrs.Flying_Hrs,CASE WHEN StudentWise_FlyingHrs.StudentID IS NULL THEN 0 ELSE 1 END AS IsUpdated FROM StudentRegDetail INNER JOIN FeeHead ON StudentRegDetail.FeeID=FeeHead.FeeHeadId LEFT JOIN StudentWise_FlyingHrs ON StudentWise_FlyingHrs.StudentID=StudentRegDetail.StudentID AND StudentWise_FlyingHrs.VoucherNo=StudentRegDetail.VoucherNo AND StudentWise_FlyingHrs.FeeHeadID=StudentRegDetail.FeeID AND StudentWise_FlyingHrs.InstID=StudentRegDetail.InstituteID WHERE StudentRegDetail.StudentID='" + txtNameStudent.SelectedValue + "' AND StudentRegDetail.VoucherNo='" + voucher + "'");

        grdFee.DataSource = Feedetail;
        grdFee.DataBind();
    }

    public void FillVouchers()
    {
        try
        {
            FeeServices objF = new FeeServices();
            DataSet set = new DataSet();

            set = FillFeeVoucher_New(Convert.ToInt32(Session["InstID"].ToString()), Convert.ToInt32(txtNameStudent.SelectedValue), 1, Convert.ToInt32(0));
            
            DataTable fill_Voucher = new DataTable();
            DataTable fill_Voucher1 = new DataTable();
            if (set.Tables[0].Rows.Count > 0)
            {
                fill_Voucher = set.Tables[0];
            }
            if (set.Tables[1].Rows.Count > 0)
            {
                fill_Voucher1 = set.Tables[1];
            }
            ddlVoucher.DataSource = fill_Voucher;
            ddlVoucher.DataTextField = "VNo";
            ddlVoucher.DataValueField = "VoucherNo";
            ddlVoucher.DataBind();
            ddlVoucher.Items.Insert(0, "--Select--");

            if (fill_Voucher1.Rows.Count > 0)
            {
                // ddlVoucher.Visible = false;
                chkVou.DataSource = fill_Voucher1;
                chkVou.DataTextField = "VNo";
                chkVou.DataValueField = "VoucherNo";
                chkVou.DataBind();
            }
            else
            {
                ddlVoucher.Visible = true;
            }
        }
        catch (Exception ex)
        {
            objFun.MsgBox1(ex.Message, UpdatePanel1);
            return;
        }
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
 
    protected void grdFee_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtHr = (TextBox)e.Row.FindControl("txtHr");
            ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");

            string isUpdated = DataBinder.Eval(e.Row.DataItem, "IsUpdated").ToString();
            string flyingHrs = DataBinder.Eval(e.Row.DataItem, "Flying_Hrs").ToString();

            if (txtHr != null)
                txtHr.Text = flyingHrs;

            if (btnAdd != null)
            {
                if (isUpdated == "1") 
                {
                   // btnAdd.ImageUrl = "~/images/update.png";
                    btnAdd.ImageUrl = ResolveUrl("~/Update.png");
                    btnAdd.Width = Unit.Pixel(25);
                    btnAdd.Height = Unit.Pixel(25);
                    btnAdd.ToolTip = "Update Flying Hours";
                }
                else
                {
                    btnAdd.ImageUrl = "~/images/dash_add_icon.png";
                    btnAdd.ToolTip = "Add Flying Hours";
                }
            }
        }
    }
   
    protected void chkVou_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            grdFee.DataSource = null;
            grdFee.DataBind();
            string voucher = "";

            int selectedCount = 0;
            foreach (System.Web.UI.WebControls.ListItem item in chkVou.Items)
            {
                if (item.Selected)
                {
                    selectedCount++;
                    if (selectedCount > 1)
                    {
                        objFun.MsgBox("Please select only one voucher at a time...", this);
                        foreach (System.Web.UI.WebControls.ListItem li in chkVou.Items)
                        {
                            li.Selected = false;
                        }
                        return; // Stop execution
                    }
                    voucher = item.Value; // Store first selected value
                }
            }


            if (txtSearchName.Text == String.Empty)
            {
                objFun.MsgBox1("Select Student Name", UpdatePanel1);
                txtSearchName.Focus();
                return;
            }

            DataTable Feedetail = new DataTable();
           // Feedetail = objFun.FillDataTable("SELECT StudentRegDetail.StudentDetailID, StudentRegDetail.StudentID,FeeHead.FeeHeadName, StudentRegDetail.FeeID, StudentRegDetail.TotalAmount,StudentRegDetail.PaidAmount, StudentRegDetail.BalanceAmount,StudentRegDetail.InstituteID,StudentRegDetail.VoucherNo, StudentRegDetail.VNo FROM StudentRegDetail INNER JOIN FeeHead ON StudentRegDetail.FeeID = FeeHead.FeeHeadId WHERE (StudentRegDetail.StudentID ='" + txtNameStudent.SelectedValue + "') and StudentRegDetail.VoucherNo='" + voucher + "' ");
          //   Feedetail = objFun.FillDataTable("SELECT StudentRegDetail.StudentDetailID,StudentRegDetail.StudentID,FeeHead.FeeHeadName,StudentRegDetail.FeeID,StudentRegDetail.TotalAmount,StudentRegDetail.PaidAmount,StudentRegDetail.BalanceAmount,StudentRegDetail.InstituteID,StudentRegDetail.VoucherNo,StudentRegDetail.VNo,ISNULL(StudentWise_FlyingHrs.Flying_Hrs,0) AS Flying_Hrs,CASE WHEN StudentWise_FlyingHrs.StudentID IS NULL THEN 0 ELSE 1 END AS IsUpdated FROM StudentRegDetail INNER JOIN FeeHead ON StudentRegDetail.FeeID=FeeHead.FeeHeadId LEFT JOIN StudentWise_FlyingHrs ON StudentWise_FlyingHrs.StudentID=StudentRegDetail.StudentID AND StudentWise_FlyingHrs.VoucherNo=StudentRegDetail.VoucherNo AND StudentWise_FlyingHrs.FeeHeadID=StudentRegDetail.FeeID WHERE StudentRegDetail.StudentID='" + txtNameStudent.SelectedValue + "' AND StudentRegDetail.VoucherNo='" + voucher + "'");
            Feedetail = objFun.FillDataTable("SELECT StudentRegDetail.StudentDetailID,StudentRegDetail.StudentID,FeeHead.FeeHeadName,StudentRegDetail.FeeID,StudentRegDetail.TotalAmount,StudentRegDetail.PaidAmount,StudentRegDetail.BalanceAmount,StudentRegDetail.InstituteID,StudentRegDetail.VoucherNo,StudentRegDetail.VNo, StudentWise_FlyingHrs.Flying_Hrs,CASE WHEN StudentWise_FlyingHrs.StudentID IS NULL THEN 0 ELSE 1 END AS IsUpdated FROM StudentRegDetail INNER JOIN FeeHead ON StudentRegDetail.FeeID=FeeHead.FeeHeadId LEFT JOIN StudentWise_FlyingHrs ON StudentWise_FlyingHrs.StudentID=StudentRegDetail.StudentID AND StudentWise_FlyingHrs.VoucherNo=StudentRegDetail.VoucherNo AND StudentWise_FlyingHrs.FeeHeadID=StudentRegDetail.FeeID AND StudentWise_FlyingHrs.InstID=StudentRegDetail.InstituteID WHERE StudentRegDetail.StudentID='" + txtNameStudent.SelectedValue + "' AND StudentRegDetail.VoucherNo='" + voucher + "'");
            if (Feedetail.Rows.Count > 0)
            {
                grdFee.DataSource = Feedetail;
                grdFee.DataBind();
            }
            else
            {
                grdFee.DataSource = null;
                grdFee.DataBind();
            }
         


        }
        catch (Exception ex)
        {
            objFun.MsgBox1(ex.Message, UpdatePanel1);
            return;
        }
    }
    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btn = sender as ImageButton;
        GridViewRow row = btn.NamingContainer as GridViewRow;
        string FeeHeadID = grdFee.DataKeys[row.RowIndex].Values[1].ToString();
        string VoucherNo = grdFee.DataKeys[row.RowIndex].Values[2].ToString();
        string VN_No = grdFee.DataKeys[row.RowIndex].Values[3].ToString();
        TextBox txtHr = (TextBox)row.FindControl("txtHr");

        if (txtSearchName.Text == String.Empty || txtNameStudent.SelectedValue=="")
        {
            objFun.MsgBox1("Select Student Name", UpdatePanel1);
            txtSearchName.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(txtHr.Text))
        {
            objFun.MsgBox1("Please Enter Flying Hours", UpdatePanel1);
            txtHr.Focus();
            return;
        }

        string exists = objFun.Get_details("SELECT COUNT(*) FROM StudentWise_FlyingHrs WHERE StudentID='" + txtNameStudent.SelectedValue + "' AND VoucherNo='" + VoucherNo + "' AND FeeHeadID='" + FeeHeadID + "'");
        if (Convert.ToInt32(exists) > 0)
        {
          int count =  objFun.ExecuteDML("UPDATE StudentWise_FlyingHrs SET Flying_Hrs='" + txtHr.Text + "' WHERE StudentID='" + txtNameStudent.SelectedValue +"' AND VoucherNo='" + VoucherNo +"' AND FeeHeadID='" + FeeHeadID + "'");
            if (count > 0)
            {
                BindGrid(VoucherNo);

                objFun.MsgBox("Data Update Successfully...", this);
                 
                return;
            }
        }
        else
        {
            int insert = objFun.ExecuteDML("INSERT INTO StudentWise_FlyingHrs (StudentID, VoucherNo, VNNo, FeeHeadID, Flying_Hrs, EntryBy, EntryDate, InstID, SessionID) VALUES ('" + txtNameStudent.SelectedValue + "','" + VoucherNo + "','" + VN_No + "','" + FeeHeadID + "','" + txtHr.Text + "','" + Session["UID"] + "','" + DateTime.Now + "','" + Session["InstID"] + "','" + Session["sesnID"] + "')");
            if (insert > 0)
            {
                BindGrid(VoucherNo);

                objFun.MsgBox("Data Submited Successfully...", this);
                 
                return;
            }
        }
      
    }
}
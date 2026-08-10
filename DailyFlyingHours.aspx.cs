using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Configuration;
using System.Globalization;

public partial class Flying_Hour_DailyFlyingHours : System.Web.UI.Page
{
    DbFunctions objFun = new DbFunctions();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UID"] == null)
        {
            Response.Redirect("../error_404_2.html");
            return;
        }

        if (Session["InstID"] == null)
        {
            Response.Redirect("../error_404_2.html");
            return;
        }

        if (Session["sesnID"] == null)
        {
            Response.Redirect("../error_404_2.html");
            return;
        }

        if (!IsPostBack)
        {
            try
            {
                fillgrid();
                objFun.FillActivityLog(Convert.ToInt32(Session["UID"]), "Add Flying Hours StudentWise", "Visit Add Flying Hours StudentWisePage", Convert.ToInt32(Session["InstID"]));
            }
            catch (Exception ex)
            {
                objFun.MsgBox1(ex.Message, UpdatePanel1);
                return;
            }
        }
    }

    protected void btnDownloadExcel_Click(object sender, EventArgs e)
{
    Response.Clear();
    Response.Buffer = true;
    Response.AddHeader("content-disposition", "attachment;filename=FlyingDataTemplate.xls");
    Response.ContentType = "application/vnd.ms-excel";
    Response.Charset = "";
 
    Response.Write("StudentReg\tInstructorName\tFlyingDate\tFlyingHours");

    Response.End();
}
    protected void btnUploadExcel_Click(object sender, EventArgs e)
    {
        if (!fuExcel.HasFile)
        {
            objFun.MsgBox("Please select .xls or .csv file.", this);
            return;
        }

        string ext = Path.GetExtension(fuExcel.FileName).ToLower();

        if (ext != ".xls" && ext != ".csv")
        {
            objFun.MsgBox("Please upload only .xls or .csv file.", this);
            return;
        }

        string folder = Server.MapPath("~/Uploads/");

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string filePath = Path.Combine(folder, Guid.NewGuid().ToString() + ext);

        fuExcel.SaveAs(filePath);

        DataTable dt = new DataTable();

        dt.Columns.Add("StudentReg", typeof(string));
        dt.Columns.Add("InstructorName", typeof(string));
        dt.Columns.Add("FlyingDate", typeof(DateTime));
        dt.Columns.Add("FlyingHours", typeof(decimal));
        dt.Columns.Add("UploadedDate", typeof(DateTime));
        dt.Columns.Add("UploadedBy", typeof(int));

        try
        {
            string[] lines = File.ReadAllLines(filePath);

            char separator = (ext == ".csv") ? ',' : '\t';

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                string[] cols = lines[i].Split(separator);

                if (cols.Length < 4)
                    continue;

                DataRow dr = dt.NewRow();

                dr["StudentReg"] = cols[0].Trim();
                dr["InstructorName"] = cols[1].Trim();

                // Flying Date
                DateTime flyingDate;
                string[] formats = { "dd/MM/yyyy", "dd-MM-yyyy", "dd MMM yyyy", "yyyy-MM-dd", "MM/dd/yyyy" };

                if (DateTime.TryParseExact(cols[2].Trim(), formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out flyingDate))
                {
                    dr["FlyingDate"] = flyingDate;
                }
                else if (DateTime.TryParse(cols[2].Trim(), out flyingDate))
                {
                    dr["FlyingDate"] = flyingDate;
                }
                else
                {
                    objFun.MsgBox("Invalid Flying Date at row : " + (i + 1), this);
                    return;
                }

                // Flying Hours
                decimal flyingHours;

                if (!decimal.TryParse(cols[3].Trim(), out flyingHours))
                {
                    objFun.MsgBox("Invalid Flying Hours at row : " + (i + 1), this);
                    return;
                }

                dr["FlyingHours"] = flyingHours;

                dr["UploadedDate"] = DateTime.Now;
                dr["UploadedBy"] = Convert.ToInt32(Session["UID"]);

                dt.Rows.Add(dr);
            }

            if (dt.Rows.Count == 0)
            {
                objFun.MsgBox("No data found in file.", this);
                return;
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FeesManagementConn"].ConnectionString))
            {
                con.Open();

                using (SqlBulkCopy bulk = new SqlBulkCopy(con))
                {
                    bulk.DestinationTableName = "ExcelFlyingData";

                    bulk.ColumnMappings.Add("StudentReg", "StudentReg");
                    bulk.ColumnMappings.Add("InstructorName", "InstructorName");
                    bulk.ColumnMappings.Add("FlyingDate", "FlyingDate");
                    bulk.ColumnMappings.Add("FlyingHours", "FlyingHours");
                    bulk.ColumnMappings.Add("UploadedDate", "UploadedDate");
                    bulk.ColumnMappings.Add("UploadedBy", "UploadedBy");

                    bulk.WriteToServer(dt);
                }
            }

            objFun.MsgBox(dt.Rows.Count + " record(s) uploaded successfully.", this);

            fillgrid();
            Reset();
        }
        catch (Exception ex)
        {
            objFun.MsgBox(ex.Message, this);
        }
        finally
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    } 

    public void fillgrid()
    {
        DataTable dt = new DataTable();

        dt = objFun.FillDataTable("SELECT ExcelFlyingData.StudentReg, ExcelFlyingData.InstructorName, CONVERT(VARCHAR(11), ExcelFlyingData.FlyingDate,106) AS FlyingDate, ExcelFlyingData.FlyingHours, CONVERT(VARCHAR(11), ExcelFlyingData.UploadedDate,106) AS UploadedDate, Employee_Master.empName+' '+Employee_Master.MiddelName+' '+Employee_Master.lastName AS UploadedBy, ISNULL(ExcelFlyingData.Status,'Not Match') AS Status FROM ExcelFlyingData INNER JOIN Employee_Master ON ExcelFlyingData.UploadedBy=Employee_Master.empId WHERE ISNULL(ExcelFlyingData.Status,'')<>'Saved' ORDER BY ExcelFlyingData.UploadedDate DESC;");

        if (dt.Rows.Count > 0)
        {
            gridview.DataSource = dt;
            gridview.DataBind();

        }
        else
        {
            gridview.DataSource = null;
            gridview.DataBind();
            objFun.MsgBox("Record not found!", this);
            return;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FeesManagementConn"].ConnectionString))
            {
                con.Open();

                // Pending records ko Matched karo
                string updateStatus = "UPDATE EFD SET Status='Matched' FROM ExcelFlyingData EFD INNER JOIN StudentReg SR ON EFD.StudentReg = SR.RegNo INNER JOIN Instructor_Master IM ON EFD.InstructorName = IM.Name WHERE ISNULL(EFD.Status,'Pending')='Pending';";

                new SqlCommand(updateStatus, con).ExecuteNonQuery();
 
                string sql = "INSERT INTO Student_DailyFlyingHours (StudentID, InstructorID, FlyingDate, FlyingHours, Remark, EntryBy, EntryDate) SELECT SR.StudentID, IM.ID, EFD.FlyingDate, CAST(DATEADD(SECOND, CAST(EFD.FlyingHours * 3600 AS INT), '00:00:00') AS TIME), '', EFD.UploadedBy, EFD.UploadedDate FROM ExcelFlyingData EFD INNER JOIN StudentReg SR ON EFD.StudentReg = SR.RegNo INNER JOIN Instructor_Master IM ON EFD.InstructorName = IM.Name WHERE EFD.Status='Matched';";

                int count = new SqlCommand(sql, con).ExecuteNonQuery();
 
                string updateSaved = "UPDATE ExcelFlyingData SET Status='Saved' WHERE Status='Matched'";
                new SqlCommand(updateSaved, con).ExecuteNonQuery();

                objFun.MsgBox(count + " record(s) saved successfully.", this);

                fillgrid();
            }
        }
        catch (Exception ex)
        {
            objFun.MsgBox(ex.Message, this);
        }
    }

    private void Reset()
    {
        
        fuExcel.Attributes.Clear();
        fillgrid();
    }
}

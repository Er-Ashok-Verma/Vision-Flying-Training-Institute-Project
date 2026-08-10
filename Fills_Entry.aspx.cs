using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Flying_Hour_Fills_Entry : System.Web.UI.Page
{
    DbFunctions objFunc = new DbFunctions();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ViewFile"] != null && Request.QueryString["ID"] != null)
        {
            ViewFile();
            return;
        }

        if (Session["UID"] == null)
        {
            Response.Redirect("../error_404_2.html");
            return;
        }

        if (Session["instID"] == null)
        {
            Response.Redirect("../error_404_2.html");
            return;
        }

        if (Session["SesnID"] == null)
        {
            Response.Redirect("../error_404_2.html");
            return;
        }

        if (!IsPostBack)
        {
            fillgrid();
        }
    }

    public void fillgrid()
    {
        DataTable dt = objFunc.FillDataTable("SELECT StudentSelfRegistration.ID,StudentSelfRegistration.StudentName,StudentSelfRegistration.MobileNo,StudentSelfRegistration.Email,StudentSelfRegistration.Gender,StudentSelfRegistration.Photo,StudentSelfRegistration.PhotoFileName,StudentSelfRegistration.Marksheet10,StudentSelfRegistration.MarksheetFileName,CONVERT(VARCHAR(11),StudentSelfRegistration.EntryDate,106) AS EntryDate,StudentSelfRegistration.EntryBy,StudentSelfRegistration.Status,StudentSelfRegistration.CourseID,Course.CourseName,StudentSelfRegistration.FatherName,StudentSelfRegistration.MotherName,StudentSelfRegistration.DOB,StudentSelfRegistration.AadhaarNo,StudentSelfRegistration.StudentPAN,StudentSelfRegistration.FatherMobNo,StudentSelfRegistration.FatherEmail,StudentSelfRegistration.FatherAadhaar,StudentSelfRegistration.FatherPAN,StudentSelfRegistration.MotherMobNo,StudentSelfRegistration.MotherEmail,StudentSelfRegistration.MotherAadhaar,StudentSelfRegistration.SPLNumber,StudentSelfRegistration.ComputerNumber,StudentSelfRegistration.PCityID,StudentSelfRegistration.PStateID,StudentSelfRegistration.PCountryID,StudentSelfRegistration.PAddress,StudentSelfRegistration.PZipCode,StudentSelfRegistration.CCityID,StudentSelfRegistration.CStateID,StudentSelfRegistration.CCountryID,StudentSelfRegistration.CAddress,StudentSelfRegistration.CZipCode,StudentSelfRegistration.StudentSign,StudentSelfRegistration.StudentSignFileName FROM StudentSelfRegistration INNER JOIN Course ON StudentSelfRegistration.CourseID=Course.CourseId WHERE StudentSelfRegistration.Status='0'");

        if (dt.Rows.Count > 0)
        {
            gridview.DataSource = dt;
            gridview.DataBind();
        }
        else
        {
            gridview.DataSource = null;
            gridview.DataBind();
            objFunc.MsgBox("Record not found!", this);
        }
    }

    protected void lnkFillData_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        int id = Convert.ToInt32(btn.CommandArgument);

        Response.Redirect("../Academic/Student_Registration_Vision.aspx?ID=" + id);
    }

    private void ViewFile()
    {
        int id;

        if (!int.TryParse(Request.QueryString["ID"], out id))
            return;

        string type = Request.QueryString["ViewFile"];

        string sql = "";

        if (type == "Marksheet")
        {
            sql = "SELECT Marksheet10 FROM StudentSelfRegistration WHERE ID=@ID";
        }
        else if (type == "Sign")
        {
            sql = "SELECT StudentSign FROM StudentSelfRegistration WHERE ID=@ID";
        }
        else
        {
            return;
        }

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FeesManagementConn"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                con.Open();

                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    return;

                byte[] fileData = result as byte[];

                if (fileData == null || fileData.Length == 0)
                    return;

                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.Buffer = true;

                if (type == "Marksheet")
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "inline;filename=Marksheet.pdf");
                }
                else if (type == "Sign")
                {
                    Response.ContentType = "image/jpeg";
                    Response.AddHeader("Content-Disposition", "inline;filename=StudentSign.jpg");
                }

                Response.BinaryWrite(fileData);
                Response.Flush();
                Response.End();
            }
        }
    }
}
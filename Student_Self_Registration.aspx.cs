using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class Flying_Hour_Student_Self_Registration : System.Web.UI.Page
{
    DbFunctions objfun = new DbFunctions();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UID"] == null ||
            Session["instID"] == null ||
            Session["SesnID"] == null)
        {
            Response.Redirect("../error_404_2.html");
        }

        if (!IsPostBack)
        {

             objfun.FillCity(ddlCity);

                objfun.FillState(ddlState);

                objfun.FillCity(ddlPCity);

                objfun.FillState(ddlPState);

                objfun.FillCountry(ddlPCountry);

                objfun.FillCountry(ddlCountry);
                objfun.FillActivityLog(Convert.ToInt32(Session["UID"]), "Student Registration", "Visit Student Self Registration", Convert.ToInt32(Session["instID"].ToString()));
            
                 objfun.FillDropdownCheckbox(wizReg_ddlCourse, "CourseName","CourseId", "SELECT CourseId, CourseName FROM Course", "---Select---");
           
        }
         
    }
    protected void ddlPCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        Academicsvc objA = new Academicsvc();
        List<Academic.AcademicData.StudentDetailCityDM> objCity = new List<Academic.AcademicData.StudentDetailCityDM>();

        if (((DropDownList)sender).ID == "ddlCity")
        {
            objCity = objA.FillStudentDetailCity(Convert.ToInt32(ddlCity.SelectedValue), 7);

            if (objCity.Count > 0)
            {
                ddlState.SelectedValue = objCity[0].stateid.ToString();
                ddlCountry.SelectedValue = objCity[0].countryid.ToString();
            }
        }

        if (((DropDownList)sender).ID == "ddlPCity")
        {
            objCity = objA.FillStudentDetailCity(Convert.ToInt32(ddlPCity.SelectedValue), 7);

            if (objCity.Count > 0)
            {
                ddlPState.SelectedValue = objCity[0].stateid.ToString();
                ddlPCountry.SelectedValue = objCity[0].countryid.ToString();
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtStudentName.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Student Name.!", this);
            txtStudentName.Focus();
            return;
        }

        if (txtFatherName.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Father Name.!", this);
            txtFatherName.Focus();
            return;
        }

        if (txtMotherName.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Mother Name.!", this);
            txtMotherName.Focus();
            return;
        }

        if (txtDOB.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Date of Birth.!", this);
            txtDOB.Focus();
            return;
        }

        if (ddlGender.SelectedIndex == 0)
        {
            objfun.MsgBox("Please Select Gender.!", this);
            ddlGender.Focus();
            return;
        }

        if (txtMobileNo.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Student Mobile No.!", this);
            txtMobileNo.Focus();
            return;
        }

        if (txtEmail.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Student Email.!", this);
            txtEmail.Focus();
            return;
        }

        if (txtAadhaarNo.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Aadhaar Number.!", this);
            txtAadhaarNo.Focus();
            return;
        }

        if (txtStudentPAN.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Student PAN Number.!", this);
            txtStudentPAN.Focus();
            return;
        }

        if (txtFatherMobNo.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Father Mobile No.!", this);
            txtFatherMobNo.Focus();
            return;
        }

        if (txtFatherEmail.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Father Email ID.!", this);
            txtFatherEmail.Focus();
            return;
        }

        if (txtFatherAadhaar.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Father Aadhaar No.!", this);
            txtFatherAadhaar.Focus();
            return;
        }

        if (txtFatherPAN.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Father PAN Number.!", this);
            txtFatherPAN.Focus();
            return;
        }


        if (txtMotherMobNo.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Mother Mobile No.!", this);
            txtMotherMobNo.Focus();
            return;
        }

        if (txtMotherEmail.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Mother Email ID.!", this);
            txtMotherEmail.Focus();
            return;
        }

        if (txtMotherAadhaar.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Mother Aadhaar No.!", this);
            txtMotherAadhaar.Focus();
            return;
        }

        if (wizReg_ddlCourse.SelectedIndex == -1)
        {
            objfun.MsgBox("Please Select Class.!", this);
            return;
        }

        if (ddlPCity.SelectedIndex == 0)
        {
            objfun.MsgBox("Please Select Permanent City.!", this);
            return;
        }

        if (ddlPState.SelectedIndex == 0)
        {
            objfun.MsgBox("Please Select Permanent State.!", this);
            return;
        }

        if (ddlPCountry.SelectedIndex == 0)
        {
            objfun.MsgBox("Please Select Permanent Country.!", this);
            return;
        }

        if (txtPAddress.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Permanent Address.!", this);
            return;
        }

        if (txtPZip.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Permanent PIN Code.!", this);
            return;
        }

        if (ddlCity.SelectedIndex == 0)
        {
            objfun.MsgBox("Please Select Current City.!", this);
            return;
        }

        if (ddlState.SelectedIndex == 0)
        {
            objfun.MsgBox("Please Select Current State.!", this);
            return;
        }

        if (ddlCountry.SelectedIndex == 0)
        {
            objfun.MsgBox("Please Select Current Country.!", this);
            return;
        }

        if (txtCAddress.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Current Address.!", this);
            return;
        }

        if (txtCZip.Text.Trim() == "")
        {
            objfun.MsgBox("Please Enter Current PIN Code.!", this);
            return;
        }

        if (!filePhoto.HasFile)
        {
            objfun.MsgBox("Please Upload Student Photo.", this);
            return;
        }

        if (!fu10thMarksheet.HasFile)
        {
            objfun.MsgBox("Please Upload 10th Marksheet.", this);
            return;
        }

        if (!fuStudentSign.HasFile)
        {
            objfun.MsgBox("Please Upload Student Sign.", this);
            return;
        }

        byte[] photo = filePhoto.FileBytes;
        byte[] marksheet = fu10thMarksheet.FileBytes;
        byte[] sign = fuStudentSign.FileBytes;

        string photoName = Path.GetFileName(filePhoto.FileName);
        string marksheetName = Path.GetFileName(fu10thMarksheet.FileName);
        string signName = Path.GetFileName(fuStudentSign.FileName);

        string CourseID = "";

        foreach (ListItem item in wizReg_ddlCourse.Items)
        {
            if (item.Selected)
            {
                CourseID += item.Value + ",";
            }
        }

        CourseID = CourseID.TrimEnd(',');

        string sql = "INSERT INTO StudentSelfRegistration (StudentName,FatherName,MotherName,DOB,MobileNo,Email,Gender,AadhaarNo,StudentPAN,FatherMobNo,FatherEmail,FatherAadhaar,FatherPAN,MotherMobNo,MotherEmail,MotherAadhaar,CourseID,SPLNumber,ComputerNumber,PCityID,PStateID,PCountryID,PAddress,PZipCode,CCityID,CStateID,CCountryID,CAddress,CZipCode,Photo,PhotoFileName,Marksheet10,MarksheetFileName,StudentSign,StudentSignFileName,EntryDate,EntryBy,Status) VALUES (@StudentName,@FatherName,@MotherName,@DOB,@MobileNo,@Email,@Gender,@AadhaarNo,@StudentPAN,@FatherMobNo,@FatherEmail,@FatherAadhaar,@FatherPAN,@MotherMobNo,@MotherEmail,@MotherAadhaar,@CourseID,@SPLNumber,@ComputerNumber,@PCityID,@PStateID,@PCountryID,@PAddress,@PZipCode,@CCityID,@CStateID,@CCountryID,@CAddress,@CZipCode,@Photo,@PhotoFileName,@Marksheet10,@MarksheetFileName,@StudentSign,@StudentSignFileName,GETDATE(),@EntryBy,@Status)";

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FeesManagementConn"].ConnectionString))
        {
            con.Open();

            // Mobile Duplicate Check
            SqlCommand cmdMobile = new SqlCommand("SELECT COUNT(*) FROM StudentSelfRegistration WHERE MobileNo=@MobileNo", con);
            cmdMobile.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text.Trim());

            if (Convert.ToInt32(cmdMobile.ExecuteScalar()) > 0)
            {
                objfun.MsgBox("Mobile No. already exists.", this);
                return;
            }

            // Email Duplicate Check
            SqlCommand cmdEmail = new SqlCommand("SELECT COUNT(*) FROM StudentSelfRegistration WHERE Email=@Email", con);
            cmdEmail.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

            if (Convert.ToInt32(cmdEmail.ExecuteScalar()) > 0)
            {
                objfun.MsgBox("Email already exists.", this);
                return;
            }

            SqlCommand cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@StudentName", txtStudentName.Text.Trim());
            cmd.Parameters.AddWithValue("@FatherName", txtFatherName.Text.Trim());
            cmd.Parameters.AddWithValue("@MotherName", txtMotherName.Text.Trim());

            DateTime dob = DateTime.ParseExact(txtDOB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            cmd.Parameters.AddWithValue("@DOB", dob);

            cmd.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text.Trim());
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedItem.Text);

            cmd.Parameters.AddWithValue("@AadhaarNo", txtAadhaarNo.Text.Trim());
            cmd.Parameters.AddWithValue("@StudentPAN", txtStudentPAN.Text.Trim());

            cmd.Parameters.AddWithValue("@FatherMobNo", txtFatherMobNo.Text.Trim());
            cmd.Parameters.AddWithValue("@FatherEmail", txtFatherEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@FatherAadhaar", txtFatherAadhaar.Text.Trim());
            cmd.Parameters.AddWithValue("@FatherPAN", txtFatherPAN.Text.Trim());

            cmd.Parameters.AddWithValue("@MotherMobNo", txtMotherMobNo.Text.Trim());
            cmd.Parameters.AddWithValue("@MotherEmail", txtMotherEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@MotherAadhaar", txtMotherAadhaar.Text.Trim());

            cmd.Parameters.AddWithValue("@CourseID", CourseID);
            cmd.Parameters.AddWithValue("@SPLNumber", txtSPLNumber.Text.Trim());
            cmd.Parameters.AddWithValue("@ComputerNumber", txtComputerNumber.Text.Trim());

            cmd.Parameters.AddWithValue("@PCityID", ddlPCity.SelectedValue);
            cmd.Parameters.AddWithValue("@PStateID", ddlPState.SelectedValue);
            cmd.Parameters.AddWithValue("@PCountryID", ddlPCountry.SelectedValue);
            cmd.Parameters.AddWithValue("@PAddress", txtPAddress.Text.Trim());
            cmd.Parameters.AddWithValue("@PZipCode", txtPZip.Text.Trim());

            cmd.Parameters.AddWithValue("@CCityID", ddlCity.SelectedValue);
            cmd.Parameters.AddWithValue("@CStateID", ddlState.SelectedValue);
            cmd.Parameters.AddWithValue("@CCountryID", ddlCountry.SelectedValue);
            cmd.Parameters.AddWithValue("@CAddress", txtCAddress.Text.Trim());
            cmd.Parameters.AddWithValue("@CZipCode", txtCZip.Text.Trim());

            cmd.Parameters.Add("@Photo", SqlDbType.VarBinary).Value = photo;
            cmd.Parameters.AddWithValue("@PhotoFileName", photoName);

            cmd.Parameters.Add("@Marksheet10", SqlDbType.VarBinary).Value = marksheet;
            cmd.Parameters.AddWithValue("@MarksheetFileName", marksheetName);

            cmd.Parameters.Add("@StudentSign", SqlDbType.VarBinary).Value = sign;
            cmd.Parameters.AddWithValue("@StudentSignFileName", signName);

            cmd.Parameters.AddWithValue("@EntryBy", Session["UID"]);

            // New Entry = Status 0
            cmd.Parameters.AddWithValue("@Status", 0);

            cmd.ExecuteNonQuery();
        }

        objfun.MsgBox("Registration Successfully.", this);
        Reset();
    }
 
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }

    public void Reset()
    {
        txtStudentName.Text = "";
        txtFatherName.Text = "";
        txtMotherName.Text = "";
        txtDOB.Text = "";

        txtMobileNo.Text = "";
        txtEmail.Text = "";
        ddlGender.SelectedIndex = 0;

        txtAadhaarNo.Text = "";
        txtStudentPAN.Text = "";

        txtFatherMobNo.Text = "";
        txtFatherEmail.Text = "";
        txtFatherAadhaar.Text = "";
        txtFatherPAN.Text = "";

        txtMotherMobNo.Text = "";
        txtMotherEmail.Text = "";
        txtMotherAadhaar.Text = "";

        txtSPLNumber.Text = "";
        txtComputerNumber.Text = "";

        ddlPCity.SelectedIndex = 0;
        ddlPState.SelectedIndex = 0;
        ddlPCountry.SelectedIndex = 0;
        txtPAddress.Text = "";
        txtPZip.Text = "";

        ddlCity.SelectedIndex = 0;
        ddlState.SelectedIndex = 0;
        ddlCountry.SelectedIndex = 0;
        txtCAddress.Text = "";
        txtCZip.Text = "";

        foreach (ListItem item in wizReg_ddlCourse.Items)
        {
            item.Selected = false;
        }

        imgPhoto.ImageUrl = "~/assets/images/User.jpg";

        lblPhotoName.Text = "";
        lbl10thFileName.Text = "";
        lblSignFileName.Text = "";

        ViewState["PhotoPath"] = null;
        ViewState["MarksheetPath"] = null;
        ViewState["SignPath"] = null;

        ViewState["_StudentPhoto"] = null;
        ViewState["fu10thMarksheet"] = null;
        ViewState["_StudentSign"] = null;
    }
   
}
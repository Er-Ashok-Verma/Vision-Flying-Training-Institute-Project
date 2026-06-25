using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NewDAL;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Data.SqlClient;
using MSS;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Net;
using System.Configuration;
using System.Text.RegularExpressions;

public partial class Academic_Student_Registration_Vision : System.Web.UI.Page
{
    DbFunctions objFunc = new DbFunctions();
    private StreamWriter streamWriter;
    string TallyServerIP = string.Empty;
    string str_BillLocationList = string.Empty;
    string str_AllLedgerEntries = string.Empty;
    string AllRegNos = "";
    string Allcourse = "";
    string allcourseids = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UID"] == null)
        {
            Response.Redirect("../error_404_2.html");
            return;
        }

        if (Session["instID"].ToString() == null)
        {
            Response.Redirect("../error_404_2.html");
        }

        if (Session["sesnID"].ToString() == null)
        {
            Response.Redirect("../error_404_2.html");
        }
        Response.Cache.SetNoStore();
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        if (!IsPostBack)
        {
            try
            {
                wizReg.HeaderText = wizReg.WizardSteps[wizReg.ActiveStepIndex].Title.ToString();
                if (Context.Request.QueryString["title"] != null)
                {
                    lbltitle.InnerText = Context.Request.QueryString["title"].ToString();
                }
                objFunc.FillCity(ddlcity);

                objFunc.FillState(ddlState);

                objFunc.FillCity(ddlPcity);

                objFunc.FillState(ddlPState);

                objFunc.FillCountry(ddlPCountry);

                objFunc.FillCountry(ddlCountry);
                objFunc.Fill_Alloted_Institutes(ddlInstitute, Context.Request.QueryString["inst_ids"].ToString(), "---Select---");
                ddlInstitute.Items.RemoveAt(0);
                ddlInstitute.SelectedValue = Session["instID"].ToString();
                ddlInstitute_SelectedIndexChanged(sender, e);
                objFunc.FillActivityLog(Convert.ToInt32(Session["UID"]), "Student Registration", "Visit Student Registration", Convert.ToInt32(Session["instID"].ToString()));
            }

            catch (Exception ex)
            {
                objFunc.MsgBox1(ex.Message, UpdatePanel1);
                return;
            }
            wizReg_txtContact_TextChanged(sender, e);
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "initializeSelect2", "initializeSelect2();", true);
    }

    protected void ddlInstitute_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(objFunc.Get_details("Select Org_Type from college where CollegeID = " + ddlInstitute.SelectedValue + "")) == 2)
            {
                Div_Spec.Style.Add("display", "none");
                Div_YrSem.Style.Add("display", "none");
                Div_SpecSem2.Style.Add("display", "none");
                Div_YrSem2.Style.Add("display", "none");
                lbl_Course.Text = "Class";
                lbl_CourseCaption.Text = "Class";
            }
            else
            {
                Div_Spec.Style.Add("display", "block");
                Div_YrSem.Style.Add("display", "block");
                Div_SpecSem2.Style.Add("display", "block");
                Div_YrSem2.Style.Add("display", "block");
                lbl_Course.Text = "Course";
                lbl_CourseCaption.Text = "Course-Specialization";
            }

           // objFunc.FillCourse(wizReg_ddlCourse, ddlInstitute.SelectedValue, "---Select---");
            objFunc.FillDropdownCheckbox(wizReg_ddlCourse, "CourseName", "CourseId", "SELECT CourseId, CourseName FROM Course", "---Select---");

            //objFunc.Fillsession(wizReg_ddlSession, ddlInstitute.SelectedValue, "", "---Select---");
            // select rtrim(Session)  as sessionyear  from Session where active='True' and SessionID in ('11','12','13','15','14','16','6','7')  order by sessionyear
            //objFunc.FillDropdownlist(wizReg_ddlSession, "sessionyear", "sessionyear", "select rtrim(Session)  as sessionyear  from Session where active='True' and SessionID in ('11','12','13','15','14','16','6','7')  order by sessionyear", "--Select--");   
            if (Context.Request.QueryString["PermissionType"] == "A")
            {
                objFunc.FillDropdownlist(wizReg_ddlSession, "sessionyear", "sessionyear", "select rtrim(Session)  as sessionyear  from Session where active='True' and SessionID in ('11','12','13','15','14','16','6','7')  order by sessionyear", "--Select--");
            }
            else
            {
                objFunc.FillDropdownlist(wizReg_ddlSession, "sessionyear", "sessionyear", "select rtrim(Session)  as sessionyear  from Session where active='True' and CurrentSession in ('1','Yes')  order by sessionyear", "--Select--");
            }
            // objFunc.FillDropdownlist(wizReg_ddlSession, "sessionyear", "sessionyear", "select rtrim(Session)  as sessionyear  from Session where active='True' and CurrentSession in ('1','Yes') and SessionID!=7  order by sessionyear", "--Select--");
            Academicsvc objSelect = new Academicsvc();

            // objSelect.FillCastCategory(wizReg_ddlCategory, 25, "---Select---");
            objSelect.FillAdmissionSource(wizReg_ddlAdmSource, int.Parse(ddlInstitute.SelectedValue), 27, "---Select---");
            objSelect.FillAdmissionType(wizReg_ddlAdmType, int.Parse(ddlInstitute.SelectedValue), 26, "---Select---");

            if (wizReg_ddlAdmSource.Items.Count == 2)
            {
                wizReg_ddlAdmSource.SelectedIndex = 1;
                Div_AdmissionSource.Style.Add("display", "none");
            }
            else
            {
                Div_AdmissionSource.Style.Add("display", "block");
            }

            if (wizReg_ddlAdmType.Items.Count == 2)
            {
                wizReg_ddlAdmType.SelectedIndex = 1;
                Div_AdmType.Style.Add("display", "none");
                wizReg_ddlAdmType_SelectedIndexChanged(sender, e);
            }
            else
            {
                Div_AdmType.Style.Add("display", "block");
            }

            Button btn = (Button)wizReg.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton");
            btn.Text = "Next";
            wizReg_txtStudentName.Focus();
        }
        catch (Exception ex)
        {
            objFunc.MsgBox1(ex.Message, UpdatePanel1);
            return;
        }
    }


    protected void btnSearchStu_Click(object sender, EventArgs e)
    {
        try
        {
            int StudentIDD = Int32.Parse(txtNameStudent.SelectedValue);
            Academicsvc objA = new Academicsvc();
            List<Academic.AcademicData.StudentVerifyDetailDM> objSVD = new List<Academic.AcademicData.StudentVerifyDetailDM>();
            objSVD = objA.FillVerifyStudentDetail(Convert.ToInt32(ddlInstitute.SelectedValue), StudentIDD, 6);
            wizReg_txtStudentName.Text = objSVD[0].StudentName;
            wizReg_txtFatherName.Text = objSVD[0].FatherName;
            wizReg_ddlGender.SelectedValue = objSVD[0].Gender;
            wizReg_txtDOB.Text = objSVD[0].DateOfBirth.ToString("dd-MMM-yyyy");
            wizReg_txtContact.Text = objSVD[0].ContactNo;
            //  wizReg_ddlCategory.SelectedValue = objSVD[0].Category;
            wizReg_ddlAdmSource.SelectedValue = objSVD[0].AdmissionSource;
            wizReg_ddlAdmType.SelectedValue = objSVD[0].AdmissionType;
            Session["sesnID"] = objSVD[0].SessionID;
            wizReg_ddlCourse.SelectedValue = objSVD[0].Courseid.ToString();
            objFunc.FillSpecilization(this.wizReg_ddlSpec, ddlInstitute.SelectedValue, wizReg_ddlCourse.SelectedValue, "--Select--");
            wizReg_ddlSpec.SelectedValue = objFunc.Get_details("select Specialization from StudentReg where StudentID='" + txtNameStudent.SelectedValue + "' and InstituteID = '" + ddlInstitute.SelectedValue + "' and courseid='" + wizReg_ddlCourse.SelectedValue + "'");
            wizReg_ddlSession_SelectedIndexChanged(sender, e);
            wizReg_txtStudentName.Focus();
            objFunc.FillYearsem(wizReg_ddlYrSem, hdnCType.Value, Convert.ToInt32(wizReg_ddlCourse.SelectedValue), "0", Session["sesnID"].ToString(), Convert.ToInt32(ddlInstitute.SelectedValue), "---Select---");
            wizReg_ddlYrSem.SelectedValue = objSVD[0].Sid.ToString();
            string hostel = objFunc.Get_details("select HostelFacilityReq from StudentReg where StudentID='" + txtNameStudent.SelectedValue + "' and InstituteID = '" + ddlInstitute.SelectedValue + "'");
            string bus = objFunc.Get_details("select BusFacilityReq from StudentReg where StudentID='" + txtNameStudent.SelectedValue + "' and InstituteID = '" + ddlInstitute.SelectedValue + "'");
            if (hostel == "True")
            {
                wizReg_chkHostel.Checked = true;
            }
            else if (hostel == "False")
            {
                wizReg_chkHostel.Checked = false;
            }
            if (bus != "")
            {
                wizReg_RBLMOT.SelectedValue = bus;
            }
            Button btn = (Button)wizReg.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton");
            btn.Text = "Update";
        }
        catch (Exception ex)
        {
            objFunc.MsgBox1(ex.Message, UpdatePanel1);
            return;
        }
    }

    public void Reset()
    {
        txtAadhaar.Text = String.Empty;
        wizReg_txtStudentName.Text = String.Empty;
        wizReg_txtFatherName.Text = String.Empty;
        wizReg_txtDOB.Text = String.Empty;
        //txtFHindi.Text = String.Empty;
        //TxtMhindi.Text = String.Empty;
        //TextBox3.Text = String.Empty;
        wizReg_txtContact.Text = String.Empty;
        // wizReg_ddlCategory.SelectedIndex = 0;
        lbl10thFileName.Text="";
        lblsign.Text = "";

        wizReg_ddlCourse.SelectedIndex = 0;
        wizReg_ddlSession.SelectedIndex = 0;
        //wizReg_ddlYrSem.SelectedIndex = 0;
        //wizReg_RBLMOT.ClearSelection();
        wizReg_chkHostel.Checked = false;
        hdnRegID.Value = String.Empty;
        hdnCType.Value = String.Empty;
        hdnStatus.Value = String.Empty;
        lblRegNo.Text = String.Empty;
        lblSession.Text = String.Empty;
        lblFatherName.Text = String.Empty;
        lblCategory.Text = String.Empty;
        lblContact.Text = String.Empty;
        lblCourse.Text = String.Empty;
        lblSession.Text = String.Empty;
        lblYrSem.Text = String.Empty;
        gvFee.DataSource = null;
        gvFee.DataBind();
        hdnbatch.Value = String.Empty;
        ViewState["documentDes"] = null;
        hdnStuID.Value = string.Empty;
        txtmotherName.Text = "";
        txtCAddress.Text = "";
        ddlcity.SelectedIndex = 0;
        ddlCountry.SelectedIndex = 0;
        ddlState.SelectedIndex = 0;
        imgPhoto.ImageUrl = "~/assets/images/User.jpg";
        wizReg_txtStudentName.Focus();

        ///------////
        ///// Student Details
        txtemail.Text = String.Empty;
        txtStudentPANNumber.Text = String.Empty;

        // Parent Details
        txtFatherMobNo.Text = String.Empty;
        txtFemail.Text = String.Empty;
        txtFatherAadharNo.Text = String.Empty;
        FatherPANNumber.Text = String.Empty;

        txtMotherMobNo.Text = String.Empty;
        txtMemail.Text = String.Empty;
        txtMotherAadharNo.Text = String.Empty;

        // Academic Details
        txtSPLNumber.Text = String.Empty;
        txtComputerNumber.Text = String.Empty;

        // Dropdowns
        wizReg_ddlGender.SelectedIndex = 0;

        ddlPcity.SelectedIndex = 0;
        ddlPState.SelectedIndex = 0;
        ddlPCountry.SelectedIndex = 0;

        // Permanent Address
        txtPZip.Text = String.Empty;
        txtPAddress.Text = String.Empty;

        // Current Address
        txtCZip.Text = String.Empty;

        // Aadhaar Verify Tick
        Span1.Style["display"] = "none";
    }

    public void UpdateReset()
    {
        wizReg_txtStudentName.Text = String.Empty;
        txtSearchName.Text = String.Empty;
        wizReg_txtFatherName.Text = String.Empty;
        wizReg_txtDOB.Text = String.Empty;
        //txtFHindi.Text = String.Empty;
        //TxtMhindi.Text = String.Empty;
        //TextBox3.Text = String.Empty;
        wizReg_txtContact.Text = String.Empty;
        // wizReg_ddlCategory.SelectedIndex = 0;
        //wizReg_ddlAdmSource.SelectedIndex = 0;
        //wizReg_ddlAdmType.SelectedIndex = 0;
        wizReg_ddlCourse.SelectedIndex = 0;
        wizReg_ddlSession.SelectedIndex = 0;
        //wizReg_ddlYrSem.SelectedIndex = 0;
        wizReg_RBLMOT.ClearSelection();
        txtNameStudent.SelectedValue = String.Empty;
        wizReg_ddlGender.SelectedIndex = 0;
        //wizReg_ddlSpec.SelectedIndex = 0;
        wizReg_chkHostel.Checked = false;
        hdnStuID.Value = String.Empty;
        hdnRegID.Value = String.Empty;
        hdnCType.Value = String.Empty;
        hdnStatus.Value = String.Empty;
        Button btn = (Button)wizReg.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton");
        btn.Text = "Next";
        wizReg.ActiveStepIndex = 0;
        ///// Student Details
        txtemail.Text = String.Empty;
        txtStudentPANNumber.Text = String.Empty;

        // Parent Details
        txtFatherMobNo.Text = String.Empty;
        txtFemail.Text = String.Empty;
        txtFatherAadharNo.Text = String.Empty;
        FatherPANNumber.Text = String.Empty;

        txtMotherMobNo.Text = String.Empty;
        txtMemail.Text = String.Empty;
        txtMotherAadharNo.Text = String.Empty;

        // Academic Details
        txtSPLNumber.Text = String.Empty;
        txtComputerNumber.Text = String.Empty;

        // Dropdowns
        wizReg_ddlGender.SelectedIndex = 0;

        ddlPcity.SelectedIndex = 0;
        ddlPState.SelectedIndex = 0;
        ddlPCountry.SelectedIndex = 0;

        // Permanent Address
        txtPZip.Text = String.Empty;
        txtPAddress.Text = String.Empty;

        // Current Address
        txtCZip.Text = String.Empty;

        // Aadhaar Verify Tick
        Span1.Style["display"] = "none";

        //////------//////


    }

    protected void StartNextButton_Click(object sender, EventArgs e)
    {
        try
        {

            Button btn = (Button)wizReg.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton");
            if (btn.Text == "Next")
            {

                if (wizReg_txtStudentName.Text == String.Empty)
                {
                    objFunc.MsgBox1("Please Enter Student Name..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    wizReg_txtStudentName.Focus();
                    return;
                }
                if (wizReg_txtFatherName.Text == String.Empty)
                {
                    objFunc.MsgBox1("Please Enter Father Name..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    wizReg_txtFatherName.Focus();
                    return;
                }

                if (txtmotherName.Text == String.Empty)
                {
                    objFunc.MsgBox1("Please Enter Mother Name..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    txtmotherName.Focus();
                    return;
                }


                if (wizReg_txtDOB.Text == String.Empty)
                {
                    objFunc.MsgBox1("PLease Enter Date of Birth..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    wizReg_txtDOB.Focus();
                    return;
                }

                if (wizReg_ddlGender.SelectedIndex == 0)
                {
                    objFunc.MsgBox1("Please Select Gender..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    wizReg_ddlGender.Focus();
                    return;
                }

                if (wizReg_txtContact.Text == String.Empty)
                {
                    objFunc.MsgBox1("Please Enter Contact No..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    wizReg_txtContact.Focus();
                    return;
                }

                if (txtAadhaar.Text == String.Empty)
                {
                    objFunc.MsgBox1("Please Enter Aadhaar No..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    txtAadhaar.Focus();
                    return;
                }

                if (txtFatherMobNo.Text == String.Empty)
                {
                    objFunc.MsgBox1("Please Enter Father Mobile Number..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    txtFatherMobNo.Focus();
                    return;
                }

                if (txtFemail.Text == String.Empty)
                {
                    objFunc.MsgBox1("Please Enter Father Email ID..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    txtFemail.Focus();
                    return;
                }

                if (txtFatherAadharNo.Text == String.Empty)
                {
                    objFunc.MsgBox1("Please Enter Father Aadhaar No..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    txtFatherAadharNo.Focus();
                    return;
                }

                if (FatherPANNumber.Text == String.Empty)
                {
                    objFunc.MsgBox1("Please Enter Father PAN Number..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    FatherPANNumber.Focus();
                    return;
                }

                if (txtMotherMobNo.Text == String.Empty)
                {
                    objFunc.MsgBox1("Please Enter Mother Mobile Number..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    txtMotherMobNo.Focus();
                    return;
                }

                if (txtMemail.Text == String.Empty)
                {
                    objFunc.MsgBox1("Please Enter Mother Email ID..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    txtMemail.Focus();
                    return;
                }

                if (txtMotherAadharNo.Text == String.Empty)
                {
                    objFunc.MsgBox1("Please Enter Mother Aadhaar No..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    txtMotherAadharNo.Focus();
                    return;
                }

                if (txtPAddress.Text == String.Empty)
                {
                    objFunc.MsgBox1("PLease Enter Parmanent Address..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    txtPAddress.Focus();
                    return;
                }

                if (ddlPcity.SelectedIndex < 1)
                {
                    objFunc.MsgBox1("Please Select Parmanent City..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    ddlPcity.Focus();
                    return;
                }

                if (ddlPState.SelectedIndex < 1)
                {
                    objFunc.MsgBox1("Please Select Parmanent State..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    ddlPState.Focus();
                    return;
                }

                if (ddlPCountry.SelectedIndex < 1)
                {
                    objFunc.MsgBox1("Please Select Parmanent Country..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    ddlPCountry.Focus();
                    return;
                }


                if (txtCAddress.Text == String.Empty)
                {
                    objFunc.MsgBox1("PLease Enter Address..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    txtCAddress.Focus();
                    return;
                }

                if (ddlcity.SelectedIndex < 1)
                {
                    objFunc.MsgBox1("Please Select Current City..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    ddlcity.Focus();
                    return;
                }

                if (ddlState.SelectedIndex < 1)
                {
                    objFunc.MsgBox1("Please Select Current State..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    ddlState.Focus();
                    return;
                }

                if (ddlCountry.SelectedIndex < 1)
                {
                    objFunc.MsgBox1("Please Select Current Country..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    ddlCountry.Focus();
                    return;
                }

                if (ddlCountry.SelectedValue == "1")
                {
                    if (txtAadhaar.Text == String.Empty)
                    {
                        objFunc.MsgBox1("Please Enter Aadhaar No..!!", UpdatePanel1);
                        wizReg.ActiveStepIndex = 0;
                        txtAadhaar.Focus();
                        return;
                    }
                }

                if (txtCZip.Text == String.Empty)
                {
                    objFunc.MsgBox1("Please Enter Zip Code..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    txtCZip.Focus();
                    return;
                }

                if (txtPZip.Text == String.Empty)
                {
                    objFunc.MsgBox1("Please Enter Zip Code..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    txtPZip.Focus();
                    return;
                }


                bool isCourseSelected = false;

                foreach (ListItem li in wizReg_ddlCourse.Items)
                {
                    if (li.Selected)
                    {
                        isCourseSelected = true;
                        break;
                    }
                }

                if (!isCourseSelected)
                {
                    objFunc.MsgBox1("Please Select Course..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    wizReg_ddlCourse.Focus();
                    return;
                }

                if (ViewState["_StudentPhoto"] == null)
                {
                    objFunc.MsgBox1("Please Select Photo..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    filePhoto.Focus();
                    return;
                }
                if (ViewState["_StudentSign"] == null)
                {
                    objFunc.MsgBox1("Please upload Signature..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    _StudentSign.Focus();
                    return;
                } if (ViewState["fu10thMarksheet"] == null)
                {
                    objFunc.MsgBox1("Please upload Marksheet..!!", UpdatePanel1);
                    wizReg.ActiveStepIndex = 0;
                    fu10thMarksheet.Focus();
                    return;
                }
            }

            
            

            foreach (ListItem item in wizReg_ddlCourse.Items)
            {
                

                if (item.Selected)
                {
                    
                    int feesModuleActivate = Convert.ToInt32(objFunc.Get_details("select count(*) from College where CollegeID=" + ddlInstitute.SelectedValue + " and FeeModuleActivate=1"));
                    if (feesModuleActivate > 0)
                    {
                        int count = Convert.ToInt32(objFunc.FetchScalar("select * from master_courseFeeStruc where cid='" + wizReg_ddlYrSem.SelectedValue + "' and CourseCode='" + item.Value + "' and SessionID='" + Session["sesnID"].ToString() + "' and InstituteID='" + ddlInstitute.SelectedValue + "' and StudentType='" + wizReg_ddlAdmType.SelectedValue + "'"));
                        if (count == 0)
                        {
                            objFunc.MsgBox1("Cannot proceed because Fee Structure is not defined", UpdatePanel1);
                            wizReg.ActiveStepIndex = 0;
                            btn.Enabled = false;
                            return;
                        }
                    }
                    else
                    {
                         
                            System.Array arr1;
                            arr1 = Microsoft.VisualBasic.Strings.Split(Session["sesnID"].ToString(), "-", -1, 0);
                            int duration = Convert.ToInt32(objFunc.Get_details("select CourseDuration from Course where courseid='" + item.Value + "' and InstituteID = '" + ddlInstitute.SelectedValue + "'"));
                            string batch = arr1.GetValue(0).ToString().ToString() + "-" + (Int32.Parse(arr1.GetValue(0).ToString()) + duration).ToString();
                           // lblBatch.Text = batch;
                            hdnbatch.Value = batch;

                            int Count = Convert.ToInt32(objFunc.Get_details("Select Count(*) from Course_spl Where Courseid='" + item.Value + "' and Batch='" + hdnbatch.Value + "' and SpecilizationID='" + wizReg_ddlSpec.SelectedValue + "'"));
                            if (Count > 0)
                            {
                                int count = Convert.ToInt32(objFunc.Get_details("select COUNT(*) from StudentReg Where Courseid='" + item.Value + "' and Specialization='" + wizReg_ddlSpec.SelectedValue + "' and SessionID='" + wizReg_ddlSession.SelectedItem.Text + "'"));
                                int totalIntake = Convert.ToInt32(objFunc.Get_details("Select TotalIntake from Course_spl Where Courseid='" + item.Value + "' and Batch='" + hdnbatch.Value + "' and SpecilizationID='" + wizReg_ddlSpec.SelectedValue + "'"));
                                if (count >= totalIntake)
                                {
                                    objFunc.MsgBox1("The total Intake capacity of " + totalIntake + " seats for the Course " + item.Value + " with Specialisation " + wizReg_ddlSpec.SelectedItem.Text + " has already been filled.", UpdatePanel1);
                                    wizReg.ActiveStepIndex = 0;
                                    return;
                                }
                            }

                            string regno = String.Empty;
                            Academic.AcademicData.StudentRegDetailDM objDM = new Academic.AcademicData.StudentRegDetailDM();
                            if (btn.Text == "Next")
                            {
                                objDM.StudentID = 0;
                            }
                            else if (btn.Text == "Update")
                            {
                                objDM.StudentID = Convert.ToInt32(txtNameStudent.SelectedValue);
                            }
                            objDM.StudentName = wizReg_txtStudentName.Text;
                            objDM.FatherName = wizReg_txtFatherName.Text;
                            objDM.DateOfBirth = Convert.ToDateTime(wizReg_txtDOB.Text);
                            objDM.ContactNo = wizReg_txtContact.Text;
                            objDM.Category = "0";
                            objDM.AdmissionSource = wizReg_ddlAdmSource.SelectedValue;
                            objDM.AdmissionType = wizReg_ddlAdmType.SelectedValue;

                            if (wizReg_chkHostel.Checked == true)
                            {
                                objDM.HostelFacilityReq = "True";
                            }
                            else
                            {
                                objDM.HostelFacilityReq = "False";
                            }

                            objDM.BusFacilityReq = wizReg_RBLMOT.SelectedValue;

                            if (btn.Text == "Next")
                            {
                                regno = "";
                            }
                            else if (btn.Text == "Update")
                            {
                                regno = objFunc.Get_details("select RegNo from StudentReg where StudentID='" + txtNameStudent.SelectedValue + "' and InstituteID = '" + ddlInstitute.SelectedValue + "'");
                            }

                            objDM.RegNo = regno;
                            objDM.Diploma = "";
                            objDM.CourseID = Int32.Parse(item.Value);
                            objDM.SessionID = Session["sesnID"].ToString();
                            objDM.Sid = 1;
                            objDM.InstituteID = Int32.Parse(ddlInstitute.SelectedValue);
                            objDM.MotherName = "";
                            objDM.Gender = wizReg_ddlGender.SelectedValue;
                            objDM.Status = "No";
                            objDM.RegDate = DateTime.Now;
                            objDM.VerifyDate = DateTime.Now;
                            if (wizReg_ddlSpec.SelectedIndex == 0)
                            {
                                objDM.Specialization = "0";
                            }
                            else
                            {
                                objDM.Specialization = wizReg_ddlSpec.SelectedValue;
                            }

                            if (btn.Text == "Next")
                            {
                                objDM.Flag = "I";
                            }
                            else if (btn.Text == "Update")
                            {
                                objDM.Flag = "U";
                            }

                            //---------------------------------------------------------------------------------

                            Academic.AcademicData.StudentRegStatusDM objSDM = new Academic.AcademicData.StudentRegStatusDM();
                            if (btn.Text == "Next")
                            {
                                objSDM.StudentID = 0;
                            }
                            else if (btn.Text == "Update")
                            {
                                objSDM.StudentID = Convert.ToInt32(txtNameStudent.SelectedValue);
                            }
                            objSDM.Batch = hdnbatch.Value;
                            objSDM.SessionID = Session["sesnID"].ToString();
                            objSDM.YearID = Convert.ToInt32(1);
                            objSDM.SemesterID = 1;// Int32.Parse(ddlSession1.SelectedValue);
                            if (btn.Text == "Next")
                            {
                                regno = "";
                            }
                            else if (btn.Text == "Update")
                            {
                                regno = objFunc.Get_details("select RegNo from StudentReg where StudentID='" + txtNameStudent.SelectedValue + "' and InstituteID = '" + ddlInstitute.SelectedValue + "'");
                            }
                            objSDM.RegNo = regno;
                            objSDM.Status = "C";
                            objSDM.CourseID = Convert.ToInt32(item.Value);
                            objSDM.CCategory = "0";
                            objSDM.InstituteID = Int32.Parse(ddlInstitute.SelectedValue);
                            if (btn.Text == "Next")
                            {
                                objSDM.Flag = "I";
                            }
                            else if (btn.Text == "Update")
                            {
                                objSDM.Flag = "U";
                            }

                            Academicsvc objA = new Academicsvc();

                             
                                if (objSDM.Flag == "I")
                                {
                                    string ret = objA.StudentRegDetailInsert(objDM, objSDM);
                                    hdnRegID.Value = ret;
                                    // wizReg.ActiveStepIndex = 0;
                                }
                                else
                                {
                                    string ret = objA.StudentRegDetailInsert(objDM, objSDM);
                                    hdnRegID.Value = ret;
                                }

                            System.Array arr;
                            arr = Microsoft.VisualBasic.Strings.Split(hdnRegID.Value, "-", -1, 0);
                            hdnStuID.Value = arr.GetValue(1).ToString();
                            
                            hdnRegID.Value = arr.GetValue(0).ToString();
                            if (hdnStuID.Value != "" && hdnRegID.Value != "")
                            {
                                


                                if (ViewState["_StudentPhoto"] == null)
                                {

                                    byte[] imgbin = { 1, 1, 0, 0, 1, 0, 0, 0 };
                                    ViewState["_StudentPhoto"] = imgbin;
                                }
                                else
                                {
                                    ViewState["_StudentPhoto"] = (byte[])ViewState["_StudentPhoto"];
                                }
                                if (ViewState["_StudentSign"] == null)
                                {
                                    byte[] imgbin_StudentSign = { 1, 1, 0, 0, 1, 0, 0, 0 };
                                    ViewState["_StudentSign"] = imgbin_StudentSign;
                                }
                                else
                                {
                                    ViewState["_StudentSign"] = (byte[])ViewState["_StudentSign"];
                                }
                                byte[] marksheetPdf = null;

                                if (ViewState["fu10thMarksheet"] != null)
                                {
                                    marksheetPdf = (byte[])ViewState["fu10thMarksheet"];
                                }
                                byte[] imgbin_StudentThumb = { 1, 1, 0, 0, 1, 0, 0, 0 };
                                ViewState["_StudentThumb"] = imgbin_StudentThumb;
                                //string updateQuery = "UPDATE StudentReg SET MotherName = @MotherName, EntryBy=@EntryBy,ACTN_No=@ACTN_No,StudentNameInHindi=@StudentNameHindi,FatherNameInHindi=@FatherNameHindi,MotherNameInHindi=@MotherNameHindi,AdharCardNo=@AdharCardNo WHERE StudentID = @StudentID AND RegNo = @RegNo  ";
                                string updateQuery = "UPDATE StudentReg SET MotherName = @MotherName, EntryBy=@EntryBy,ACTN_No=@ACTN_No,AdharCardNo=@AdharCardNo,StudentPanNumber= @StudentPan, SPL_Number=@SPLno, Computer_Number=@ComputerNo WHERE StudentID = @StudentID AND RegNo = @RegNo  ";
                                string insertQuery = "INSERT INTO StudentDetailEntry (StudentID, Photo, CAddress, CCityID, CStateID, CCountryID, CZipCode, PAddress, PCityID, PStateID, PCountryID, PZipCode, MotherName, InstituteID,Signature,Thumb,FatherContactNo,MotherContactNo) VALUES (@StudentID, @Photo, @CAddress, @CCityID, @CStateID, @CCountryID, @CZipCode, @PAddress, @PCityID, @PStateID, @PCountryID, @PZipCode, @MotherName, @InstituteID,@Sign,@Thumb,@FatherMobNo,@MotherMobNo)";
                                string docQuery = "INSERT INTO Student_Academic_Documents(StudentID,Qualifacation,FilePath,FileName,InstituteID,Session,UploadedBy,DateOfUpload,ScanedCopy,FileFormat,QualificationID) VALUES(@StudentID,@Qualifacation,@filepath,@filename,@instituteid,@Session,@uploadedby,@UplodeDate,@Scancopy,@FileFormat,@QualificationID)";
                                try
                                {

                                    string ConnectionStrings = ConfigurationManager.ConnectionStrings["FeesManagementConn"].ConnectionString;
                                    using (SqlConnection connection = new SqlConnection(ConnectionStrings))
                                    {
                                        connection.Open();
                                        // Update query
                                        using (SqlCommand command = new SqlCommand(updateQuery, connection))
                                        {
                                            command.Parameters.AddWithValue("@StudentID", hdnStuID.Value);
                                            command.Parameters.AddWithValue("@RegNo", hdnRegID.Value);
                                            command.Parameters.AddWithValue("@MotherName", txtmotherName.Text);
                                            command.Parameters.AddWithValue("@EntryBy", Session["UID"]);
                                            command.Parameters.AddWithValue("@ACTN_No", "E" + hdnRegID.Value);
                                            //command.Parameters.AddWithValue("@StudentNameHindi", "");
                                            //command.Parameters.AddWithValue("@MotherNameHindi", "");
                                            //command.Parameters.AddWithValue("@FatherNameHindi", "");
                                            command.Parameters.AddWithValue("@AdharCardNo", txtAadhaar.Text);
                                            //command.Parameters.AddWithValue("@PassportNo", txtpassport.Text);
                                            //command.Parameters.AddWithValue("@Passportdate", txtpassportdate.Text);
                                            //command.Parameters.AddWithValue("@VisaNo", txtvisa.Text);
                                            //command.Parameters.AddWithValue("@Visadate", txtvisadate.Text);
                                            //command.Parameters.AddWithValue("@FRRONo", txtfrro.Text);
                                            //command.Parameters.AddWithValue("@frrodate", txtfrrodate.Text);
                                            command.Parameters.AddWithValue("@StudentPan", txtStudentPANNumber.Text);
                                            command.Parameters.AddWithValue("@SPLno", txtSPLNumber.Text);
                                            command.Parameters.AddWithValue("@ComputerNo", txtComputerNumber.Text);
                                            command.ExecuteNonQuery();

                                        }


                                        // Insert query
                                        using (SqlCommand command = new SqlCommand(insertQuery, connection))
                                        {
                                            command.Parameters.AddWithValue("@StudentID", hdnStuID.Value);
                                            command.Parameters.AddWithValue("@Photo", ViewState["_StudentPhoto"]);
                                            command.Parameters.AddWithValue("@CAddress", txtCAddress.Text);
                                            command.Parameters.AddWithValue("@CCityID", ddlcity.SelectedValue);
                                            command.Parameters.AddWithValue("@CStateID", ddlState.SelectedValue);
                                            command.Parameters.AddWithValue("@CCountryID", ddlCountry.SelectedValue);
                                            command.Parameters.AddWithValue("@CZipCode", txtCZip.Text);
                                            command.Parameters.AddWithValue("@PAddress", txtPAddress.Text);
                                            command.Parameters.AddWithValue("@PCityID", ddlPcity.SelectedValue);
                                            command.Parameters.AddWithValue("@PStateID", ddlPState.SelectedValue);
                                            command.Parameters.AddWithValue("@PCountryID", ddlPCountry.SelectedValue);
                                            command.Parameters.AddWithValue("@PZipCode", txtPZip.Text);
                                            command.Parameters.AddWithValue("@MotherName", txtmotherName.Text);
                                            command.Parameters.AddWithValue("@InstituteID", ddlInstitute.SelectedValue);
                                            command.Parameters.AddWithValue("@Sign", ViewState["_StudentSign"]);
                                            command.Parameters.AddWithValue("@Thumb", ViewState["_StudentThumb"]);
                                            command.Parameters.AddWithValue("@FatherMobNo", txtFatherMobNo.Text);
                                            command.Parameters.AddWithValue("@MotherMobNo", txtMotherMobNo.Text);

                                            command.ExecuteNonQuery();
                                        }

                                        // Student_Document Insert 10th Marksheet
                                        using (SqlCommand command = new SqlCommand(docQuery, connection))
                                        {
                                            command.Parameters.AddWithValue("@StudentID", hdnStuID.Value);
                                            command.Parameters.AddWithValue("@Qualifacation", "Proof of HighSchool");
                                            command.Parameters.AddWithValue("@filepath", "");
                                            command.Parameters.AddWithValue("@filename", ViewState["fu10thMarksheetfilname"]);
                                            command.Parameters.AddWithValue("@instituteid", Session["instID"]);
                                            command.Parameters.AddWithValue("@Session", Session["sesnID"]);
                                            command.Parameters.AddWithValue("@uploadedby", Session["UID"]);
                                            command.Parameters.AddWithValue("@UplodeDate", DateTime.Now);
                                            command.Parameters.AddWithValue("@FileFormat", ViewState["fu10thMarksheetext"]);
                                            command.Parameters.AddWithValue("@Scancopy", ViewState["fu10thMarksheet"]);
                                            command.Parameters.AddWithValue("@QualificationID", "1");
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Handle exceptions here
                                    Console.WriteLine("Error: " + ex.Message);
                                }
                                AllRegNos += hdnRegID.Value + ", ";
                                Allcourse += item.Text + ", ";
                                allcourseids += item.Value + ", ";
                                lblSession.Text = Session["sesnID"].ToString();
                                lblBatch.Text = "N/A";
                                lblStType.Text = "N/A";
                                string StuDetailID = objFunc.Get_details("select StDetailID from StudentDetailEntry where StudentID='" + hdnStuID.Value + "'");
                                if (StuDetailID != "")
                                {

                                    objFunc.ExecuteDML("insert StudentFamilyDetail (FatherEmail,MotherEmail,FatherPanNumber,FatherAadharNumber, MotherAadharNumber,StuDetailID,StudentID,FatherName,MotherName,InstituteID,FatherIncome,MotherIncome,GuardianIncome,FAge,MAge,GAge) values ('" + txtFemail.Text + "', '" + txtMemail.Text + "', '" + FatherPANNumber.Text + "', '" + txtFatherAadharNo.Text + "', '" + txtMotherAadharNo.Text + "','" + StuDetailID + "','" + hdnStuID.Value + "','" + wizReg_txtFatherName.Text + "','" + txtmotherName.Text + "','" + ddlInstitute.SelectedValue + "','0.00','0.00','0.00','0','0','0')");
                                }
                            }

                            if (btn.Text == "Update")
                            {
                                //wizReg.ActiveStepIndex = 0;
                            }
                            
                    }
                }
                
            }
            
        }
            
        catch (Exception ex)
        {
            objFunc.MsgBox1(ex.Message, UpdatePanel1);
            return;
        }
        
    }
    public int CreateLedger()
    {
        int Ret = 1;
        try
        {
            string xmlstc = "";
            xmlstc = "<ENVELOPE>";
            xmlstc = xmlstc + "<HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER>";
            xmlstc = xmlstc + "<BODY><IMPORTDATA><REQUESTDESC><REPORTNAME>All Masters</REPORTNAME>";
            xmlstc = xmlstc + "<STATICVARIABLES>";
            xmlstc = xmlstc + "<SVCURRENTCOMPANY>" + hdnCompanyName.Value + "</SVCURRENTCOMPANY>";
            xmlstc = xmlstc + "</STATICVARIABLES></REQUESTDESC>";
            xmlstc = xmlstc + "<REQUESTDATA>";
            xmlstc = xmlstc + "<TALLYMESSAGE xmlns:UDF='TallyUDF'><CURRENCY NAME='Rs' RESERVEDNAME=''><ADDITIONALNAME>RUPEE</ADDITIONALNAME><ORIGINALSYMBOL>Rs</ORIGINALSYMBOL><EXPANDEDSYMBOL>RUPEE</EXPANDEDSYMBOL><ISSUFFIX>No</ISSUFFIX><HASSPACE>Yes</HASSPACE><INMILLIONS>No</INMILLIONS><DECIMALPLACES>2</DECIMALPLACES></CURRENCY></TALLYMESSAGE>";
            xmlstc = xmlstc + "<TALLYMESSAGE xmlns:UDF='TallyUDF'>";
            xmlstc = xmlstc + "<LEDGER NAME='' RESERVEDNAME=''>";
            xmlstc = xmlstc + "<NAME.LIST>";
            xmlstc = xmlstc + "<NAME>" + hdnStudentLedgerName.Value + "</NAME>";
            xmlstc = xmlstc + "<NAME>" + hdnAdditionalName.Value + "</NAME>";
            xmlstc = xmlstc + "</NAME.LIST>";
            xmlstc = xmlstc + "<ADDITIONALNAME></ADDITIONALNAME>";
            xmlstc = xmlstc + "<ISINTERESTON>No</ISINTERESTON>";
            xmlstc = xmlstc + "<PARENT>" + hdnStudentLedgerParent.Value + "</PARENT>";
            //xmlstc = xmlstc + "<OPENINGBALANCE>-1000</OPENINGBALANCE>";
            xmlstc = xmlstc + "<CURRENCYNAME>Rs</CURRENCYNAME><ISBILLWISEON>Yes</ISBILLWISEON><ISCOSTCENTRESON>Yes</ISCOSTCENTRESON><AFFECTSSTOCK>No</AFFECTSSTOCK><ISCONDENSED>No</ISCONDENSED><SORTPOSITION> 1000</SORTPOSITION><INTERESTONBILLWISE>No</INTERESTONBILLWISE><OVERRIDEINTEREST>No</OVERRIDEINTEREST><OVERRIDEADVINTEREST>No</OVERRIDEADVINTEREST><FORPAYROLL>No</FORPAYROLL><PAYTYPE/><SHOWINPAYSLIP>No</SHOWINPAYSLIP><ASSLABRATE>No</ASSLABRATE><TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE><TDSDEDUCTEEISCACERTIFIED>No</TDSDEDUCTEEISCACERTIFIED><ISTDSAPPLICABLE>No</ISTDSAPPLICABLE><USEFORVAT>No</USEFORVAT><IGNORETDSEXEMPT>No</IGNORETDSEXEMPT><TDSDEDUCTEETYPE/><TAXTYPE/><TAXCLASSIFICATIONNAME/><TDSRATENAME/>";
            xmlstc = xmlstc + "</LEDGER>";
            xmlstc = xmlstc + "</TALLYMESSAGE>";
            xmlstc = xmlstc + "</REQUESTDATA>";
            xmlstc = xmlstc + "</IMPORTDATA>";
            xmlstc = xmlstc + "</BODY>";
            xmlstc = xmlstc + "</ENVELOPE>";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + TallyServerIP + "");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentLength = xmlstc.Length;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            streamWriter.Write(xmlstc);
            streamWriter.Close();
            string result;
            HttpWebResponse objResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();
                Ret = 1;

            }
        }

        catch (Exception ex)
        {
            Ret = 0;
            if (ex.Message == "Unable to connect to the remote server")
            {
                objFunc.MsgBox1("Your Tally Server is not running, Please start the tally so that system can create the leadger..!!", UpdatePanel1);
            }
            else
            {
                objFunc.MsgBox1(ex.Message, UpdatePanel1);
            }

        }

        return Ret;
    }

    public void CreateGroups()
    {
        try
        {
            string xmlstc = "";

            xmlstc = "<ENVELOPE>";
            xmlstc = xmlstc + "<HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER>";
            xmlstc = xmlstc + "<BODY><IMPORTDATA><REQUESTDESC><REPORTNAME>All Masters</REPORTNAME>";
            xmlstc = xmlstc + "<STATICVARIABLES>";
            xmlstc = xmlstc + "<SVCURRENTCOMPANY>" + hdnCompanyName.Value + "</SVCURRENTCOMPANY>";
            xmlstc = xmlstc + "</STATICVARIABLES></REQUESTDESC>";
            xmlstc = xmlstc + "<REQUESTDATA>";
            xmlstc = xmlstc + "<TALLYMESSAGE xmlns:UDF='TallyUDF'>" +
        "<GROUP NAME='' RESERVEDNAME=''>" +
        "<GUID>d0594dee-c340-40fd-9b57-1fa4fce8c19c-00000007</GUID>" +
        "<PARENT>" + hdnGroupParent.Value + "</PARENT>" +
        "<BASICGROUPISCALCULABLE>No</BASICGROUPISCALCULABLE>" +
        "<ADDLALLOCTYPE/>" +
        "<GRPDEBITPARENT/>" +
        "<GRPCREDITPARENT/>" +
        "<ISBILLWISEON>Yes</ISBILLWISEON>" +
        "<ISCOSTCENTRESON>No</ISCOSTCENTRESON>" +
        "<ISADDABLE>No</ISADDABLE>" +
        "<ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" +
        "<ASORIGINAL>Yes</ASORIGINAL>" +
        "<ISSUBLEDGER>No</ISSUBLEDGER>" +
        "<ISREVENUE>No</ISREVENUE>" +
        "<AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>" +
        "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" +
        "<TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>" +
        "<ISCONDENSED>No</ISCONDENSED>" +
        "<AFFECTSSTOCK>No</AFFECTSSTOCK>" +
        "<ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>" +
        "<ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>" +
        "<ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>" +
        "<ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>" +
        "<SORTPOSITION> 70</SORTPOSITION>" +
        "<ALTERID> 8</ALTERID>" +
        "<SERVICETAXDETAILS.LIST></SERVICETAXDETAILS.LIST>" +
        "<VATDETAILS.LIST></VATDETAILS.LIST>" +
        "<LANGUAGENAME.LIST>" +
        " <NAME.LIST TYPE='String'>" +
         " <NAME>" + hdnMainGroupName.Value + "</NAME>" +
         "</NAME.LIST>" +
         "<LANGUAGEID>1033</LANGUAGEID>" +
        "</LANGUAGENAME.LIST>" +
        "<XBRLDETAIL.LIST></XBRLDETAIL.LIST>" +
        "<AUDITDETAILS.LIST></AUDITDETAILS.LIST>" +
        "<SCHVIDETAILS.LIST></SCHVIDETAILS.LIST>" +
        "<EXCISETARIFFDETAILS.LIST></EXCISETARIFFDETAILS.LIST>" +
        "<TCSCATEGORYDETAILS.LIST></TCSCATEGORYDETAILS.LIST>" +
        "<TDSCATEGORYDETAILS.LIST></TDSCATEGORYDETAILS.LIST>" +
        "<EXTARIFFDUTYHEADDETAILS.LIST></EXTARIFFDUTYHEADDETAILS.LIST>" +
       "</GROUP>" +
      "</TALLYMESSAGE>";
            xmlstc = xmlstc + "</REQUESTDATA>";
            xmlstc = xmlstc + "</IMPORTDATA>";
            xmlstc = xmlstc + "</BODY>";
            xmlstc = xmlstc + "</ENVELOPE>";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + TallyServerIP + "");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentLength = xmlstc.Length;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            streamWriter.Write(xmlstc);
            streamWriter.Close();
            string result;
            HttpWebResponse objResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();
                objFunc.MsgBox(result, this);
                return;
            }
        }

        catch (Exception ex)
        {
            if (ex.Message == "Unable to connect to the remote server")
            {
                hdnMsg.Value = "Failed";
                return;
            }
            else
            {
                objFunc.MsgBox(ex.Message, this);
                return;
            }

        }
    }

    public void CreateSubGroups()
    {
        try
        {
            string xmlstc = "";

            xmlstc = "<ENVELOPE>";
            xmlstc = xmlstc + "<HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER>";
            xmlstc = xmlstc + "<BODY><IMPORTDATA><REQUESTDESC><REPORTNAME>All Masters</REPORTNAME>";
            xmlstc = xmlstc + "<STATICVARIABLES>";
            xmlstc = xmlstc + "<SVCURRENTCOMPANY>" + hdnCompanyName.Value + "</SVCURRENTCOMPANY>";
            xmlstc = xmlstc + "</STATICVARIABLES></REQUESTDESC>";
            xmlstc = xmlstc + "<REQUESTDATA>";
            xmlstc = xmlstc + "<TALLYMESSAGE xmlns:UDF='TallyUDF'>" +
        "<GROUP NAME='' RESERVEDNAME=''>" +
        "<GUID>d0594dee-c340-40fd-9b57-1fa4fce8c19c-00000007</GUID>" +
        "<PARENT>" + hdnMainGroupName.Value + "</PARENT>" +
        "<BASICGROUPISCALCULABLE>No</BASICGROUPISCALCULABLE>" +
        "<ADDLALLOCTYPE/>" +
        "<GRPDEBITPARENT/>" +
        "<GRPCREDITPARENT/>" +
        "<ISBILLWISEON>Yes</ISBILLWISEON>" +
        "<ISCOSTCENTRESON>No</ISCOSTCENTRESON>" +
        "<ISADDABLE>No</ISADDABLE>" +
        "<ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" +
        "<ASORIGINAL>Yes</ASORIGINAL>" +
        "<ISSUBLEDGER>No</ISSUBLEDGER>" +
        "<ISREVENUE>No</ISREVENUE>" +
        "<AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>" +
        "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" +
        "<TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>" +
        "<ISCONDENSED>No</ISCONDENSED>" +
        "<AFFECTSSTOCK>No</AFFECTSSTOCK>" +
        "<ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>" +
        "<ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>" +
        "<ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>" +
        "<ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>" +
        "<SORTPOSITION> 70</SORTPOSITION>" +
        "<ALTERID> 8</ALTERID>" +
        "<SERVICETAXDETAILS.LIST></SERVICETAXDETAILS.LIST>" +
        "<VATDETAILS.LIST></VATDETAILS.LIST>" +
        "<LANGUAGENAME.LIST>" +
        " <NAME.LIST TYPE='String'>" +
         " <NAME>" + hdnSubGroupName.Value + "</NAME>" +
         "</NAME.LIST>" +
         "<LANGUAGEID>1033</LANGUAGEID>" +
        "</LANGUAGENAME.LIST>" +
        "<XBRLDETAIL.LIST></XBRLDETAIL.LIST>" +
        "<AUDITDETAILS.LIST></AUDITDETAILS.LIST>" +
        "<SCHVIDETAILS.LIST></SCHVIDETAILS.LIST>" +
        "<EXCISETARIFFDETAILS.LIST></EXCISETARIFFDETAILS.LIST>" +
        "<TCSCATEGORYDETAILS.LIST></TCSCATEGORYDETAILS.LIST>" +
        "<TDSCATEGORYDETAILS.LIST></TDSCATEGORYDETAILS.LIST>" +
        "<EXTARIFFDUTYHEADDETAILS.LIST></EXTARIFFDUTYHEADDETAILS.LIST>" +
       "</GROUP>" +
      "</TALLYMESSAGE>";
            xmlstc = xmlstc + "</REQUESTDATA>";
            xmlstc = xmlstc + "</IMPORTDATA>";
            xmlstc = xmlstc + "</BODY>";
            xmlstc = xmlstc + "</ENVELOPE>";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + TallyServerIP + "");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentLength = xmlstc.Length;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            streamWriter.Write(xmlstc);
            streamWriter.Close();
            string result;
            HttpWebResponse objResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();
                objFunc.MsgBox(result, this);
                return;
            }
        }

        catch (Exception ex)
        {
            if (ex.Message == "Unable to connect to the remote server")
            {
                hdnMsg.Value = "Failed";
                return;
            }
            else
            {
                objFunc.MsgBox(ex.Message, this);
                return;
            }

        }
    }

    protected void wizReg_ddlSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            DataTable dt = new DataTable();
            dt = objFunc.FillDataTable("select distinct FeeDueType from FeeMaster where Courseid='" + wizReg_ddlCourse.SelectedValue + "' and Instid='" + Session["instID"].ToString() + "' and Session='" + Session["sesnID"].ToString() + "'  and Active='True'");
            if (dt.Rows.Count > 0)
            {
                ddlModule.DataSource = dt;
                ddlModule.DataTextField = "FeeDueType";
                ddlModule.DataValueField = "FeeDueType";
                ddlModule.DataBind();
            }
            else
            {
                objFunc.MsgBox1("Fee setup not created yet...", UpdatePanel1);
                return;
            }
            ddlModule.Items.Insert(0, new ListItem("--Due Type--", "0"));
        }
        catch (Exception ex)
        {
            objFunc.MsgBox1(ex.Message, UpdatePanel1);
            return;
        }

        DataTable year = new DataTable();
        year = objFunc.FillDataTable("select distinct Year from Semester_View where courseid='" + wizReg_ddlCourse.SelectedValue + "' and inst_id='" + Session["instID"].ToString() + "' and sessionyear='" + Session["sesnID"].ToString() + "' and Active='true'");
        if (year.Rows.Count > 0)
        {
            ddlyear.DataSource = year;
            ddlyear.DataTextField = "Year";
            ddlyear.DataValueField = "Year";
            ddlyear.DataBind();
        }

        else
        {
            objFunc.MsgBox1("Year not found for selected course and session.", UpdatePanel1);
            return;
        }
        ddlyear.Items.Insert(0, new ListItem("--Select Year--", "0"));

    }
    protected void wizReg_ActiveStepChanged(object sender, EventArgs e)
    {
        try
        {
            if (wizReg.ActiveStep.ID != "step1")
            {
                StartNextButton_Click(sender, e);
            }

            if (txtNameStudent.SelectedValue != String.Empty)
            {
                Button btn = (Button)wizReg.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton");
                if (btn.Text == "Update")
                {
                    objFunc.MsgBox1("Record Updated", UpdatePanel1);
                    wizReg_txtStudentName.Focus();
                    UpdateReset();
                }
            }

            Academicsvc objA = new Academicsvc();
            wizReg.HeaderText = wizReg.WizardSteps[wizReg.ActiveStepIndex].Title.ToString();

            if (wizReg.ActiveStep.ID == "step2")
            {
                Button btn = (Button)wizReg.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton");
                btnfinish.Focus();
                lblStudentName.Text = wizReg_txtStudentName.Text;
                lblFatherName.Text = wizReg_txtFatherName.Text;
                 lblCategory.Text =  "N/A";
                lblContact.Text = wizReg_txtContact.Text;
                lblCourse.Text = Allcourse.TrimEnd(',', ' ');
                //lblCourse.Text = wizReg_ddlCourse.SelectedItem.Text + '-' + wizReg_ddlSpec.SelectedItem.Text;
                lblSession.Text = Session["sesnID"].ToString();
                lblYrSem.Text = "0";
                lblRegNo.Text = AllRegNos.TrimEnd(',', ' '); 
                //lblRegNo.Text = hdnRegID.Value;
                lblStType.Text = "N/A";
                btn.Focus();
                lblBatch.Text = hdnbatch.Value;
                btnfinish.Visible = true;
                DataTable dt = objFunc.FillDataTable(@"SELECT master_courseFeeStruc.courseFeeId, master_courseFeeStruc.CourseCode,  Course.CourseName, 
                   master_courseFeeStruc.FeeHeadCode, FeeHead.FeeHeadName,master_courseFeeStruc.TotalAmount,
                   master_courseFeeStruc.Category,master_courseFeeStruc.InstituteID, master_courseFeeStruc.SessionID, master_courseFeeStruc.CID, master_courseFeeStruc.yearid, master_courseFeeStruc.batch,
                   master_courseFeeDetail.FeeDetailID, master_courseFeeDetail.LastDate,master_courseFeeDetail.BreakoffTime, master_courseFeeDetail.Amount, master_courseFeeStruc.StudentType,
                   master_courseFeeStruc.FeeClass,  master_courseFeeStruc.DueType FROM master_courseFeeStruc INNER JOIN master_courseFeeDetail ON master_courseFeeStruc.courseFeeId = master_courseFeeDetail.courseFeeId 
                   INNER JOIN Course ON master_courseFeeStruc.CourseCode = Course.CourseId INNER JOIN FeeHead ON master_courseFeeStruc.FeeHeadCode = FeeHead.FeeHeadId
                   WHERE master_courseFeeStruc.CourseCode IN (" + allcourseids.TrimEnd(',', ' ') + ") AND master_courseFeeStruc.SessionID = '" + Session["sesnID"].ToString() + "' AND master_courseFeeStruc.InstituteID = '" + Session["instID"].ToString() + "' ORDER BY master_courseFeeStruc.CourseCode,master_courseFeeStruc.DueType");
                //objARDM = objA.FillGrdFeeNew(allcourseids.TrimEnd(',', ' '), lblSession.Text, Int32.Parse(ddlInstitute.SelectedValue), "0");
                //objARDM = objA.FillGrdFeeNew(Int32.Parse(allcourseids.TrimEnd(',', ' ')), lblSession.Text, Int32.Parse(ddlInstitute.SelectedValue), "0");
               // objARDM = objA.FillGrdFee(Convert.ToInt32(0), Int32.Parse(wizReg_ddlCourse.SelectedValue), lblSession.Text, Int32.Parse(ddlInstitute.SelectedValue), wizReg_ddlAdmType.SelectedValue, 0, ddlModule.SelectedValue, wizReg_ddlSpec.SelectedValue);

                if (dt.Rows.Count > 0)
                {
                    // Hostel Selected
                    if (wizReg_chkHostel.Checked)
                    {
                        DataRow[] dr = dt.Select("FeeHeadName <> 'BUS FEE'");
                        dt = dr.Length > 0 ? dr.CopyToDataTable() : dt.Clone();
                    }

                    // Bus Selected
                    if (wizReg_RBLMOT.SelectedValue == "Bus")
                    {
                        DataRow[] dr = dt.Select("FeeHeadName NOT IN ('HOSTEL FEE','CAUTION MONEY HOSTEL')");
                        dt = dr.Length > 0 ? dr.CopyToDataTable() : dt.Clone();
                    }

                    // Neither Hostel Nor Bus
                    if (!wizReg_chkHostel.Checked && wizReg_RBLMOT.SelectedValue != "Bus")
                    {
                        DataRow[] dr = dt.Select("FeeHeadName NOT IN ('HOSTEL FEE','CAUTION MONEY HOSTEL','BUS FEE')");
                        dt = dr.Length > 0 ? dr.CopyToDataTable() : dt.Clone();
                    }

                    gvFee.DataSource = dt;
                    gvFee.DataBind();

                    if (gvFee.FooterRow != null)
                    {
                        gvFee.FooterRow.Cells[1].Text = "Total Amount";

                        decimal totalAmount = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            totalAmount += Convert.ToDecimal(row["Amount"]);
                        }

                        gvFee.FooterRow.Cells[2].Text = totalAmount.ToString("0.00");
                    }
                }

                for (int i = 0; i < gvFee.Rows.Count; i++)
                {
                    if (gvFee.DataKeys[i].Values[3].ToString() == "Mandatory")
                    {
                        ((CheckBox)gvFee.Rows[i].FindControl("chkF")).Enabled = false;
                    }
                    else
                    {
                        ((CheckBox)gvFee.Rows[i].FindControl("chkF")).Enabled = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            objFunc.MsgBox1(ex.Message, UpdatePanel1);
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------

    protected void wizReg_ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            objFunc.FillSpecilization(this.wizReg_ddlSpec, ddlInstitute.SelectedValue, wizReg_ddlCourse.SelectedValue, "--Select--");
            if (wizReg_ddlSpec.Items.Count == 2)
            {
                wizReg_ddlSpec.SelectedIndex = 1;
            }

            wizReg_ddlCourse.Focus();
        }
        catch (Exception ex)
        {
            objFunc.MsgBox1(ex.Message, UpdatePanel1);
            return;
        }
    }

    protected void wizReg_ddlYrSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Button btn = (Button)wizReg.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton");
            if (Convert.ToInt32(objFunc.Get_details("select count(*) from College where CollegeID=" + ddlInstitute.SelectedValue + " and FeeModuleActivate=1")) > 0)
            {
                int count = Convert.ToInt32(objFunc.FetchScalar("select * from master_courseFeeStruc where cid='" + wizReg_ddlYrSem.SelectedValue + "' and CourseCode='" + wizReg_ddlCourse.SelectedValue + "' and SessionID='" + Session["sesnID"].ToString() + "' and InstituteID='" + ddlInstitute.SelectedValue + "' and StudentType='" + wizReg_ddlAdmType.SelectedValue + "'"));
                if ((count == 0) && (wizReg_ddlAdmType.SelectedItem.Text != "Diploma"))
                {
                    objFunc.MsgBox1("Cannot proceed because Fee Structure is not defined", UpdatePanel1);
                    btn.Enabled = false;
                    return;
                }
            }
            else
            {
                btn.Enabled = true;
            }
            wizReg_ddlYrSem.Focus();
        }
        catch (Exception ex)
        {
            objFunc.MsgBox1(ex.Message, UpdatePanel1);
            return;
        }
    }
    protected void wizReg_ddlAdmType_SelectedIndexChanged(object sender, EventArgs e)
    {
        wizReg_ddlCourse.SelectedIndex = 0;
        wizReg_ddlSession.SelectedIndex = 0;
        wizReg_ddlSpec.SelectedIndex = 0;
        if (wizReg_ddlYrSem.Items.Count > 0)
        {
            wizReg_ddlYrSem.SelectedIndex = 0;
        }
        wizReg_ddlAdmType.Focus();
    }



    protected void btnfinish_Click(object sender, EventArgs e)
    {
        string regNos = "'" + lblRegNo.Text.Replace(" ", "").Replace(",", "','") + "'";
        try
        {
            if (hdnStatus.Value == String.Empty)
            {

                if (objFunc.Get_details("select Isactive from TallyConfiguration where InstID =" + ddlInstitute.SelectedValue + "") == "True")
                {

                    hdnDueAmount.Value = "0";
                    //  hdnFeeHeadLedgerParentName.Value = "Sales Accounts";
                    hdnTransDate.Value = DateTime.Now.ToString("yyyyMMdd");
                    hdnTransMode.Value = "Cash";
                    hdnVoucherType.Value = "Journal";
                    hdnNarration.Value = "Being Amount Due";
                    TallyServerIP = objFunc.Get_details("select TallyServerIP+':'+TallyServerPort as TallyServerIP from TallyConfiguration where InstID =" + ddlInstitute.SelectedValue + "");

                    DataTable dt_fee = new DataTable();
                    
                  //  dt_fee = objFunc.fill_Semester_wise_fee_details(0, Int32.Parse(wizReg_ddlCourse.SelectedValue), lblSession.Text, Int32.Parse(ddlInstitute.SelectedValue), wizReg_ddlAdmType.SelectedValue, wizReg_ddlCategory.SelectedValue, 79);
                    for (int i = 0; i <= dt_fee.Rows.Count - 1; i++)
                    {
                        hdnDueAmount.Value = (Convert.ToDecimal(hdnDueAmount.Value) + Convert.ToDecimal(dt_fee.Rows[i]["Amount"].ToString())).ToString();
                        if (str_BillLocationList == string.Empty)
                        {
                            str_BillLocationList = "<BILLALLOCATIONS.LIST>" +
                                                       "<NAME>" + dt_fee.Rows[i]["CourseYear"].ToString() + "</NAME>" +
                                                       "<BILLCREDITPERIOD JD='43190' P='1-Apr-2018'>1-Apr-2018</BILLCREDITPERIOD>" +
                                                       "<BILLTYPE>New Ref</BILLTYPE>" +
                                                       "<TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE>" +
                                                       "<AMOUNT>" + Convert.ToDecimal(dt_fee.Rows[i]["Amount"].ToString()) * -1 + "</AMOUNT>" +
                                                       "<INTERESTCOLLECTION.LIST></INTERESTCOLLECTION.LIST>" +
                                                       "<STBILLCATEGORIES.LIST></STBILLCATEGORIES.LIST>" +
                                                   "</BILLALLOCATIONS.LIST>";
                        }
                        else
                        {
                            str_BillLocationList = str_BillLocationList + "<BILLALLOCATIONS.LIST>" +
                                                       "<NAME>" + dt_fee.Rows[i]["CourseYear"].ToString() + "</NAME>" +
                                                       "<BILLCREDITPERIOD JD='43190' P='1-Apr-2018'>1-Apr-2018</BILLCREDITPERIOD>" +
                                                       "<BILLTYPE>New Ref</BILLTYPE>" +
                                                       "<TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE>" +
                                                       "<AMOUNT>" + Convert.ToDecimal(dt_fee.Rows[i]["Amount"].ToString()) * -1 + "</AMOUNT>" +
                                                       "<INTERESTCOLLECTION.LIST></INTERESTCOLLECTION.LIST>" +
                                                       "<STBILLCATEGORIES.LIST></STBILLCATEGORIES.LIST>" +
                                                   "</BILLALLOCATIONS.LIST>";
                        }
                    }
                }
                List<Academic.AcademicData.StudentRegDetailTDM> objFPD = new List<Academic.AcademicData.StudentRegDetailTDM>();
                for (int i = 0; i < gvFee.Rows.Count; i++)
                {
                    if (((CheckBox)gvFee.Rows[i].FindControl("chkF")).Checked == true)
                    {
                        int month = DateTime.Now.Month;
                        int vno = Convert.ToInt32(objFunc.Get_details("select isnull(max(VNo),0) from StudentRegDetail where Month='" + month + "' and InstituteID = '" + ddlInstitute.SelectedValue + "'").ToString());

                        System.Array arr1;
                        arr1 = Microsoft.VisualBasic.Strings.Split(Session["sesnID"].ToString(), "-", -1, 0);
                        string sessn = arr1.GetValue(0).ToString();
                        sessn = sessn.Remove(0, 2);

                        Academic.AcademicData.StudentRegDetailTDM objDM = new Academic.AcademicData.StudentRegDetailTDM();
                        objDM.StudentDetailID = 0;
                        objDM.StudentID = Convert.ToInt32(objFunc.Get_details("SELECT DISTINCT StudentStatus.StudentID FROM StudentReg INNER JOIN StudentStatus ON StudentReg.StudentID = StudentStatus.StudentID WHERE (StudentStatus.Status = 'C') and (StudentReg.RegNo in (" + regNos + ")) AND (StudentStatus.CourseID = '" + gvFee.DataKeys[i].Values[5].ToString() + "')"));
                        objDM.FeeID = Convert.ToInt32(gvFee.DataKeys[i].Values[2].ToString());
                        TextBox txtAmt = (TextBox)gvFee.Rows[i].FindControl("TextBox2");
                        string lblFeeHeadName = gvFee.Rows[i].Cells[1].Text.Trim();
                        objDM.TotalAmount = Convert.ToDecimal(txtAmt.Text);
                        objDM.PaidAmount = 0;
                        objDM.BalanceAmount = Convert.ToDecimal(txtAmt.Text);
                        objDM.Refund = 0;
                        objDM.Status = "C";
                        objDM.ODC = 0;
                        objDM.InstituteID = Convert.ToInt32(ddlInstitute.SelectedValue);
                        objDM.FeeSubID = Convert.ToInt32(gvFee.DataKeys[i].Values[0].ToString());
                        objDM.Batch = lblBatch.Text;
                        objDM.SessionID = Session["sesnID"].ToString();
                        objDM.Sid = Convert.ToInt32(objFunc.Get_details("select top(1) SemesterID from StudentStatus where StudentId='" + hdnStuID.Value + "' and Status='C'"));
                        objDM.CourseID = Convert.ToInt32(gvFee.DataKeys[i].Values[5].ToString());
                        objDM.YearID = Convert.ToInt32(1);
                        objDM.VoucherNo = sessn + "" + DateTime.Now.Month.ToString("d2") + "" + "01-" + (vno + 1).ToString();
                        objDM.VNo = vno + 1;
                        objDM.EntryDate = Convert.ToDateTime(DateTime.Now);
                        objDM.EntryBy = Session["uid"].ToString();
                        objDM.Narration = "Being Amount Due For " + objFunc.Get_details("Select CourseYear from Semester_View where SID = '" + Convert.ToInt32(gvFee.DataKeys[i].Values[4].ToString()) + "'");
                        objDM.DebitCredit = "Dr";
                        objDM.Month = DateTime.Now.Month;
                        objFPD.Add(objDM);

                        if (str_AllLedgerEntries == string.Empty)
                        {

                            str_AllLedgerEntries = "<ALLLEDGERENTRIES.LIST>" +
                                                  "<LEDGERNAME>" + lblFeeHeadName + "</LEDGERNAME>" +
                                                     "<VOUCHERFBTCATEGORY/>" +
                                                    "<GSTCLASS/>" +
                                                    "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" +
                                                    "<LEDGERFROMITEM>No</LEDGERFROMITEM>" +
                                                    "<ISPARTYLEDGER>No</ISPARTYLEDGER>" +
                                                    "<ISLASTDEEMEDPOSITIVE>No</ISLASTDEEMEDPOSITIVE>" +
                                                    "<AMOUNT>" + txtAmt.Text + "</AMOUNT>" +
                                                    "<VATEXPAMOUNT>" + txtAmt.Text + "</VATEXPAMOUNT>" +
                                                    "<CURRPARTYLEDGERNAME>" + lblFeeHeadName + "</CURRPARTYLEDGERNAME>" +
                                                   "<SERVICETAXDETAILS.LIST></SERVICETAXDETAILS.LIST>" +
                                                   "<CATEGORYALLOCATIONS.LIST></CATEGORYALLOCATIONS.LIST>" +
                                                   "<BANKALLOCATIONS.LIST></BANKALLOCATIONS.LIST>" +
                                                   " <BILLALLOCATIONS.LIST></BILLALLOCATIONS.LIST>" +
                                                    "<INTERESTCOLLECTION.LIST>" +
                                                    "</INTERESTCOLLECTION.LIST>" +
                                                    "<OLDAUDITENTRIES.LIST></OLDAUDITENTRIES.LIST>" +
                                                    "<ACCOUNTAUDITENTRIES.LIST></ACCOUNTAUDITENTRIES.LIST>" +
                                                    "<AUDITENTRIES.LIST></AUDITENTRIES.LIST>" +
                                                    "<INPUTCRALLOCS.LIST></INPUTCRALLOCS.LIST>" +
                                                    "<INVENTORYALLOCATIONS.LIST></INVENTORYALLOCATIONS.LIST>" +
                                                    "<DUTYHEADDETAILS.LIST></DUTYHEADDETAILS.LIST>" +
                                                    "<EXCISEDUTYHEADDETAILS.LIST></EXCISEDUTYHEADDETAILS.LIST>" +
                                                    "<RATEDETAILS.LIST></RATEDETAILS.LIST>" +
                                                    "<SUMMARYALLOCS.LIST></SUMMARYALLOCS.LIST>" +
                                                    "<STPYMTDETAILS.LIST></STPYMTDETAILS.LIST>" +
                                                    "<EXCISEPAYMENTALLOCATIONS.LIST></EXCISEPAYMENTALLOCATIONS.LIST>" +
                                                    "<TAXBILLALLOCATIONS.LIST></TAXBILLALLOCATIONS.LIST>" +
                                                    "<TAXOBJECTALLOCATIONS.LIST></TAXOBJECTALLOCATIONS.LIST>" +
                                                    "<TDSEXPENSEALLOCATIONS.LIST></TDSEXPENSEALLOCATIONS.LIST>" +
                                                    "<VATSTATUTORYDETAILS.LIST></VATSTATUTORYDETAILS.LIST>" +
                                                    "<REFVOUCHERDETAILS.LIST></REFVOUCHERDETAILS.LIST>" +
                                                    "<INVOICEWISEDETAILS.LIST></INVOICEWISEDETAILS.LIST>" +
                                                    "<VATITCDETAILS.LIST></VATITCDETAILS.LIST>" +
                                                    "<ADVANCETAXDETAILS.LIST></ADVANCETAXDETAILS.LIST>" +
                                                    "</ALLLEDGERENTRIES.LIST>";
                        }
                        else
                        {
                            str_AllLedgerEntries = str_AllLedgerEntries +

                                                 "<ALLLEDGERENTRIES.LIST>" +
                                                  "<LEDGERNAME>" + lblFeeHeadName + "</LEDGERNAME>" +
                                                     "<VOUCHERFBTCATEGORY/>" +
                                                    "<GSTCLASS/>" +
                                                    "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" +
                                                    "<LEDGERFROMITEM>No</LEDGERFROMITEM>" +
                                                    "<ISPARTYLEDGER>No</ISPARTYLEDGER>" +
                                                    "<ISLASTDEEMEDPOSITIVE>No</ISLASTDEEMEDPOSITIVE>" +
                                                    "<AMOUNT>" + txtAmt.Text + "</AMOUNT>" +
                                                    "<VATEXPAMOUNT>" + txtAmt.Text + "</VATEXPAMOUNT>" +
                                                    "<CURRPARTYLEDGERNAME>" + lblFeeHeadName + "</CURRPARTYLEDGERNAME>" +
                                                   "<SERVICETAXDETAILS.LIST></SERVICETAXDETAILS.LIST>" +
                                                   "<CATEGORYALLOCATIONS.LIST></CATEGORYALLOCATIONS.LIST>" +
                                                   "<BANKALLOCATIONS.LIST></BANKALLOCATIONS.LIST>" +
                                                   " <BILLALLOCATIONS.LIST></BILLALLOCATIONS.LIST>" +
                                                    "<INTERESTCOLLECTION.LIST>" +
                                                    "</INTERESTCOLLECTION.LIST>" +
                                                    "<OLDAUDITENTRIES.LIST></OLDAUDITENTRIES.LIST>" +
                                                    "<ACCOUNTAUDITENTRIES.LIST></ACCOUNTAUDITENTRIES.LIST>" +
                                                    "<AUDITENTRIES.LIST></AUDITENTRIES.LIST>" +
                                                    "<INPUTCRALLOCS.LIST></INPUTCRALLOCS.LIST>" +
                                                    "<INVENTORYALLOCATIONS.LIST></INVENTORYALLOCATIONS.LIST>" +
                                                    "<DUTYHEADDETAILS.LIST></DUTYHEADDETAILS.LIST>" +
                                                    "<EXCISEDUTYHEADDETAILS.LIST></EXCISEDUTYHEADDETAILS.LIST>" +
                                                    "<RATEDETAILS.LIST></RATEDETAILS.LIST>" +
                                                    "<SUMMARYALLOCS.LIST></SUMMARYALLOCS.LIST>" +
                                                    "<STPYMTDETAILS.LIST></STPYMTDETAILS.LIST>" +
                                                    "<EXCISEPAYMENTALLOCATIONS.LIST></EXCISEPAYMENTALLOCATIONS.LIST>" +
                                                    "<TAXBILLALLOCATIONS.LIST></TAXBILLALLOCATIONS.LIST>" +
                                                    "<TAXOBJECTALLOCATIONS.LIST></TAXOBJECTALLOCATIONS.LIST>" +
                                                    "<TDSEXPENSEALLOCATIONS.LIST></TDSEXPENSEALLOCATIONS.LIST>" +
                                                    "<VATSTATUTORYDETAILS.LIST></VATSTATUTORYDETAILS.LIST>" +
                                                    "<REFVOUCHERDETAILS.LIST></REFVOUCHERDETAILS.LIST>" +
                                                    "<INVOICEWISEDETAILS.LIST></INVOICEWISEDETAILS.LIST>" +
                                                    "<VATITCDETAILS.LIST></VATITCDETAILS.LIST>" +
                                                    "<ADVANCETAXDETAILS.LIST></ADVANCETAXDETAILS.LIST>" +
                                                    "</ALLLEDGERENTRIES.LIST>";
                        }
                    }
                }
                if (objFunc.Get_details("select Isactive from TallyConfiguration where InstID =" + ddlInstitute.SelectedValue + "") == "True")
                {

                    DueStudentFees();
                }
                Academicsvc objA = new Academicsvc();
                string Retv = objA.StudentRegDetailTInsert(objFPD);
                hdnStatus.Value = hdnStuID.Value;
                if (Retv == "S")
                {
                    wizReg.ActiveStepIndex = 0;
                    btnfinish.Visible = false;
                    Reset();
                    objFunc.FillActivityLog(Convert.ToInt32(Session["UID"]), "Student Registration", "New Student Registered", Convert.ToInt32(Session["instID"].ToString()));
                    objFunc.MsgBox1("Student has been registered successfully....!!!", UpdatePanel1);
                    return;
                }
                else
                {
                    objFunc.MsgBox1(Retv, UpdatePanel1);
                    return;
                }

            }
        }

        catch (Exception ex)
        {
            objFunc.MsgBox1(ex.Message, UpdatePanel1);
            return;
        }
    }

    public void DueStudentFees()
    {
        try
        {
            string xmlstc = String.Empty;
            xmlstc = "<ENVELOPE>";
            xmlstc = xmlstc + " <HEADER>";
            xmlstc = xmlstc + "<TALLYREQUEST>Import Data</TALLYREQUEST>";
            xmlstc = xmlstc + "</HEADER>";
            xmlstc = xmlstc + "<BODY>";
            xmlstc = xmlstc + "<IMPORTDATA>";
            xmlstc = xmlstc + "<REQUESTDESC>";
            xmlstc = xmlstc + "<REPORTNAME>Vouchers</REPORTNAME>";
            xmlstc = xmlstc + "<STATICVARIABLES>";
            xmlstc = xmlstc + "<SVCURRENTCOMPANY>" + ddlInstitute.SelectedItem.Text + "</SVCURRENTCOMPANY>";
            xmlstc = xmlstc + "</STATICVARIABLES>";
            xmlstc = xmlstc + "</REQUESTDESC>";
            xmlstc = xmlstc + "<REQUESTDATA>";
            xmlstc = xmlstc + "<TALLYMESSAGE xmlns:UDF='TallyUDF'>";
            xmlstc = xmlstc + "<VOUCHER REMOTEID='bb329008-15db-4af7-a670-30df6fb011b1-00000000' VCHKEY='bb329008-15db-4af7-a670-30df6fb011b1-0000a8b6:00000000' VCHTYPE='Journal' ACTION='Create' OBJVIEW='Accounting Voucher View'>";
            xmlstc = xmlstc + "<DATE>01/04/2018</DATE>";
            xmlstc = xmlstc + "<GUID>bb329008-15db-4af7-a670-30df6fb011b1-00000000</GUID>";
            xmlstc = xmlstc + "<VOUCHERTYPENAME>Journal</VOUCHERTYPENAME>";
            xmlstc = xmlstc + "<VOUCHERNUMBER>0</VOUCHERNUMBER>";
            xmlstc = xmlstc + "<CSTFORMISSUETYPE/>";
            xmlstc = xmlstc + "<CSTFORMRECVTYPE/>";
            xmlstc = xmlstc + "<PERSISTEDVIEW>Accounting Voucher View</PERSISTEDVIEW>";
            xmlstc = xmlstc + "<VCHGSTCLASS/>";
            xmlstc = xmlstc + "<DIFFACTUALQTY>No</DIFFACTUALQTY>";
            xmlstc = xmlstc + "<ASORIGINAL>No</ASORIGINAL>";
            xmlstc = xmlstc + "<FORJOBCOSTING>No</FORJOBCOSTING>";
            xmlstc = xmlstc + "<ISOPTIONAL>No</ISOPTIONAL>";
            xmlstc = xmlstc + "<EFFECTIVEDATE>01/04/2018</EFFECTIVEDATE>";
            xmlstc = xmlstc + "<USEFOREXCISE>No</USEFOREXCISE>";
            xmlstc = xmlstc + "<USEFORINTEREST>No</USEFORINTEREST>";
            xmlstc = xmlstc + "<USEFORGAINLOSS>No</USEFORGAINLOSS>";
            xmlstc = xmlstc + "<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>";
            xmlstc = xmlstc + "<USEFORCOMPOUND>No</USEFORCOMPOUND>";
            xmlstc = xmlstc + "<USEFORSERVICETAX>No</USEFORSERVICETAX>";
            xmlstc = xmlstc + "<EXCISETAXOVERRIDE>No</EXCISETAXOVERRIDE>";
            xmlstc = xmlstc + "<ISTDSOVERRIDDEN>No</ISTDSOVERRIDDEN>";
            xmlstc = xmlstc + "<ISTCSOVERRIDDEN>No</ISTCSOVERRIDDEN>";
            xmlstc = xmlstc + "<ISVATOVERRIDDEN>No</ISVATOVERRIDDEN>";
            xmlstc = xmlstc + "<ISSERVICETAXOVERRIDDEN>No</ISSERVICETAXOVERRIDDEN>";
            xmlstc = xmlstc + "<ISEXCISEOVERRIDDEN>No</ISEXCISEOVERRIDDEN>";
            xmlstc = xmlstc + "<ISGSTOVERRIDDEN>No</ISGSTOVERRIDDEN>";
            xmlstc = xmlstc + "<ISCANCELLED>No</ISCANCELLED>";
            xmlstc = xmlstc + "<HASCASHFLOW>No</HASCASHFLOW>";
            xmlstc = xmlstc + "<ISPOSTDATED>No</ISPOSTDATED>";
            xmlstc = xmlstc + "<USETRACKINGNUMBER>No</USETRACKINGNUMBER>";
            xmlstc = xmlstc + "<ISINVOICE>No</ISINVOICE>";
            xmlstc = xmlstc + "<MFGJOURNAL>No</MFGJOURNAL>";
            xmlstc = xmlstc + "<HASDISCOUNTS>No</HASDISCOUNTS>";
            xmlstc = xmlstc + "<ASPAYSLIP>No</ASPAYSLIP>";
            xmlstc = xmlstc + "<ISCOSTCENTRE>No</ISCOSTCENTRE>";
            xmlstc = xmlstc + "<ISSTXNONREALIZEDVCH>No</ISSTXNONREALIZEDVCH>";
            xmlstc = xmlstc + "<ISBLANKCHEQUE>No</ISBLANKCHEQUE>";
            xmlstc = xmlstc + "<ISVOID>No</ISVOID>";
            xmlstc = xmlstc + "<ISONHOLD>No</ISONHOLD>";
            xmlstc = xmlstc + "<ORDERLINESTATUS>No</ORDERLINESTATUS>";
            xmlstc = xmlstc + "<ISVATDUTYPAID>Yes</ISVATDUTYPAID>";
            xmlstc = xmlstc + "<ISDELETED>No</ISDELETED>";
            xmlstc = xmlstc + "<EXCLUDEDTAXATIONS.LIST></EXCLUDEDTAXATIONS.LIST>";
            xmlstc = xmlstc + "<OLDAUDITENTRIES.LIST></OLDAUDITENTRIES.LIST>";
            xmlstc = xmlstc + "<ACCOUNTAUDITENTRIES.LIST></ACCOUNTAUDITENTRIES.LIST>";
            xmlstc = xmlstc + "<AUDITENTRIES.LIST></AUDITENTRIES.LIST>";
            xmlstc = xmlstc + "<DUTYHEADDETAILS.LIST></DUTYHEADDETAILS.LIST>";
            xmlstc = xmlstc + "<SUPPLEMENTARYDUTYHEADDETAILS.LIST></SUPPLEMENTARYDUTYHEADDETAILS.LIST>";
            xmlstc = xmlstc + "<EWAYBILLDETAILS.LIST></EWAYBILLDETAILS.LIST>";
            xmlstc = xmlstc + "<INVOICEDELNOTES.LIST></INVOICEDELNOTES.LIST>";
            xmlstc = xmlstc + "<INVOICEORDERLIST.LIST></INVOICEORDERLIST.LIST>";
            xmlstc = xmlstc + "<INVOICEINDENTLIST.LIST></INVOICEINDENTLIST.LIST>";
            xmlstc = xmlstc + "<ATTENDANCEENTRIES.LIST></ATTENDANCEENTRIES.LIST>";
            xmlstc = xmlstc + "<ORIGINVOICEDETAILS.LIST></ORIGINVOICEDETAILS.LIST>";
            xmlstc = xmlstc + "<INVOICEEXPORTLIST.LIST></INVOICEEXPORTLIST.LIST>";


            xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>";
            xmlstc = xmlstc + "<LEDGERNAME>" + hdnStudentLedgerName.Value + "</LEDGERNAME>";
            xmlstc = xmlstc + "<VOUCHERFBTCATEGORY/>";
            xmlstc = xmlstc + "<GSTCLASS/>";
            xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>";
            xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>";
            xmlstc = xmlstc + "<ISPARTYLEDGER>No</ISPARTYLEDGER>";
            xmlstc = xmlstc + "<ISLASTDEEMEDPOSITIVE>Yes</ISLASTDEEMEDPOSITIVE>";
            xmlstc = xmlstc + "<AMOUNT>" + Convert.ToDecimal(hdnDueAmount.Value) * -1 + "</AMOUNT>";
            xmlstc = xmlstc + "<VATEXPAMOUNT>" + Convert.ToDecimal(hdnDueAmount.Value) * -1 + "</VATEXPAMOUNT>";
            xmlstc = xmlstc + "<CURRPARTYLEDGERNAME>" + hdnStudentLedgerName.Value + "</CURRPARTYLEDGERNAME>";
            xmlstc = xmlstc + "<SERVICETAXDETAILS.LIST></SERVICETAXDETAILS.LIST>";
            xmlstc = xmlstc + "<CATEGORYALLOCATIONS.LIST></CATEGORYALLOCATIONS.LIST>";
            xmlstc = xmlstc + "<BANKALLOCATIONS.LIST></BANKALLOCATIONS.LIST>";
            xmlstc = xmlstc + str_BillLocationList;
            xmlstc = xmlstc + "<INTERESTCOLLECTION.LIST></INTERESTCOLLECTION.LIST>";
            xmlstc = xmlstc + "<OLDAUDITENTRIES.LIST></OLDAUDITENTRIES.LIST>";
            xmlstc = xmlstc + "<ACCOUNTAUDITENTRIES.LIST></ACCOUNTAUDITENTRIES.LIST>";
            xmlstc = xmlstc + "<AUDITENTRIES.LIST></AUDITENTRIES.LIST>";
            xmlstc = xmlstc + "<INPUTCRALLOCS.LIST></INPUTCRALLOCS.LIST>";
            xmlstc = xmlstc + "<INVENTORYALLOCATIONS.LIST></INVENTORYALLOCATIONS.LIST>";
            xmlstc = xmlstc + "<DUTYHEADDETAILS.LIST></DUTYHEADDETAILS.LIST>";
            xmlstc = xmlstc + "<EXCISEDUTYHEADDETAILS.LIST></EXCISEDUTYHEADDETAILS.LIST>";
            xmlstc = xmlstc + "<RATEDETAILS.LIST></RATEDETAILS.LIST>";
            xmlstc = xmlstc + "<SUMMARYALLOCS.LIST></SUMMARYALLOCS.LIST>";
            xmlstc = xmlstc + "<STPYMTDETAILS.LIST></STPYMTDETAILS.LIST>";
            xmlstc = xmlstc + "<EXCISEPAYMENTALLOCATIONS.LIST></EXCISEPAYMENTALLOCATIONS.LIST>";
            xmlstc = xmlstc + "<TAXBILLALLOCATIONS.LIST></TAXBILLALLOCATIONS.LIST>";
            xmlstc = xmlstc + "<TAXOBJECTALLOCATIONS.LIST></TAXOBJECTALLOCATIONS.LIST>";
            xmlstc = xmlstc + "<TDSEXPENSEALLOCATIONS.LIST></TDSEXPENSEALLOCATIONS.LIST>";
            xmlstc = xmlstc + "<VATSTATUTORYDETAILS.LIST></VATSTATUTORYDETAILS.LIST>";
            xmlstc = xmlstc + "<REFVOUCHERDETAILS.LIST></REFVOUCHERDETAILS.LIST>";
            xmlstc = xmlstc + "<INVOICEWISEDETAILS.LIST></INVOICEWISEDETAILS.LIST>";
            xmlstc = xmlstc + "<VATITCDETAILS.LIST></VATITCDETAILS.LIST>";
            xmlstc = xmlstc + "<ADVANCETAXDETAILS.LIST></ADVANCETAXDETAILS.LIST>";
            xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>";

            xmlstc = xmlstc + str_AllLedgerEntries;


            //xmlstc = xmlstc + "<ALLLEDGERENTRIES.LIST>";
            //xmlstc = xmlstc + "<LEDGERNAME>Fee Bus</LEDGERNAME>";
            //xmlstc = xmlstc + "<VOUCHERFBTCATEGORY/>";
            //xmlstc = xmlstc + "<GSTCLASS/>";
            //xmlstc = xmlstc + "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>";
            //xmlstc = xmlstc + "<LEDGERFROMITEM>No</LEDGERFROMITEM>";
            //xmlstc = xmlstc + "<ISPARTYLEDGER>No</ISPARTYLEDGER>";
            //xmlstc = xmlstc + "<ISLASTDEEMEDPOSITIVE>No</ISLASTDEEMEDPOSITIVE>";
            //xmlstc = xmlstc + "<AMOUNT>" + Convert.ToDecimal(hdnDueAmount.Value) + "</AMOUNT>";
            //xmlstc = xmlstc + "<VATEXPAMOUNT>" + Convert.ToDecimal(hdnDueAmount.Value) + "</VATEXPAMOUNT>";
            //xmlstc = xmlstc + "<CURRPARTYLEDGERNAME>"+l+"</CURRPARTYLEDGERNAME>";
            //xmlstc = xmlstc + "<SERVICETAXDETAILS.LIST></SERVICETAXDETAILS.LIST>";
            //xmlstc = xmlstc + "<CATEGORYALLOCATIONS.LIST></CATEGORYALLOCATIONS.LIST>";
            //xmlstc = xmlstc + "<BANKALLOCATIONS.LIST></BANKALLOCATIONS.LIST>";
            //xmlstc = xmlstc + "<BILLALLOCATIONS.LIST></BILLALLOCATIONS.LIST>";
            //xmlstc = xmlstc + "<INTERESTCOLLECTION.LIST></INTERESTCOLLECTION.LIST>";
            //xmlstc = xmlstc + "<OLDAUDITENTRIES.LIST></OLDAUDITENTRIES.LIST>";
            //xmlstc = xmlstc + "<ACCOUNTAUDITENTRIES.LIST></ACCOUNTAUDITENTRIES.LIST>";
            //xmlstc = xmlstc + "<AUDITENTRIES.LIST></AUDITENTRIES.LIST>";
            //xmlstc = xmlstc + "<INPUTCRALLOCS.LIST></INPUTCRALLOCS.LIST>";
            //xmlstc = xmlstc + "<INVENTORYALLOCATIONS.LIST></INVENTORYALLOCATIONS.LIST>";
            //xmlstc = xmlstc + "<DUTYHEADDETAILS.LIST></DUTYHEADDETAILS.LIST>";
            //xmlstc = xmlstc + "<EXCISEDUTYHEADDETAILS.LIST></EXCISEDUTYHEADDETAILS.LIST>";
            //xmlstc = xmlstc + "<RATEDETAILS.LIST></RATEDETAILS.LIST>";
            //xmlstc = xmlstc + "<SUMMARYALLOCS.LIST></SUMMARYALLOCS.LIST>";
            //xmlstc = xmlstc + "<STPYMTDETAILS.LIST></STPYMTDETAILS.LIST>";
            //xmlstc = xmlstc + "<EXCISEPAYMENTALLOCATIONS.LIST></EXCISEPAYMENTALLOCATIONS.LIST>";
            //xmlstc = xmlstc + "<TAXBILLALLOCATIONS.LIST></TAXBILLALLOCATIONS.LIST>";
            //xmlstc = xmlstc + "<TAXOBJECTALLOCATIONS.LIST></TAXOBJECTALLOCATIONS.LIST>";
            //xmlstc = xmlstc + "<TDSEXPENSEALLOCATIONS.LIST></TDSEXPENSEALLOCATIONS.LIST>";
            //xmlstc = xmlstc + "<VATSTATUTORYDETAILS.LIST></VATSTATUTORYDETAILS.LIST>";
            //xmlstc = xmlstc + "<REFVOUCHERDETAILS.LIST></REFVOUCHERDETAILS.LIST>";
            //xmlstc = xmlstc + "<INVOICEWISEDETAILS.LIST></INVOICEWISEDETAILS.LIST>";
            //xmlstc = xmlstc + "<VATITCDETAILS.LIST></VATITCDETAILS.LIST>";
            //xmlstc = xmlstc + "<ADVANCETAXDETAILS.LIST></ADVANCETAXDETAILS.LIST>";
            //xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>";



            xmlstc = xmlstc + "<VCHLEDTOTALTREE.LIST></VCHLEDTOTALTREE.LIST>";
            xmlstc = xmlstc + "<PAYROLLMODEOFPAYMENT.LIST></PAYROLLMODEOFPAYMENT.LIST>";
            xmlstc = xmlstc + "<ATTDRECORDS.LIST></ATTDRECORDS.LIST>";
            xmlstc = xmlstc + "<TEMPGSTRATEDETAILS.LIST></TEMPGSTRATEDETAILS.LIST>";
            xmlstc = xmlstc + "<GSTEWAYCONSIGNORADDRESS.LIST></GSTEWAYCONSIGNORADDRESS.LIST>";
            xmlstc = xmlstc + "<GSTEWAYCONSIGNEEADDRESS.LIST></GSTEWAYCONSIGNEEADDRESS.LIST>";
            xmlstc = xmlstc + "</VOUCHER>";
            xmlstc = xmlstc + "</TALLYMESSAGE>";
            xmlstc = xmlstc + "</REQUESTDATA>";
            xmlstc = xmlstc + "</IMPORTDATA>";
            xmlstc = xmlstc + "</BODY>";
            xmlstc = xmlstc + "</ENVELOPE>";



            //xmlstc = "<ENVELOPE>";
            //xmlstc = xmlstc + "<HEADER>";
            //xmlstc = xmlstc + "<TALLYREQUEST>Import Data</TALLYREQUEST>";
            //xmlstc = xmlstc + "</HEADER>";
            //xmlstc = xmlstc + "<BODY>";
            //xmlstc = xmlstc + "<IMPORTDATA><REQUESTDESC><REPORTNAME>Vouchers</REPORTNAME>";
            //xmlstc = xmlstc + "<STATICVARIABLES>";
            //xmlstc = xmlstc + "<SVCURRENTCOMPANY>" + hdnCompanyName.Value + "</SVCURRENTCOMPANY>";
            //xmlstc = xmlstc + "</STATICVARIABLES>";
            //xmlstc = xmlstc + "</REQUESTDESC>";
            //xmlstc = xmlstc + "<REQUESTDATA>";
            //xmlstc = xmlstc + "<TALLYMESSAGE xmlns:UDF='TallyUDF'>";
            //xmlstc = xmlstc + "<VOUCHER REMOTEID='bb329008-15db-4af7-a670-30df6fb011b1-00000000' VCHKEY='bb329008-15db-4af7-a670-30df6fb011b1-0000a8b6:00000000' VCHTYPE='Journal' ACTION='Create' OBJVIEW='Accounting Voucher View'>";
            //xmlstc = xmlstc + "<OLDAUDITENTRYIDS.LIST TYPE='Number'><OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS></OLDAUDITENTRYIDS.LIST>";
            //xmlstc = xmlstc + "<DATE>" + hdnTransDate.Value + "</DATE>";
            //xmlstc = xmlstc + "<GUID>bb329008-15db-4af7-a670-30df6fb011b1-00000000</GUID>";
            //xmlstc = xmlstc + "<NARRATION>" + hdnNarration.Value + "</NARRATION>";
            //xmlstc = xmlstc + "<VOUCHERTYPENAME>" + hdnVoucherType.Value + "</VOUCHERTYPENAME>";
            //xmlstc = xmlstc + "<VOUCHERNUMBER>2</VOUCHERNUMBER>";
            //xmlstc = xmlstc + "<PARTYLEDGERNAME>" + hdnStudentLedgerName.Value + "</PARTYLEDGERNAME>";
            //xmlstc = xmlstc + "<CSTFORMISSUETYPE/>";
            //xmlstc = xmlstc + "<CSTFORMRECVTYPE/>";
            //xmlstc = xmlstc + "<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>";
            //xmlstc = xmlstc + "<PERSISTEDVIEW>Accounting Voucher View</PERSISTEDVIEW>";
            //xmlstc = xmlstc + "<VCHGSTCLASS/>";
            //xmlstc = xmlstc + "<DIFFACTUALQTY>No</DIFFACTUALQTY><ISMSTFROMSYNC>No</ISMSTFROMSYNC><ASORIGINAL>No</ASORIGINAL><AUDITED>No</AUDITED><FORJOBCOSTING>No</FORJOBCOSTING><ISOPTIONAL>No</ISOPTIONAL><EFFECTIVEDATE>" + hdnTransDate.Value + "</EFFECTIVEDATE><USEFOREXCISE>No</USEFOREXCISE><ISFORJOBWORKIN>No</ISFORJOBWORKIN>";
            //xmlstc = xmlstc + "<ALLOWCONSUMPTION>No</ALLOWCONSUMPTION><USEFORINTEREST>No</USEFORINTEREST><USEFORGAINLOSS>No</USEFORGAINLOSS><USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER><USEFORCOMPOUND>No</USEFORCOMPOUND><USEFORSERVICETAX>No</USEFORSERVICETAX><ISEXCISEVOUCHER>No</ISEXCISEVOUCHER><EXCISETAXOVERRIDE>No</EXCISETAXOVERRIDE>";
            //xmlstc = xmlstc + "<USEFORTAXUNITTRANSFER>No</USEFORTAXUNITTRANSFER><EXCISEOPENING>No</EXCISEOPENING><USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION><ISTDSOVERRIDDEN>No</ISTDSOVERRIDDEN><ISTCSOVERRIDDEN>No</ISTCSOVERRIDDEN><ISTDSTCSCASHVCH>No</ISTDSTCSCASHVCH><INCLUDEADVPYMTVCH>No</INCLUDEADVPYMTVCH><ISSUBWORKSCONTRACT>No</ISSUBWORKSCONTRACT><ISVATOVERRIDDEN>No</ISVATOVERRIDDEN><IGNOREORIGVCHDATE>No</IGNOREORIGVCHDATE>";
            //xmlstc = xmlstc + "<ISSERVICETAXOVERRIDDEN>No</ISSERVICETAXOVERRIDDEN><ISISDVOUCHER>No</ISISDVOUCHER><ISEXCISEOVERRIDDEN>No</ISEXCISEOVERRIDDEN><ISEXCISESUPPLYVCH>No</ISEXCISESUPPLYVCH><ISVATPRINCIPALACCOUNT>No</ISVATPRINCIPALACCOUNT><ISSHIPPINGWITHINSTATE>No</ISSHIPPINGWITHINSTATE><ISCANCELLED>No</ISCANCELLED><HASCASHFLOW>No</HASCASHFLOW><ISPOSTDATED>No</ISPOSTDATED><USETRACKINGNUMBER>No</USETRACKINGNUMBER>";
            //xmlstc = xmlstc + "<ISINVOICE>No</ISINVOICE><MFGJOURNAL>No</MFGJOURNAL><HASDISCOUNTS>No</HASDISCOUNTS><ASPAYSLIP>No</ASPAYSLIP><ISCOSTCENTRE>No</ISCOSTCENTRE><ISSTXNONREALIZEDVCH>No</ISSTXNONREALIZEDVCH><ISEXCISEMANUFACTURERON>No</ISEXCISEMANUFACTURERON><ISBLANKCHEQUE>No</ISBLANKCHEQUE><ISVOID>No</ISVOID><ISONHOLD>No</ISONHOLD><ORDERLINESTATUS>No</ORDERLINESTATUS><VATISAGNSTCANCSALES>No</VATISAGNSTCANCSALES>";
            //xmlstc = xmlstc + "<VATISPURCEXEMPTED>No</VATISPURCEXEMPTED><ISVATRESTAXINVOICE>No</ISVATRESTAXINVOICE><VATISASSESABLECALCVCH>No</VATISASSESABLECALCVCH><ISVATDUTYPAID>Yes</ISVATDUTYPAID><ISDELIVERYSAMEASCONSIGNEE>No</ISDELIVERYSAMEASCONSIGNEE><ISDISPATCHSAMEASCONSIGNOR>No</ISDISPATCHSAMEASCONSIGNOR><ISDELETED>No</ISDELETED><CHANGEVCHMODE>No</CHANGEVCHMODE><ALTERID>3</ALTERID><MASTERID>3</MASTERID><VOUCHERKEY>0</VOUCHERKEY>";
            //xmlstc = xmlstc + "<EXCLUDEDTAXATIONS.LIST></EXCLUDEDTAXATIONS.LIST><OLDAUDITENTRIES.LIST></OLDAUDITENTRIES.LIST><ACCOUNTAUDITENTRIES.LIST></ACCOUNTAUDITENTRIES.LIST><AUDITENTRIES.LIST></AUDITENTRIES.LIST><DUTYHEADDETAILS.LIST></DUTYHEADDETAILS.LIST><SUPPLEMENTARYDUTYHEADDETAILS.LIST></SUPPLEMENTARYDUTYHEADDETAILS.LIST><INVOICEDELNOTES.LIST></INVOICEDELNOTES.LIST><INVOICEORDERLIST.LIST></INVOICEORDERLIST.LIST><INVOICEINDENTLIST.LIST></INVOICEINDENTLIST.LIST>";
            //xmlstc = xmlstc + "<ATTENDANCEENTRIES.LIST></ATTENDANCEENTRIES.LIST><ORIGINVOICEDETAILS.LIST></ORIGINVOICEDETAILS.LIST><INVOICEEXPORTLIST.LIST></INVOICEEXPORTLIST.LIST><ALLLEDGERENTRIES.LIST><OLDAUDITENTRYIDS.LIST TYPE='Number'><OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS></OLDAUDITENTRYIDS.LIST>";
            //xmlstc = xmlstc + "<LEDGERNAME>" + hdnStudentLedgerName.Value + "</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM><REMOVEZEROENTRIES>No</REMOVEZEROENTRIES><ISPARTYLEDGER>Yes</ISPARTYLEDGER><ISLASTDEEMEDPOSITIVE>Yes</ISLASTDEEMEDPOSITIVE>";
            //xmlstc = xmlstc + "<AMOUNT>" + Convert.ToDecimal(hdnDueAmount.Value) * -1 + "</AMOUNT>";
            //xmlstc = xmlstc + "<BANKALLOCATIONS.LIST></BANKALLOCATIONS.LIST>" + str_BillLocationList;
            //xmlstc = xmlstc + "<INTERESTCOLLECTION.LIST></INTERESTCOLLECTION.LIST><OLDAUDITENTRIES.LIST></OLDAUDITENTRIES.LIST><ACCOUNTAUDITENTRIES.LIST></ACCOUNTAUDITENTRIES.LIST><AUDITENTRIES.LIST></AUDITENTRIES.LIST>";
            //xmlstc = xmlstc + "<TAXBILLALLOCATIONS.LIST></TAXBILLALLOCATIONS.LIST><TAXOBJECTALLOCATIONS.LIST></TAXOBJECTALLOCATIONS.LIST><TDSEXPENSEALLOCATIONS.LIST></TDSEXPENSEALLOCATIONS.LIST><VATSTATUTORYDETAILS.LIST></VATSTATUTORYDETAILS.LIST><COSTTRACKALLOCATIONS.LIST></COSTTRACKALLOCATIONS.LIST>";
            //xmlstc = xmlstc + "</ALLLEDGERENTRIES.LIST>" + str_AllLedgerEntries;
            //xmlstc = xmlstc + "<ATTDRECORDS.LIST></ATTDRECORDS.LIST>";
            //xmlstc = xmlstc + "<UDF:INSTTYPEUDF.LIST DESC = 'InstTypeUDF' ISLIST = 'YES' TYPE = 'String'>";
            //xmlstc = xmlstc + "<UDF:INSTTYPEUDF DESC = 'InstTypeUDF'> Cheque </UDF:INSTTYPEUDF>";
            //xmlstc = xmlstc + "</UDF:INSTTYPEUDF.LIST>";
            //xmlstc = xmlstc + "<PAYROLLMODEOFPAYMENT.LIST></PAYROLLMODEOFPAYMENT.LIST><ATTDRECORDS.LIST></ATTDRECORDS.LIST>";
            //xmlstc = xmlstc + "</VOUCHER>";
            //xmlstc = xmlstc + "</TALLYMESSAGE>";
            //xmlstc = xmlstc + "</REQUESTDATA>";
            //xmlstc = xmlstc + "</IMPORTDATA>";
            //xmlstc = xmlstc + "</BODY>";
            //xmlstc = xmlstc + "</ENVELOPE>";


            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + TallyServerIP + "");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentLength = xmlstc.Length;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            streamWriter.Write(xmlstc);
            streamWriter.Close();
            string result;
            HttpWebResponse objResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();
                objFunc.MsgBox(result, this);
                return;
            }
        }

        catch (Exception ex)
        {
            if (ex.Message == "Unable to connect to the remote server")
            {
                objFunc.MsgBox("Tally is not running, please start the tally..!!", this);
                return;
            }
            else
            {
                objFunc.MsgBox(ex.Message, this);
                return;
            }

        }

    }

    protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        string ctype = objFunc.Get_details("select type from Course where courseid='" + wizReg_ddlCourse.SelectedValue + "' and InstituteID = '" + ddlInstitute.SelectedValue + "'");
        hdnCType.Value = ctype;

        objFunc.FillYearsem(wizReg_ddlYrSem, hdnCType.Value, Convert.ToInt32(wizReg_ddlCourse.SelectedValue), "0", Session["sesnID"].ToString(), Convert.ToInt32(ddlInstitute.SelectedValue), "---Select---");
        if (wizReg_ddlYrSem.Items.Count == 1)
        {
            objFunc.MsgBox1("Quarters are not defined yet, please define quartes first..!!", UpdatePanel1);
            return;
        }
        wizReg_ddlYrSem.SelectedIndex = 1;
        wizReg_ddlSession.Focus();
    }
    protected void wizReg_txtContact_TextChanged(object sender, EventArgs e)
    {
        if (wizReg_txtContact.Text != "")
        {
            string RecordExist = objFunc.Get_details("select StudentID from StudentReg where StudentName='" + wizReg_txtStudentName.Text + "' and FatherName='" + wizReg_txtFatherName.Text + "' and ContactNo='" + wizReg_txtContact.Text + "' ");
            if (RecordExist != "")
            {
                int RecordExist1 = Convert.ToInt32(objFunc.Get_details("select count(*) from StudentStatus where StudentID='" + RecordExist + "' and Status='C'"));
                if (RecordExist1 > 0)
                {
                    Div_msg.Style.Add("display", "block");
                    lblMsg.Text = "Student Having Same Student Name: " + wizReg_txtStudentName.Text + ", Father Name: " + wizReg_txtFatherName.Text + " and Contact No: " + wizReg_txtContact.Text + " ,is already in our database..";
                    wizReg_txtContact.Text = "";
                    wizReg_txtContact.Focus();
                }
                else
                {
                    Div_msg.Style.Add("display", "none");
                    lblMsg.Text = "";
                }
            }

            else
            {
                DataTable RecordMExist = objFunc.FillDataTable("select StudentName,FatherName,StudentID from StudentReg where  ContactNo='" + wizReg_txtContact.Text + "' ");
                if (RecordMExist.Rows.Count > 0)
                {
                    int RecordExist1 = Convert.ToInt32(objFunc.Get_details("select count(*) from StudentStatus where StudentID='" + RecordMExist.Rows[0]["StudentID"].ToString() + "' and Status='C'"));
                    if (RecordExist1 > 0)
                    {
                        Div_msg.Style.Add("display", "block");
                        lblMsg.Text = wizReg_txtContact.Text + " Contact number of a  Student name: " + RecordMExist.Rows[0]["StudentName"].ToString() + ", Father Name: " + RecordMExist.Rows[0]["FatherName"].ToString() + " is already in our database..";
                        wizReg_txtContact.Text = "";
                        wizReg_txtContact.Focus();
                    }
                    else
                    {
                        Div_msg.Style.Add("display", "none");
                        lblMsg.Text = "";
                    }
                }
                else
                {
                    Div_msg.Style.Add("display", "none");
                    lblMsg.Text = "";
                }
            }
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        Session["pic"] = null;
        Session["pic1"] = null;


        if (filePhoto.HasFile)
        {
            string ext = filePhoto.PostedFile.FileName.Substring(filePhoto.PostedFile.FileName.LastIndexOf(".") + 1);
            int filesize = filePhoto.PostedFile.ContentLength;
            if (filesize > 204800)
            {
                objFunc.MsgBox("Photo size can not exceed to 200kb", this);
                return;
            }
            if ((ext.ToLower().Equals("png")) || (ext.ToLower().Equals("jpeg")) || (ext.ToLower().Equals("jpg")))
            {
                byte[] imgbin = { 1, 1, 0, 0, 1, 0, 0, 0 };
                Stream imgStream = filePhoto.PostedFile.InputStream;
                int imglen = filePhoto.PostedFile.ContentLength;
                imgbin = new byte[imglen + 1];
                Int32 n = imgStream.Read(imgbin, 0, imglen);
                ViewState["_StudentPhoto"] = imgbin;
                Session["pic"] = imgbin;
                imgPhoto.ImageUrl = "~/Picture_Reg.aspx?type=photo";
                imgStream.Dispose();
                // Session["UserID"] = "1";                
            }
            else
            {
                objFunc.MsgBox("Please Upload image in (png,jpeg,jpg) formats only", this);
            }
        }
        else
        {
            objFunc.MsgBox("Please Choose file before upload", this);
        }
        if (_StudentSign.HasFile)
        {
            string folder = Server.MapPath("~/Uploads/Sign/");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string fileName = Path.GetFileName(_StudentSign.FileName);

            string savePath = Path.Combine(folder, fileName);

            _StudentSign.SaveAs(savePath);

            imgSign.ImageUrl = "~/Uploads/Sign/" + fileName;
            imgSign.Style["display"] = "none";
            lblsign.Text = "Uploaded File : " + fileName;
        }
        if (_StudentSign.HasFile)
        {
            string ext = _StudentSign.PostedFile.FileName.Substring(_StudentSign.PostedFile.FileName.LastIndexOf(".") + 1);
            int filesize = _StudentSign.PostedFile.ContentLength;
            if (filesize > 204800)
            {
                objFunc.MsgBox("Photo size can not exceed to 200kb", this);
                return;
            }
             
                byte[] imgbin1 = { 1, 1, 0, 0, 1, 0, 0, 0 };
                Stream imgStream = _StudentSign.PostedFile.InputStream;
                int imglen = _StudentSign.PostedFile.ContentLength;
                imgbin1 = new byte[imglen + 1];
                Int32 n = imgStream.Read(imgbin1, 0, imglen);
                ViewState["_StudentSign"] = imgbin1;
                Session["pic1"] = imgbin1;
                imgSign.ImageUrl = "~/Picture_Reg.aspx?type=sign";
                imgStream.Dispose();
                // Session["UserID"] = "1";                
        }
        else
        {
            objFunc.MsgBox("Please Choose file before upload", this);
        }
        if (fu10thMarksheet.HasFile)
        {
            string fileName = Path.GetFileName(fu10thMarksheet.FileName);

            string folderPath = Server.MapPath("~/Uploads/Marksheet/");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string savePath = Path.Combine(folderPath, fileName);

            fu10thMarksheet.SaveAs(savePath);

            lbl10thFileName.Text = "Uploaded File : " + fileName;
        }
        if (fu10thMarksheet.HasFile)
        {
            string ext = Path.GetExtension(fu10thMarksheet.FileName).ToLower();
            int filesize = fu10thMarksheet.PostedFile.ContentLength;

            if (filesize > 2097152)
            {
                objFunc.MsgBox1("PDF size cannot exceed 2 MB", UpdatePanel1);
                return;
            }

            if (ext == ".pdf")
            {
                byte[] fileData;

                using (Stream fileStream = fu10thMarksheet.PostedFile.InputStream)
                {
                    fileData = new byte[fu10thMarksheet.PostedFile.ContentLength];
                    fileStream.Read(fileData, 0, fileData.Length);
                }

                ViewState["fu10thMarksheet"] = fileData;
                ViewState["fu10thMarksheetext"] = ext;
                ViewState["fu10thMarksheetfilname"] = fu10thMarksheet.FileName;
                Session["fu10thMarksheet"] = fileData;
            }
            else
            {
                objFunc.MsgBox1("Please upload PDF file only.", UpdatePanel1);
                return;
            }
        }
        else
        {
            objFunc.MsgBox1("Please choose a PDF file before upload for Marksheet .", UpdatePanel1);
            return;
        }
    }
 
    protected void ddlcity_SelectedIndexChanged(object sender, EventArgs e)
    {
        Academicsvc objA = new Academicsvc();
        List<Academic.AcademicData.StudentDetailCityDM> objCity = new List<Academic.AcademicData.StudentDetailCityDM>();

        if (((DropDownList)sender).ID == "ddlcity")
        {
            objCity = objA.FillStudentDetailCity(Convert.ToInt32(ddlcity.SelectedValue), 7);

            if (objCity.Count == 0)
            {
                return;
            }

            ddlState.SelectedValue = objCity[0].stateid.ToString();
            ddlCountry.SelectedValue = objCity[0].countryid.ToString();
        }

        if (((DropDownList)sender).ID == "ddlPcity")
        {
            objCity = objA.FillStudentDetailCity(Convert.ToInt32(ddlPcity.SelectedValue), 7);

            if (objCity.Count == 0)
            {
                return;
            }

            ddlPState.SelectedValue = objCity[0].stateid.ToString();
            ddlPCountry.SelectedValue = objCity[0].countryid.ToString();
        }
    }
    protected void wizReg_txtDOB_TextChanged(object sender, EventArgs e)
    {
        if (wizReg_txtDOB.Text != "")
        {
            string RecordExist = objFunc.Get_details("select StudentID from StudentReg where StudentName='" + wizReg_txtStudentName.Text + "' and FatherName='" + wizReg_txtFatherName.Text + "' and DateOfBirth='" + wizReg_txtDOB.Text + "' ");
            if (RecordExist != "")
            {
                int RecordExist1 = Convert.ToInt32(objFunc.Get_details("select count(*) from StudentStatus where StudentID='" + RecordExist + "' and Status='C'"));
                if (RecordExist1 > 0)
                {

                    Div_msg.Style.Add("display", "block");
                    lblMsg.Text = "Student Having Same Student Name: " + wizReg_txtStudentName.Text + ", Father Name: " + wizReg_txtFatherName.Text + " and Date of Birth: " + wizReg_txtDOB.Text + " ,is already in our database..";
                    wizReg_txtDOB.Text = "";
                    wizReg_txtStudentName.Text = "";
                    wizReg_txtFatherName.Text = "";
                    wizReg_txtDOB.Focus();
                }
                else
                {
                    Div_msg.Style.Add("display", "none");
                    lblMsg.Text = "";
                }
            }
            else
            {
                Div_msg.Style.Add("display", "none");
                lblMsg.Text = "";
            }
        }
    }
    protected void txtAadhaar_TextChanged(object sender, EventArgs e)
    {
        if (txtAadhaar.Text != "")
        {
            if (ddlCountry.SelectedIndex > 0)
            {

                DataTable RecordMExist = objFunc.FillDataTable("select StudentName,FatherName,StudentID from StudentReg where  AdharCardNo='" + txtAadhaar.Text + "' ");
                if (RecordMExist.Rows.Count > 0)
                {
                    int RecordExist1 = Convert.ToInt32(objFunc.Get_details("select count(*) from StudentStatus where StudentID='" + RecordMExist.Rows[0]["StudentID"].ToString() + "' and Status='C'"));
                    if (RecordExist1 > 0)
                    {
                        Div_msg.Style.Add("display", "block");
                        lblMsg.Text = wizReg_txtContact.Text + " Aadhaar or Passport number of a  Student name: " + RecordMExist.Rows[0]["StudentName"].ToString() + ", Father Name: " + RecordMExist.Rows[0]["FatherName"].ToString() + " is already in our database..";
                        txtAadhaar.Text = "";
                        txtAadhaar.Focus();
                    }
                    else
                    {
                        if (ddlCountry.SelectedValue == "1")
                        {


                            string sAdhaar = txtAadhaar.Text;
                            if (sAdhaar.Length == 12)
                            {

                                bool isValidnumber = aadharcard.validateVerhoeff(sAdhaar);
                                if (isValidnumber)
                                {
                                    Div_msg.Style.Add("display", "none");
                                    lblMsg.Text = "";
                                    //   addon1.Style.Add("display", "block");
                                    txtAadhaar.Width = 170;

                                }
                                else
                                {
                                    Div_msg.Style.Add("display", "block");
                                    lblMsg.Text = "  Invalid Aadhaar Number: " + txtAadhaar.Text + "..";
                                    txtAadhaar.Text = "";
                                    //    addon1.Style.Add("display", "none");
                                    txtAadhaar.Width = 220;

                                }
                            }
                            else
                            {
                                Div_msg.Style.Add("display", "block");
                                lblMsg.Text = "  Invalid Aadhaar Number: " + txtAadhaar.Text + "..";
                                txtAadhaar.Text = "";
                                //  addon1.Style.Add("display", "none");
                                txtAadhaar.Width = 220;
                            }
                        }
                    }
                }

                else
                {
                    if (ddlCountry.SelectedValue == "1")
                    {
                        string sAdhaar = txtAadhaar.Text;
                        if (sAdhaar.Length == 12)
                        {

                            bool isValidnumber = aadharcard.validateVerhoeff(sAdhaar);
                            if (isValidnumber)
                            {
                                Div_msg.Style.Add("display", "none");
                                lblMsg.Text = "";
                                //   addon1.Style.Add("display", "block");
                                txtAadhaar.Width = 170;

                            }
                            else
                            {
                                Div_msg.Style.Add("display", "block");
                                lblMsg.Text = "  Invalid Aadhaar Number: " + txtAadhaar.Text + "..";
                                txtAadhaar.Text = "";
                                // addon1.Style.Add("display", "none");
                                txtAadhaar.Width = 220;

                            }
                        }
                        else
                        {
                            Div_msg.Style.Add("display", "block");
                            lblMsg.Text = "  Invalid Aadhaar Number: " + txtAadhaar.Text + "..";
                            txtAadhaar.Text = "";
                            //   addon1.Style.Add("display", "none");
                            txtAadhaar.Width = 220;
                        }
                    }
                    else
                    {
                        Div_msg.Style.Add("display", "none");
                        lblMsg.Text = "";
                    }
                }
            }
            //else
            //{
            //    objFunc.MsgBox("Please enter Address  details first..!!", this);
            //    txtAadhaar.Text = "";
            //    return;
            //}
        }
    }




    public class aadharcard
    {
        static int[,] d = new int[,]  
    
  {   
  {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},   
  {1, 2, 3, 4, 0, 6, 7, 8, 9, 5},  
  {2, 3, 4, 0, 1, 7, 8, 9, 5, 6},   
  {3, 4, 0, 1, 2, 8, 9, 5, 6, 7},  
  {4, 0, 1, 2, 3, 9, 5, 6, 7, 8},  
  {5, 9, 8, 7, 6, 0, 4, 3, 2, 1},   
  {6, 5, 9, 8, 7, 1, 0, 4, 3, 2},   
  {7, 6, 5, 9, 8, 2, 1, 0, 4, 3},   
  {8, 7, 6, 5, 9, 3, 2, 1, 0, 4},   
  {9, 8, 7, 6, 5, 4, 3, 2, 1, 0}   
  };
        static int[,] p = new int[,]   
       {  
       {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},  
       {1, 5, 7, 6, 2, 8, 3, 0, 9, 4},  
       {5, 8, 0, 3, 7, 9, 6, 1, 4, 2},  
       {8, 9, 1, 6, 0, 4, 3, 5, 2, 7},  
       {9, 4, 5, 3, 1, 2, 6, 8, 7, 0},  
       {4, 2, 8, 6, 5, 7, 3, 9, 0, 1},   
       {2, 7, 9, 3, 8, 0, 6, 4, 1, 5},   
       {7, 0, 4, 6, 9, 1, 3, 2, 5, 8}  
       };

        static int[] inv = { 0, 4, 3, 2, 1, 5, 6, 7, 8, 9 };
        public static bool validateVerhoeff(string num)
        {
            int c = 0; int[] myArray = StringToReversedIntArray(num);
            for (int i = 0; i < myArray.Length; i++)
            {
                c = d[c, p[(i % 8), myArray[i]]];
            } return c == 0;

        }
        private static int[] StringToReversedIntArray(string num)
        {
            int[] myArray = new int[num.Length];
            for (int i = 0; i < num.Length; i++)
            {
                myArray[i] = int.Parse(num.Substring(i, 1));
            }
            Array.Reverse(myArray); return myArray;
        }
    }

    
 
       

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCountry.SelectedValue == "1")
        {
            div_Aadhaar.Style.Add("display", "block");
            div_frrodate.Style.Add("display", "none");
            Div_frro.Style.Add("display", "none");
            Div_Visadate.Style.Add("display", "none");
            Div_visa.Style.Add("display", "none");
            div_passortdate.Style.Add("display", "none");
            Div_Passport.Style.Add("display", "none");
            if (txtAadhaar.Text != "")
            {
                string sAdhaar = txtAadhaar.Text;
                if (sAdhaar.Length == 12)
                {

                    bool isValidnumber = aadharcard.validateVerhoeff(sAdhaar);
                    if (isValidnumber)
                    {
                        Div_msg.Style.Add("display", "none");
                        lblMsg.Text = "";
                        // addon1.Style.Add("display", "block");
                        txtAadhaar.Width = 170;
                        div_frrodate.Style.Add("display", "none");
                        Div_frro.Style.Add("display", "none");
                        Div_Visadate.Style.Add("display", "none");
                        Div_visa.Style.Add("display", "none");
                        div_passortdate.Style.Add("display", "none");
                        Div_Passport.Style.Add("display", "none");

                    }
                    else
                    {
                        Div_msg.Style.Add("display", "block");
                        lblMsg.Text = "  Invalid Aadhaar Number: " + txtAadhaar.Text + "..";
                        txtAadhaar.Text = "";
                        //  addon1.Style.Add("display", "none");
                        txtAadhaar.Width = 170;
                        div_frrodate.Style.Add("display", "none");
                        Div_frro.Style.Add("display", "none");
                        Div_Visadate.Style.Add("display", "none");
                        Div_visa.Style.Add("display", "none");
                        div_passortdate.Style.Add("display", "none");
                        Div_Passport.Style.Add("display", "none");

                    }
                }
                else
                {
                    Div_msg.Style.Add("display", "block");
                    lblMsg.Text = "  Invalid Aadhaar Number: " + txtAadhaar.Text + "..";
                    txtAadhaar.Text = "";
                    //   addon1.Style.Add("display", "none");
                    txtAadhaar.Width = 170;
                    div_frrodate.Style.Add("display", "none");
                    Div_frro.Style.Add("display", "none");
                    Div_Visadate.Style.Add("display", "none");
                    Div_visa.Style.Add("display", "none");
                    div_passortdate.Style.Add("display", "none");
                    Div_Passport.Style.Add("display", "none");
                }
            }
        }
        else
        {
            div_Aadhaar.Style.Add("display", "none");
            div_frrodate.Style.Add("display", "block");
            Div_frro.Style.Add("display", "block");
            Div_Visadate.Style.Add("display", "block");
            Div_visa.Style.Add("display", "block");
            div_passortdate.Style.Add("display", "block");
            Div_Passport.Style.Add("display", "block");
            Div_msg.Style.Add("display", "none");
            lblMsg.Text = "";
            // addon1.Style.Add("display", "none");
            txtAadhaar.Width = 170;
        }
    }
    //protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlCountry.SelectedValue == "1")
    //    {
    //        if (txtAadhaar.Text != "")
    //        {
    //            string sAdhaar = txtAadhaar.Text;
    //            if (sAdhaar.Length == 12)
    //            {

    //                bool isValidnumber = aadharcard.validateVerhoeff(sAdhaar);
    //                if (isValidnumber)
    //                {
    //                    Div_msg.Style.Add("display", "none");
    //                    lblMsg.Text = "";
    //                    addon1.Style.Add("display", "block");
    //                    txtAadhaar.Width = 170;

    //                }
    //                else
    //                {
    //                    Div_msg.Style.Add("display", "block");
    //                    lblMsg.Text = "  Invalid Aadhaar Number: " + txtAadhaar.Text + "..";
    //                    txtAadhaar.Text = "";
    //                    addon1.Style.Add("display", "none");
    //                    txtAadhaar.Width = 170;

    //                }
    //            }
    //            else
    //            {
    //                Div_msg.Style.Add("display", "block");
    //                lblMsg.Text = "  Invalid Aadhaar Number: " + txtAadhaar.Text + "..";
    //                txtAadhaar.Text = "";
    //                addon1.Style.Add("display", "none");
    //                txtAadhaar.Width = 170;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Div_msg.Style.Add("display", "none");
    //        lblMsg.Text = "";
    //        addon1.Style.Add("display", "none");
    //        txtAadhaar.Width = 170;
    //    }
    //}

    public class pancard
    {
        public static bool ValidatePAN(string pan)
        {
            if (string.IsNullOrWhiteSpace(pan))
                return false;

            pan = pan.Trim().ToUpper();

            // PAN Format Check
            if (!Regex.IsMatch(pan, @"^[A-Z]{5}[0-9]{4}[A-Z]$"))
                return false;

            // 4th Character Category Check
            char category = pan[3];

            switch (category)
            {
                case 'P': // Individual
                case 'C': // Company
                case 'H': // HUF
                case 'F': // Firm
                case 'A': // AOP
                case 'T': // Trust
                case 'B': // BOI
                case 'L': // Local Authority
                case 'J': // Artificial Juridical Person
                case 'G': // Government
                    return true;

                default:
                    return false;
            }
        }
    }

    protected void txtStudentPANNumber_TextChanged(object sender, EventArgs e)
    {
        if (txtStudentPANNumber.Text.Trim() != "")
        {
            string pan = txtStudentPANNumber.Text.Trim().ToUpper();

            // PAN Validation
            if (!pancard.ValidatePAN(pan))
            {
                Div_msg.Style.Add("display", "block");
                lblMsg.Text = "Invalid PAN Number : " + pan;

                txtStudentPANNumber.Text = "";
                txtStudentPANNumber.Focus();
                return;
            }

            // Duplicate PAN Check
            DataTable RecordMExist = objFunc.FillDataTable("select StudentName,FatherName,StudentID from StudentReg where StudentPanNumber='" + pan + "'");

            if (RecordMExist.Rows.Count > 0)
            {
                Div_msg.Style.Add("display", "block");
                lblMsg.Text = "PAN Number already exists for Student Name : " + RecordMExist.Rows[0]["StudentName"].ToString() + ", Father Name : " + RecordMExist.Rows[0]["FatherName"].ToString();
                txtStudentPANNumber.Text = "";
                txtStudentPANNumber.Focus();
                return;
            }

            Div_msg.Style.Add("display", "none");
            lblMsg.Text = "";
        }
    }
    
}
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

public partial class Fee_Instructor_Master : System.Web.UI.Page
{
    DbFunctions objFunc = new DbFunctions();

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

        if (!IsPostBack)
        {
            fillgrid();


        }
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        int insert = objFunc.ExecuteDML("INSERT INTO Instructor_Master(Name, Eamil, MobileNo) VALUES ('" + txtname.Text + "','" + txtemail.Text + "','" + txtmobileNo.Text + "')");
         if (insert > 0)
         {

             objFunc.MsgBox("Data Submited Successfully...", this);

             return;
         }
    }

    public void fillgrid()
    {
        DataTable dt = new DataTable();
        dt = objFunc.FillDataTable("SELECT ID, Name, Eamil, MobileNo FROM Instructor_Master");
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows.Count > 0)
            {
                gridview.DataSource = dt;
                gridview.DataBind();
            }
            else
            {
                gridview.DataSource = null;
                gridview.DataBind();
            }
        }

    }
}

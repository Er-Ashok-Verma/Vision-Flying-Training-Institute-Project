using System;
using System.Net;
using System.Web;
using System.Web.UI;
 

public partial class Flying_Hour_Link_and_QR_Generate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        if (!IsPostBack)
        {
            GenerateRegistrationLink();
        }
    }

    private void GenerateRegistrationLink()
    {
        try
        {
            string baseUrl =  Request.Url.GetLeftPart(UriPartial.Authority);
            string registrationPath = ResolveUrl("~/Flying_Hour/Student_Self_Registration.aspx");
            string registrationUrl =  baseUrl + registrationPath;
            txtRegistrationLink.Text = registrationUrl;
            string qrUrl = "https://api.qrserver.com/v1/create-qr-code/?size=500x500&data="  + HttpUtility.UrlEncode(registrationUrl);
            imgQRCode.Visible = false;
            ViewState["QRUrl"] = qrUrl;
        }
        catch (Exception ex)
        {
            lblMessage.Text ="Error: " + ex.Message;
        }
    }

    protected void btnDownloadQR_Click(object sender, EventArgs e)
    {
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string qrUrl = Convert.ToString(ViewState["QRUrl"]);

            if (string.IsNullOrEmpty(qrUrl))
            {
                GenerateRegistrationLink();
                qrUrl = Convert.ToString(ViewState["QRUrl"]);
            }

            if (string.IsNullOrEmpty(qrUrl))
            {
                lblMessage.Text = "QR Code URL not found.";
                return;
            }

            using (WebClient client = new WebClient())
            {
                byte[] qrBytes = client.DownloadData(qrUrl);

                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.ContentType = "image/png";
                Response.AddHeader("Content-Disposition", "attachment; filename=Student_Self_Registration_QR.png");
                Response.AddHeader("Content-Length", qrBytes.Length.ToString());
                Response.OutputStream.Write( qrBytes, 0,qrBytes.Length);
                Response.Flush();
                HttpContext.Current .ApplicationInstance .CompleteRequest();
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "QR Download Error: " + ex.Message;
        }
    }
}
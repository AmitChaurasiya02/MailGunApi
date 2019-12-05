using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace MailGunApi
{
    public partial class SendMail : System.Web.UI.Page
    {
        SqlConnection con = connection.makeconnection();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id", typeof(int)),
                        new DataColumn("Name", typeof(string)),
                        new DataColumn("Email",typeof(string)) });
                string str = @"select * from tblmember";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                da.SelectCommand.CommandType = CommandType.Text;
                da.Fill(dt);
                grdview.DataSource = dt;
                grdview.DataBind();
            }
        }
        public static IRestResponse SendComplexMessage(string email, string sub, string text)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator =
                new HttpBasicAuthenticator("api",
                                            "2a0a92c389971bf39627f7d8e80bb07a-af6c0cec-c32da416");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "sandboxdae0aa60af644030bd428f9ef815ad19.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "amitdunitech@gmail.com");
            request.AddParameter("to", email);

            request.AddParameter("subject", sub);
            request.AddParameter("text", text);
            request.AddParameter("html",
                                  "<html>" + text + "</html>");
            //request.AddFile("attachment", Path.Combine("files", "test.jpg"));
            //request.AddFile("attachment", Path.Combine("files", "test.txt"));
            request.Method = Method.POST;
            return client.Execute(request);
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            ////Create a temporary DataTable
            //DataTable dtCustomers = new DataTable();
            //dtCustomers.Columns.AddRange(new DataColumn[2] { new DataColumn("Name", typeof(string)),
            //            new DataColumn("Email",typeof(string)) });

            //foreach (GridViewRow row in grdview.Rows)
            //{
            //    if ((row.FindControl("chkSelect") as CheckBox).Checked)
            //    {
            //        dtCustomers.Rows.Add(row.Cells[2].Text, (row.FindControl("lnkEmail") as HyperLink).Text);
            //    }
            //}
            //string subject = "Welcome Email";
            //string body = "Hello {0},<br /><br />Welcome to ASPSnippets<br /><br />Thanks.";

            ////Using Parallel Multi-Threading send multiple bulk email.
            //Parallel.ForEach(dtCustomers.AsEnumerable(), row =>
            //{
            //    SendComplexMessage(row["Email"].ToString(), subject, string.Format(body, row["Name"]));

            //});
            try
            {
                foreach (GridViewRow grow in grdview.Rows)
                {
                    //string strCustomerID = grow.Cells[0].Text.Trim();
                    string strContactName = grow.Cells[2].Text.Trim();
                    //string strAddress = grow.Cells[2].Text.Trim();
                    //string strPhone = grow.Cells[3].Text.Trim();
                    string strEmail = grow.Cells[3].Text.Trim();


                    try
                    {
                        SendComplexMessage(strEmail, txtSub.Text, txtMessage.Text.Trim());
                        ShowMessage("Email Sending successfully...!" + strContactName + " &nbsp;");
                    }
                    catch (Exception ex)
                    {
                        ShowMessage(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        protected void ImportCSV(object sender, EventArgs e)
        {
            //Upload and save the file  
            string csvPath = Server.MapPath("~/Files/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
            FileUpload1.SaveAs(csvPath);

            //Create a DataTable.  
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id", typeof(string)),
        new DataColumn("Name", typeof(string)),
        new DataColumn("Email", typeof(string))       
         });

            //Read the contents of CSV file.  
            string csvData = File.ReadAllText(csvPath);

            //Execute a loop over the rows.  
            foreach (string row in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row))
                {
                    dt.Rows.Add();
                    int i = 0;

                    //Execute a loop over the columns.  
                    foreach (string cell in row.Split(','))
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = cell;
                        i++;
                    }
                }
            }

            //Bind the DataTable.  
            grdview.DataSource = dt;
            grdview.DataBind();
        }
        void ShowMessage(string msg)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<scriptlanguage = 'javascript' > alert('" + msg + "');</ script > ");
        }
    }
}
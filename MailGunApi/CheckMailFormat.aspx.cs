using System;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;

namespace MailGunApi
{
    public partial class CheckMailFormat : System.Web.UI.Page
    {
        bool flag;
        string email = string.Empty;
        SqlConnection con = connection.makeconnection();
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] emailAddresses = { "david.jones@proseware.com", "d.j@server1.proseware.com",
                                    "jones@ms1.proseware.com", "j.@server1.proseware.com",
                                    "j@proseware.com9", "js#internal@proseware.com",
                                    "j_9@[129.126.118.1]", "j..s@proseware.com",
                                    "js*@proseware.com", "js@proseware..com",
                                    "js@proseware.com9", "j.s@server1.proseware.com",
                                    "\"j\\\"s\\\"\"@proseware.com", "js@contoso.中国" };

            foreach (var emailAddress in emailAddresses)
            {
                flag = IsValidEmail(emailAddress);
                if (flag == true)
                {
                   // createnewrow(emailAddress, "valid");


                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("validandinvalidemail", con);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;                        
                        da.SelectCommand.Parameters.AddWithValue("@action", "valid");
                        da.SelectCommand.Parameters.AddWithValue("@Email", emailAddress);
                        da.SelectCommand.Parameters.AddWithValue("@status", "valid");
                        con.Open();
                        int i = da.SelectCommand.ExecuteNonQuery();
                        con.Close();
                        if (i >= 1)
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (flag == false)
                {
                   // createnewrowinvalid(emailAddress, "Invalid");
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter("validandinvalidemail", con);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@action", "Invalid");
                        da.SelectCommand.Parameters.AddWithValue("@Email", emailAddress);
                        da.SelectCommand.Parameters.AddWithValue("@status", "Invalid");
                        con.Open();
                        int i = da.SelectCommand.ExecuteNonQuery();
                        con.Close();
                        if (i >= 1)
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                //if (RegexUtilities.IsValidEmail(emailAddress))
                //    Console.WriteLine($"Valid:   {emailAddress}");
                //else
                //    Console.WriteLine($"Invalid: {emailAddress}");
            }

        }
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public void createnewrow(string email, string valid)
        {
            DataTable mytable1 = new DataTable();
            if (ViewState["Row"] != null)
            {
                mytable1 = (DataTable)ViewState["Row"];
                DataRow dr = null;
                if (mytable1.Rows.Count > 0)
                {
                    //mytable.Columns.Add("Email", typeof(string));
                    //mytable.Columns.Add("valid", typeof(string));
                    dr = mytable1.NewRow();
                    dr["Email"] = email;
                    dr["valid"] = valid;
                    mytable1.Rows.Add(dr);
                    ViewState["Row"] = mytable1;
                    //GridView1.DataSource = ViewState["Row"];
                    //GridView1.DataBind();
                }
            }
            else
            {
                mytable1.Columns.Add("Email", typeof(string));
                mytable1.Columns.Add("valid", typeof(string));
                DataRow dr1 = mytable1.NewRow();
                dr1 = mytable1.NewRow();
                dr1["Email"] = email;
                dr1["valid"] = valid;
                mytable1.Rows.Add(dr1);
                ViewState["Row"] = mytable1;
                //GridView1.DataSource = ViewState["Row"];
                //GridView1.DataBind();
            }
            try
            {


                SqlDataAdapter da = new SqlDataAdapter("validandinvalidemail", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                //foreach (DataRow item in mytable.Rows)
                //{
                //    string emails = item.ItemArray[0].ToString();
                //}
                da.SelectCommand.Parameters.AddWithValue("@action", "valid");
                da.SelectCommand.Parameters.AddWithValue("@dt", mytable1);
                con.Open();
                int i = da.SelectCommand.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {

                }

            }
            catch (Exception ex)
            {

            }
        }

        public void createnewrowinvalid(string email, string valid)
        {
            DataTable mytable = new DataTable();
            if (ViewState["Row"] != null)
            {
                mytable = (DataTable)ViewState["Row"];
                DataRow dr = null;
                if (mytable.Rows.Count > 0)
                {
                    //mytable.Columns.Add("Email", typeof(string));
                    //mytable.Columns.Add("valid", typeof(string));
                    dr = mytable.NewRow();
                    dr["Email"] = email;
                    dr["valid"] = valid;
                    mytable.Rows.Add(dr);
                    ViewState["Row"] = mytable;
                    //GridView1.DataSource = ViewState["Row"];
                    //GridView1.DataBind();
                }
            }
            else
            {
                mytable.Columns.Add("Email", typeof(string));
                mytable.Columns.Add("valid", typeof(string));
                DataRow dr1 = mytable.NewRow();
                dr1 = mytable.NewRow();
                dr1["Email"] = email;
                dr1["valid"] = valid;
                mytable.Rows.Add(dr1);
                ViewState["Row"] = mytable;
                //GridView1.DataSource = ViewState["Row"];
                //GridView1.DataBind();
            }

            try
            {

                SqlDataAdapter da = new SqlDataAdapter("validandinvalidemail", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                //foreach (DataRow item in mytable.Rows)
                //{
                //    string emails = item.ItemArray[0].ToString();
                //}
                da.SelectCommand.Parameters.AddWithValue("@action", "invalid");
                da.SelectCommand.Parameters.AddWithValue("@ddt", mytable);
                con.Open();
                int i = da.SelectCommand.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {

                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
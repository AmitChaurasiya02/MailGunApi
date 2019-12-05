using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace MailGunApi
{
    public class connection
    {
        public connection()
        {

        }

        public static SqlConnection makeconnection()
        {
            string str = ConfigurationManager.ConnectionStrings["cstr"].ConnectionString;
            SqlConnection con = new SqlConnection(str);
            return con;
        }
    }
   
}
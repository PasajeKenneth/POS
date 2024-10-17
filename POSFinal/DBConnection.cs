using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;



namespace POSFinal
{
    public class DBConnection
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        public string MyConnection()
        {
            string con = @"Server=DESKTOP-MDKIL2O\SQLEXPRESS;Database=POS_DEMO_DB;Integrated Security=True;TrustServerCertificate=True";
            return con;
        }
        public double GetVal()
        {
            double vat = 0;
            cn = new SqlConnection(MyConnection());

            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT * FROM tblVat", cn);
                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    vat = Double.Parse(dr["vat"].ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dr?.Close(); 
                cn?.Close(); 
            }

            return vat;
        }
    }
}




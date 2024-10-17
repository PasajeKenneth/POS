using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace POSFinal
{
    public partial class frmQty : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        frmPOS fpos;
        private string pcode;
        private double price;
        private string transno;
        string stitle = "Simple POS System";
        public frmQty(frmPOS frmpos)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            fpos = frmpos;
        }

        private void frmQty_Load(object sender, EventArgs e)
        {
            
        }

        public void ProductDetails(String pcode, double price, string transno)
        {
            this.pcode = pcode;
            this.price = price;
            this.transno = transno;
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == 13) && (txtQty.Text != String.Empty))
            {
                cn.Open();
                cm = new SqlCommand("INSERT INTO tblcart (transno, pcode, price, qty, sdate) VALUES (@transno, @pcode, @price, @qty, @sdate)", cn);
                cm.Parameters.AddWithValue("@transno", transno); 
                cm.Parameters.AddWithValue("@pcode", pcode); 
                cm.Parameters.AddWithValue("@price", price); 
                cm.Parameters.AddWithValue("@qty", int.Parse(txtQty.Text)); 
                cm.Parameters.AddWithValue("@sdate", DateTime.Now); 
                cm.ExecuteNonQuery();
                cn.Close();

                fpos.ClearSearch();
                fpos.LoadCart();
                this.Dispose();

            }
        }
    }
}

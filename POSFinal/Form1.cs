using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace POSFinal
{
    public partial class Form1 : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        DBConnection dbcon = new DBConnection();

        public Form1()
        {
            InitializeComponent();

            try
            {
                cn = new SqlConnection(dbcon.MyConnection());
                cn.Open();
                MessageBox.Show("Connected");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnBrand_Click(object sender, EventArgs e)
        {
            frmBrandList frm = new frmBrandList();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            frmCategoryList frm = new frmCategoryList();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.LoadCategory();
            frm.Show();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            frmProductList frm = new frmProductList();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.LoadRecords();
            frm.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnStockIn_Click(object sender, EventArgs e)
        {
            frmStockIn frm = new frmStockIn();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
            
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            frmPOS frm = new frmPOS();
            frm.ShowDialog();
        }
    }
}
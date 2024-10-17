using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace POSFinal
{
    public partial class frmPOS : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        frmBrandList frmlist;
        string stitle = "Simple POS System";
        public frmPOS()
        {
            InitializeComponent();
            lblDate.Text = DateTime.Now.ToLongDateString();
            cn = new SqlConnection(dbcon.MyConnection());
            this.KeyPreview = true;
        }

        private void GetTransNo()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                string transno;
                int count;
                cn.Open();
                cm = new SqlCommand("select top 1 transno from tblcart where transno like '" + sdate + "%'order by id desc", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lblTransno.Text = sdate + (count + 1);
                }
                else
                {
                    transno = sdate + "1001";
                    lblTransno.Text = transno;
                }
                dr.Close();
                cn.Close();
               
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            GetTransNo();
            txtSearch.Enabled = true; 
            txtSearch.Focus();
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch.Text == String.Empty)
                {
                    return;
                }
                else
                {
                    cn.Open();
                    cm = new SqlCommand("select * from tblproduct where barcode like '" + txtSearch.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        frmQty frm = new frmQty(this);
                        frm.ProductDetails(dr["pcode"].ToString(), double.Parse(dr["price"].ToString()), lblTransno.Text);
                        dr.Close();
                        cn.Close();
                        frm.ShowDialog();
                    }
                    else
                    {
                        dr.Close();
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void LoadCart()
        {
            try 
            {
                dataGridView1.Rows.Clear();
                int i = 0;
                double total = 0;
                cn.Open();
                cm = new SqlCommand("select c.id, c.pcode, p.pdesc, c.price, c.qty, c.disc, c.total from tblcart as c inner join tblproduct as p on c.pcode = p.pcode where transno like '" + lblTransno.Text + "'", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    total += Double.Parse(dr["total"].ToString());
                    dataGridView1.Rows.Add(i,dr["id"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), Double.Parse(dr["total"].ToString()).ToString("#,##0.00"));
                }
                dr.Close (); 
                cn.Close();
                lblTotal.Text = total.ToString("#,##0.00");
                GetCartTotal();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cn.Close();
            }
        }
        public void ClearSearch()
        {
            txtSearch.Clear();
            txtSearch.Focus();
        }

        public void GetCartTotal()
        {

            double sales = Double.Parse(lblTotal.Text);
            double discount = 0;
            double vat = sales * dbcon.GetVal();
            double vatable = sales - vat;
            lblVat.Text = vat.ToString("#,##0.00");
            lblVatable.Text = vatable.ToString("#,##0.00");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (lblTransno.Text == "00000000000000") { return; }
            frmLookUp frm = new frmLookUp(this);
            frm.LoadRecords();
            frm.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Remove this item?" , stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("delete from tblcart where id like '" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close ();
                    MessageBox.Show("Item has successfully removed", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCart();
                }
            }
        }
    }
}

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
    public partial class frmSearchProductStockin : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        string stitle = "simple POS system";
        frmStockIn slist;
        public frmSearchProductStockin(frmStockIn flist)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            slist = flist;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void LoadProduct()
        {
            int i = 0;
            dataGridView1.Rows.Clear();

            using (SqlConnection cn = new SqlConnection(dbcon.MyConnection()))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cm = new SqlCommand("SELECT pcode, pdesc, qty FROM tblProduct WHERE pdesc LIKE @search ORDER BY pdesc", cn))
                    {
                        cm.Parameters.Add("@search", SqlDbType.VarChar).Value = "%" + txtSearch.Text + "%";
                        using (SqlDataReader dr = cm.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                i++;
                                dataGridView1.Rows.Add(i, dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["qty"].ToString());
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, stitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "colSelect")
            {
                if (slist.txtRefNo.Text == string.Empty) { MessageBox.Show("Please enter reference no", stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning); slist.txtRefNo.Focus(); return; }
                if (slist.txtBy.Text == string.Empty) { MessageBox.Show("Please enter stock in by", stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning); slist.txtBy.Focus(); return; }
                if (MessageBox.Show("Add this item", stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    cn.Open();
                cm = new SqlCommand("insert into tblStockin (refno,pcode, sdate,stockinby)values(@refno,@pcode, @sdate,@stockinby)", cn);
                cm.Parameters.AddWithValue("@refno", slist.txtRefNo.Text);
                cm.Parameters.AddWithValue("@pcode", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                cm.Parameters.AddWithValue("sdate", slist.dt1.Value);
                cm.Parameters.AddWithValue("stockinby", slist.txtBy.Text);
                cm.ExecuteNonQuery();
                cn.Close();

                MessageBox.Show("Successfully added", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                slist.LoadStockIn();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }
    }
}

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
    public partial class frmProductList : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();

        public frmProductList()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmProduct frm = new frmProduct(this);
            frm.btnSave.Enabled = true;
            frm.btnUpdate.Enabled = false;
            frm.LoadBrand();
            frm.LoadCategory();
            frm.ShowDialog();
        }

        public void LoadRecords()
        {
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT p.pcode, p.barcode, p.pdesc, b.brand, c.category, p.price, p.qty FROM tblProduct AS p INNER JOIN tblBrand AS b ON b.id = p.bid INNER JOIN tblCategory AS c ON c.id = p.cid WHERE p.pdesc LIKE @search", cn);
            cm.Parameters.AddWithValue("@search", txtSearch.Text + "%");
            dr = cm.ExecuteReader();
            int i = 0;
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                frmProduct frm = new frmProduct(this);
                frm.LoadCategory();
                frm.LoadBrand();
                frm.btnSave.Enabled = false;
                frm.btnUpdate.Enabled = true;

                
                frm.txtPcode.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                frm.txtBarcode.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                frm.txtPdesc.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                frm.txtPrice.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                frm.comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                frm.comboBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();

                frm.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tblProduct WHERE pcode = @pcode", cn);
                    cm.Parameters.AddWithValue("@pcode", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                    cm.ExecuteNonQuery();
                    cn.Close();
                    LoadRecords();
                }
            }
        }
    }
}
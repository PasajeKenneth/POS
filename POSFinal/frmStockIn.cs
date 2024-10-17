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

namespace POSFinal
{
    public partial class frmStockIn : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        string stitle = "simple POS system";

        public frmStockIn()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose(); // Close the form
        }
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46)
            {

            }
            else if (e.KeyChar == 8)
            {

            }
            else if ((e.KeyChar < 48) || (e.KeyChar > 57))
            {
                e.Handled = true;
            }
        } 

        
        public void LoadStockIn()
        {
            int i=0; 
            dataGridView2.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select * from vwStockin where refno like '" + txtRefNo.Text + "'and status like 'Pending'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView2.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }

        

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView2.Columns[e.ColumnIndex].Name;
            if (colName == "colDelete")
            {
                if(MessageBox.Show("Remove this item?" , stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("delete from tblstockin where id = '" + dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString() + "'",cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Item has been successfully removed.", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStockIn();
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmSearchProductStockin frm = new frmSearchProductStockin(this);
            frm.LoadProduct();
            frm.ShowDialog();
        }
        private void LoadStockInHistory()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select * from vwStockin where cast(sdate as date) between '" + date1.Value.ToShortDateString() + "' and '" + date2.Value.ToShortDateString() + "' and status like 'Done'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[5].ToString()).ToShortDateString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void Clear()
        {
            txtBy.Clear();
            txtRefNo.Clear();   
            dt1.Value = DateTime.Now;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.Rows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure you want to save this records?", stitle, MessageBoxButtons.YesNo , MessageBoxIcon.Question ) == DialogResult.Yes) {
                        for (int i = 0; i < dataGridView2.Rows.Count; i++)
                        {
                            cn.Open();
                            cm = new SqlCommand("update tblproduct set qty = qty + " + int.Parse(dataGridView2.Rows[i].Cells[5].Value.ToString()) + " where pcode like '" + dataGridView2.Rows[i].Cells[3].Value.ToString() + "'", cn);
                            cm.ExecuteNonQuery();
                            cn.Close();

                            cn.Open();
                            cm = new SqlCommand("update tblstockin set qty = qty + " + int.Parse(dataGridView2.Rows[i].Cells[5].Value.ToString()) + ", status = 'Done' where id like '" + dataGridView2.Rows[i].Cells[1].Value.ToString() + "'", cn);
                            cm.ExecuteNonQuery();
                            cn.Close();
                        }
                        Clear();
                        LoadStockIn();
                    }
                }
             
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadStockInHistory();
        }
    }
}
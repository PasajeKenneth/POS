﻿using System;
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
    public partial class frmLookUp : Form
    {
        frmPOS f;
        SqlConnection cn;
        SqlCommand cm;
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        string stitle = "Simple POS System";
        public frmLookUp(frmPOS frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            f = frm;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Select")
            {
                frmQty frm = new frmQty(f);
                frm.ProductDetails(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), Double.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString()), f.lblTransno.Text);
                frm.ShowDialog();
            }
        }
    }
}

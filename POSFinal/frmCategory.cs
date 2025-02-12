﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;

namespace POSFinal
{
    public partial class frmCategory : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        DBConnection dbcon = new DBConnection();
        frmCategoryList flist;
        public frmCategory(frmCategoryList frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            flist = frm;

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void Clear()
        {
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            txtCategory.Clear();
            txtCategory.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure want to save this Category?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("INSERT into tblCategory(category) VALUES(@category)", cn);
                    cm.Parameters.AddWithValue("@category", txtCategory.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Category has been added successfully.");
                    Clear();
                    flist.LoadCategory();


                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void frmCategory_Load(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure want to update this category?", "Update Category", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("UPDATE tblCategory set category = @category where id like '" + lblID.Text + "'", cn);
                    cm.Parameters.AddWithValue("@category", txtCategory.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Category Updated Successfully");
                    flist.LoadCategory();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }
    }
}

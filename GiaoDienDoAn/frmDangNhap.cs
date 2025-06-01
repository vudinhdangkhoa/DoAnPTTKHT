using QuanLiVLXD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GiaoDienDoAn
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }
        DBConnect db = new DBConnect();

        private void btn_DN_Click(object sender, EventArgs e)
        {
            string tendn = txt_TenDN.Text;
            string mk = txt_MK.Text;
            string sql_sel = "select * from NHANVIEN,NHOMNGUOIDUNG where NHANVIEN.MANHOM = NHOMNGUOIDUNG.MANHOM AND TENDANGNHAP='" + tendn + "'and MATKHAU='" + mk + "'";

            DataTable dt = db.getDataTable(sql_sel);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("That bai");
                txt_TenDN.Clear();
                txt_MK.Clear();
            }
            else
            {
                formBanHang f = new formBanHang(dt.Rows[0]["MANHANVIEN"].ToString());
                 
                //formBanHang.nhom = dt.Rows[0]["NHOMNGUOIDUNG"].ToString();
                this.Hide();
                f.ShowDialog();
                //this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmDangNhapcs_Load(object sender, EventArgs e)
        {

        }
    }
}

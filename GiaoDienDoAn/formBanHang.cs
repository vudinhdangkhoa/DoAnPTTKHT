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
    public partial class formBanHang : Form
    {
        public string maNv;
        public formBanHang(string ma)
        {
            maNv = ma;
            InitializeComponent();
        }
        DBConnect db = new DBConnect();
        void HT_LoaiHang()
        {

            string sqlquery = "select * from LOAIHANG";
            DataTable dt = db.getDataTable(sqlquery);
            cboLoai.DataSource = dt;
            cboLoai.DisplayMember = "TENLOAI";
            cboLoai.ValueMember = "MALOAI";

            if (dt.Rows.Count > 0)  
{  
    cboLoai.SelectedIndex = 0;  
}  
        }
        bool check = false;
        bool checkChonLoai = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            HT_LoaiHang();
            check = true;
            txtMa.Enabled = false;
            button2.Enabled = false;

        }


        private void cboLoai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!check) return;
            string a = cboLoai.SelectedValue.ToString();

            string str = "select * from HANG,DONGIA where MALOAI='" + a + "' and DONGIA.MAHANG=HANG.MAHANG ";
            DataTable dt = db.getDataTable(str);
            cboTenHang.DataSource = dt;
            cboTenHang.ValueMember = "MAHANG";
            cboTenHang.DisplayMember = "TENHG";
            checkChonLoai = true;
        }

        private void btn_chon_Click(object sender, EventArgs e)
        {
            int parsedValue; // Biến tạm để lưu giá trị khi TryParse thành công
            if (!int.TryParse(txt_soluong.Text, out parsedValue))
            {
                MessageBox.Show("Chỉ nhập số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_soluong.Focus(); // Đặt lại con trỏ vào TextBox
                return;
            }


            string strslt = "SELECT SOLUONGTON FROM HANG WHERE MAHANG='" + cboTenHang.SelectedValue + "'";
            DataTable slt = db.getDataTable(strslt);
            if (int.Parse(txt_soluong.Text) > int.Parse(slt.Rows[0]["SOLUONGTON"].ToString()))
            {
                MessageBox.Show("Số lượng tồn không đủ ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            if (!string.IsNullOrEmpty(txt_soluong.Text))
            {
                int n;
                if (int.TryParse(txt_soluong.Text, out n) == true)
                {
                    if (listBox1.DataSource != null)
                    {
                        DataTable dt = (DataTable)listBox1.DataSource;
                        bool check = false;

                        DataTable dt2 = (DataTable)cboTenHang.DataSource;
                        foreach (DataRow d in dt.Rows)
                        {
                            if (d["tenhang"].ToString() == cboTenHang.Text)
                            {
                                int sl = int.Parse(d["soluong"].ToString());
                                int tongtien = int.Parse(d["tongtien"].ToString());
                                sl = sl + int.Parse(txt_soluong.Text);
                                d["soluong"] = sl;

                                int gia = int.Parse(dt2.Rows[0]["GIACAPNHAT"].ToString());
                                tongtien = tongtien + gia * int.Parse(txt_soluong.Text);
                                d["tongtien"] = tongtien;
                                d["get"] = cboTenHang.Text + '-' + sl + '-' + d["tongtien"].ToString();
                                check = true;
                            }

                        }
                        if (!check)
                        {
                            string mh = cboTenHang.Text.ToString();

                            DataRow dr = dt.NewRow();
                            dr["mahang"] = cboTenHang.SelectedValue.ToString();
                            dr["tenhang"] = cboTenHang.Text.ToString();
                            dr["soluong"] = txt_soluong.Text;
                            dr["dongia"] = dt2.Rows[0]["GIACAPNHAT"].ToString();
                            int gia = int.Parse(dt2.Rows[0]["GIACAPNHAT"].ToString());
                            dr["tongtien"] = gia * Convert.ToUInt32(txt_soluong.Text);
                            dr["get"] = cboTenHang.Text.ToString() + '-' + txt_soluong.Text + '-' + dr["tongtien"].ToString();

                            dt.Rows.Add(dr);
                            listBox1.DataSource = dt;
                            listBox1.DisplayMember = "get";
                        }

                    }
                    else
                    {
                        string mh = cboTenHang.Text.ToString();
                        DataTable dt2 = (DataTable)cboTenHang.DataSource;
                        DataTable dt = new DataTable();
                        dt.Columns.Add("mahang");
                        dt.Columns.Add("tenhang");
                        dt.Columns.Add("soluong");
                        dt.Columns.Add("dongia");
                        dt.Columns.Add("tongtien");
                        dt.Columns.Add("get");
                        DataRow dr = dt.NewRow();
                        dr["mahang"] = cboTenHang.SelectedValue.ToString();
                        dr["tenhang"] = cboTenHang.Text.ToString();
                        dr["soluong"] = txt_soluong.Text;
                        dr["dongia"] = dt2.Rows[0]["GIACAPNHAT"];
                        int gia = int.Parse(dt2.Rows[0]["GIACAPNHAT"].ToString());
                        dr["tongtien"] = gia * Convert.ToUInt32(txt_soluong.Text);
                        dr["get"] = cboTenHang.Text.ToString() + '-' + txt_soluong.Text + '-' + dr["tongtien"].ToString();

                        dt.Rows.Add(dr);
                        listBox1.DataSource = dt;
                        listBox1.DisplayMember = "get";
                    }
                }
                else
                {
                    MessageBox.Show("khong duoc nhap chu");
                }
            }
            else
            {
                MessageBox.Show("khong duoc de trong so luong");
            }

        }

        private void button3_Click(object sender, EventArgs e)

        {

            if (listBox1.SelectedIndex != -1)
            {
                if (!string.IsNullOrEmpty(txt_slXoa.Text))
                {
                    int n;
                    if (int.TryParse(txt_slXoa.Text, out n))
                    {

                        DataTable dt = (DataTable)listBox1.DataSource;
                        if (dt != null)
                        {


                            if ((dt.Rows[listBox1.SelectedIndex]["soluong"].ToString() == "1" && txt_slXoa.Text != "0") || int.Parse(dt.Rows[listBox1.SelectedIndex]["soluong"].ToString()) <= int.Parse(txt_slXoa.Text))
                            {
                                dt.Rows.RemoveAt(listBox1.SelectedIndex);
                                dt.AcceptChanges();
                                txt_slXoa.Clear();
                                return;
                            }

                            int sl = int.Parse(dt.Rows[listBox1.SelectedIndex]["soluong"].ToString());
                            int gia = Convert.ToInt32(dt.Rows[listBox1.SelectedIndex]["tongtien"].ToString());

                            int GiaGoc = Convert.ToInt32(dt.Rows[listBox1.SelectedIndex]["dongia"]);

                            sl = sl - int.Parse(txt_slXoa.Text);
                            gia = gia - GiaGoc * int.Parse(txt_slXoa.Text);
                            dt.Rows[listBox1.SelectedIndex]["tongtien"] = gia;
                            dt.Rows[listBox1.SelectedIndex]["soluong"] = sl;
                            if (sl < 0)
                            {
                                dt.Rows.RemoveAt(listBox1.SelectedIndex);
                                dt.AcceptChanges();
                                txt_slXoa.Clear();
                                return;
                            }
                            else
                            {
                                dt.Rows[listBox1.SelectedIndex]["soluong"] = sl;
                                dt.Rows[listBox1.SelectedIndex]["get"] = cboTenHang.Text.ToString() + '-' + sl + '-' + dt.Rows[listBox1.SelectedIndex]["tongtien"].ToString();
                                txt_slXoa.Clear();
                                return;
                            }


                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("khong duoc nhap chu ");
                    }
                }
                else
                {
                    MessageBox.Show("khong duoc bo trong");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một mục để xóa.");
            }
        }
        public bool KT_TrungSDT(string sdt)
        {

            string str = "select count(*)from KHACHHANG WHERE DIENTHOAI='" + sdt + "'";
            object r = db.getScalar(str);
            if ((int)r == 1) return false;
            return true;
        }
        private void button2_Click(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)listBox1.DataSource;
            string dshang = "";
            string tenkh = txtTenKH.Text;
            string diachi = txtDIaChi.Text;
            string sdt = txtSDT.Text;
            int tongtien = 0;
            if (!string.IsNullOrEmpty(tenkh) && !string.IsNullOrEmpty(diachi) && !string.IsNullOrEmpty(sdt))
            {

                if (listBox1.Items.Count > 0)
                {
                    string ma = "";


                    
                    if (KT_TrungSDT(sdt) == true)
                    {
                        DataTable laybang = new DataTable();
                        string strl = "select top 1 * from KHACHHANG order by CAST(SUBSTRING(MAKHACHHANG,3,LEN(MAKHACHHANG)-2)AS INT) DESC";
                        laybang = db.getDataTable(strl);
                        if(laybang.Rows.Count > 0)
                        {
                            string stt = laybang.Rows[0]["MAKHACHHANG"].ToString();
                            stt = stt.Substring(2);
                            int so = int.Parse(stt) + 1;
                            ma = "KH" + so;
                        }
                        else
                        {
                            ma = "KH1";
                        }



                            string them = "insert into KHACHHANG values('" + ma + "',N'" + txtTenKH.Text + "',N'" + txtDIaChi.Text + "','" + txtSDT.Text + "')";
                        db.getNonQuery(them);


                    }
                    else
                    {
                        string laykh = "select * from KHACHHANG where DIENTHOAI='" + txtSDT.Text + "'";
                        DataTable ktKH = db.getDataTable(laykh);
                        if (ktKH != null && ktKH.Rows.Count > 0)
                        {
                            ma = ktKH.Rows[0]["MAKHACHHANG"].ToString();
                        }
                    }

                    string mahoadon = "HD";
                    string strhd = "SELECT TOP 1 * FROM HOADON ORDER BY CAST(SUBSTRING(MAHOADON, 3, LEN(MAHOADON) - 2) AS INT) DESC";
                    DataTable hd = db.getDataTable(strhd);
                    if (hd.Rows.Count > 0)
                    {
                        string stthd = hd.Rows[0]["MAHOADON"].ToString();
                        stthd = stthd.Substring(2);
                        int sohd = 0;
                        sohd = sohd + Convert.ToInt32(stthd) + 1;
                        mahoadon = mahoadon + sohd;
                    }
                    else
                    {
                        mahoadon = mahoadon + "1";
                    }

                    string ngaymua = DateTime.Now.ToString("yyyy-MM-dd");
                    int giamgia = 0;

                    string nhapHD = "insert into HOADON VALUES('" + mahoadon + "','" + maNv + "','" + ngaymua + "','" + ma + "','" + tongtien + "','" + giamgia + "','" + (tongtien - giamgia) + "')";
                    int kq = db.getNonQuery(nhapHD);
                    if (kq == 1)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string mahang = row["mahang"].ToString();
                            string soluon = row["soluong"].ToString();
                            string giaban = row["dongia"].ToString();
                            string thanhtien = row["tongtien"].ToString();
                            string themCTHD = "insert into CHiTIET_HOADON values('" + mahoadon + "','" + mahang + "','" + soluon + "','" + giaban + "','" + thanhtien + "')";
                            int kqCTHD = db.getNonQuery(themCTHD);
                        }
                    }
                    foreach (DataRow row in dt.Rows)
                    {

                        string tenhang = row["tenhang"].ToString();
                        string soluong = row["soluong"].ToString();
                        tongtien += int.Parse(row["tongtien"].ToString());
                        dshang += "Tên hàng: " + tenhang + "--Số lượng: " + soluong + "\n";
                        string trusl = "update HANG set SOLUONGTON=SOLUONGTON-" + row["soluong"].ToString() + " where MAHANG='" + row["mahang"].ToString() + "'";
                        int kqtrusl = db.getNonQuery(trusl);
                    }
                    MessageBox.Show("Tên Khách Hàng: " + tenkh + "\nĐịa chỉ: " + diachi + "\nSố điện thoại: " + sdt + "\nDanh sách hàng:\n" + dshang);
                    for (int i = 0; dt.Rows.Count > 0;)
                    {
                        dt.Rows.RemoveAt(i);
                    }
                    txtDIaChi.Clear();
                    txtSDT.Clear();
                    txtTenKH.Clear();
                    txt_soluong.Clear();
                    
                   
                }
                else
                {
                    MessageBox.Show("Phải nhập vào danh sách hàng");
                }
            }
            else
            {
                MessageBox.Show("Nhập thông tin khách hàng");
            }


        }

        private void cboTenHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!checkChonLoai) return;
            string a = cboTenHang.SelectedValue.ToString();

            string laysoluong = "select * from HANG WHERE MAHANG='" + a + "'";
            DataTable laysoluongton = db.getDataTable(laysoluong);
            if (Convert.ToInt32(laysoluongton.Rows[0]["SOLUONGTON"]) == 0)
            {
                MessageBox.Show("het hang");
            }
        }

        private void btnma_Click(object sender, EventArgs e)
        {

            string mahoadon = "HD";
            string strhd = "SELECT TOP 1 * FROM HOADON ORDER BY CAST(SUBSTRING(MAHOADON, 3, LEN(MAHOADON) - 2) AS INT) DESC";
            DataTable hd = db.getDataTable(strhd);
            if (hd.Rows.Count > 0)
            {
                string stthd = hd.Rows[0]["MAHOADON"].ToString();
                stthd = stthd.Substring(2);
                int sohd = 0;
                sohd = sohd + Convert.ToInt32(stthd) + 1;
                mahoadon = mahoadon + sohd;
            }
            else
            {
                mahoadon = mahoadon + "1";

            }
            txtMa.Text = mahoadon;


        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void txtMa_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

    }
}

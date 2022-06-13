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

namespace OnTap_QLMonAn_New
{
    public partial class frmQuanLyMonAn : Form
    {
        private QuanLyNhomMonAn QL_NhomMonAn;
        private QuanLyMonAn QL_MonAn;

        private string connStr = 
            "Server=DESKTOP-UT3VJ3N\\SQLEXPRESS; Database=OnTap_QLMonAn; Integrated Security=true;";

        public frmQuanLyMonAn()
        {
            InitializeComponent();
            QL_NhomMonAn = new QuanLyNhomMonAn();
            QL_MonAn = new QuanLyMonAn();
        }

        private void layDuLieuNhomMonAn()
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT * FROM NhomMonAn";

            conn.Open();

            SqlDataReader records = cmd.ExecuteReader();
            while (records.Read())
            {
                NhomMonAn i = new NhomMonAn()
                {
                    maNhom = Convert.ToInt32(records["MaNhom"]),
                    tenNhom = records["TenNhom"].ToString()
                };

                QL_NhomMonAn.themNhomMonAn(i);
            }

            conn.Close();
        }

        private void hienThiNhomMonAn()
        {
            layDuLieuNhomMonAn();
            cbNhomMonAn.DataSource = QL_NhomMonAn.dsNhomMonAn;
            cbNhomMonAn.DisplayMember = "tenNhom";
            cbNhomMonAn.ValueMember = "maNhom";
            cbNhomMonAn.SelectedItem = null;
        }

        private void layDuLieuMonAn()
        {
            QL_MonAn.clear();

            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT * FROM MonAn";

            conn.Open();

            SqlDataReader records = cmd.ExecuteReader();
            while (records.Read())
            {
                MonAn i = new MonAn()
                {
                    maMonAn = Convert.ToInt32(records["MaMonAn"]),
                    tenMonAn = records["TenMonAn"].ToString(),
                    nhom = Convert.ToInt32(records["Nhom"])
                };

                QL_MonAn.themMonAn(i);
            }

            conn.Close();
        }

        private string layTenNhomMonAn(int maNhom)
        {
            NhomMonAn found = QL_NhomMonAn.timNhomMonAnTheoMa(maNhom);
            return found.tenNhom;
        }

        private void hienThiMonAn(List<MonAn> dsMonAn)
        {
            lvMonAn.Items.Clear();
            foreach (MonAn i in dsMonAn)
            {
                string[] info = {
                    i.maMonAn.ToString(),
                    i.tenMonAn,
                    layTenNhomMonAn(i.nhom) // return TenNhom
                };

                ListViewItem item = new ListViewItem(info);
                lvMonAn.Items.Add(item);
            }
        }

        private void reloadMonAn()
        {
            layDuLieuMonAn();
            hienThiMonAn(QL_MonAn.dsMonAn);
        }

        private void resetDefault()
        {
            txtMaMonAn.Text = null;
            txtTenMonAn.Text = null;
            cbNhomMonAn.SelectedItem = null;
            txtTimKiem.Text = null;
        }

        private void xoaMonAn(int maMonAn)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "DELETE FROM MonAn WHERE MaMonAn=" + maMonAn;

            conn.Open();

            int res = cmd.ExecuteNonQuery();
            if (res > 0)
            {
                MessageBox.Show("Xoá món ăn thành công");
                reloadMonAn();
                resetDefault();
            }
            else
            {
                MessageBox.Show("Xoá món ăn thất bại");
            }

            conn.Close();
        }

        private void suaMonAn(MonAn i)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = conn.CreateCommand();

            // VS 2013 (TV4)
            /*cmd.CommandText = 
                "UPDATE MonAn SET " +
                "TenMonAn=" + "N'" + i.tenMonAn + "'" + "," +
                "Nhom=" + i.nhom + " " +
                "WHERE MaMonAn=" + i.maMonAn;*/

            // VS 2019 (TV3)
            cmd.CommandText =
                "UPDATE MonAn SET " +
                $"TenMonAn=N'{i.tenMonAn}'," +
                $"Nhom={i.nhom} " +
                $"WHERE MaMonAn={i.maMonAn}";

            conn.Open();

            int res = cmd.ExecuteNonQuery();
            if (res > 0)
            {
                MessageBox.Show("Sửa món ăn thành công");
                reloadMonAn();
                resetDefault();
            }
            else
            {
                MessageBox.Show("Sửa món ăn thất bại");
            }

            conn.Close();
        }

        private void themMonAn(MonAn i)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = conn.CreateCommand();

            // VS 2013 (TV4)
            /*cmd.CommandText = 
                "INSERT INTO MonAn(TenMonAn, Nhom) VALUES " +
                "(" + 
                    "N'" + i.tenMonAn + "'" + "," +
                    i.nhom +
                ")";*/

            // VS 2019 (TV3)
            cmd.CommandText =
                "INSERT INTO MonAn(TenMonAn, Nhom) VALUES " +
                $"(N'{i.tenMonAn}', {i.nhom})";

            conn.Open();

            int res = cmd.ExecuteNonQuery();
            if (res > 0)
            {
                MessageBox.Show("Thêm món ăn thành công");
                reloadMonAn();
                resetDefault();
            }
            else
            {
                MessageBox.Show("Thêm món ăn thất bại");
            }

            conn.Close();
        }

        private void frmQuanLyMonAn_Load(object sender, EventArgs e)
        {
            hienThiNhomMonAn();
            layDuLieuMonAn();
            hienThiMonAn(QL_MonAn.dsMonAn);
        }

        private void lvMonAn_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected) // ngăn chặn xử lý 2 lần
            {
                ListView owner = sender as ListView;
                int maMonAn = Convert.ToInt32(owner.SelectedItems[0].Text);
                MonAn found = QL_MonAn.timMonAnTheoMa(maMonAn);
                if (found != null)
                {
                    txtMaMonAn.Text = found.maMonAn.ToString();
                    txtTenMonAn.Text = found.tenMonAn;
                    cbNhomMonAn.SelectedValue = found.nhom;
                }
            }
        }

        private void btnMacDinh_Click(object sender, EventArgs e)
        {
            resetDefault();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string ten = txtTimKiem.Text;
            List<MonAn> founds = QL_MonAn.timMonAnTheoTen(ten);
            hienThiMonAn(founds);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (lvMonAn.SelectedItems.Count > 0)
            {
                int maMonAn = int.Parse(lvMonAn.SelectedItems[0].Text);
                xoaMonAn(maMonAn);
            }
            else
            {
                MessageBox.Show("Chưa chọn món ăn để xoá");
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // kiểm tra thông tin nhập liệu trước khi thêm hoặc sửa
            if (
                string.IsNullOrEmpty(txtTenMonAn.Text) || 
                string.IsNullOrWhiteSpace(txtTenMonAn.Text) ||
                cbNhomMonAn.SelectedItem == null
            )
            {
                MessageBox.Show("Chưa nhập đầy đủ thông tin");
                return;
            }

            // tạo đối tượng món ăn để thêm mới hoặc sửa thông tin
            MonAn i = new MonAn()
            {
                tenMonAn = txtTenMonAn.Text,
                nhom = int.Parse(cbNhomMonAn.SelectedValue.ToString())
            };

            if (string.IsNullOrEmpty(txtMaMonAn.Text))
            {
                themMonAn(i);
            }
            else
            {
                i.maMonAn = int.Parse(txtMaMonAn.Text);
                suaMonAn(i);
            }
        }
    }
}

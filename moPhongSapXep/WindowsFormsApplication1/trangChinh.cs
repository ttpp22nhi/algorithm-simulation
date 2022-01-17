using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class formTrangChinh : Form
    {
        //khai báo các biến chính
        int n = 0, dem = 0; //n lưu số phần tử hiện tại của mảng label, dem là vị trí hiện tại của mảng (sd cho thêm bằng tay)
        int tocDo = 26; //giá trị để sử dụng cho hàm Thread.Sleep() - tăng giảm tốc độ
        int gtTocDo = 0; //biểu diễn tốc độ cao - thấp hiện tại
        int dau = 1; //sử dụng để quyết định sắp xếp tăng hay giảm, gt ban đầu = 1
        int luaChon = 0; //kiểm tra lựa chọn thuật toán sắp xếp trước đó là gì
        Random rd = new Random(); //khai báo một kiểu dữ liệu random cho sinh số ngẫu nhiên
        Label[] lb = new Label[30]; //mảng label
        Point[] pt = new Point[30]; //mảng tọa độ label
        sapXep sapXep1; //khai báo một đối tượng của lớp sapXep
        Thread luong1; //thread thứ nhất
        Thread luong2; //thread thứ 2


        //khai báo các biến phụ cho đổi chỗ trực tiếp
        Label lbi, lbj; Point pti, ptj;

        //khai báo các biến phụ cho sx chọn
        Label lbMin; Point ptMin;

        //khai báo các biến phụ cho sx nổi bọt
        Label lbj1; Point ptj1;

        //khai báo các biến phụ cho sx chèn
        Label lbPos, lbPos1; Point ptPos, ptPos1;

        //khai báo các biến phụ cho sx nhanh, trộn
        Label lbLeft, lbMid, lbRight;
        Point ptLeft, ptMid, ptRight;

        //khai báo các biến phụ cho sx vun đống
        Point[] ptHeap = new Point[30];

        public formTrangChinh()
        {
            InitializeComponent();
            //khởi tạo
            Control.CheckForIllegalCrossThreadCalls = false;
            rdTuDong.Checked = true;
            rdTang.Checked = true;
            btnTamDung.Enabled = false;
            btnDatLai.Enabled = false;
            txtTocDo.Text = gtTocDo.ToString();
        }
        
        //RadianButton "Tự động" được chọn
        private void rdTuDong_CheckedChanged(object sender, EventArgs e)
        {
            if (rdTuDong.Checked == true)
            {
                lbSoLuong.Enabled = true;
                txtSoLuong.Enabled = true;
                lbMang.Enabled = false;
                txtNhap.Enabled = false;
            }
        }

        //RadianButton "Bằng tay" được chọn
        private void rdBangTay_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBangTay.Checked == true)
            {
                btnThem.Enabled = true;
                lbMang.Enabled = true;
                txtNhap.Enabled = true;;
                lbSoLuong.Enabled = false;
                txtSoLuong.Enabled = false;
            }
        }

        //Button "+" được click
        private void btnGiamToc_Click(object sender, EventArgs e)
        {
            if (tocDo < 26)
            {
                tocDo += 5;
                txtTocDo.Text = (--gtTocDo).ToString();
            }
        }

        //Button "-" được click
        private void btnTangToc_Click(object sender, EventArgs e)
        {
            if (tocDo > 1)
            {
                tocDo -= 5;
                txtTocDo.Text = (++gtTocDo).ToString();
            }
        }

        //Button "Thấp" được click
        private void btnThap_Click(object sender, EventArgs e)
        {
            tocDo = 26;
            gtTocDo = 0;
            txtTocDo.Text = gtTocDo.ToString();
        }

        //Button "Cao" được click
        private void btnCao_Click(object sender, EventArgs e)
        {
            tocDo = 1;
            gtTocDo = 5;
            txtTocDo.Text = gtTocDo.ToString();
        }

        //RadianButton "Tăng" được chọn
        private void rdTang_CheckedChanged(object sender, EventArgs e)
        {
            dau = 1;
        }

        //RadianButton "Giảm" được chọn
        private void rdGiam_CheckedChanged(object sender, EventArgs e)
        {
            dau = -1;
        }

        //Button "Đặt lại" được click
        private void btnDatLai_Click(object sender, EventArgs e)
        {
            //kiểm tra trạng thái của từng luồng
            if(luong1 != null)
            {
                if ((luong1.ThreadState & ThreadState.SuspendRequested) == ThreadState.SuspendRequested)
                {
                    luong1.Resume();
                    luong1.Abort();
                }
                else if ((luong1.ThreadState & ThreadState.Suspended) == ThreadState.Suspended)
                {
                    luong1.Resume();
                    luong1.Abort();
                }
                else
                {
                    luong1.Abort();
                }
            }

            //đặt lại dữ liệu
            lb = new Label[30];
            pt = new Point[30];
            n = 0;
            dem = 0;
            tocDo = 26;
            gtTocDo = 0;
            dau = 1;

            //đặt lại giao diện
            groupBox3.Controls.Clear();
            txtTocDo.Text = gtTocDo.ToString();
            btnTamDung.Text = "Tạm dừng";
            btnTamDung.Enabled = false;
            btnDatLai.Enabled = btnTamDung.Enabled = false;
            btnBatDau.Enabled = true;
            rdTang.Enabled = rdGiam.Enabled = true;
            rdTuDong.Checked = rdTang.Checked = true;
            rdTrucTiep.Checked = true;
            rdChon.Checked = false;
            rdChen.Checked = false;
            rdNoiBot.Checked = false;
            rdNhanh.Checked = false;
            rdVunDong.Checked = false;
            rdTron.Checked = false;
            txtNhap.ResetText();
            txtSoLuong.ResetText();
        }

        //Button "Thêm" được click
        private void btnThem_Click(object sender, EventArgs e)
        {
            //kiem tra tinh dung dan cua dau vao
            if (rdTuDong.Checked == true && txtSoLuong.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập số lượng phần tử!", "Chú ý!");
                txtSoLuong.Focus();
                return;
            }
            if (rdBangTay.Checked == true && txtNhap.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập phần tử!", "Chú ý!");
                txtNhap.Focus();
                return;
            }

            try
            {
                if (rdTuDong.Checked == true)
                {
                    //xóa các phần tử cũ
                    if (lb[0] != null)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            groupBox3.Controls.Remove(lb[i]);
                        }
                    }

                    n = int.Parse(txtSoLuong.Text);
                    if(n>19)
                    {
                        MessageBox.Show("Số lượng phần tử không vượt quá 18!", "Chú ý!");
                        return;
                    }
                    if (n < 2)
                    {
                        MessageBox.Show("Số lượng phần tử không nhỏ hơn 2!", "Chú ý!");
                        return;
                    }
                    sapXep1 = new sapXep();

                    for (int i = 0; i < n; i++)
                    {
                        sapXep1.khoiTaoNhan(ref lb[i], ref pt[i], ref rd, i);
                        this.Invoke((MethodInvoker)delegate
                        {
                            this.groupBox3.Controls.Add(lb[i]);
                        });
                    }
                    dem = n;
                }
                else if(rdBangTay.Checked == true)
                {
                    if (n > 18)
                    {
                        MessageBox.Show("Số lượng phần tử phải nhỏ hơn hoặc bằng 100!", "Chú ý!");
                        return;
                    }
                    sapXep1 = new sapXep();
                    int tam = int.Parse(txtNhap.Text);
                    sapXep1.khoiTaoNhan(ref lb[dem], ref pt[dem], tam.ToString(), dem);
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.groupBox3.Controls.Add(lb[dem]);
                    });
                    dem++;
                    n++;
                }
                btnDatLai.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Nhập sai dữ liệu, xin mời nhập lại!", "Chú ý!");
                return;
            }

        }

        //hàm tạo thread
        public void taoLuong()
        {
            if (rdTrucTiep.Checked == true)
                luong1 = new Thread(trucTiep);
            else if (rdChon.Checked == true)
                luong1 = new Thread(chon);
            else if (rdChen.Checked == true)
                luong1 = new Thread(chen);
            else if (rdNoiBot.Checked == true)
                luong1 = new Thread(noiBot);
            else if (rdNhanh.Checked == true)
                luong1 = new Thread(nhanh);
            else if (rdVunDong.Checked == true)
                luong1 = new Thread(vunDong);
            else if (rdTron.Checked == true)
                luong1 = new Thread(tron);
        }

        //hàm tạo các phần tử phụ cho từng thuật toán sx
        public void taoPhanTuPhu()
        {
            //thiet lap cac phan tu phu
            if (rdTrucTiep.Checked == true)
            {
                lbi = new Label();
                lbj = new Label();
                pti = new Point();
                ptj = new Point();
                sapXep1.khoiTaoNhan(ref lbi, ref pti, ref rd, 0);
                sapXep1.khoiTaoNhan(ref lbj, ref ptj, ref rd, 0);
                pti.X = 0; pti.Y = 100; ptj.X = 60; ptj.Y = 100;
                lbi.Location = pti; lbj.Location = ptj;
                lbi.Text = "  i"; lbj.Text = "  j";
                this.Invoke((MethodInvoker)delegate
                {
                    this.groupBox3.Controls.Add(lbi);
                    this.groupBox3.Controls.Add(lbj);
                });
                luaChon = 0;
            }

            else if (rdChon.Checked == true)
            {
                lbi = new Label();
                lbj = new Label();
                lbMin = new Label();
                pti = new Point();
                ptj = new Point();
                ptMin = new Point();
                sapXep1.khoiTaoNhan(ref lbi, ref pti, ref rd, 0);
                sapXep1.khoiTaoNhan(ref lbj, ref ptj, ref rd, 0);
                sapXep1.khoiTaoNhan(ref lbMin, ref ptMin, ref rd, 0);
                pti.X = 0; pti.Y = 100; ptj.X = 60; ptj.Y = 100; ptMin.X = 0; ptMin.Y = 130;
                lbi.Location = pti; lbj.Location = ptj; lbMin.Location = ptMin;
                lbi.Text = "  i"; lbj.Text = "  j"; lbMin.Text = "min";
                if (rdGiam.Checked == true)
                    lbMin.Text = "max";
                this.Invoke((MethodInvoker)delegate
                {
                    this.groupBox3.Controls.Add(lbi);
                    this.groupBox3.Controls.Add(lbj);
                    this.groupBox3.Controls.Add(lbMin);
                });
                luaChon = 1;
            }

            else if (rdNoiBot.Checked == true)
            {
                lbi = new Label();
                lbj = new Label();
                lbj1 = new Label();
                pti = new Point();
                ptj = new Point();
                ptj1 = new Point();
                sapXep1.khoiTaoNhan(ref lbi, ref pti, ref rd, 0);
                sapXep1.khoiTaoNhan(ref lbj, ref ptj, ref rd, 0);
                sapXep1.khoiTaoNhan(ref lbj1, ref ptj1, ref rd, 0);
                pti.X = 0; pti.Y = 90; ptj.X = 60 * (n - 1); ptj.Y = 120; ptj1.X = 60 * (n - 2); ptj1.Y = 120;
                lbi.Location = pti; lbj.Location = ptj; lbj1.Location = ptj1;
                lbi.Text = "  i"; lbj.Text = "  j"; lbj1.Text = " j-1";
                this.Invoke((MethodInvoker)delegate
                {
                    this.groupBox3.Controls.Add(lbi);
                    this.groupBox3.Controls.Add(lbj);
                    this.groupBox3.Controls.Add(lbj1);
                });
                luaChon = 2;
            }

            else if (rdChen.Checked == true)
            {
                lbPos = new Label();
                lbPos1 = new Label();
                ptPos = new Point();
                ptPos1 = new Point();
                sapXep1.khoiTaoNhan(ref lbi, ref pti, ref rd, 0);
                sapXep1.khoiTaoNhan(ref lbPos, ref ptPos, ref rd, 0);
                sapXep1.khoiTaoNhan(ref lbPos1, ref ptPos1, ref rd, 0);

                pti.X = 0; pti.Y = 100;
                ptPos.X = 60; ptPos.Y = 20;
                ptPos1.X = 0; ptPos1.Y = 20;

                lbi.Location = pti;
                lbPos.Location = ptPos;
                lbPos1.Location = ptPos1;

                lbi.Text = "  i";
                lbPos.Text = "  p";
                lbPos1.Text = "p-1";
                this.Invoke((MethodInvoker)delegate
                {
                    this.groupBox3.Controls.Add(lbi);
                    this.groupBox3.Controls.Add(lbPos);
                    this.groupBox3.Controls.Add(lbPos1);
                });
                luaChon = 3;
            }
            else if (rdNhanh.Checked == true)
            {
                lbi = new Label();
                lbj = new Label();
                pti = new Point();
                ptj = new Point();
                lbLeft = new Label();
                lbMid = new Label();
                lbRight = new Label();
                ptLeft = new Point();
                ptMid = new Point();
                ptRight = new Point();

                sapXep1.khoiTaoNhan(ref lbi, ref pti, ref rd, 0);
                sapXep1.khoiTaoNhan(ref lbj, ref ptj, ref rd, 0);
                sapXep1.khoiTaoNhan(ref lbLeft, ref ptLeft, ref rd, 0);
                sapXep1.khoiTaoNhan(ref lbMid, ref ptMid, ref rd, 0);
                sapXep1.khoiTaoNhan(ref lbRight, ref ptRight, ref rd, 0);

                pti.X = 0; pti.Y = 170;
                ptj.X = 60 * (n - 1); ptj.Y = 200;
                ptLeft.X = 0; ptLeft.Y = 130;
                ptMid.X = 60 * ((n - 1) / 2); ptMid.Y = 20;
                ptRight.X = 60 * (n - 1); ptRight.Y = 130;

                lbi.Location = pti;
                lbj.Location = ptj;
                lbLeft.Location = ptLeft;
                lbMid.Location = ptMid;
                lbRight.Location = ptRight;

                lbi.Text = "  i";
                lbj.Text = "  j";
                lbLeft.Text = "left";
                lbMid.Text = "mid";
                lbRight.Text = "right";
                this.Invoke((MethodInvoker)delegate
                {
                    this.groupBox3.Controls.Add(lbLeft);
                    this.groupBox3.Controls.Add(lbMid);
                    this.groupBox3.Controls.Add(lbRight);
                    this.groupBox3.Controls.Add(lbi);
                    this.groupBox3.Controls.Add(lbj);
                });
                luaChon = 4;
            }

            else if (rdVunDong.Checked == true)
            {
                luaChon = 5;
            }
            else if (rdTron.Checked == true)
            {
                lbLeft = new Label();
                lbRight = new Label();
                ptLeft = new Point();
                ptRight = new Point();

                sapXep1.khoiTaoNhan(ref lbLeft, ref ptLeft, ref rd, 0);
                sapXep1.khoiTaoNhan(ref lbRight, ref ptRight, ref rd, 0);

                ptLeft.X = 0; ptLeft.Y = 10;
                ptRight.X = 60 * (n - 1); ptRight.Y = 10;

                lbLeft.Location = ptLeft;
                lbRight.Location = ptRight;

                lbLeft.Text = "left";
                lbRight.Text = "right";
                this.Invoke((MethodInvoker)delegate
                {
                    this.groupBox3.Controls.Add(lbLeft);
                    this.groupBox3.Controls.Add(lbRight);
                });
                luaChon = 6;
            }
        }

        //hàm xóa các phần tử phụ
        public void xoaPhanTuPhu()
        {
            if(luaChon == 0)
            {
                groupBox3.Controls.Remove(lbi);
                groupBox3.Controls.Remove(lbj);
            }
            else if (luaChon == 1)
            {
                groupBox3.Controls.Remove(lbi);
                groupBox3.Controls.Remove(lbj);
                groupBox3.Controls.Remove(lbMin);
            }
            else if (luaChon == 2)
            {
                groupBox3.Controls.Remove(lbi);
                groupBox3.Controls.Remove(lbj);
                groupBox3.Controls.Remove(lbj1);
            }
            else if (luaChon == 3)
            {
                groupBox3.Controls.Remove(lbi);
                groupBox3.Controls.Remove(lbPos);
                groupBox3.Controls.Remove(lbPos1);
            }
            else if (luaChon == 4)
            {
                groupBox3.Controls.Remove(lbLeft);
                groupBox3.Controls.Remove(lbMid);
                groupBox3.Controls.Remove(lbRight);
                groupBox3.Controls.Remove(lbi);
                groupBox3.Controls.Remove(lbj);
            }
            else if (luaChon == 6)
            {
                groupBox3.Controls.Remove(lbLeft);
                groupBox3.Controls.Remove(lbRight);
            }
        }

        //Button "Tạm dừng" được click
        private void btnTamDung_Click(object sender, EventArgs e)
        {
            if(btnTamDung.Text == "Tạm dừng")
            {
                luong1.Suspend();
                btnTamDung.Text = "Tiếp tục";
            }
            else
            {
                luong1.Resume();
                btnTamDung.Text = "Tạm dừng";
            }
        }

        //Button "Bắt đầu" được click
        private void btnBatDau_Click(object sender, EventArgs e)
        {
            //kiem tra du lieu dau vao
            if (lb[0] == null)
            {
                MessageBox.Show("Bạn hãy khởi tạo dữ liệu trước khi bắt đầu!", "Chú ý!");
                return;
            }
            if (rdTrucTiep.Checked == false && rdChon.Checked == false && rdChen.Checked == false
                && rdNoiBot.Checked == false && rdNhanh.Checked == false && rdVunDong.Checked == false
                && rdTron.Checked == false)
            {
                MessageBox.Show("Bạn chưa lựa chọn thuật toán sắp xếp!", "Chú ý!");
                return;
            }
            btnThem.Enabled = false;

            //xóa bỏ các nhãn phụ trước đó
            xoaPhanTuPhu();

            //tạo nhãn phụ
            taoPhanTuPhu();
            //tạo luồng
            taoLuong();
            luong1.IsBackground = true;
            luong1.Start();

            //
            rdTang.Enabled = false;
            rdGiam.Enabled = false;
            btnBatDau.Enabled = false;
            btnTamDung.Enabled = true;

            //kiểm tra luồng 1 đã kết thúc chưa
            luong2 = new Thread(kiemTraLuong1);
            luong2.IsBackground = true;
            luong2.Start();
        }

        //kiểm tra xem thread 1 đã kết thúc chưa
        public void kiemTraLuong1()
        {
            while (true)
            {
                if ((luong1.ThreadState & ThreadState.Aborted) == ThreadState.Aborted 
                    || (luong1.ThreadState & ThreadState.Stopped) == ThreadState.Stopped)
                {
                    btnBatDau.Enabled = true;
                    btnTamDung.Enabled = false;
                    btnThem.Enabled = true;
                    rdTang.Enabled = true;
                    rdGiam.Enabled = true;
                    break;
                }
            }
        }

        //hàm sắp xếp đổi chỗ trực tiếp
        public void trucTiep()
        {
            //đặt lại vị trí cho dãy label
            for (int i = 0; i < n; i++)
            {
                pt[i].Y = 60;
                lb[i].Location = pt[i];
            }
            //sắp xếp
            for (int i = 0; i < n - 1; i++)
            {
                lb[i].BackColor = Color.Red;
                lb[i].ForeColor = Color.White;
                pti.X = i * 60;
                lbi.Location = pti;
                Thread.Sleep(tocDo * 40);
                for (int j = i + 1; j < n; j++)
                {
                    lb[j].BackColor = Color.Red;
                    lb[j].ForeColor = Color.White;
                    ptj.X = j * 60;
                    lbj.Location = ptj;
                    Thread.Sleep(tocDo * 40);
                    if (int.Parse(lb[i].Text) * dau > int.Parse(lb[j].Text) * dau)
                    {
                        sapXep1.daoNhanMoPhong(ref lb[i], ref lb[j], ref pt[i], ref pt[j],ref tocDo);
                        sapXep1.daoNhan(ref lb[i], ref lb[j]);
                        sapXep1.daoDiem(ref pt[i], ref pt[j]);

                        lb[i].BackColor = Color.Red;
                        lb[i].ForeColor = Color.White;
                        lb[j].BackColor = Color.Red;
                        lb[j].ForeColor = Color.White;

                        Thread.Sleep(tocDo * 40);
                    }
                    lb[j].BackColor = Color.Yellow;
                    lb[j].ForeColor = Color.Black;
                }
                lb[i].BackColor = Color.Yellow;
                lb[i].ForeColor = Color.Black;
            }
            MessageBox.Show("Đã sắp xếp xong!", "Thông báo!");
        }

        //hàm sắp xếp chọn
        public void chon()
        {
            //đặt lại vị trí cho dãy label
            for (int i = 0; i < n; i++)
            {
                pt[i].Y = 60;
                lb[i].Location = pt[i];
            }
            //sắp xếp
            for (int i = 0; i < n - 1; i++)
            {
                pti.X = i * 60;
                lbi.Location = pti;

                lb[i].BackColor = Color.Red;
                lb[i].ForeColor = Color.White;

                int min = i;
                ptMin.X = i * 60;
                lbMin.Location = ptMin;
                Thread.Sleep(tocDo * 40);
                for (int j = i + 1; j < n; j++)
                {
                    lb[j].BackColor = Color.Red;
                    lb[j].ForeColor = Color.White;
                    ptj.X = j * 60;
                    lbj.Location = ptj;
                    Thread.Sleep(tocDo * 40);
                    if (int.Parse(lb[j].Text) * dau < int.Parse(lb[min].Text) * dau)
                    {
                        lb[min].BackColor = Color.Yellow;
                        lb[min].ForeColor = Color.Black;
                        min = j;
                        ptMin.X = j * 60;
                        lbMin.Location = ptMin;
                        Thread.Sleep(tocDo * 40);
                    }
                    lb[j].BackColor = Color.Yellow;
                    lb[j].ForeColor = Color.Black;
                    lb[min].BackColor = Color.Red;
                    lb[min].ForeColor = Color.White;
                }
                if(i != min)
                {
                    sapXep1.daoNhanMoPhong(ref lb[i], ref lb[min], ref pt[i], ref pt[min], ref tocDo);
                    sapXep1.daoNhan(ref lb[i], ref lb[min]);
                    sapXep1.daoDiem(ref pt[i], ref pt[min]);
                    lb[i].BackColor = Color.Red;
                    lb[i].ForeColor = Color.White;
                    lb[min].BackColor = Color.Red;
                    lb[min].ForeColor = Color.White;
                    Thread.Sleep(tocDo * 40);
                }
                lb[i].BackColor = Color.Yellow;
                lb[i].ForeColor = Color.Black;
                lb[min].BackColor = Color.Yellow;
                lb[min].ForeColor = Color.Black;
            }
            MessageBox.Show("Đã sắp xếp xong!", "Thông báo!");
        }

        //hàm sắp xếp chèn
        public void chen()
        {
            //đặt lại vị trí cho dãy label
            for (int i = 0; i < n; i++)
            {
                pt[i].Y = 60;
                lb[i].Location = pt[i];
            }
            //sắp xếp
            Label lbTam = new Label();
            Point ptTam = new Point();
            for (int i = 1; i < n; i++)
            {
                lb[i].BackColor = Color.Red;
                lb[i].ForeColor = Color.White;
                pti.X = i * 60;
                lbi.Location = pti;
                Thread.Sleep(tocDo * 40);
                int x = int.Parse(lb[i].Text);
                int pos = i;

                ptPos.X = pos * 60;
                lbPos.Location = ptPos;
                ptPos1.X = (pos - 1) * 60;
                lbPos1.Location = ptPos1;
                Thread.Sleep(tocDo * 40);

                sapXep1.diChuyen(ref lb[pos], ref pt[pos], lb[pos].Location.X, 140, ref tocDo);
                lb[i].BackColor = Color.Red;
                lb[i].ForeColor = Color.White;
                lbTam = lb[pos];
                ptTam = pt[pos];
                Thread.Sleep(tocDo * 40);

                while (pos > 0 && int.Parse(lb[pos - 1].Text) * dau > x * dau)
                {
                    ptPos.X = pos * 60;
                    lbPos.Location = ptPos;
                    ptPos1.X = (pos - 1) * 60;
                    lbPos1.Location = ptPos1;
                    lb[pos - 1].BackColor = Color.Red;
                    lb[pos - 1].ForeColor = Color.White;
                    Thread.Sleep(tocDo * 40);

                    sapXep1.diChuyen(ref lb[pos - 1], ref pt[pos - 1], pos * 60, lb[pos-1].Location.Y, ref tocDo);
                    lb[pos] = lb[pos - 1];
                    pt[pos] = pt[pos - 1];
                    lb[pos - 1].BackColor = Color.Red;
                    lb[pos - 1].ForeColor = Color.White;
                    Thread.Sleep(tocDo * 40);

                    pos--;
                    lb[pos].BackColor = Color.Yellow;
                    lb[pos].ForeColor = Color.Black;
                }

                lb[pos] = lbTam;
                pt[pos] = ptTam;
                sapXep1.diChuyen(ref lb[pos], ref pt[pos], (pos) * 60, lb[pos].Location.Y, ref tocDo);
                sapXep1.diChuyen(ref lb[pos], ref pt[pos], lb[pos].Location.X, 60, ref tocDo);
                Thread.Sleep(tocDo * 40);
            }
            MessageBox.Show("Đã sắp xếp xong!", "Thông báo!");
        }

        //hàm sắp xếp nổi bọt
        public void noiBot()
        {
            //đặt lại vị trí cho dãy label
            for (int i = 0; i < n; i++)
            {
                pt[i].Y = 60;
                lb[i].Location = pt[i];
            }
            //sắp xếp
            for (int i = 0; i < n - 1; i++)
            {
                pti.X = i * 60;
                lbi.Location = pti;
                lb[i].BackColor = Color.Red;
                lb[i].ForeColor = Color.White;
                Thread.Sleep(tocDo * 40);
                for (int j = n - 1; j > i; j--)
                {
                    ptj.X = j * 60;
                    lbj.Location = ptj;
                    ptj1.X = (j - 1) * 60;
                    lbj1.Location = ptj1;
                    lb[j].BackColor = Color.Red;
                    lb[j].ForeColor = Color.White;
                    lb[j - 1].BackColor = Color.Red;
                    lb[j - 1].ForeColor = Color.White;
                    Thread.Sleep(tocDo * 40);
                    if (int.Parse(lb[j - 1].Text) * dau > int.Parse(lb[j].Text) * dau)
                    {
                        sapXep1.daoNhanMoPhong(ref lb[j - 1], ref lb[j], ref pt[j - 1], ref pt[j], ref tocDo);
                        sapXep1.daoNhan(ref lb[j - 1], ref lb[j]);
                        sapXep1.daoDiem(ref pt[j - 1], ref pt[j]);
                        lb[j - 1].BackColor = Color.Red;
                        lb[j - 1].ForeColor = Color.White;
                        lb[j].BackColor = Color.Red;
                        lb[j].ForeColor = Color.White;
                        Thread.Sleep(tocDo * 40);
                    }
                    lb[j - 1].BackColor = Color.Yellow;
                    lb[j - 1].ForeColor = Color.Black;
                    lb[j].BackColor = Color.Yellow;
                    lb[j].ForeColor = Color.Black;
                }
                lb[i].BackColor = Color.Yellow;
                lb[i].ForeColor = Color.Black;
            }
            MessageBox.Show("Đã sắp xếp xong!", "Thông báo!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Home a = new Home();
            a.Show();
            this.Hide();
        }

        //hàm gọi đến hàm sắp xếp nhanh
        public void nhanh()
        {
            //đặt lại vị trí cho dãy label
            for (int i = 0; i < n; i++)
            {
                pt[i].Y = 100;
                lb[i].Location = pt[i];
            }
            //sắp xếp
            sapXepNhanh(lb, pt, 0, n - 1);
            MessageBox.Show("Đã sắp xếp xong!", "Thông báo!");
        }

        //hàm sắp xếp nhanh
        public void sapXepNhanh(Label[] a, Point[] b, int l, int r)
        {
            int key = int.Parse(a[(l + r) / 2].Text);
            int i = l;
            int j = r;
            ptLeft.X = lb[l].Location.X;
            lbLeft.Location = ptLeft;
            pti.X = lbLeft.Location.X;
            lbi.Location = pti;

            ptRight.X = lb[r].Location.X;
            lbRight.Location = ptRight;
            ptj.X = lbRight.Location.X;
            lbj.Location = ptj;

            ptMid.X = lb[(l + r) / 2].Location.X;
            lbMid.Location = ptMid;

            a[(l+r) / 2].BackColor = Color.Red;
            a[(l + r) / 2].ForeColor = Color.White;
            a[l].BackColor = Color.Red;
            a[l].ForeColor = Color.White;
            a[r].BackColor = Color.Red;
            a[r].ForeColor = Color.White;

            Thread.Sleep(tocDo * 120);
            while (i <= j)
            {
                while (int.Parse(a[i].Text) * dau < key * dau)
                {
                    i++;
                    pti.X = 60 * i;
                    lbi.Location = pti;
                    Thread.Sleep(tocDo * 80);

                }
                while (int.Parse(a[j].Text) * dau > key * dau)
                {
                    j--;
                    ptj.X = 60 * j;
                    lbj.Location = ptj;
                    Thread.Sleep(tocDo * 80);
                }
                if (i <= j)
                {
                    sapXep1.daoNhan(ref a[i], ref a[j]);
                    sapXep1.daoDiem(ref b[i], ref b[j]);

                    sapXep1.daoNhanMoPhong(ref a[i], ref a[j], ref b[i], ref b[j], ref tocDo);

                    Thread.Sleep(tocDo * 40);

                    i++;
                    pti.X = 60 * i;
                    lbi.Location = pti;

                    j--;
                    ptj.X = 60 * j;
                    lbj.Location = ptj;
                }
            }
            a[(l + r) / 2].BackColor = Color.Yellow;
            a[(l + r) / 2].ForeColor = Color.Black;
            a[l].BackColor = Color.Yellow;
            a[l].ForeColor = Color.Black;
            a[r].BackColor = Color.Yellow;
            a[r].ForeColor = Color.Black;
            Thread.Sleep(tocDo*80);
            if (l < j)
                sapXepNhanh(a, b, l, j);
            if (r > i)
                sapXepNhanh(a, b, i, r);
        }

        //hàm gọi đến hàm sắp xếp vun đống
        public void vunDong()
        {
            if (n < 3)
            {
                MessageBox.Show("Yêu cầu số phần tử lớn hơn 2!", "Chú ý!");
                return;
            }
            //đặt lại vị trí cho dãy label
            for (int i = 0 ; i < n ; i++)
            {
                ptHeap[i] = pt[i];
                ptHeap[i].Y = 220;

                pt[i].Y = 10;
                lb[i].Location = pt[i];
            }
            Thread.Sleep(tocDo * 40 + 1000);

            //mô phỏng thành cấu trúc cây
            sapXep1.diChuyen(ref lb[0], ref pt[0], 500, 40 , ref tocDo);
            sapXep1.diChuyen(ref lb[1], ref pt[1], lb[0].Location.X - 130, lb[0].Location.Y + 30, ref tocDo);
            sapXep1.diChuyen(ref lb[2], ref pt[2], lb[0].Location.X + 130, lb[0].Location.Y + 30, ref tocDo);
            for (int i = 1; 2*i+2 < n; i++)
            {
                if(i%2==0)
                {
                    sapXep1.diChuyen(ref lb[2 * i + 1], ref pt[2 * i + 1], lb[i].Location.X - 30, lb[i].Location.Y + 30, ref tocDo);
                    sapXep1.diChuyen(ref lb[2 * i + 2], ref pt[2 * i + 2], lb[i].Location.X + 70, lb[i].Location.Y + 30, ref tocDo);
                }
                else
                {
                    sapXep1.diChuyen(ref lb[2 * i + 1], ref pt[2 * i + 1], lb[i].Location.X - 70, lb[i].Location.Y + 30, ref tocDo);
                    sapXep1.diChuyen(ref lb[2 * i + 2], ref pt[2 * i + 2], lb[i].Location.X + 30, lb[i].Location.Y + 30, ref tocDo);
                }
                
            }
            if(n%2==0)
            {
                if((n/2-1) % 2 ==0)
                    sapXep1.diChuyen(ref lb[n - 1], ref pt[n - 1], lb[n / 2 - 1].Location.X - 30, lb[n / 2 - 1].Location.Y + 30, ref tocDo);
                else
                    sapXep1.diChuyen(ref lb[n - 1], ref pt[n - 1], lb[n / 2 - 1].Location.X - 70, lb[n / 2 - 1].Location.Y + 30, ref tocDo);
            }

            Thread.Sleep(tocDo * 40 + 1000);
            MessageBox.Show("Đã tạo xong cây nhị phân!", "Thông báo!");

            //sắp xếp vun đống
            sapXepVunDong(lb, pt, n);
            MessageBox.Show("Đã sắp xếp xong!", "Thông báo!");
        }

        //hàm chuẩn hóa cây con của sắp xếp vun đống
        void heapify(Label[] a, Point[] b, int n, int i)
        {
            int max = i; // khoi tao max nhu la root
            int l = 2 * i + 1; // left = 2*i + 1
            int r = 2 * i + 2; // right = 2*i + 2

            // Neu nut con trai lon hon so voi root
            if (l < n && int.Parse(a[l].Text) * dau > int.Parse(a[max].Text) * dau)
                max = l;

            // Neu nut con phai lon hon so voi root
            if (r < n && int.Parse(a[r].Text) * dau > int.Parse(a[max].Text) * dau)
                max = r;

            // Neu root khong phai la lon nhat
            if (max != i)
            {
                sapXep1.daoNhanHeap(ref a[i], ref a[max], ref b[i], ref b[max], ref tocDo);
                sapXep1.daoNhan(ref a[i], ref a[max]);
                sapXep1.daoDiem(ref b[i], ref b[max]);
                Thread.Sleep(tocDo * 80);

                heapify(a, b, n, max);
            }
        }

        //hàm sắp xếp vun đống
        void sapXepVunDong(Label[] a, Point[] b, int n)
        {
            // Tao mot dong (Sap xep lai mang)
            for (int i = n / 2 - 1; i >= 0; i--)
                heapify(a, b, n, i);
            // Trích xuất từng một phần tử một từ heap
            for (int i = n - 1; i >= 0; i--)
            {
                // Di chuyen root ve cuoi cung
                sapXep1.daoNhanHeap(ref a[0], ref a[i], ref b[0], ref b[i], ref tocDo);
                Thread.Sleep(tocDo * 80);
                sapXep1.daoNhan(ref a[0], ref a[i]);
                sapXep1.daoDiem(ref b[0], ref b[i]);
                a[i].BackColor = Color.Red;
                a[i].ForeColor = Color.White;
                if (i > 1)
                    MessageBox.Show("Đã đổi chỗ nút gốc!", "Thông báo!");
                heapify(a, b, i, 0);
            }
            MessageBox.Show("Đã chuẩn hóa xong cây nhị phân!", "Thông báo!");
            for (int i = 0; i < n; i++)
            {
                sapXep1.diChuyen(ref a[i], ref b[i], ptHeap[i].X, ptHeap[i].Y, ref tocDo);
                Thread.Sleep(tocDo * 80);
            }
        }

        //gọi đến sắp xếp trộn
        void tron()
        {
            for (int i = 0; i < n; i++)
            {
                pt[i].Y = 60;
                lb[i].Location = pt[i];
            }
            Thread.Sleep(tocDo * 40 + 1000);
            sapXepTron(lb, pt, 0, n - 1);
            MessageBox.Show("Đã sắp xếp xong!", "Thông báo!");
        }

        //trộn
        public void merge(Label [] a, Point [] b, int l, int m, int r)
        {
            for(int i1 = l;i1 <=r;i1++)
            {
                a[i1].BackColor = Color.Red;
                a[i1].ForeColor = Color.White;
            }
            ptLeft.X = pt[l].X; lbLeft.Location = ptLeft;
            ptRight.X = pt[r].X; lbRight.Location = ptRight;
            Thread.Sleep(tocDo * 120);
            int i, j, k;
            int n1 = m - l + 1;
            int n2 = r - m;
            //Tạo các mảng tạm
            Point[] ptL = new Point[100];
            Point[] ptR = new Point[100];

            Label[] lbTamL = new Label[100];
            Point[] ptTamL = new Point[100];

            Label[] lbTamR = new Label[100];
            Point[] ptTamR = new Point[100];

            Point[] ptTam = new Point[200];

            //khởi tạo giá trị cho các điểm tạm
            for (i = 0; i < n1; i++)
            {
                ptL[i].X = pt[i].X + 60;
                ptL[i].Y = pt[i].Y + 60;
            }    
            for (j = 0; j < n2; j++)
            {
                ptR[j].X = pt[j].X + 60;
                ptR[j].Y = pt[j].Y + 90;
            }

            //di chuyển các nhãn sang các điểm tạm left right, lưu các nhãn và điểm sang mảng nhãn điểm l r
            for (i = 0; i < n1; i++)
            {
                ptTam[l + i].X = pt[l + i].X;
                ptTam[l + i].Y = pt[l + i].Y;

                ptTamL[i] = pt[l + i];
                lbTamL[i] = lb[l + i];
                sapXep1.diChuyen(ref lbTamL[i], ref ptTamL[i], ptL[i].X, ptL[i].Y, ref tocDo);
                lbTamL[i].BackColor = Color.Red;
                lbTamL[i].ForeColor = Color.White;
            }
            for (j = 0; j < n2; j++)
            {
                ptTam[m + 1 + j].X = pt[m + 1 + j].X;
                ptTam[m + 1 + j].Y = pt[m + 1 + j].Y;

                ptTamR[j] = pt[m + 1 + j];
                lbTamR[j] = lb[m + 1 + j];
                sapXep1.diChuyen(ref lbTamR[j], ref ptTamR[j], ptR[j].X, ptR[j].Y, ref tocDo);
                lbTamR[j].BackColor = Color.Red;
                lbTamR[j].ForeColor = Color.White;
            }
            //Gộp hai mảng tạm vừa rồi vào mảng arr
            i = 0; j = 0; k = l;
            Thread.Sleep(tocDo * 120);
            while (i < n1 && j < n2)
            {
                if (int.Parse(lbTamL[i].Text) * dau <= int.Parse(lbTamR[j].Text) * dau)
                {
                    sapXep1.diChuyen(ref lbTamL[i], ref ptTamL[i], ptTam[k].X, ptTam[k].Y, ref tocDo);
                    lbTamL[i].BackColor = Color.Red;
                    lbTamL[i].ForeColor = Color.White;
                    pt[k] = ptTamL[i];
                    lb[k] = lbTamL[i];
                    i++;
                }
                else
                {
                    sapXep1.diChuyen(ref lbTamR[j], ref ptTamR[j], ptTam[k].X, ptTam[k].Y, ref tocDo);
                    lbTamR[j].BackColor = Color.Red;
                    lbTamR[j].ForeColor = Color.White;
                    pt[k] = ptTamR[j];
                    lb[k] = lbTamR[j];
                    j++;
                }
                k++;
            }
            //Copy các phần tử còn lại của mảng L vào a nếu có
            while (i < n1)
            {
                sapXep1.diChuyen(ref lbTamL[i], ref ptTamL[i], ptTam[k].X, ptTam[k].Y, ref tocDo);
                lbTamL[i].BackColor = Color.Red;
                lbTamL[i].ForeColor = Color.White;
                pt[k] = ptTamL[i];
                lb[k] = lbTamL[i];
                i++;
                k++;
            }
            //Copy các phần tử còn lại của mảng R vào a nếu có
            while (j < n2)
            {
                sapXep1.diChuyen(ref lbTamR[j], ref ptTamR[j], ptTam[k].X, ptTam[k].Y, ref tocDo);
                lbTamR[j].BackColor = Color.Red;
                lbTamR[j].ForeColor = Color.White;
                pt[k] = ptTamR[j];
                lb[k] = lbTamR[j];
                j++;
                k++;
            }
            Thread.Sleep(tocDo * 120);
            for (int i1 = l; i1 <= r; i1++)
            {
                a[i1].BackColor = Color.Yellow;
                a[i1].ForeColor = Color.Black;
            }
        }

        //l là chỉ số trái và r là chỉ số phải của mảng cần được sắp xếp
        public void sapXepTron(Label[] a, Point[] b, int l, int r)
        {
            if (l < r)
            {
                //Tương tự (l + r) / 2, nhưng cách này tránh tràn số khi l và r lớn
                int m = l + (r - l) / 2;

                //Gọi hàm đệ quy tiếp tục chia đôi từng nửa mảng
                sapXepTron(a, b, l, m);
                sapXepTron(a, b, m + 1, r);

                merge(a, b, l, m, r);
            }
        }

        //đưa ra thông báo khi nhấn nút đóng form
        private void formTrangChinh_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát chương trình?", "Chú ý!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (luong1 != null)
                {
                    if ((luong1.ThreadState & ThreadState.SuspendRequested) == ThreadState.SuspendRequested)
                    {
                        luong1.Resume();
                        luong1.Abort();
                    }
                    else if ((luong1.ThreadState & ThreadState.Suspended) == ThreadState.Suspended)
                    {
                        luong1.Resume();
                        luong1.Abort();
                    }
                    else
                    {
                        luong1.Abort();
                    }
                }
                e.Cancel = false;
            }
            else
                e.Cancel = true;
        }
    }
}
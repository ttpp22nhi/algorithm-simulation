using System;
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
    //tạo lớp sapXep kế thừa từ lớp formTrangChinh
    class sapXep:formTrangChinh
    {
        //khởi tạo giá trị cho một nhãn a có tọa độ b nhận g trị ngẫu nhiên rd và vị trí thứ i
        public void khoiTaoNhan(ref Label a, ref Point b, ref Random rd, int i)
        {
            a = new Label(); //khởi tạo một đối tượng nhãn
            a.Text = rd.Next(0, 100).ToString(); //gán nội dung cho nhãn
            a.Name = "lb" + i.ToString(); //đặt tên cho nhãn
            b = new Point(); //khởi tạo một điểm
            b.X = i * 60; //gán hoành độ cho điểm
            b.Y = 60; //gán tung độ cho điểm
            a.Location = b; //gán vị trí cho nhãn bằng điểm vừa tạo ra
            a.Size = new System.Drawing.Size(35, 20); //đặt kích thước cho nhãn
            a.BorderStyle = BorderStyle.FixedSingle; //tạo đường viền
            a.BackColor = Color.Yellow; //màu nền của nhãn
            //a.ForeColor = Color.White; //màu nội dung của nhãn
        }

        //ghi đè phương thức trên, sử dụng cho thêm bằng tay, nhận trực tiếp giá trị Text là "giaTri"
        public void khoiTaoNhan(ref Label a, ref Point b, string giaTri, int i)
        {
            a = new Label(); //khởi tạo một đối tượng nhãn
            a.Text = giaTri; //gán nội dung cho nhãn
            a.Name = "lb" + i.ToString(); //đặt tên cho nhãn
            b = new Point(); //khởi tạo một điểm
            b.X = i * 60; //gán tọa độ trục hoành cho điểm
            b.Y = 60; //gán tọa độ trục tung cho điểm
            a.Location = b; //gán vị trí cho nhãn bằng điểm vừa tạo ra
            a.Size = new System.Drawing.Size(35, 20); //đặt kích thước cho nhãn
            a.BorderStyle = BorderStyle.FixedSingle; //tạo đường viền
            a.BackColor = Color.Yellow; //màu nền của nhãn
            //a.ForeColor = Color.White; //màu nội dung của nhãn
        }

        //đảo vị trí 2 điểm (Point) trong mảng điểm (Point)
        public void daoDiem(ref Point a, ref Point b)
        {
            Point tam = new Point();
            tam = a;
            a = b;
            b = tam;
        }

        //đảo vị trí 2 nhãn (Label) trong mảng nhãn (Label)
        public void daoNhan(ref Label a, ref Label b)
        {
            Label temp = new Label();
            temp = a;
            a = b;
            b = temp;
        }

        //mô phỏng đảo nhãn (di chuyển 2 nhãn a và b đến tọa độ của nhau)
        public void daoNhanMoPhong(ref Label a, ref Label b, ref Point c, ref Point d, ref int tocDo)
        {
            int x1 = a.Location.X;
            int x2 = b.Location.X;
            int y = a.Location.Y;
            a.BackColor = Color.Red;
            b.BackColor = Color.Red;
            a.ForeColor = Color.White;
            b.ForeColor = Color.White;
            if (a.Location.X < b.Location.X)
            {
                while (a.Location.Y > y - 40)
                {
                    Thread.Sleep(tocDo);
                    c.Y--;
                    d.Y--;
                    a.Location = c;
                    b.Location = d;
                }
                while (a.Location.X < x2 && b.Location.X > x1)
                {
                    Thread.Sleep(tocDo);
                    c.X++;
                    d.X--;
                    a.Location = c;
                    b.Location = d;
                }
                while (a.Location.Y < y)
                {
                    Thread.Sleep(tocDo);
                    c.Y++;
                    d.Y++;
                    a.Location = c;
                    b.Location = d;
                }
            }
            else
            {
                while (a.Location.Y > y - 40)
                {
                    Thread.Sleep(tocDo);
                    c.Y--;
                    d.Y--;
                    a.Location = c;
                    b.Location = d;
                }
                while (a.Location.X > x2 && b.Location.X < x1)
                {
                    Thread.Sleep(tocDo);
                    c.X--;
                    d.X++;
                    a.Location = c;
                    b.Location = d;
                }
                while (a.Location.Y < y)
                {
                    Thread.Sleep(tocDo);
                    c.Y++;
                    d.Y++;
                    a.Location = c;
                    b.Location = d;
                }
            }
            a.BackColor = Color.Yellow;
            b.BackColor = Color.Yellow;
            a.ForeColor = Color.Black;
            b.ForeColor = Color.Black;
        }

        //di chuyển nhãn a tọa độ b đến điểm (diemX, diemY) với tốc độ = tocDo
        public void diChuyen(ref Label a, ref Point b, int diemX, int diemY, ref int tocDo){
            //lên
            a.BackColor = Color.Red; //đổi màu nền nhãn thành đỏ
            a.ForeColor = Color.White;
            if (b.X == diemX && b.Y > diemY){
                
                while (a.Location.Y > diemY){
                    Thread.Sleep(tocDo);
                    b.Y--;
                    a.Location = b;
                }
            }
            //xuống
            else if (b.X == diemX && b.Y < diemY){
                while (a.Location.Y < diemY){
                    Thread.Sleep(tocDo);
                    b.Y++;
                    a.Location = b;
                }
            }
            //trái
            else if (b.Y == diemY && b.X > diemX){
                while (a.Location.X > diemX){
                    Thread.Sleep(tocDo);
                    b.X--;
                    a.Location = b;
                }
            }
            //phải
            else if (b.Y == diemY && b.X < diemX){
                while (a.Location.X < diemX){
                    Thread.Sleep(tocDo);
                    b.X++;
                    a.Location = b;
                }
            }
            //lên phải
            else if (b.X < diemX && b.Y > diemY)
            {
                while (a.Location.Y > diemY)
                {
                    Thread.Sleep(tocDo);
                    b.Y--;
                    a.Location = b;
                }
                while (a.Location.X < diemX)
                {
                    Thread.Sleep(tocDo);
                    b.X++;
                    a.Location = b;
                }
            }
            //lên trái
            else if (b.X > diemX && b.Y > diemY)
            {
                while (a.Location.Y > diemY)
                {
                    Thread.Sleep(tocDo);
                    b.Y--;
                    a.Location = b;
                }
                while (a.Location.X > diemX)
                {
                    Thread.Sleep(tocDo);
                    b.X--;
                    a.Location = b;
                }
            }
            //xuống phải
            else if (b.X < diemX && b.Y < diemY)
            {
                while (a.Location.Y < diemY)
                {
                    Thread.Sleep(tocDo);
                    b.Y++;
                    a.Location = b;
                }
                while (a.Location.X < diemX)
                {
                    Thread.Sleep(tocDo);
                    b.X++;
                    a.Location = b;
                }
            }
            //xuống trái
            else if (b.X > diemX && b.Y < diemY)
            {
                while (a.Location.Y < diemY)
                {
                    Thread.Sleep(tocDo);
                    b.Y++;
                    a.Location = b;
                }
                while (a.Location.X > diemX)
                {
                    Thread.Sleep(tocDo);
                    b.X--;
                    a.Location = b;
                }
            }
            a.BackColor = Color.Yellow;
            a.ForeColor = Color.Black;
        }

        //mô phỏng đảo nhãn sử dụng cho sắp xếp vun đống
        public void daoNhanHeap(ref Label a, ref Label b, ref Point c, ref Point d, ref int tocDo)
        {
            Point x1 = a.Location;
            Point x2 = b.Location;

            a.BackColor = Color.Red;
            b.BackColor = Color.Red;
            a.ForeColor = Color.White;
            b.ForeColor = Color.White;
            if (a.Location.X < b.Location.X && a.Location.Y < b.Location.Y)
            {
                //a xuống b lên
                while (a.Location.Y < x2.Y && b.Location.Y > x1.Y)
                {
                    Thread.Sleep(tocDo);
                    c.Y++;
                    d.Y--;
                    a.Location = c;
                    b.Location = d;
                }
                //a phải b trái
                while (a.Location.X < x2.X && b.Location.X > x1.X)
                {
                    Thread.Sleep(tocDo);
                    c.X++;
                    d.X--;
                    a.Location = c;
                    b.Location = d;
                }
            }
            else if(a.Location.X < b.Location.X && a.Location.Y > b.Location.Y)
            {
                //a lên b xuống
                while (a.Location.Y > x2.Y && b.Location.Y < x1.Y)
                {
                    Thread.Sleep(tocDo);
                    c.Y--;
                    d.Y++;
                    a.Location = c;
                    b.Location = d;
                }
                //a phải b trái
                while (a.Location.X < x2.X && b.Location.X > x1.X)
                {
                    Thread.Sleep(tocDo);
                    c.X++;
                    d.X--;
                    a.Location = c;
                    b.Location = d;
                }
            }
            else if (a.Location.X > b.Location.X && a.Location.Y < b.Location.Y)
            {
                //a xuống b lên
                while (a.Location.Y < x2.Y && b.Location.Y > x1.Y)
                {
                    Thread.Sleep(tocDo);
                    c.Y++;
                    d.Y--;
                    a.Location = c;
                    b.Location = d;
                }
                //a trái b phải
                while (a.Location.X > x2.X && b.Location.X < x1.X)
                {
                    Thread.Sleep(tocDo);
                    c.X--;
                    d.X++;
                    a.Location = c;
                    b.Location = d;
                }
            }
            else if (a.Location.X > b.Location.X && a.Location.Y > b.Location.Y)
            {
                //a lên b xuống
                while (a.Location.Y > x2.Y && b.Location.Y < x1.Y)
                {
                    Thread.Sleep(tocDo);
                    c.Y--;
                    d.Y++;
                    a.Location = c;
                    b.Location = d;
                }
                //a trái b phải
                while (a.Location.X > x2.X && b.Location.X < x1.X)
                {
                    Thread.Sleep(tocDo);
                    c.X--;
                    d.X++;
                    a.Location = c;
                    b.Location = d;
                }
            }
            a.BackColor = Color.Yellow;
            b.BackColor = Color.Yellow;
            a.ForeColor = Color.Black;
            b.ForeColor = Color.Black;
        }
    }
}

/*formTrangChinh formTam;
        public sapXep(formTrangChinh a)
        {
            this.formTam = a;
        }*/
//tach so tu chuoi
//else if (rdBangTay.Checked == true)
//            {
//                //tách số từ chuỗi bất kì
//                string input = txtDanhSach.Text;
//// Split on one or more non-digit characters.
//string[] numbers = Regex.Split(input, @"\D+");
//int j = 0;
//mang = new int[numbers.Length];
//                foreach (string value in numbers)
//                {
//                    if (!string.IsNullOrEmpty(value))
//                    {
//                        int i = int.Parse(value);
//mang[j] = i;
//                        j++;
//                    }
//                }
//                if (mang.Length == 2)
//                {
//                    MessageBox.Show("Bạn đã nhập sai dữ liệu!", "Chú ý!");
//                    return;
//                }

//                n = numbers.Length;
//                lb = new Label[n];
//                pt = new Point[n];
//                sapXep1 = new sapXep(this);
//                for (int i = 0; i<n; i++)
//                {
//                    sapXep1.khoiTaoNhan(ref lb[i], ref pt[i], mang[i], i);
//                    this.Invoke((MethodInvoker)delegate
//                    {
//                        this.groupBox3.Controls.Add(lb[i]);
//});

//                }
//            }
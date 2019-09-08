using System;
using System.Data;
using System.Data.SqlClient;                  //veri tabanı bağlantısı için gerekli kütüphane
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Görev_Yöneticisi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=PC\\SQLEXPRESS;Initial Catalog=ProgramDatabase;Integrated Security=True");            //veritabanının path ini gösterir
        SqlDataAdapter da;
        DataTable dt;
        
        public string dosyaYolu;
        public string dosyaAdi;
        public string ProgramAdi;
        public string ProgramPath;
        public string Durum;
        OpenFileDialog programlar;

        private void programCalisma()
        {                             
            
        }
        DataTable tabloDoldur()
        {
            baglanti.Open();                                                     //datagridview i tablo ile doldurduk
            da = new SqlDataAdapter("SELECT * FROM Programlar ", baglanti);      //adapter oluşturuldu , adapter için sorgu yazıldı
            dt = new DataTable();                                                //datatable oluşturuldu
            da.Fill(dt);                                                         //sqlAdapter doldurulur
            baglanti.Close();
            return dt;
        }                
        public void durumCalisiyor(string dosyaAdi)
        {
            Durum = dataGridView1.CurrentRow.Cells["Durum"].Value.ToString();
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Programlar SET Durum = 'Çalışıyor' WHERE ProgramPath like '%"+ dosyaAdi + "%'", baglanti);              //çalışan her uygulama için duru güncellenir
            cmd.ExecuteNonQuery();                                                                                                                         //komut çalıştırılır
            baglanti.Close();
            dataGridView1.DataSource = tabloDoldur();
            dataGridView1.Refresh();                                                                                              //datagridview güncellenir
        }
        public void durumDevreDisi(string dosyaAdi)
        {
            Durum = dataGridView1.CurrentRow.Cells["Durum"].Value.ToString();
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Programlar SET Durum = 'Devre Dışı' WHERE ProgramPath like '%" + dosyaAdi + "%'", baglanti);
            cmd.ExecuteNonQuery();
            baglanti.Close();
            dataGridView1.DataSource = tabloDoldur();                                     //datagridview in güncellenmesi için tekrar bu metodu çağırdık
            dataGridView1.Refresh();
        }
        private void calismaRengi(object sender)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.RosyBrown;
            if (btn.BackColor == Color.RosyBrown)
                btn.BackColor = Color.Yellow;
        }
        private void devreDisiRengi(object sender)
        {
            Button btn = (Button)sender;
            if (btn.BackColor == Color.Yellow)
                btn.BackColor = Color.Green;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = tabloDoldur();                                    //form yüklendiği anda veriler dataGridView de görüntülenir
        }

        private void button1_Click(object sender, EventArgs e)
        {
            calismaRengi(sender);

            OpenFileDialog programlar = new OpenFileDialog();                   //openfiledialog tanımlaması 
            programlar.InitialDirectory = "c:\\";
            programlar.RestoreDirectory = true;                                 //açık dosya iletişim kutusunun kapatmadan önce geçerli dizini geri yükleyeceği anlamına gelir
            programlar.Filter = "Exe Files (.exe)|*.exe|All Files (*.*)|*.*";
            programlar.CheckFileExists = false;                                 //Dosya adının elle girilmesi durumunda dosyanın var olup olmadığı kontrol edilir
            programlar.Title = "Çalıştırmak üzere exe dosyasını seçiniz.";

            if (programlar.ShowDialog() == DialogResult.OK)                     //iletişim kutusunda bir dosya seçildi mi sorgusu     
            {                                                                   //dosya seçili ise
                dosyaYolu = programlar.FileName;                                // seçilen dosyanın tüm yolunu verir
                button1.Text = programlar.SafeFileName;                         // seçilen dosyanın adını verir.

                Thread t1 = new Thread(programCalisma);
                t1.Start();

                Process proc = Process.Start(dosyaYolu);                        //programı başlat                  

                durumCalisiyor(programlar.SafeFileName);

                while (proc.MainWindowHandle == IntPtr.Zero)                    //yüklendi mi bekleyelim
                    Thread.Sleep(5000);
                proc.Refresh();
                proc.CloseMainWindow();                                         //program arayüzü kapanır
                proc.Close();                                                   //program kapanır

                durumDevreDisi(programlar.SafeFileName);                                               //durum güncellenir                             
            }
            devreDisiRengi(sender);           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            calismaRengi(sender);
            
            OpenFileDialog programlar = new OpenFileDialog();
            programlar.InitialDirectory = "c:\\";
            programlar.RestoreDirectory = true;
            programlar.Filter = "Exe Files (.exe)|*.exe|All Files (*.*)|*.*";
            programlar.CheckFileExists = false;
            programlar.Title = "Çalıştırmak üzere exe dosyasını seçiniz.";

            if (programlar.ShowDialog() == DialogResult.OK)
            {
                dosyaYolu = programlar.FileName;
                button2.Text = programlar.SafeFileName;

                Thread t2 = new Thread(programCalisma);
                t2.Start();

                Process proc2 = Process.Start(dosyaYolu);

                durumCalisiyor(programlar.SafeFileName);

                while (proc2.MainWindowHandle == IntPtr.Zero)
                    Thread.Sleep(6000);
                proc2.Refresh();
                proc2.CloseMainWindow();
                proc2.Close();

                durumDevreDisi(programlar.SafeFileName);
            }
            devreDisiRengi(sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            calismaRengi(sender);

            OpenFileDialog programlar = new OpenFileDialog();
            programlar.InitialDirectory = "c:\\";
            programlar.RestoreDirectory = true;
            programlar.Filter = "Exe Files (.exe)|*.exe|All Files (*.*)|*.*";
            programlar.CheckFileExists = false;
            programlar.Title = "Çalıştırmak üzere exe dosyasını seçiniz.";

            if (programlar.ShowDialog() == DialogResult.OK)
            {
                dosyaYolu = programlar.FileName;
                button3.Text = programlar.SafeFileName;

                Thread t3 = new Thread(programCalisma);
                t3.Start();

                Process proc3 = Process.Start(dosyaYolu);

                durumCalisiyor(programlar.SafeFileName);

                while (proc3.MainWindowHandle == IntPtr.Zero)                    //yüklendi mi bekleyelim
                    Thread.Sleep(7000);
                proc3.Refresh();
                proc3.CloseMainWindow();
                proc3.Close();

                durumDevreDisi(programlar.SafeFileName);
            }
            devreDisiRengi(sender);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            calismaRengi(sender);

            OpenFileDialog programlar = new OpenFileDialog();
            programlar.InitialDirectory = "c:\\";
            programlar.RestoreDirectory = true;
            programlar.Filter = "Exe Files (.exe)|*.exe|All Files (*.*)|*.*";
            programlar.CheckFileExists = false;
            programlar.Title = "Çalıştırmak üzere exe dosyasını seçiniz.";

            if (programlar.ShowDialog() == DialogResult.OK)
            {
                dosyaYolu = programlar.FileName;
                button4.Text = programlar.SafeFileName;

                Thread t4 = new Thread(programCalisma);
                t4.Start();

                Process proc4 = Process.Start(dosyaYolu);

                durumCalisiyor(programlar.SafeFileName);

                while (proc4.MainWindowHandle == IntPtr.Zero)                    //yüklendi mi bekleyelim
                    Thread.Sleep(8000);
                proc4.Refresh();
                proc4.CloseMainWindow();
                proc4.Close();

                durumDevreDisi(programlar.SafeFileName);
            }
            devreDisiRengi(sender);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            calismaRengi(sender);

            OpenFileDialog programlar = new OpenFileDialog();
            programlar.InitialDirectory = "c:\\";
            programlar.RestoreDirectory = true;
            programlar.Filter = "Exe Files (.exe)|*.exe|All Files (*.*)|*.*";
            programlar.CheckFileExists = false;
            programlar.Title = "Çalıştırmak üzere exe dosyasını seçiniz.";

            if (programlar.ShowDialog() == DialogResult.OK)
            {
                dosyaYolu = programlar.FileName;
                button5.Text = programlar.SafeFileName;

                Thread t5 = new Thread(programCalisma);
                t5.Start();

                Process proc5 = Process.Start(dosyaYolu);

                durumCalisiyor(programlar.SafeFileName);

                while (proc5.MainWindowHandle == IntPtr.Zero)                    //yüklendi mi bekleyelim
                    Thread.Sleep(9000);
                proc5.Refresh();
                proc5.CloseMainWindow();
                proc5.Close();

                durumDevreDisi(programlar.SafeFileName);
            }
            devreDisiRengi(sender);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            calismaRengi(sender);

            OpenFileDialog programlar = new OpenFileDialog(); 
            programlar.InitialDirectory = "c:\\";
            programlar.RestoreDirectory = true;
            programlar.Filter = "Exe Files (.exe)|*.exe|All Files (*.*)|*.*";
            programlar.CheckFileExists = false;                                 
            programlar.Title = "Çalıştırmak üzere exe dosyasını seçiniz.";

            if (programlar.ShowDialog() == DialogResult.OK)                          
            {                                                                   
                dosyaYolu = programlar.FileName;
                button6.Text = programlar.SafeFileName;

                Thread t6 = new Thread(programCalisma);
                t6.Start();

                Process proc6 = Process.Start(dosyaYolu);

                durumCalisiyor(programlar.SafeFileName);

                while (proc6.MainWindowHandle == IntPtr.Zero)                    //yüklendi mi bekleyelim
                    Thread.Sleep(10000);
                proc6.Refresh();
                proc6.CloseMainWindow();
                proc6.Close();

                durumDevreDisi(programlar.SafeFileName);
            }
            devreDisiRengi(sender);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            calismaRengi(sender);

            OpenFileDialog programlar = new OpenFileDialog();
            programlar.InitialDirectory = "c:\\";
            programlar.RestoreDirectory = true;
            programlar.Filter = "Exe Files (.exe)|*.exe|All Files (*.*)|*.*";
            programlar.CheckFileExists = false;
            programlar.Title = "Çalıştırmak üzere exe dosyasını seçiniz.";

            if (programlar.ShowDialog() == DialogResult.OK)
            {
                dosyaYolu = programlar.FileName;
                button7.Text = programlar.SafeFileName;

                Thread t7 = new Thread(programCalisma);
                t7.Start();

                Process proc7 = Process.Start(dosyaYolu);
                durumCalisiyor(programlar.SafeFileName);

                while (proc7.MainWindowHandle == IntPtr.Zero)                    //yüklendi mi bekleyelim
                    Thread.Sleep(11000);
                proc7.Refresh();
                proc7.CloseMainWindow();
                proc7.Close();

                durumDevreDisi(programlar.SafeFileName);
            }
            devreDisiRengi(sender);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            calismaRengi(sender);

            OpenFileDialog programlar = new OpenFileDialog();
            programlar.InitialDirectory = "c:\\";
            programlar.RestoreDirectory = true;
            programlar.Filter = "Exe Files (.exe)|*.exe|All Files (*.*)|*.*";
            programlar.CheckFileExists = false;
            programlar.Title = "Çalıştırmak üzere exe dosyasını seçiniz.";

            if (programlar.ShowDialog() == DialogResult.OK)
            {
                dosyaYolu = programlar.FileName;
                button8.Text = programlar.SafeFileName;

                Thread t8 = new Thread(programCalisma);
                t8.Start();

                Process proc8 = Process.Start(dosyaYolu);

                durumCalisiyor(programlar.SafeFileName);

                while (proc8.MainWindowHandle == IntPtr.Zero)                    //yüklendi mi bekleyelim
                    Thread.Sleep(12000);
                proc8.Refresh();
                proc8.CloseMainWindow();
                proc8.Close();

                durumDevreDisi(programlar.SafeFileName);
            }
            devreDisiRengi(sender);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            calismaRengi(sender);

            OpenFileDialog programlar = new OpenFileDialog(); 
            programlar.InitialDirectory = "c:\\";
            programlar.RestoreDirectory = true; 
            programlar.Filter = "Exe Files (.exe)|*.exe|All Files (*.*)|*.*";
            programlar.CheckFileExists = false;                                 
            programlar.Title = "Çalıştırmak üzere exe dosyasını seçiniz.";

            if (programlar.ShowDialog() == DialogResult.OK)                          
            {                                                                   
                dosyaYolu = programlar.FileName;                                
                button9.Text = programlar.SafeFileName;

                Thread t9 = new Thread(programCalisma);
                t9.Start();

                Process proc9 = Process.Start(dosyaYolu);

                durumCalisiyor(programlar.SafeFileName);

                while (proc9.MainWindowHandle == IntPtr.Zero)                    //yüklendi mi bekleyelim
                    Thread.Sleep(14000);
                proc9.Refresh();
                proc9.CloseMainWindow();
                proc9.Close();

                durumDevreDisi(programlar.SafeFileName);
            }
            devreDisiRengi(sender);
        }
        
        private void button10_Click(object sender, EventArgs e)
        {    //çalışmıyor
            foreach (var process in Process.GetProcessesByName(comboBox1.Text))
            {
                if (button1.BackColor == Color.Yellow || button1.BackColor == Color.Green)         //renkleri eski haline döner
                    button1.BackColor = Color.RosyBrown;
                else if (button2.BackColor == Color.Yellow)
                    button2.BackColor = Color.RosyBrown;
                else if (button3.BackColor == Color.Yellow)
                    button3.BackColor = Color.RosyBrown;
                else if (button4.BackColor == Color.Yellow)
                    button4.BackColor = Color.RosyBrown;
                else if (button5.BackColor == Color.Yellow)
                    button5.BackColor = Color.RosyBrown;
                else if (button6.BackColor == Color.Yellow)
                    button6.BackColor = Color.RosyBrown;
                else if (button7.BackColor == Color.Yellow)
                    button7.BackColor = Color.RosyBrown;
                else if (button8.BackColor == Color.Yellow)
                    button8.BackColor = Color.RosyBrown;
                else if (button9.BackColor == Color.Yellow)
                    button9.BackColor = Color.RosyBrown;

                process.Kill();                        //process kapatılır

            }
        }
    }
}

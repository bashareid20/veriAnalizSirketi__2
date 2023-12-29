using System;
using Npgsql;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace veriAnalizSirketi__2
{
    public partial class ketegori : Form
    {
        public ketegori()
        {
            InitializeComponent();
        
        }
        NpgsqlConnection baglanti1 = new NpgsqlConnection("server=localHost;port=5432;" +
            "Database=DBVeriAnalizSirketi;user ID=postgres;password=12345");
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

     

        private void BtnListele_Click(object sender, EventArgs e)
        {
            string sorgu = "SELECT *from \"kategoriler\" order by \"kategoriID\" ";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti1);
            DataSet ds = new DataSet();
              da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
          
            baglanti1.Open();

            NpgsqlCommand komut1 = new NpgsqlCommand("insert into \"kategoriler\" (\"kategoriID\",\"kategoriad\") values (@p1,@p2)", baglanti1);
            komut1.Parameters.AddWithValue("@p1", int.Parse(TextKategoriID.Text));
            komut1.Parameters.AddWithValue("@p2", TextKategoriAd.Text);
            komut1.ExecuteNonQuery();
            baglanti1.Close();
            MessageBox.Show("Ekleme İşlemi Başarılı");
            BtnListele_Click( sender,  e);
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            baglanti1.Open();
            DialogResult cevap = MessageBox.Show("Silmek istediğinizden emin misiniz?", "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (cevap == DialogResult.Yes)
            {
                // TextKategoriID'nin boş olup olmadığını kontrol et
                if (!string.IsNullOrWhiteSpace(TextKategoriID.Text))
                {
                    int kategoriID;
                    // TextKategoriID'nin geçerli bir tamsayı olup olmadığını kontrol et
                    if (int.TryParse(TextKategoriID.Text, out kategoriID))
                    {
                        NpgsqlCommand komut3 = new NpgsqlCommand("DELETE FROM kategoriler WHERE \"kategoriID\"=@p1", baglanti1);
                        komut3.Parameters.AddWithValue("@p1", kategoriID);
                        komut3.ExecuteNonQuery();
                        MessageBox.Show("Ürün silme işlemi başarılı bir şekilde gerçekleşti", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        BtnListele_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Lütfen geçerli bir Kategori ID giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Kategori ID boş bırakılamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Silme işlemi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            baglanti1.Close();

        }

        private void BtnAra_Click(object sender, EventArgs e)
        {
            // Bağlantınızı açın
            baglanti1.Open();

            // Arama terimini TextBox'tan alın
            string searchTerm = TextKategoriAd.Text;

            // DataTable nesnesi oluşturun. Bu, verileri geçici olarak saklayacak
            DataTable dt = new DataTable();

            // SQL sorgunuzu kullanarak veritabanını sorgulayın
            using (var cmd = new NpgsqlCommand("SELECT * FROM \"kategoriler\" WHERE \"kategoriad\" LIKE @p", baglanti1))
            {
                cmd.Parameters.AddWithValue("@p", "%" + searchTerm + "%");

                // NpgsqlDataAdapter kullanarak verileri DataTable'a doldurun
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);

                    // DataGridView'in DataSource'unu ayarlayarak verileri gösterin
                    dataGridView1.DataSource = dt;
                }
            }

            // Bağlantıyı kapatın
            baglanti1.Close();
        }

        private void BtnUrun_Click(object sender, EventArgs e)
        {

            FormUrun form = new FormUrun();
            this.Hide();
            form.Show();

        }

        private void BtnKullanci_Click(object sender, EventArgs e)
        {
           kullancilar form = new kullancilar();
            this.Hide();
            form.Show();
        }

        private void BtnSatis_Click(object sender, EventArgs e)
        {satislar form = new satislar();
            this.Hide();
            form.Show();
        }
    }
}

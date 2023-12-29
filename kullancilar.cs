using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace veriAnalizSirketi__2
{
    public partial class kullancilar : Form
    {
        public kullancilar()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost;port=5432;" +
        "Database=DBVeriAnalizSirketi;user ID=postgres;password=12345");
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void BtnListele_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut5 = new NpgsqlCommand("select * from \"KullanciListesi\" order by \"Kullancı ID\"", baglanti);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut5);
            DataSet dt = new DataSet();
            da.Fill(dt);
            dataGridView1.DataSource = dt.Tables[0];
            baglanti.Close();
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
          
            baglanti.Open();

            // Boş alan kontrolü
            if (string.IsNullOrEmpty(TextId.Text) ||
                string.IsNullOrEmpty(TextAd.Text) ||
                string.IsNullOrEmpty(TextSoyad.Text) ||
                string.IsNullOrEmpty(TextRolId.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglanti.Close(); 
                return;
            }

         
            NpgsqlCommand komut = new NpgsqlCommand("insert into \"Kullancilar\"(\"KullanciID\",\"adi\",\"soyAd\",\"rolid\") values (@p1,@p2,@p3,@p4)", baglanti);
            komut.Parameters.AddWithValue("@p1", int.Parse(TextId.Text));
            komut.Parameters.AddWithValue("@p2", TextAd.Text);
            komut.Parameters.AddWithValue("@p3", TextSoyad.Text);
            komut.Parameters.AddWithValue("@p4", int.Parse(TextRolId.Text));

            komut.ExecuteNonQuery();

        
            baglanti.Close();

          
            MessageBox.Show("Kullanıcı kaydı başarılı bir şekilde gerçekleşti", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

          
            BtnListele_Click(sender, e);

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut5 = new NpgsqlCommand("select * from \"roller\" order by \"rolid\"", baglanti);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut5);
            DataSet dt = new DataSet();
            da.Fill(dt);
            dataGridView1.DataSource = dt.Tables[0];
            baglanti.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
       


        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
           
            
                if (string.IsNullOrEmpty(TextId.Text) ||
                    string.IsNullOrEmpty(TextAd.Text) ||
                    string.IsNullOrEmpty(TextSoyad.Text) ||
                    string.IsNullOrEmpty(TextRolId.Text))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                  
                    baglanti.Open();

                    NpgsqlCommand komut = new NpgsqlCommand("UPDATE \"Kullancilar\" SET adi=@p2, \"soyAd\"=@p3, rolid=@p4 WHERE \"KullanciID\"=@p1", baglanti);
                    komut.Parameters.AddWithValue("@p1", int.Parse(TextId.Text));
                    komut.Parameters.AddWithValue("@p2", TextAd.Text);
                    komut.Parameters.AddWithValue("@p3", TextSoyad.Text);
                    komut.Parameters.AddWithValue("@p4", int.Parse(TextRolId.Text));

             
                    int result = komut.ExecuteNonQuery();

                 
                    if (result > 0)
                    {
                        MessageBox.Show("Güncelleme başarılı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                    }
                    else
                    {
                        MessageBox.Show("Güncelleme yapılamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                 
                    MessageBox.Show("Hata: " + ex.Message);
                }
                finally
                {
               
                    baglanti.Close();
                }
            

        }

        private void BtnGitUrun_Click(object sender, EventArgs e)
        {
            FormUrun form = new FormUrun();
            this.Hide();
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ketegori form = new ketegori();
            this.Hide();
            form.Show();
        }

        private void kullancilar_Load(object sender, EventArgs e)
        {
          
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            DialogResult cevap = MessageBox.Show("Silmek istediğinizden emin misiniz?", "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (cevap == DialogResult.Yes)
            {
                // TextKategoriID'nin boş olup olmadığını kontrol et
                if (!string.IsNullOrWhiteSpace(TextId.Text))
                {
                    int kategoriID;
                    // TextKategoriID'nin geçerli bir tamsayı olup olmadığını kontrol et
                    if (int.TryParse(TextId.Text, out kategoriID))
                    {
                        NpgsqlCommand komut3 = new NpgsqlCommand("DELETE FROM \"Kullancilar\"  WHERE \"KullanciID\"=@p1", baglanti);
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

            baglanti.Close();

        }

        private void BtnAra_Click(object sender, EventArgs e)
        {

            baglanti.Open();


            string searchTerm = TextAd.Text;


            DataTable dt = new DataTable();


            using (var cmd = new NpgsqlCommand("SELECT * FROM \"Kullancilar\"  WHERE \"adi\" LIKE @p", baglanti))
            {
                cmd.Parameters.AddWithValue("@p", "%" + searchTerm + "%");


                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);


                    dataGridView1.DataSource = dt;
                }
            }


            baglanti.Close();
        }
    }
    }


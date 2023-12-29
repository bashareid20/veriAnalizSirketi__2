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
using System.Globalization;

namespace veriAnalizSirketi__2
{
    public partial class FormUrun : Form
    {
        public FormUrun()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost;port=5432;Database=DBVeriAnalizSirketi;user ID=postgres;password=12345");
        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void FormUrun_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select * from \"kategoriler\" order by \"kategoriID\"", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DisplayMember = "kategoriad";
            comboBox1.ValueMember = "kategoriID";
            comboBox1.DataSource = dt;
            baglanti.Close();
        }

        private void BtnListele_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut5 = new NpgsqlCommand("select * from urunlistesi order by \"UrunID\" ", baglanti);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut5);
            DataSet dt = new DataSet();
            da.Fill(dt);
            dataGridView1.DataSource = dt.Tables[0];
            baglanti.Close();

        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextId.Text) ||
        string.IsNullOrWhiteSpace(TextAd.Text) ||
        string.IsNullOrWhiteSpace(TextGorsel.Text) ||
        string.IsNullOrWhiteSpace(TextMaliyet.Text) ||
        string.IsNullOrWhiteSpace(TextSatisFiyati.Text) ||
        comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                baglanti.Open();
                NpgsqlCommand komut = new NpgsqlCommand("insert into \"Urunler\"(\"UrunID\",\"Urunad\",\"ayniAndaHizmetkapasitesi\",\"gorsel\",\"kategori\",\"maliyet\",\"hizmetBedeli\") values (@p1,@p2,@p3,@p4,@p5,@p6,@p7)", baglanti);
                komut.Parameters.AddWithValue("@p1", int.Parse(TextId.Text));
                komut.Parameters.AddWithValue("@p2", TextAd.Text);
                komut.Parameters.AddWithValue("@p3", Convert.ToInt32(numericUpDown1.Value));
                komut.Parameters.AddWithValue("@p4", TextGorsel.Text);
                komut.Parameters.AddWithValue("@p5", Convert.ToInt32(comboBox1.SelectedValue));
                komut.Parameters.AddWithValue("@p6", decimal.Parse(TextMaliyet.Text));
                komut.Parameters.AddWithValue("@p7", decimal.Parse(TextSatisFiyati.Text));

                komut.ExecuteNonQuery();
                MessageBox.Show("Ürün kaydı başarılı bir şekilde gerçekleşti", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                baglanti.Close();
            }

            BtnListele_Click(sender, e);


        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult cevap = MessageBox.Show("Silmek istediğinizden emin misiniz?", "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (cevap == DialogResult.Yes)
            {
                if (string.IsNullOrWhiteSpace(TextId.Text))
                {
                    MessageBox.Show("Lütfen silmek istediğiniz ürünün ID'sini giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                baglanti.Open();
                NpgsqlCommand komut2 = new NpgsqlCommand("DELETE FROM \"Urunler\" WHERE \"UrunID\"=@p1", baglanti);
                komut2.Parameters.AddWithValue("@p1", int.Parse(TextId.Text));
                komut2.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Ürün silme işlemi başarılı bir şekilde gerçekleşti", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BtnListele_Click(sender, e);
            }
            else
            {
                return;
            }


        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextId.Text) ||
     string.IsNullOrWhiteSpace(TextAd.Text) ||
     string.IsNullOrWhiteSpace(TextGorsel.Text) ||
     string.IsNullOrWhiteSpace(TextMaliyet.Text) ||
     string.IsNullOrWhiteSpace(TextSatisFiyati.Text) ||
     comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    baglanti.Open();
                    NpgsqlCommand komut4 = new NpgsqlCommand("update \"Urunler\" set \"Urunad\"=@p2,\"ayniAndaHizmetkapasitesi\"=@p3,\"gorsel\"=@p4,\"kategori\"=@p5,\"maliyet\"=@p6,\"hizmetBedeli\"=@p7 where \"UrunID\"=@p1", baglanti);
                    komut4.Parameters.AddWithValue("@p1", int.Parse(TextId.Text));
                    komut4.Parameters.AddWithValue("@p2", TextAd.Text);
                    komut4.Parameters.AddWithValue("@p3", Convert.ToInt32(numericUpDown1.Value));
                    komut4.Parameters.AddWithValue("@p4", TextGorsel.Text);
                    komut4.Parameters.AddWithValue("@p5", Convert.ToInt32(comboBox1.SelectedValue));
                    komut4.Parameters.AddWithValue("@p6", Convert.ToDecimal(TextMaliyet.Text));
                    komut4.Parameters.AddWithValue("@p7", Convert.ToDecimal(TextSatisFiyati.Text));
                    komut4.ExecuteNonQuery();
                    MessageBox.Show("Ürün güncelleme başarılı bir şekilde gerçekleşti", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (FormatException fe)
                {
                    MessageBox.Show("Sayısal alanlarda hatalı format: " + fe.Message, "Format Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (NpgsqlException ne)
                {
                    MessageBox.Show("Veritabanı hatası: " + ne.Message, "Veritabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Beklenmeyen bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (baglanti.State == ConnectionState.Open)
                    {
                        baglanti.Close();
                    }
                }

                BtnListele_Click(sender, e);
            }


        }

        private void BtnKategori_Click(object sender, EventArgs e)
        {
            ketegori form = new ketegori();
            this.Hide();
            form.Show();


        }

        private void BtnAra_Click(object sender, EventArgs e)
        {

            
            baglanti.Open();

          
            string searchTerm = TextAd.Text;

           
            DataTable dt = new DataTable();

           
            using (var cmd = new NpgsqlCommand("SELECT * FROM \"Urunler\" WHERE \"Urunad\" LIKE @p", baglanti))
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BtnGitUrun_Click(object sender, EventArgs e)
        {
            kullancilar form = new kullancilar();
            this.Hide();
            form.Show();
        }

        private void BtnGitSatis_Click(object sender, EventArgs e)
        {
            satislar form = new satislar();
            this.Hide();
            form.Show();
        }

        private void FiyatDegisim_Click(object sender, EventArgs e)
        {

            baglanti.Open();
            NpgsqlCommand komut5 = new NpgsqlCommand("select * from \"FiyatListe\" order by \"kayitNo\" ", baglanti);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut5);
            DataSet dt = new DataSet();
            da.Fill(dt);
            dataGridView1.DataSource = dt.Tables[0];
            baglanti.Close();
        }
    } 
}


    


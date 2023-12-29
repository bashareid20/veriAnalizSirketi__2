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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace veriAnalizSirketi__2
{
    public partial class satislar : Form
    {
        public satislar()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost;port=5432;" +
         "Database=DBVeriAnalizSirketi;user ID=postgres;password=12345");

        private void satislar_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select * from \"departmanlar\" order by \"departmanid\"", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DisplayMember = "departmanad";
            comboBox1.ValueMember = "departmanid";
            comboBox1.DataSource = dt;
            baglanti.Close();


        }

        private void BtnToplam_Click(object sender, EventArgs e)
        {

            baglanti.Open();
            NpgsqlCommand komut = new NpgsqlCommand("select toplamSatis(@p1) as \"Toplam Satış\" ;", baglanti);
            komut.Parameters.AddWithValue("@p1", int.Parse(TextId.Text));
          
            komut.ExecuteNonQuery();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut);
            DataSet dt = new DataSet();
            da.Fill(dt);
            dataGridView1.DataSource = dt.Tables[0];
            baglanti.Close();
         


        }

        private void BtnListele_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut5 = new NpgsqlCommand("select * from listelesatislar  ", baglanti);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut5);
            DataSet dt = new DataSet();
            da.Fill(dt);
            dataGridView1.DataSource = dt.Tables[0];
            baglanti.Close();



        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut5 = new NpgsqlCommand("select * from \"islemGunlugu\"  ", baglanti);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut5);
            DataSet dt = new DataSet();
            da.Fill(dt);
            dataGridView1.DataSource = dt.Tables[0];
            baglanti.Close();

        }

        private void BtnListeleC_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut5 = new NpgsqlCommand("select * from \"calisanListele\" order by \"Çalışan ID\" ", baglanti);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut5);
            DataSet dt = new DataSet();
            da.Fill(dt);
            dataGridView1.DataSource = dt.Tables[0];
            baglanti.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(calisanID.Text) || comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    baglanti.Open();
                    NpgsqlCommand komut4 = new NpgsqlCommand("UPDATE calasanlar SET departmanid = @p2 WHERE calisanid = @p1;", baglanti);
                    komut4.Parameters.AddWithValue("@p1", int.Parse(calisanID.Text));
                    komut4.Parameters.AddWithValue("@p2", Convert.ToInt32(comboBox1.SelectedValue));

                    komut4.ExecuteNonQuery();
                    MessageBox.Show("Çalışan güncelleme başarılı bir şekilde gerçekleşti", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                BtnListeleC_Click(sender, e);
            }

        }
    }
}

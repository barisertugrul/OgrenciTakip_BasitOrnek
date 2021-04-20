using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Markup;
using Microsoft.Data.SqlClient;

namespace OgrenciTakip
{
    public partial class Form1 : Form
    {

        private SqlConnection baglanti =
            new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OgrenciDB;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
        }

        private void GridListele(string filter="")
        {
            SqlCommand sorgu;
            if (filter != "")
            {
                sorgu = new SqlCommand("Select * From OgrenciBilgileri Where Adi like @adi",baglanti);
                sorgu.Parameters.AddWithValue("@adi", "%" + filter + "%");
            }
            else
            {
                sorgu = new SqlCommand("Select * From OgrenciBilgileri", baglanti);
            }

            SqlDataAdapter data = new SqlDataAdapter(sorgu);
            DataSet dataSet = new DataSet();
            baglanti.Open();
            data.Fill(dataSet,"OgrenciBilgileri");
            dgvKisiselBilgi.DataSource = dataSet.Tables["OgrenciBilgileri"];
            txtId.Text = "";
            txtAdi.Text = "";
            txtSoyadi.Text = "";
            baglanti.Close();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand eklemeSorgusu = new SqlCommand(
                "insert into OgrenciBilgileri (Adi,Soyadi) values (@adi,@soyadi)",baglanti);
            eklemeSorgusu.Parameters.AddWithValue("@adi", txtAdi.Text);
            eklemeSorgusu.Parameters.AddWithValue("@soyadi", txtSoyadi.Text);
            eklemeSorgusu.ExecuteNonQuery();
            MessageBox.Show("Öğrenci bilgileri veritabanına kaydedildi", "Bilgi", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            baglanti.Close();
            GridListele();
        }

        private void dgvKisiselBilgi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtId.Text = dgvKisiselBilgi.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtAdi.Text = dgvKisiselBilgi.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtSoyadi.Text = dgvKisiselBilgi.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand silmeSorgusu = new SqlCommand("Delete from OgrenciBilgileri where Id=@id", baglanti);
            silmeSorgusu.Parameters.AddWithValue("@id", txtId.Text);
            var yanit = MessageBox.Show("Öğrenci bilgilerini silmek istediğinize emin misiniz?", "Uyarı",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (yanit == DialogResult.Yes)
            {
                silmeSorgusu.ExecuteNonQuery();
                MessageBox.Show("Öğrenci bilgileri veritabanından silindi", "Bilgi", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            
            baglanti.Close();
            GridListele();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand guncellemeSorgusu =
                new SqlCommand("Update OgrenciBilgileri set Adi=@adi, Soyadi=@soyadi where Id=@id", baglanti);
            guncellemeSorgusu.Parameters.AddWithValue("@adi", txtAdi.Text);
            guncellemeSorgusu.Parameters.AddWithValue("@soyadi", txtSoyadi.Text);
            guncellemeSorgusu.Parameters.AddWithValue("@id", txtId.Text);
            baglanti.Close();
            MessageBox.Show("Öğrenci bilgileri güncellendi", "Bilgi", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            GridListele();
        }

        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            GridListele(txtAra.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GridListele();
        }
    }
}

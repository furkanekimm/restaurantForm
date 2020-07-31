using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace restaurant
{
    public partial class frmSiparisKontrol : Form
    {
        public frmSiparisKontrol()
        {
            InitializeComponent();
        }

        private void frmSiparisKontrol_Load(object sender, EventArgs e)
        {
            cAdisyon c = new cAdisyon();
            int butonSayisi = c.paketAdisyonIdbulAdedi();
            c.acikPaketAdisyonlar(lvMusteriler);
            int alt = 50;
            int sol = 1;
            int bol = Convert.ToInt32(Math.Ceiling(Math.Sqrt(butonSayisi)));

            for (int i = 1; i <= butonSayisi; i++)
            {
                Button btn = new Button();

                btn.AutoSize = false;
                btn.Size = new Size(100, 40);
                btn.FlatStyle = FlatStyle.Flat;
                btn.Name= lvMusteriler.Items[i - 1].SubItems[0].Text;
                btn.Text= lvMusteriler.Items[i - 1].SubItems[1].Text;
                btn.Font = new Font(btn.Font.FontFamily.Name, 18);
                btn.Location = new Point(sol, alt);
                this.Controls.Add(btn);

                sol += btn.Width + 5;

                if (i==2)
                {
                    sol = 1;
                    alt += 50;
                }
                btn.Click += new EventHandler(dinamikMetod);
                btn.MouseEnter += new EventHandler(dinamikMetod2);
            }
        }
        
        protected void dinamikMetod(object sender, EventArgs e)
        {
            cAdisyon c = new cAdisyon();
            Button dinamikButon = (sender as Button);
            frmBill frm = new frmBill();
            this.Close();
            cGenel._ServisTurNo = 2;
            cGenel._AdisyonId = Convert.ToString(c.musterininsonadisyonId(Convert.ToInt32(dinamikButon.Name)));
            frm.Show();
        }
        protected void dinamikMetod2(object sender, EventArgs e)
        {
            Button dinamikButon = (sender as Button);
            cAdisyon c = new cAdisyon();
            c.musteriDetaylar(lvMusteriDetaylari, Convert.ToInt32(dinamikButon.Name));
            sonSiparisTarihi();
            lvSatisDetaylari.Items.Clear();
            cSiparis s = new cSiparis();
            cGenel._ServisTurNo = 2;
            cGenel._AdisyonId = Convert.ToString(c.musterininsonadisyonId(Convert.ToInt32(dinamikButon.Name)));
            lblGenelToplam.Text = s.GenelToplamBul(Convert.ToInt32(dinamikButon.Name)).ToString() + "TL";

            
        }
        void sonSiparisTarihi()
        {
            if (lvMusteriDetaylari.Items.Count>0)
            {
                int s = lvMusteriDetaylari.Items.Count;
                lblSonSiparisTarihi.Text = lvMusteriDetaylari.Items[s - 1].SubItems[3].Text;
                txtToplamTutar.Text = s + "Adet";

            }
        }
        void toplam()
        {
            int kayitSayisi = lvSatisDetaylari.Items.Count;
            decimal toplam = 0;
            for (int i = 0; i < kayitSayisi; i++)
            {
                toplam += Convert.ToDecimal(lvSatisDetaylari.Items[i].SubItems[2].Text) * Convert.ToDecimal(lvSatisDetaylari.Items[i].SubItems[3].Text);
            }
            lblToplamSiparis.Text = toplam.ToString() + "TL";
        }

        private void lvMusteriDetaylari_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMusteriDetaylari.SelectedItems.Count>0)
            {
                cSiparis c = new cSiparis();
                c.adisyonpaketsiparisDetaylari(lvSatisDetaylari, Convert.ToInt32(lvMusteriDetaylari.SelectedItems[0].SubItems[4].Text));
                toplam();

                lblGenelToplam.Text = c.GenelToplamBul(Convert.ToInt32(lvMusteriDetaylari.SelectedItems[0].SubItems[0].Text)).ToString() + "TL";

            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Çıkmak istediğinize emin misiniz !!!", "Uyarı !!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void btnGeri_Click(object sender, EventArgs e)
        {
            frmMenu frm = new frmMenu();
            this.Close();
            frm.Show();
        }
    }
}

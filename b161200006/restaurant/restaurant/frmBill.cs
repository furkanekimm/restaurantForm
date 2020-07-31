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
    public partial class frmBill : Form
    {
        public frmBill()
        {
            InitializeComponent();
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


        cSiparis cs = new cSiparis(); int odemeTuru = 0;
        private void frmBill_Load(object sender, EventArgs e)
        {
            gbIndirim.Visible = false;
            if (cGenel._ServisTurNo==1)
            {
                lblAdisyonId.Text = cGenel._AdisyonId;
                txtIndirimTutari.TextChanged += new EventHandler(txtIndirimTutari_TextChanged);
                cs.getByOrder(lvUrunler, Convert.ToInt32(lblAdisyonId.Text));

                if (lvUrunler.Items.Count>0)
                {
                    decimal toplam = 0;
                    for (int i = 0; i < lvUrunler.Items.Count; i++)
                    {
                        toplam += Convert.ToDecimal(lvUrunler.Items[i].SubItems[3].Text);
                    }
                    lbToplamTutar.Text = string.Format("{0:0.000}", toplam);
                    lbOdenecek.Text = string.Format("{0:0.000}", toplam);
                    decimal kdv = Convert.ToDecimal(lbOdenecek.Text) * 18 / 100;
                    lbKdv.Text = string.Format("{0:0.000}", kdv);
                }
                gbIndirim.Visible = true;
                txtIndirimTutari.Clear();
            }
            else if (cGenel._ServisTurNo == 2)
            {
                lblAdisyonId.Text = cGenel._AdisyonId;
                cPaketler pc = new cPaketler();
                odemeTuru = pc.OdemeTurIdGetir(Convert.ToInt32(lblAdisyonId.Text));
                txtIndirimTutari.TextChanged += new EventHandler(txtIndirimTutari_TextChanged);
                cs.getByOrder(lvUrunler, Convert.ToInt32(lblAdisyonId.Text));

                if (odemeTuru == 1)
                {
                    rbNakit.Checked = true;
                }
                else if (odemeTuru == 2)
                {
                    rbKrediKarti.Checked = true;
                }
                else if (odemeTuru == 3)
                {
                    rbTicket.Checked = true;
                }


                if (lvUrunler.Items.Count > 0)
                {
                    decimal toplam = 0;
                    for (int i = 0; i < lvUrunler.Items.Count; i++)
                    {
                        toplam += Convert.ToDecimal(lvUrunler.Items[i].SubItems[3].Text);
                    }
                    lbToplamTutar.Text = string.Format("{0:0.000}", toplam);
                    lbOdenecek.Text = string.Format("{0:0.000}", toplam);
                    decimal kdv = Convert.ToDecimal(lbOdenecek.Text) * 18 / 100;
                    lbKdv.Text = string.Format("{0:0.000}", kdv);
                }
                gbIndirim.Visible = true;
                txtIndirimTutari.Clear();
            }
        }

        private void txtIndirimTutari_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDecimal(txtIndirimTutari.Text)<Convert.ToDecimal(lbToplamTutar.Text))
                {
                    try
                    {
                        lblndirim.Text = string.Format("{0:0.000}", Convert.ToDecimal(txtIndirimTutari.Text));
                    }
                    catch (Exception)
                    {

                        lblndirim.Text = string.Format("{0:0.000}", 0);
                    }  
                }
                else
                {
                    MessageBox.Show("İndirim Tutarı Toplam Tutardan Fazla Olamaz !!!");
                }
            }
            catch (Exception)
            {
                lblndirim.Text = string.Format("{0:0.000}", 0);

            }
        }

        private void chkIndirim_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIndirim.Checked)
            {
                gbIndirim.Visible = true;
                txtIndirimTutari.Clear();
            }
            else
            {
                gbIndirim.Visible = false;
                txtIndirimTutari.Clear();
            }
        }

        private void lblndirim_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(lblndirim.Text)>0)
            {
                decimal odenecek = 0;
                lbOdenecek.Text = lbToplamTutar.Text;
                odenecek = Convert.ToDecimal(lbOdenecek.Text) - Convert.ToDecimal(lblndirim.Text);
                lbOdenecek.Text = string.Format("{0:0.000}", odenecek);
            }
            decimal kdv = Convert.ToDecimal(lbOdenecek.Text)*18/100;
            lbKdv.Text= string.Format("{0:0.000}", kdv);
        }

        cMasalar masalar = new cMasalar();
        cRezervasyon rezerve = new cRezervasyon();
        private void button2_Click(object sender, EventArgs e)
        {
            if (cGenel._ServisTurNo == 1)
            {
                int masaid = masalar.TableGetbyNumber(cGenel._ButtonName);
                int musteriId = 0;

                if (masalar.TableGetbyState(masaid,4)==true)
                {
                    musteriId = rezerve.getByClientIdFromRezervasyon(masaid);
                }
                else
                {
                    musteriId = 1;
                }

                int odemeTurId = 0;

                if (rbNakit.Checked )
                {
                    odemeTurId = 1;
                }

                if (rbKrediKarti.Checked)
                {
                    odemeTurId = 2;
                }
                if (rbTicket.Checked)
                {
                    odemeTurId = 3;
                }
                cOdeme odeme = new cOdeme();
                //ADISYONID,ODEMETURID,MUSTERIID,ARATOPLAM,KDVTUTARI,TOPLAMTUTAR,INDIRIM
                odeme.AdisyonID= Convert.ToInt32(lblAdisyonId.Text);
                odeme.OdemeTurId = odemeTurId;
                odeme.MusteriId = musteriId;
                odeme.AraToplam = Convert.ToDecimal(lbOdenecek.Text);
                odeme.KdvTuari = Convert.ToDecimal(lbKdv.Text);
                odeme.GenelToplam = Convert.ToDecimal(lbToplamTutar.Text);
                odeme.Indirim = Convert.ToDecimal(lblndirim.Text);

                bool result = odeme.billClose(odeme);
                if (result)
                {
                    MessageBox.Show("Hesap Kapatılmıştır.");
                    masalar.setChangeTableState(Convert.ToString(masaid), 1);

                    cRezervasyon c = new cRezervasyon();
                    c.rezervationclose(Convert.ToInt32(lblAdisyonId.Text));

                    cAdisyon a = new cAdisyon();
                    a.adisyonkapat(Convert.ToInt32(lblAdisyonId.Text),0);

                    this.Close();
                    frmMasalar frm = new frmMasalar();
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Hesap Kapatılırken bir hata oluştu.Lütfen Yetkililere bildiriniz.");
                }



            }//Paket Sipariş
            else if(cGenel._ServisTurNo == 2)
            {
                cOdeme odeme = new cOdeme();
                //ADISYONID,ODEMETURID,MUSTERIID,ARATOPLAM,KDVTUTARI,TOPLAMTUTAR,INDIRIM
                odeme.AdisyonID = Convert.ToInt32(lblAdisyonId.Text);
                odeme.OdemeTurId = odemeTuru;
                odeme.MusteriId = 1;//paket sipariş ıdsi gelcek
                odeme.AraToplam = Convert.ToDecimal(lbOdenecek.Text);
                odeme.KdvTuari = Convert.ToDecimal(lbKdv.Text);
                odeme.GenelToplam = Convert.ToDecimal(lbToplamTutar.Text);
                odeme.Indirim = Convert.ToDecimal(lblndirim.Text);

                bool result = odeme.billClose(odeme);
                if (result)
                {


                   

                    cAdisyon a = new cAdisyon();
                    a.adisyonkapat(Convert.ToInt32(lblAdisyonId.Text), 1);

                    cPaketler p = new cPaketler();
                    p.OrderServiceClose(Convert.ToInt32(lblAdisyonId.Text));
                    MessageBox.Show("Hesap Kapatılmıştır.");


                    this.Close();
                    frmMasalar frm = new frmMasalar();
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Hesap Kapatılırken bir hata oluştu.Lütfen Yetkililere bildiriniz.");
                }
            }
        }
        
private void btnHspOzet_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }
        Font Baslik = new Font("Verdana", 15, FontStyle.Bold);
        Font altBaslik = new Font("Verdana", 12, FontStyle.Regular);
        Font icerik = new Font("Verdana", 10);
        SolidBrush sb = new SolidBrush(Color.Black);

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            StringFormat st = new StringFormat();
            st.Alignment = StringAlignment.Near;
            e.Graphics.DrawString("Bahadır YEMEKSALONU", Baslik, sb, 250, 100, st);

            e.Graphics.DrawString("--------------------", altBaslik, sb, 300, 120, st);
            e.Graphics.DrawString("Ürün Adı                   Adet      Fiyat", altBaslik, sb, 150, 250, st);
            e.Graphics.DrawString("-------------------------------------------------------", altBaslik, sb, 150, 280, st);
            for (int i = 0; i < lvUrunler.Items.Count; i++)
            {
                e.Graphics.DrawString(lvUrunler.Items[i].SubItems[0].Text, icerik, sb, 150, 300 + i * 30, st);
                e.Graphics.DrawString(lvUrunler.Items[i].SubItems[1].Text, icerik, sb, 350, 300 + i * 30, st);
                e.Graphics.DrawString(lvUrunler.Items[i].SubItems[3].Text, icerik, sb, 420, 300 + i * 30, st);
            }
            e.Graphics.DrawString("----------------------------------------------------------", altBaslik, sb, 150, 300 + 30 * lvUrunler.Items.Count, st);
            e.Graphics.DrawString("İndirim Tutarı      :---------------" + lblndirim.Text + "TL", altBaslik, sb, 250, 300 + 30 * (lvUrunler.Items.Count + 1), st);
            e.Graphics.DrawString("KDV Tutarı          :---------------" + lbKdv.Text + "TL", altBaslik, sb, 250, 300 + 30 * (lvUrunler.Items.Count + 2), st);
            e.Graphics.DrawString("Toplam Tutar       :---------------" + lbToplamTutar.Text + "TL", altBaslik, sb, 250, 300 + 30 * (lvUrunler.Items.Count+ 3), st);
            e.Graphics.DrawString("Ödediğiniz Tutar  :---------------" + lbOdenecek.Text + "TL", altBaslik, sb, 250, 300 + 30 * (lvUrunler.Items.Count+ 4), st);
        }
    }
}

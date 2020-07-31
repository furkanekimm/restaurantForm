using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace restaurant
{
    class cAdisyon
    {
        cGenel gnl = new cGenel();

        #region Fields
        private int _ID;
        private int _ServisTurNo;
        private decimal _Tutar;
        private DateTime _Tarih;
        private int _PersonelId;
        private int _Durum;
        private int _MasaId;
        #endregion
        #region Properties
        public int ID
        {
            get
            {
                return _ID;
            }

            set
            {
                _ID = value;
            }
        }

        public int ServisTurNo
        {
            get
            {
                return _ServisTurNo;
            }

            set
            {
                _ServisTurNo = value;
            }
        }

        public decimal Tutar
        {
            get
            {
                return _Tutar;
            }

            set
            {
                _Tutar = value;
            }
        }

        public DateTime Tarih
        {
            get
            {
                return _Tarih;
            }

            set
            {
                _Tarih = value;
            }
        }

        public int PersonelId
        {
            get
            {
                return _PersonelId;
            }

            set
            {
                _PersonelId = value;
            }
        }

        public int Durum
        {
            get
            {
                return _Durum;
            }

            set
            {
                _Durum = value;
            }
        }

        public int MasaId
        {
            get
            {
                return _MasaId;
            }

            set
            {
                _MasaId = value;
            }
        } 
        #endregion

        public int getByAddition(int MasaId)
        {
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select top 1 ID From Adisyonlar Where MASAID=@MasaId Order by ID desc", con);

            cmd.Parameters.Add("@MasaId", SqlDbType.Int).Value = MasaId;
            try
            {
                if(con.State==ConnectionState.Closed)
                {
                    con.Open();
                }
                MasaId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
                
            }
            finally
            {
                con.Close();
            }
            return MasaId;
        
        }
        
        public bool setByAdditionNew(cAdisyon Bilgiler)
        {
            bool sonuc = false;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Insert Into Adisyonlar(SERVISTURNO,TARIH,PERSONELID,MASAID,DURUM) values(@ServisTurNo,@Tarih,@PersonelID,@MasaId,@Durum)", con);
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.Parameters.Add("@ServisTurNo", SqlDbType.Int).Value = Bilgiler.ServisTurNo;
                cmd.Parameters.Add("@Tarih", SqlDbType.DateTime).Value = Bilgiler.Tarih;
                cmd.Parameters.Add("@PersonelID", SqlDbType.Int).Value = Bilgiler.PersonelId;
                cmd.Parameters.Add("@MasaId", SqlDbType.Int).Value = Bilgiler.MasaId;
                cmd.Parameters.Add("@Durum", SqlDbType.Bit).Value = 0;
                sonuc = Convert.ToBoolean(cmd.ExecuteNonQuery());
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
                
            }
            finally
            {
                con.Dispose();
                con.Close();

            }
            return sonuc;



        }

        public void adisyonkapat(int adisyonID, int durum)
        {
            
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Update adisyonlar set durum = @durum where ID=@adisyonId", con);
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.Parameters.Add("adisyonId", SqlDbType.Int).Value = adisyonID;
                cmd.Parameters.Add("durum", SqlDbType.Int).Value = durum;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
                throw;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }



          
        }

        public int paketAdisyonIdbulAdedi()
        {
            int miktar = 0;
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select count(*) as Sayi from adisyonlar where (Durum=0) and (SERVISTURNO=2)", con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            try
            {
                miktar = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
                throw;
            }

            return miktar;
        }

        public void acikPaketAdisyonlar(ListView lv)
        {
            lv.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select paketSiparis.MUSTERIID,Musteriler.Ad + ' ' +Musteriler.Soyad as Musteri ,Adisyonlar.ID as adisyonID from paketSiparis Inner Join  musteriler on musteriler.ID=paketSiparis.MUSTERIID Inner Join adisyonlar on adisyonlar.ID=paketSiparis.ADISYONID where Adisyonlar.Durum=0", con);
            SqlDataReader dr = null;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                dr = cmd.ExecuteReader();
                int sayac = 0;

                while (dr.Read())
                {
                    lv.Items.Add(dr["MUSTERIID"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Musteri"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["adisyonID"].ToString());

                    sayac++;

                }
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;

            }
            finally
            {
                dr.Close();
                con.Dispose();
                con.Close();
            }



        }

        public int musterininsonadisyonId(int musteriId)
        {
            int sonuc = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select adisyonlar.ID from adisyonlar Inner Join paketSiparis on  paketSiparis.ADISYONID=adisyonlar.ID where paketSiparis.durum =0 and adisyonlar.durum=0 and paketsiparis.MUSTERIID=@musteriId", con);
            try  
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.Parameters.Add("@musteriId", SqlDbType.Int).Value = musteriId;
               
                sonuc = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;

            }
            finally
            {
                con.Dispose();
                con.Close();

            }


            return sonuc;
        }

        public void musteriDetaylar(ListView lv, int musteriId)
        {
            lv.Items.Clear();
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select paketSiparis.MUSTERIID, paketSiparis.ADISYONID, musteriler.AD,musteriler.SOYAD, CONVERT(varchar(10),adisyonlar.TARIH,104) as tarih from adisyonlar Inner Join paketSiparis on paketSiparis.ADISYONID = adisyonlar.ID Inner Join musteriler on musteriler.ID = paketSiparis.MUSTERIID where adisyonlar.SERVISTURNO = 2 and paketSiparis.MUSTERIID = @musteriId", con);

            cmd.Parameters.Add("@musteriId", SqlDbType.Int).Value = musteriId;
            SqlDataReader dr = null;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            try
            {
                int sayac = 0;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lv.Items.Add(dr["MUSTERIID"].ToString());                 
                    lv.Items[sayac].SubItems.Add(dr["AD"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["SOYAD"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["tarih"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["ADISYONID"].ToString());

                    sayac++;

                }


            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
                throw;
            }

        }

    }
}

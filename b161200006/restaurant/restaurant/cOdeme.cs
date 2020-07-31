using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace restaurant
{
    class cOdeme
    {
        cGenel gnl = new cGenel();
        #region Fields
        private int _OdemeID;
        private int _AdisyonID;
        private int _OdemeTurId;
        private decimal _AraToplam;
        private decimal _Indirim;
        private decimal _KdvTuari;
        private decimal _GenelToplam;
        private DateTime _Tarih;
        private int _MusteriId;
        #endregion

        #region Properties
        public int OdemeID
        {
            get
            {
                return _OdemeID;
            }

            set
            {
                _OdemeID = value;
            }
        }

        public int AdisyonID
        {
            get
            {
                return _AdisyonID;
            }

            set
            {
                _AdisyonID = value;
            }
        }

        public int OdemeTurId
        {
            get
            {
                return _OdemeTurId;
            }

            set
            {
                _OdemeTurId = value;
            }
        }

        public decimal AraToplam
        {
            get
            {
                return _AraToplam;
            }

            set
            {
                _AraToplam = value;
            }
        }

        public decimal Indirim
        {
            get
            {
                return _Indirim;
            }

            set
            {
                _Indirim = value;
            }
        }

        public decimal KdvTuari
        {
            get
            {
                return _KdvTuari;
            }

            set
            {
                _KdvTuari = value;
            }
        }

        public decimal GenelToplam
        {
            get
            {
                return _GenelToplam;
            }

            set
            {
                _GenelToplam = value;
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

        public int MusteriId
        {
            get
            {
                return _MusteriId;
            }

            set
            {
                _MusteriId = value;
            }
        } 
        #endregion
        //Musteri hesabını kapatma Masa
        public bool billClose(cOdeme bill)
        {
            bool result = false;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Insert Into hesapOdemeleri(ADISYONID,ODEMETURID,MUSTERIID,ARATOPLAM,KDVTUTARI,TOPLAMTUTAR,INDIRIM)values(@ADISYONID,@ODEMETURID,@MUSTERIID,@ARATOPLAM,@KDVTUTARI,@TOPLAMTUTAR,@INDIRIM)", con);

            try
            {
                if (con.State==ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.Parameters.Add("ADISYONID", SqlDbType.Int).Value = bill._AdisyonID;
                cmd.Parameters.Add("ODEMETURID", SqlDbType.Int).Value = bill._OdemeTurId;
                cmd.Parameters.Add("MUSTERIID", SqlDbType.Int).Value = bill._MusteriId;
                cmd.Parameters.Add("ARATOPLAM", SqlDbType.Money).Value = bill._AraToplam;
                cmd.Parameters.Add("KDVTUTARI", SqlDbType.Money).Value = bill._KdvTuari;
                cmd.Parameters.Add("INDIRIM", SqlDbType.Money).Value = bill._Indirim;
                cmd.Parameters.Add("TOPLAMTUTAR", SqlDbType.Money).Value = bill._GenelToplam;

                result = Convert.ToBoolean(cmd.ExecuteNonQuery());
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
            return result;
        }

        //Müşterinin toplam adisyon tutarı harcaması
        public decimal sumTotalforClientId(int clientId)
        {

            decimal total = 0;
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select sum(TOPLAMTUTAR) as total from hesapOdemeleri Where MUSTERIID=@clientId",con);
            try
            {
                if (con.State==ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.Parameters.Add("clientId", SqlDbType.Int).Value = clientId;
                total = Convert.ToDecimal(cmd.ExecuteScalar());
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
            
            return total;
            
        }
    }
}

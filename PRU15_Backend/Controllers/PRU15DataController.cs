using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PRU15_Shared;

namespace PRU15_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PRU15DataController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            //Database.Instance.InsertDataParliament();

            //var dataParlimen = Database.Instance.GetParliament();

            //var dataCalon = Database.Instance.GetCalon();

            //foreach (var data in dataCalon)
            //{
            //    DatabaseContext.Instance.InsertAllDataCalon(data, out string exp);
            //}

            //foreach (var data in dataParlimen)
            //{
            //    DatabaseContext.Instance.InsertAllDataParlimen(data, out string exp);
            //}

            return "Backend Data for PRU15";
        }

        [HttpGet]
        [Route("parlimen")]
        public List<DataParlimen> GetParlimen(string negeri)
        {
            try
            {
                var data = DatabaseContext.Instance.GetDataParlimen(negeri);

                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("calon")]
        public List<DataCalon> GetCalon(string kodParlimen)
        {
            try
            {
                //var data = Database.Instance.GetCalon(negeri, kodParlimen);
                var data = DatabaseContext.Instance.GetDataCalon(kodParlimen);

                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("jumlahkerusi")]
        public List<KerusiBertanding> GetKerusiBertanding()
        {
            try
            {
                var data = DatabaseContext.Instance.GetDataPartiBertanding();

                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("jumlahkerusi/state")]
        public List<KerusiBertanding> GetKerusiBertandingByState(string negeri)
        {
            try
            {
                var data = DatabaseContext.Instance.GetDataPartiBertanding();

                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("visitor")]
        public string GetJumlahVisitor()
        {
            string jumlahVisitors = string.Empty;
            try
            {
                jumlahVisitors = DatabaseContext.Instance.GetJumlahVisitor();

            }
            catch (Exception ex)
            {

            }

            return jumlahVisitors;
        }

        [HttpPost]
        [Route("visitor/update")]
        public int UpdateJumlahVisitor()
        {
            int errorCode;
            try
            {
                DatabaseContext.Instance.UpdateJumlahVisitor();
                errorCode = 200;

            }
            catch (Exception ex)
            {
                errorCode = 99999;
            }

            return errorCode;
        }

        [HttpGet]
        [Route("undi")]
        public List<UndianCalon> UpdateUndianCalon(string namaCalon , string kodParlimen)
        {
            try
            {
                DatabaseContext.Instance.UpdateUndianCalon(namaCalon, kodParlimen);
                var data = GetPeratusanUndianCalon(kodParlimen);
                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("undi/reset")]
        public int ResetUndianCalon(string namaCalon = null, string kodParlimen = null)
        {
            int errorCode;

            try
            {
                DatabaseContext.Instance.ResetUndianCalon(namaCalon, kodParlimen);

                errorCode = 200;
            }
            catch (Exception ex)
            {
                errorCode = 999;
            }

            return errorCode;
        }

        private List<UndianCalon> GetPeratusanUndianCalon(string kodParlimen)
        {
            List<UndianCalon> senaraiCalon = new List<UndianCalon>();

            var updatedCalon = DatabaseContext.Instance.GetDataCalon(kodParlimen);
            var senaraiJumlahKerusi = updatedCalon.Select(x => x.Jumlah_Undian).ToList();

            double jumlahUndianDalamParlimen = 0.0;

            foreach(var item in senaraiJumlahKerusi)
            {
                jumlahUndianDalamParlimen += Convert.ToDouble(item);
            }

            foreach (var item in updatedCalon)
            {
                senaraiCalon.Add(new UndianCalon
                {
                    NamaCalon = item.NamaCalon,
                    KodParlimen = item.KodParlimen,
                    NamaParlimen = item.NamaParlimen,
                    Jumlah_Undian = item.Jumlah_Undian,
                    Peratusan_Undian = ((Convert.ToDouble(item.Jumlah_Undian) / jumlahUndianDalamParlimen) * 100).ToString("0.00")
                }) ;
            }

            return senaraiCalon;
        }

    }
}

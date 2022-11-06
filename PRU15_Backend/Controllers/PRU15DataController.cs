using System;
using System.Collections.Generic;
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
            return "Backend Data for PRU15";
        }

        [HttpGet]
        [Route("parlimen")]
        public List<DataParlimen> GetParlimen(string negeri)
        {
            try
            {
            var data = Database.Instance.GetParliament(negeri);
                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("calon")]
        public List<DataCalon> GetCalon(string negeri , string kodParlimen)
        {
            try
            {
                var data = Database.Instance.GetCalon(negeri, kodParlimen);
                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

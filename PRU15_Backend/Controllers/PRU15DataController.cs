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
            return "Backend Data for PRU15";
        }

        [HttpGet]
        [Route("parlimen")]
        public List<DataParlimen> GetParlimen(string negeri)
        {
            var data = Database.Instance.GetParliament(negeri);
            return data;
        }

        [HttpGet]
        [Route("calon")]
        public List<DataCalon> GetCalon(string negeri , string kodParlimen)
        {
            var data = Database.Instance.GetCalon(negeri , kodParlimen);
            return data;
        }
    }
}

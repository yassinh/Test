using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DevTest.Models
{

    public class DevTest
    {
        public int ID { get; set; }
        public string CampaignName { get; set; }
        public DateTime Date { get; set; }
        public int? Clicks { get; set; }
        public int? Impressions { get; set; }
        public string AffiliateName { get; set; }
        public int? Conversions { get; set; }
    }
}
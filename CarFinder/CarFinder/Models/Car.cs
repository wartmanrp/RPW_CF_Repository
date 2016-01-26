using System.Collections.Generic;
using System.Data.Entity;


namespace CarFinder.Models
{
    public class Car
    {
        public int id { get; set; }
        public string make { get; set; }
        public string model_name { get; set; }
        public string model_trim { get; set; }
        public string model_year { get; set; } 
        public string body_style { get; set; }
        public string seats { get; set; }
        public string doors { get; set; }
        public string engine_cc { get; set; }
        public string engine_power_ps { get; set; }
        public string drive_type { get; set; }
        public string transmission_type { get; set; }

    }

    public class Recalls
    {
        public int Count { get; set; }
        public string Message { get; set; }
        public RecallItem[] Results { get; set; }
        
    }


    public class RecallItem 
    {
        public string Manufacturer { get; set; }
        public string NHTSACampaignNumber { get; set; }
        public string Component { get; set; }
        public string Summary { get; set; }
        public string Conequence { get; set; }
        public string Remedy { get; set; }
    }

    public class CarData
    {
        public Car car { get; set; }
        public Recalls recalls { get; set; }
        public string[] imageURLs { get; set; }

    }

    public class CarsDb : DbContext
    {
        public CarsDb()
            : base("AzureConnection")
        {         
        }

        public static CarsDb Create()
        {
            return new CarsDb();
        }
    }
}
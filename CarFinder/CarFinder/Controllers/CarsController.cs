using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CarFinder.Models;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CarFinder.Controllers
{
    public class CarsController : ApiController
    {
        CarsDb db = new CarsDb();
        //GET: api/years
        /// <summary>
        /// Retrieves a list of available car model years.
        /// </summary>
        /// <returns>IEnumerable list of years</returns>
        //[ActionName("Years")]
        [Route("api/years")]
        public async Task<IHttpActionResult> GetYears()
        {
            var retval = await db.Database.SqlQuery<string>("EXEC GetYears").ToListAsync();
            if (retval != null)
                return Ok(retval);
            else
                return NotFound();
        }

        // GET: api/makes

        /// <summary>
        /// Retrieves a list of specific car makes, based on the parameter "year"
        /// </summary>
        /// <param name="year"></param>
        /// <returns>Returns IEnumerable list of car makes by the specified year</returns>
        //[ActionName("Makes")]
        [Route("api/makes")]
        public async Task<IHttpActionResult> GetMakes(string year)
        {
            var retval = await db.Database.SqlQuery<string>("EXEC GetMakesByYear @year", 
            new SqlParameter("year", year))
            .ToListAsync();
            if (retval != null)
                return Ok(retval);
            else
                return NotFound();
        }

        // GET: api/models

        /// <summary>
        /// Retrieves car models by specified parameters "year" and "make"
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <returns>Returns IEnumerable list of car models given a specific year and make.</returns>
        /// 
        //[ActionName("Models")]
        [Route("api/models")]
        public async Task<IHttpActionResult> GetModels(string year, string make)
        {
            var retval = await db.Database.SqlQuery<string>("EXEC GetModelsByYearAndMake @year, @make",
            new SqlParameter("year", year),
            new SqlParameter("make", make))
            .ToListAsync();
            if (retval != null)
                return Ok(retval);
            else
                return NotFound();
        }

        // GET: api/trims

        /// <summary>
        /// Retrieves a list of car trims by parameters "year", "make", "model" 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns>Returns an IEnumerable list of car trims given a specified year, make and model.</returns>
        /// 
        //[ActionName("Trims")]
        [Route("api/trims")]
        public async Task<IHttpActionResult> GetTrims(string year, string make, string model)
        {
            var retval = await db.Database.SqlQuery<string>("EXEC GetTrimsByYearMakeAndModel @year, @make, @model",
            new SqlParameter("year", year),
            new SqlParameter("make", make),
            new SqlParameter("model", model))
            .ToListAsync();
            if (retval != null)
                return Ok(retval);
            else
                return NotFound();
        }

        // GET: api/notrims

        /// <summary>
        /// Retrieves an object of the Car class matching the parameters "year", "make", "model".
        /// as well as matching Recall and Image information from relevant API sources. Built to enable
        /// return of cars lacking defined trims.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns>Returns an IEnumerable list of an object of class=Car</returns>
        /// 
        //[ActionName("Notrims")]
        [Route("api/cars")]
        public async Task<IHttpActionResult> GetCars(string year, string make, string model)
        {
            var carData = new CarData();

            carData.car = await db.Database.SqlQuery<Car>("EXEC GetCarsByYearMakeModel @year, @make, @model",
            new SqlParameter("year", year),
            new SqlParameter("make", make),
            new SqlParameter("model", model)).FirstAsync();

            carData.recalls = GetRecalls(year, make, model).Result;
            carData.imageURLs = GetImages(year, make, model, null);

            return Ok(carData);
        }


        // GET: api/Cars/Cars

        /// <summary>
        /// Retrieves an object of the Car class matching the parameters "year", "make", "model", "trim".
        /// as well as matching Recall and Image information from relevant API sources.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <param name="trim"></param>
        /// <returns>Returns an IEnumerable list of an object of class=Car</returns>
        /// 
        //[ActionName("Cars")]
        [Route("api/cars")]
        public async Task<IHttpActionResult> GetCars(string year, string make, string model, string trim)
        {
            var carData = new CarData();

            carData.car = await db.Database.SqlQuery<Car>("EXEC GetCarsByYearMakeModelAndTrim @year, @make, @model, @trim",
            new SqlParameter("year", year),
            new SqlParameter("make", make),
            new SqlParameter("model", model),
            new SqlParameter("trim", trim)).FirstAsync();

            carData.recalls = GetRecalls(year, make, model).Result;
            carData.imageURLs = GetImages(year, make, model, trim);

                return Ok(carData);
        }



        //GET: api/Cars/Recalls      
        private async Task<Recalls> GetRecalls(string year, string make, string model)
        {

            //HttpResponseMessage response;
            Recalls recalls;

            using (var client = new HttpClient())
            {
                //1) establish base client address
                client.BaseAddress = new System.Uri("http://www.nhtsa.gov/");

                try
                {
                    var query = "webapi/api/Recalls/vehicle/modelyear/" + year + "/make/" + make + "/model/" + model + "?format=json";
                    //2) make request to specific URL on client
                    recalls = await client.GetAsync(query).Result.Content.ReadAsAsync<Recalls>();
                    //3) construct a Recalls object from the resulting JSON data
                    
                }
                catch (Exception)
                {
                    //return InternalServerError(e);
                    return null;
                }
            }
            return recalls;
        }

        private string[] GetImages (string year, string make, string model, string trim)
        {
            // This is the query - or you could get it from args.
            string query = "";
            if (trim == null)
                query = year + " " + make + " " + model;
            else
                query = year + " " + make + " " + model + " " + trim;

            // Create a Bing container.
            string rootUri = "https://api.datamarket.azure.com/Bing/Search";
            var bingContainer = new Bing.BingSearchContainer(new Uri(rootUri));

            // Replace this value with your account key.
            var accountKey = "ZMMsPNe6jZ6qK372fUwv7eaneomhgGw3jxLrwr0w2nk=";

            // Configure bingContainer to use your credentials.
            bingContainer.Credentials = new NetworkCredential("accountKey", accountKey);

            // Build the query.
            var imageQuery = bingContainer.Image(query, null, null, null, null, null, null);
            imageQuery = imageQuery.AddQueryOption("$top", 6);
            var imageResults = imageQuery.Execute();

            //extract the properties needed for the images

            List<string> images = new List<string>();

            foreach (var result in imageResults)
            {
                if( UrlCtrl.IsUrl(result.MediaUrl))
                {
                    images.Add(result.MediaUrl);
                }
            }
            return images.ToArray();
        }
    }

    public static class UrlCtrl
    {
        public static bool  IsUrl(string path)
        {
            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(path);
            request.Method = "HEAD";
            bool result = true;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                result = false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return result;
        }
    }
}

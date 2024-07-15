using IDistributedCacheRedis.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace IDistributedCacheRedis.Web.Controllers
{
    public class ProductController : Controller
    {
        private IDistributedCache _distributedCache;
        private readonly IWebHostEnvironment _appEnvironment;

        public ProductController(
            IDistributedCache distributedCache,
            IWebHostEnvironment appEnvironment)
        {
            _distributedCache = distributedCache;
            _appEnvironment = appEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(10);

            //_distributedCache.SetString("fromcode", "hiredis", cacheEntryOptions);

            //HGETALL fromcode 
            //HGET fromcode data

            //_distributedCache.SetString("fromcode2", "hiredis2", cacheEntryOptions);

            #region SET COMPLEX TYPE CACHE
            //Product p = new Product() { Id = 1, Name = "Kalem", Price = 300 };

            //string jsonproduct = JsonConvert.SerializeObject(p);

            //await _distributedCache.SetStringAsync("product:1", jsonproduct, cacheEntryOptions);
            #endregion

            #region SET BYTE COMPLEX TYPE CACHE
            Product p2 = new Product() { Id = 2, Name = "Kalem2", Price = 300 };

            string jsonstring = JsonConvert.SerializeObject(p2);

            Byte[] bytes = Encoding.UTF8.GetBytes(jsonstring);
            _distributedCache.Set("product:2", bytes, cacheEntryOptions);
            #endregion

            return View();
        }

        public IActionResult Show()
        {
            #region GET COMPLEX TYPE CACHE

            //var cachedProduct = _distributedCache.GetString("product:1");
            //var product = JsonConvert.DeserializeObject<Product>(cachedProduct);
            //ViewBag.product = product;

            #endregion

            #region GET BYTE COMPLEX TYPE CACHE
            Byte[] bytes = _distributedCache.Get("product:2");

            var jsonstring = Encoding.UTF8.GetString(bytes);

            var product = JsonConvert.DeserializeObject<Product>(jsonstring);
            ViewBag.product = product;
            #endregion

            return View();
        }


        /// <summary>
        ///  Image i cache e setliyoruz
        /// </summary>
        /// <returns></returns>
        public IActionResult SetImageCache()
        {
            DistributedCacheEntryOptions option = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(10),
            };

            var path = Path.Combine(_appEnvironment.WebRootPath, "img/universe.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("image:1", imageByte, option);
            return View();
        }

        /// <summary>
        /// Image i cacheten alıp dönüyoruz
        /// </summary>
        /// <returns></returns>
        public IActionResult GetImageCache()
        {
            byte[] imageByte = _distributedCache.Get("image:1");

            return File(imageByte, "image/jpg");
        }

        /// <summary>
        /// İmageyi gösteriyoruz.
        /// </summary>
        /// <returns></returns>
        public IActionResult ShowImageCache()
        {
            return View();
        }
    }
}

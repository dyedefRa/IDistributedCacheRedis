1)NUGET tan ilgili kütüphaneyi indir. >> Microsoft.Extensions.Caching.StackExchangeRedis indir.

2) Startup a tanımla. > 

//Redis Client ayakta olmalı.
            //Chocalatety ile > redis-client €
            //Docker ile > Run et.
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });

3) Controllerdan injextion yap (Product Controller.) >

 private IDistributedCache _distributedCache;
        public ProductController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
---------------------------------------------------
COMPLEX TYPE CACHE 
SET
 //Product p = new Product() { Id = 1, Name = "Kalem", Price = 300 };
    //string jsonproduct = JsonConvert.SerializeObject(p);
    //await _distributedCache.SetStringAsync("product:1", jsonproduct, cacheEntryOptions);

GET

      //var cachedProduct = _distributedCache.GetString("product:1");
    //var product = JsonConvert.DeserializeObject<Product>(cachedProduct);
    //ViewBag.product = product;
---------------------------------------------------
BYTE COMPLEX TYPE CACHE
SET
 Product p2 = new Product() { Id = 2, Name = "Kalem2", Price = 300 };
 string jsonstring = JsonConvert.SerializeObject(p2);
 Byte[] bytes = Encoding.UTF8.GetBytes(jsonstring);
 _distributedCache.Set("product:2", bytes, cacheEntryOptions);

GET
 Byte[] bytes = _distributedCache.Get("product:2");
 var jsonstring= Encoding.UTF8.GetString(bytes);
var product = JsonConvert.DeserializeObject<Product>(jsonstring);
ViewBag.product = product;
---------------------------------------------------
IMAGE BYTE CACHE

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
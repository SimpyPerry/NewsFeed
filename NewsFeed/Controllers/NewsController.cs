using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsFeed.Entites;
using NewsFeed.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace NewsFeed.Controllers
{
    public class NewsController : Controller
    {
        //Henter data fra api'en
        private readonly HttpClient _httpClient;
        //Hvor den skal finde api'en
        private Uri BaseEndPoint { get; set; }

        //private readonly INewsFeedRepo _newsFeed;
        //private readonly IMapper _mapper;

        public NewsController(INewsFeedRepo newsFeed, IMapper mapper)
        {

            //_newsFeed = newsFeed;
            //_mapper = mapper;
            //Api'en den skal bruge
            BaseEndPoint = new Uri("https://localhost:44381/api/values");
            _httpClient = new HttpClient();
        }

        //public IActionResult Index()

        //ActionResult er normalt et View
        public async Task<IActionResult> Index()
        {
            //Læser dataen fra API'en
            var repsone = await _httpClient.GetAsync(BaseEndPoint);

            //Sikre vi at vi fik en succes status kode, hvis returner den en expection
            repsone.EnsureSuccessStatusCode();

            //Laver reponsen om til en string
            var result = await repsone.Content.ReadAsStringAsync();

            
            //var news = _newsFeed.GetNews();

            //var result = _mapper.Map<IEnumerable <NewsFeedEntity>,IEnumerable <NewsFeedDTO>>(news);
            
            //Konvertere string til Json, som "deserializeres" til en liste af NewsFeedDTO
            return View(JsonConvert.DeserializeObject<IEnumerable<NewsFeedDTO>>(result));
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsFeedDTO newsFeed)
        {
            if (ModelState.IsValid)
            {
                // Post newsFeed som json til API'en
                var response = await _httpClient.PostAsJsonAsync<NewsFeedDTO>(BaseEndPoint + "/addNews", newsFeed);

                response.EnsureSuccessStatusCode();



                //string[] hashs = newsFeed.HashTags.Split(",");
                //newsFeed.HashTags = string.Join(" ", hashs);
                //var result = _mapper.Map<NewsFeedDTO, NewsFeedEntity>(newsFeed);
                //result.CreatedDate = DateTime.UtcNow;
                //result.UpdatedDate = DateTime.UtcNow;

                //_newsFeed.AddNewsToDb(result);

                //if(_newsFeed.SaveAll())
                //{
                //    return RedirectToAction("Index", "News");
                //}

                return RedirectToAction("Index", "News");


            }

            return View();
        }

        [Route("from/{fromYear}/{fromMonth}/to/{toYear}/{toMonth}")]
        public async Task<IActionResult> findNews(int fromYear, int fromMonth, int toYear, int toMonth)
        {

            var response = await _httpClient.GetAsync(BaseEndPoint + $"/from/{fromYear}/{fromMonth}/to/{toYear}/{toMonth}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            //if(fromYear > toYear || fromMonth > 12 || toMonth > 12)
            //{
            //    return BadRequest();
            //}

            //var news = _newsFeed.GetNewsFromDates(fromYear, fromMonth, toYear, toMonth);

            //if(news == null)
            //{
            //    return NotFound();
            //}

            //var result = _mapper.Map<IEnumerable<NewsFeedEntity>, IEnumerable<NewsFeedDTO>>(news);

            return View(JsonConvert.DeserializeObject<IEnumerable<NewsFeedDTO>>(result));
        }

        

        
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync(BaseEndPoint + $"/{id}");


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "News");
            }

            return RedirectToAction("index", "news");

            
            
        }
    }
}

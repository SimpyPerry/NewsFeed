using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsFeed.Entites;
using NewsFeed.Models;

namespace NewsFeedAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly INewsFeedRepo _newsFeed;
        private readonly IMapper _mapper;
        

        public ValuesController(INewsFeedRepo newsFeed, IMapper mapper)
        {
            
            _mapper = mapper;
           
            _newsFeed = newsFeed;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<NewsFeedDTO> GetNews()
        {
            var result = _mapper.Map<IEnumerable <NewsFeedEntity>, IEnumerable <NewsFeedDTO>>(_newsFeed.GetNews());



            return result;
        }

        //Fejl her FromBody var sat til LoginModel istedet for NewsFeedDTO
        [HttpPost("addNews")]
        public IActionResult CreateNews([FromBody]NewsFeedDTO model )
        {
            if(model == null)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            string[] hashs = model.HashTags.Split(",");
            model.HashTags = string.Join(" ", hashs);

            var result = _mapper.Map<NewsFeedDTO, NewsFeedEntity>(model);
            result.CreatedDate = DateTime.UtcNow;
            result.UpdatedDate = DateTime.UtcNow;
            _newsFeed.AddNewsToDb(result);
            if (_newsFeed.SaveAll())
            {
                return Ok(result);
            }

            return BadRequest();
        }

        // PUT api/values/5
        [HttpPost("DeleteANews/{id}")]
        public IActionResult UpdateANews(int id, [FromBody] NewsFeedDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var news =  _newsFeed.GetANews(id);
            if(news == null)
            {
                return NotFound();
            }

            string[] hashs = model.HashTags.Split(",");
            model.HashTags = string.Join(" ", hashs);

            var result = _mapper.Map<NewsFeedDTO, NewsFeedEntity>(model);
            result.NewsId = id;
            

            _newsFeed.UpdateEnity(result);

            if (_newsFeed.SaveAll())
            {
                return NoContent();
            }

            return BadRequest();

        }

        [HttpGet("from/{fromYear}/{fromMonth}/to/{toYear}/{toMonth}")]
        public IActionResult GetNewsByDate(int fromYear, int fromMonth, int toYear, int toMonth)
        {
            if (fromYear > toYear || fromMonth > 12 || toMonth > 12)
            {
                return BadRequest();
            }

            var news = _newsFeed.GetNewsFromDates(fromYear, fromMonth, toYear, toMonth);

            if (news == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<IEnumerable<NewsFeedEntity>, IEnumerable<NewsFeedDTO>>(news);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOneNews(int id)
        {

            

           var news = _newsFeed.GetANews(id);

            if(news == null)
            {
                return NotFound();
            }
            else
            {
                _newsFeed.DeleteTheNews(id);
                _newsFeed.SaveAll();

                return NoContent();
                
            }
            
           
        }
       
    }
}

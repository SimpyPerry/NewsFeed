using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Entites
{
    public class NewsFeedRepo : INewsFeedRepo
    {
        private readonly NewsFeedContext _newsFeed;

        public NewsFeedRepo(NewsFeedContext newsFeed)
        {
            _newsFeed = newsFeed;
        }

        public void AddNewsToDb(NewsFeedEntity result)
        {
            _newsFeed.Add(result);
        }

        public NewsFeedEntity GetANews(int id)
        {
            return _newsFeed.news.Where(n => n.NewsId == id).FirstOrDefault();
        }

        public IEnumerable<NewsFeedEntity> GetNews()
        {
            return _newsFeed.news.OrderBy(n => n.CreatedDate).ToList();
        }

        public IEnumerable<NewsFeedEntity> GetNewsFromDates(int fromYear, int fromMonth, int toYear, int toMonth)
        {
            DateTime fromDate = Convert.ToDateTime($"{fromYear}/{fromMonth}/1");
            var lastDayofMonth = DateTime.DaysInMonth(toYear, toMonth);
            DateTime toDate = Convert.ToDateTime($"{toYear}/{toMonth}/{lastDayofMonth}");

            var results = _newsFeed.news.Where(d => d.CreatedDate >= fromDate && d.CreatedDate <= toDate);


            return results;
        }

        public bool SaveAll()
        {
            return _newsFeed.SaveChanges() > 0;
        }

        public void UpdateEnity(NewsFeedEntity result)
        {
            var value = _newsFeed.news.Where(n => n.NewsId == result.NewsId).FirstOrDefault();
            if( value != null)
            {
              

                value.Title = result.Title;
                value.UpdatedDate = DateTime.Now;
                
                value.Content = result.Content;
                value.Author = result.Author;

                if (result.HashTags != null)
                {
                    value.HashTags = result.HashTags;
                }

                

            }
        }
    }
}

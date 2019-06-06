using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Entites
{
    public interface INewsFeedRepo
    {
        void AddNewsToDb(NewsFeedEntity result);
        bool SaveAll();
        IEnumerable<NewsFeedEntity> GetNews();
        NewsFeedEntity GetANews(int id);
        void UpdateEnity(NewsFeedEntity result);
        IEnumerable<NewsFeedEntity> GetNewsFromDates(int fromYear, int fromMonth, int toYear, int toMonth);
        void DeleteTheNews(int id);
    }
}

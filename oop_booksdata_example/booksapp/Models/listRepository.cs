using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace booksapp.Models
{
    public class listRepository
    {
          public static List<Publication> FakeData()
        {

            var fakeData = new List<Publication>
            {
                new Article("24/7/2018", 
                            "https://cdn.pixabay.com/photo/2018/03/02/20/22/sktop-3194194__340.jpg", 
                            "Improving the foundation of our falling sand simulator",
                            "Addison is a detective from Sale who falls in love with his best friend. The two are separated when the best friend falls for somebody else. However, Addison manages to rescue the situation by buying a magnificent t-shirt.,"),
              
                new Book("15/5/2006",
                         "https://cdn.pixabay.com/photo/2016/08/18/08/31/lemur-1602313__340.jpg",
                         "NASA fed some Apollo 11 lunar samples to cockroaches and mice",
                         "A farmer from Markham is delighted when she gets the chance to take part in the final of X-Factor. However, her chances are scuppered when her son goes missing. Unexpectedly, the farmer is bitten by a zombie and therefore is disqualified from competing.",  
                         2)
            };
            return fakeData;
        }
    }
}
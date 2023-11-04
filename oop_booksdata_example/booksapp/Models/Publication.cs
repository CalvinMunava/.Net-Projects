using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace booksapp.Models
{

    public class Publication
    {
        //Data Members
        public string _PubDate { get; set; }
        public string _Url { get; set; }
        public string _Title { get; set; }
        public string _Summary { get; set; }

        //Constructor
        public Publication(string PubDate, string Url, string Title, string Summary) {
            _PubDate = PubDate;
            _Url = Url;
            _Title = Title;
            _Summary = Summary;

        }
    }

    public class Article : Publication
    {
        //Constructors
        public Article(string pubDate, string Url, string Title, string Summary): base (pubDate, Url,Title,Summary)
        {

        }
        //Methods - Use your own method name
        public string articleData()
        {
            return "The date of publication date is " + this._PubDate;
        }
    }

    public class Book : Publication
    {
        //Data Members
        public int _Edition { get; set; }

        //Constructors
        public Book(string pubDate, string Url, string Title, string Summary, int Edition) : base(pubDate, Url, Title, Summary)
        {
            _Edition = Edition;
        }

        //Methods - Use your own Method Name
        public string bookData()
        {
            return "First publication date is " + this._PubDate + "and the number of editions is " + this._Edition;
        }
    }

}
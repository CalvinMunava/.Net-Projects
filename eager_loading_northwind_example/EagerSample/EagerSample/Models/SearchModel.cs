using System.Collections.Generic;

namespace EagerSample.Models
    {
    public class SearchModel
        {
        public SearchModel()
            {
            Results = new List<Customers>();
            }

        public string SelectedCriteria { get; set; }
        public string SearchText { get; set; }
        public List<Customers> Results { get; set; }
        public List<string> Criteria { get; set; } = new List<string> { "Customer ID", "Company Name", "Contact Name" };
        }
    }
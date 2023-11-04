using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TDO_DB_MVC.Models
{
    public class Product
    {
       public int ID { get; set; }
       public string Description { get; set; }
       public  double Price { get; set; }
       public int Quantity { get; set; }
       public  byte[] ImageBlob { get; set; }
       public string ImagePath { get; set; }
       public int ProductType { get; set; }

        //Add this to hold Name of Type 
       public string ProductTypeText { get; set; }


    }
}
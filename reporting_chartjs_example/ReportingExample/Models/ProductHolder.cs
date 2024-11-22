using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportingExample.Models
{
    public class ProductHolder
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public string backColor { get; set; } = GenerateRandomColorBack();
        public string borderColor { get; set; } = GenerateRandomColorBorder();
        public static string GenerateRandomColorBack()
        {
            Random random = new Random();
            int r = random.Next(0, 256);
            int g = random.Next(0, 256);
            int b = random.Next(0, 256);
            return $"rgba({r}, {g}, {b}, {0.2})";
        }
        public static string GenerateRandomColorBorder()
        {
            Random random = new Random();
            int r = random.Next(0, 256);
            int g = random.Next(0, 256);
            int b = random.Next(0, 256);
            return $"rgba({r}, {g}, {b}, {1})";
        }

    }
}
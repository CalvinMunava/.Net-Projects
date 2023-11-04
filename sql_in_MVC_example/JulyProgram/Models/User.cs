using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace JulyProgram.Models
{
    public class User
    {
        public int  StudentId { get; set; } 
        public string StudentName { get; set; }
        public string StudentDegree { get; set; }
        public string StudentImage { get; set; }

        public bool passed { get; set; }

    }
}
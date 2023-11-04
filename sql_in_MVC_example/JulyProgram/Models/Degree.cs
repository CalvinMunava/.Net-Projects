using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JulyProgram.Models
{
    public class Degree
    {
        [Key]
        public int DegreeId { get; set; }
        public string DegreeName { get; set; }
        public int MaxStudents { get; set; }
        public int CurrentStudents { get; set; }
        public virtual DegreeType DegreeType { get; set; }
    }
}
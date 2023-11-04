using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JulyProgram.Models
{
    public class DegreeType
    {

        [ForeignKey("Degree")]
        public int DegreeTypeId { get; set; }
        public string DegreeTypeName { get; set; }

    }
}
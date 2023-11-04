using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JulyProgram.Models;

namespace JulyProgram.Controllers
{
    public class DegreeController : Controller
    {
        // Instatiate list
        public static List<Degree> Degrees = new List<Degree>();
        public static List<DegreeType> DegreeTypes = new List<DegreeType>();

        private void CreateDegreeTypes()
        {
            DegreeType NewTypeDegree = new DegreeType();
            NewTypeDegree.DegreeTypeId = 1;
            NewTypeDegree.DegreeTypeName = "Associate";
            DegreeTypes.Add(NewTypeDegree);

            DegreeType NewTypeDegree1 = new DegreeType();
            NewTypeDegree1.DegreeTypeId = 2;
            NewTypeDegree1.DegreeTypeName = "Associate";
            DegreeTypes.Add(NewTypeDegree1);

            DegreeType NewTypeDegree2 = new DegreeType();
            NewTypeDegree2.DegreeTypeId = 3;
            NewTypeDegree2.DegreeTypeName = "Associate";
            DegreeTypes.Add(NewTypeDegree2);


            DegreeType NewTypeDegree3 = new DegreeType();
            NewTypeDegree3.DegreeTypeId = 4;
            NewTypeDegree3.DegreeTypeName = "Associate";
            DegreeTypes.Add(NewTypeDegree3);
        }
        private void CreateDegrees()
        {
            Degree NewDegree = new Degree();
            NewDegree.DegreeId = 1;
            NewDegree.DegreeName = "Informatics";
            NewDegree.MaxStudents = 250;
            NewDegree.CurrentStudents = 100;
            NewDegree.DegreeType = DegreeTypes.Where(x=> x.DegreeTypeId == 1).First();
            Degrees.Add(NewDegree);

            Degree NewDegree1 = new Degree();
            NewDegree1.DegreeId = 2;
            NewDegree.DegreeName = "Geology";
            NewDegree1.MaxStudents = 50;
            NewDegree1.CurrentStudents = 65;
            NewDegree1.DegreeType = DegreeTypes.Where(x => x.DegreeTypeId == 2).First();
            Degrees.Add(NewDegree1);

            Degree NewDegree2 = new Degree();
            NewDegree2.DegreeId = 3;
            NewDegree.DegreeName = "law";
            NewDegree2.MaxStudents = 250;
            NewDegree2.CurrentStudents = 115;
            NewDegree2.DegreeType = DegreeTypes.Where(x => x.DegreeTypeId == 3).First();
            Degrees.Add(NewDegree2);

        }
        public void Trigger()
        {
            CreateDegreeTypes();
            CreateDegrees();
        }


        public ActionResult Index()
        {
            Trigger();

            //Get the first record in the Degree table
            var firstRecord = Degrees.FirstOrDefault();

            //Get a list of all the Degrees in the Degrees table
            var Allrecords = Degrees.ToList();


            //Get a list of the Degrees in the degrees table but only return the name and total number of current students
            var customrecords = Degrees.Select(x => new {
                Name = x.DegreeName,
                CurrentStudents = x.CurrentStudents
            }).ToList();

            //Get a list of degree names and their degreetype names from the degree and degreetype table
            var DegreeData = Degrees.Select(x => new {
                Name = x.DegreeName,
                TypeName = x.DegreeType.DegreeTypeName           
            }).ToList();

            //DB

            //var DegreeDataDb = Degrees.Include(x => x.DegreeTypes).Select(x => new {
            //    Name = x.DegreeName,
            //    TypeName = x.DegreeType.DegreeTypeName
            //}).ToList();

            //Write a c# method that passes through a word as the parameter and return a list of degrees where the name contains the given parameter.

            /*public*/ List<Degree> searchobject(string parameter)
            {
                List<Degree> SearchList = Degrees.Where(x=> x.DegreeName.Contains(parameter)).ToList();
                return SearchList;
            }


            //Get a list of degrees and sort them descending by name then ascendingly by current students.
            var list = Degrees.OrderByDescending(x => x.DegreeName).OrderBy(xx => xx.CurrentStudents).ToList();



            return View();
        }




    }
}
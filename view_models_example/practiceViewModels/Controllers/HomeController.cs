using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using practiceViewModels.Models;

namespace practiceViewModels.Controllers
{
    public class HomeController : Controller
    {
        //creating a list of type dogs called list of dog objects
        public static List<Dogs> ListOfDogObjects = new List<Dogs>();

        public ActionResult setDogDate()
        {

            // first instance of dog is jessie 
            Dogs dog1 = new Dogs(1,"jessie", "rotweiler", 13);

            // she gets added to the list
            ListOfDogObjects.Add(dog1);

            Dogs dog2 = new Dogs(2,"Sheeba", "German", 9);
            ListOfDogObjects.Add(dog2);

            Dogs dog3 = new Dogs(3,"hunter", "bull terria", 1);
            ListOfDogObjects.Add(dog3);

            TempData["Message"] = "we have populated the table for you";

            return RedirectToAction("Index","Home");
           
        }

        public ActionResult Index()
        {
            if(ListOfDogObjects.Count() <= 0 || ListOfDogObjects == null)
            {
                setDogDate();
            }
            else
            {
                ViewBag.Message = TempData["Message"];
                return View(ListOfDogObjects);
            }
            ViewBag.Message = TempData["Message"];
            return View(ListOfDogObjects);
        }

        public ActionResult CreateDog()
        {
            return View();
        }



        public ActionResult CreateActualDog(int Id,string name,string Bread,int Age)
        {
            Dogs NewDog = new Dogs();
            NewDog.ID = Id;
            ListOfDogObjects.Add(NewDog);
            TempData["Message"] = "Success";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EditDog(int Id)
        {
            Dogs DogtoEdit = ListOfDogObjects.Find(x => x.ID == Id);
            if (DogtoEdit == null)
            {
                TempData["Message"] = "Object Does Not Exist In our List";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                              
                return View(DogtoEdit);

            }

        }


        public ActionResult DeleteDog(int Id)
        {
            Dogs DogtoDelete = ListOfDogObjects.Find(x => x.ID == Id);

            if(DogtoDelete == null)
            {
                TempData["Message"] = "Object Does Not Exist In our List";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ListOfDogObjects.Remove(DogtoDelete);
                TempData["Message"] = "Successfully Deleted Dog";
                return RedirectToAction("Index", "Home");


            }
        }

    }
}
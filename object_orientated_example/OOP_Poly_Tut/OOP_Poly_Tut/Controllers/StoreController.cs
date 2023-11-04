using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OOP_Poly_Tut.Models;

namespace OOP_Poly_Tut.Controllers
{
    public class StoreController : Controller
    {

        //AdminPassword
        private static string SiteUsername = "SiteAdmin";
        private static string SitePassword = "MyNamesjeff69";

        //Declare My PS
        public static List<Playstation> PSlist = new List<Playstation>();

        //Declare My Xbox
        public static List<Xbox> Xblist = new List<Xbox>();

        private void InitialiseStore()
        {
            if(StoreDb.Count() == 0)
            {
                PSlist.Add(new Playstation(1,
                                           "Playstation 4",
                                           "Second Gen Console with cpu cooling and high gpu renders on 8k games",
                                           "2004154874", 
                                           345.68, 
                                           50, 
                                           "~/Content/Images/StoreImages/ps4.png", 
                                           new DateTime(2012, 1, 25), 
                                           "playstation",
                                           "PSN255564", 
                                           4));
                PSlist.Add(new Playstation(2,
                                           "Playstation 3",
                                           "1st gen ps with thx rendering capabilities",
                                           "2004154874", 
                                           35142.23,
                                           50,
                                           "~/Content/Images/StoreImages/ps3.png",
                                           new DateTime(2012, 1, 25),
                                            "playstation",
                                           "PSN+65165564444", 
                                           4));
                PSlist.Add(new Playstation(3, 
                                           "Playstation 5",
                                           "The best console there is ", 
                                           "2004154874", 
                                           125482.254, 
                                           50,
                                           "~/Content/Images/StoreImages/ps5.png",
                                           new DateTime(2012, 1, 25),
                                            "playstation",
                                           "PSN25465453156",
                                           4));

                //Xboxs
                Xblist.Add(new Xbox(4,
                                         "Xbox 1 ",
                                         "3rd Gen Xbox with Face ID ",
                                         "XB11145",
                                         6500,
                                         50,
                                         "~/Content/Images/StoreImages/xbox1.png",
                                         new DateTime(2012, 1, 25),
                                         "xbox",
                                         "TN15415641",
                                         "AA"));
                Xblist.Add(new Xbox(5,
                                         "Xbox 360 ",
                                         "1st Gen Xbox with Kinect with Stories ",
                                         "XB1554",
                                         500,
                                         50,
                                         "~/Content/Images/StoreImages/xbox360.png",
                                         new DateTime(2012, 1, 25),
                                         "xbox",
                                         "TN1541XB44",
                                         "AAA"));



                //Populate the Store list
                StoreDb.AddRange(PSlist);
                StoreDb.AddRange(Xblist);
            }
            else
            {
                StoreDb.Clear();
                StoreDb.AddRange(PSlist);
                StoreDb.AddRange(Xblist);
                return;
            }
        }

        //Declare My Store list
        public static List<GamingConsole> StoreDb = new List<GamingConsole>();

        // GET: Store
        public ActionResult Store()
        {
            InitialiseStore();

            return View(StoreDb);
        }



        //All For PS

        //Index Table
        [HttpGet]
        public ActionResult PsIndex()
        {
            return View(PSlist);
        }

        //Create - View
        [HttpGet]
        public ActionResult AddPsToStore()
        {
            return View();
        }

        //Create - Action
        [HttpPost]
        public ActionResult AddPsToStore(Playstation whateveryouwantto)
        {
            PSlist.Add(whateveryouwantto);
            return RedirectToAction("Store");
        }


        //Edit - View 
        [HttpGet]
        public ActionResult EditPs(int Id)
        {
            Playstation findPsToEdit = PSlist.Where(x => x.ID == Id).FirstOrDefault();
            return View(findPsToEdit);
        }

        //Edit - Posting Action
        [HttpGet]
        public ActionResult EditPs(int Id, Playstation editedPs)
        {
            //Find
            Playstation findPsToEdit = PSlist.Where(x => x.ID == Id).FirstOrDefault();

            //Update
            findPsToEdit = editedPs;

            //Update Alternative
            //findPsToEdit.Name = editedPs.Name;
            //findPsToEdit.Description = editedPs.Description; etc

            //Redirect back 
            return RedirectToAction("PsIndex");
        }


        //Delete View
        [HttpGet]
        public ActionResult DeletePs(int Id)
        {
            Playstation findPsToEdit = PSlist.Where(x => x.ID == Id).FirstOrDefault();
            return View(findPsToEdit);
        }

        //Delete - Posting Action

        [HttpDelete]
        public ActionResult DeletePsInList(int Id)
        {
            Playstation findPsToEdit = PSlist.Where(x => x.ID == Id).FirstOrDefault();
            PSlist.Remove(findPsToEdit);
            return RedirectToAction("PsIndex");
        }


        //All For XB

        //Index Table
        [HttpGet]
        public ActionResult XbIndex()
        {
            return View(Xblist);
        }

        //Create Xbox
        [HttpGet]
        public ActionResult AddXbToStore()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddXbToStore(Xbox sessionsdonenow)
        {
            Xblist.Add(sessionsdonenow);
            return View();
        }





        //Fake Login

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Username, string Password)
        {
            if(Password == SitePassword && Username == SiteUsername)
            {
                return RedirectToAction("AdminDashboard");
            }
            else
            {
                ViewBag.Message = "YOU DONT HAVE ACCESS TO THIS SITE !!!";
                return View();
            }
       
        }

        [HttpGet]
        public ActionResult AdminDashboard()
        {
            return View();
        }


    }
}
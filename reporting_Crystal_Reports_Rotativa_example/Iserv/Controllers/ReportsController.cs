using CrystalDecisions.CrystalReports.Engine;
using Iserv.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Iserv.Models.ChartTest;
using Rotativa;
using System.Data.Entity;

namespace Iserv.Controllers
{
    public class ReportsController : Controller
    {
        readonly InternetServicesEntities db = new InternetServicesEntities();


        public ActionResult Index()
        {
            return View();
        }
        //Consumer Charts
        public JsonResult ConsBarChartData()
        {
            // to get data
            int count1 = db.Consumers.Where(d => d.GenderID == 1).Count();
            int count2 = db.Consumers.Where(d => d.GenderID == 2).Count();
            int count3 = db.Consumers.Where(d => d.GenderID == 3).Count();



            Chart _chart = new Chart();
            _chart.labels = new string[] { "Male", "Female", "Other" };
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Gender Distribution",
                data = new int[] { count1, count2, count3 },
                backgroundColor = new string[] { "800000", "#E9967C", "#FF0000" },
                borderColor = new string[] { "800000", "#E9967C", "#FF0000" },
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }

        // Consumer Reports
        public ActionResult ConsumerList()
        {
            return View(db.Consumers.ToList());
        }


        public ActionResult exportConsumer()
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Consumer.rpt"));
            //rd.SetDataSource(db.Consumers.ToList());
            /*
            rd.SetDataSource(db.Consumers.Select(c => new
            {
                ConsumerID = c.ConsumerID
                //Name = c.Name,
               //Surname = c.Surname,
               // Age = c.Age,
                //Email = c.Email,
                //ID_Number = c.ID_Number.FirstOrDefault(),
                //Cell_Number = c.Cell_Number,
                //Gender = c.Gender


            }).ToList());
            */

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Consumer_list.pdf");
            }
            catch
            {
                throw;
            }
        }

        //Print Consumer Html
        public ActionResult PrintConsumers()
        {

            return new ActionAsPdf("ConsumerList")
            {
                FileName = "ConsumerList.pdf"
            };
        }

        //Get SP charts
        public JsonResult SPBarChartData()
        {
            // to get data
            int count1 = db.ServiceProviders.Where(d => d.GenderID == 1).Count();
            int count2 = db.ServiceProviders.Where(d => d.GenderID == 2).Count();
            int count3 = db.ServiceProviders.Where(d => d.GenderID == 3).Count();



            Chart _chart = new Chart();
            _chart.labels = new string[] { "Male", "Female", "Other" };
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Gender Distribution",
                data = new int[] { count1, count2, count3 },
                backgroundColor = new string[] { "800000", "#E9967C", "#FF0000" },
                borderColor = new string[] { "800000", "#E9967C", "#FF0000" },
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }

        // Service Provider Reports
        public ActionResult ServiceProviderList()
        {
            return View(db.ServiceProviders.ToList());
        }

        public ActionResult exportServiceProvider()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "ServiceProvider.rpt"));
            //rd.SetDataSource(db.ServiceProviders.ToList());
            /*
            rd.SetDataSource(db.Consumers.Select(c => new
            {
                CustomerID = c.ConsumerID
            }).ToList());
            */

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "ServiceProvider_list.pdf");
            }
            catch
            {
                throw;
            }
        }

        //Print Service Provider Html
        public ActionResult PrintServiceProviders()
        {

            return new ActionAsPdf("ServiceProviderList")
            {
                FileName = "Service-Providers-OnBoard.pdf"
            };
        }

        //Admin Reports
        public ActionResult AdminList()
        {
            return View(db.Admins.ToList());
        }

        public JsonResult AdminBarChartData()
        {
            // to get data
            int count1 = db.Admins.Where(d => d.GenderID == 1).Count();
            int count2 = db.Admins.Where(d => d.GenderID == 2).Count();
            int count3 = db.Admins.Where(d => d.GenderID == 3).Count();



            Chart _chart = new Chart();
            _chart.labels = new string[] { "Male", "Female", "Other" };
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Gender Distribution",
                data = new int[] { count1, count2, count3 },
                backgroundColor = new string[] { "800000", "#E9967C", "#FF0000" },
                borderColor = new string[] { "800000", "#E9967C", "#FF0000" },
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintAdmins()
        {
            return new ActionAsPdf("AdminList")
            {
                FileName = "AdminList.pdf"
            };
        }

        //Service Order report
        public ActionResult SOLreport()
        {
            return View(db.ServiceOrderLines.ToList());
        }

        public JsonResult SOLBarChartData()
        {
            // to get data
            int count1 = (int)db.ServiceQuoteLines.Sum(p => p.ItemTotal);
            int count2 = db.Admins.Where(d => d.GenderID == 2).Count();
            int count3 = db.Admins.Where(d => d.GenderID == 3).Count();



            Chart _chart = new Chart();
            _chart.labels = new string[] { "Male", "Female", "Other" };
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Gender Distribution",
                data = new int[] { count1, count2, count3 },
                backgroundColor = new string[] { "800000", "#E9967C", "#FF0000" },
                borderColor = new string[] { "800000", "#E9967C", "#FF0000" },
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }

        // Service quote line stuff
        public ActionResult SQLreport()
        {
            //var pls = new InternetServicesEntities();
            //var x = pls.getSQL();
            return View(db.ServiceQuotes.ToList());
        }


    }
}
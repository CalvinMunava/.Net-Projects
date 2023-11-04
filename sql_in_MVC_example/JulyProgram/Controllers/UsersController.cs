using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JulyProgram.Models; //Remember

namespace JulyProgram.Controllers
{
    public class UsersController : Controller
    {
        //Connection Address
        //1st Craete Connection String 
        public static string connectionstring = "Data Source=SLIMREAPER;Initial Catalog=DbUsers;Integrated Security=True";

        //2nd Instatiate connection to db 
        SqlConnection cnn = new SqlConnection(connectionstring);


        //Declare List to hold objects
        public static List<User> DbUsers = new List<User>();

        // GET: Users
        public ActionResult Index()
        {
            //Reading Data
            try
            {
                DbUsers.Clear();

                //Create Command
                SqlCommand readStudents = new SqlCommand("SELECT * FROM Students", cnn);


                //Open connection

                cnn.Open();

                //Read the data from the command
                SqlDataReader readingStudents = readStudents.ExecuteReader(); // Read Command "Execute Reader"

                //proccess the read data
                while (readingStudents.Read())
                {
                    User foundUser = new User();
                    foundUser.StudentId = (int)readingStudents["StudentId"];
                    foundUser.StudentName = (string)readingStudents["StudentName"];
                    foundUser.StudentDegree = (string)readingStudents["StudentDegree"];
                    foundUser.StudentImage = (string)readingStudents["StudentImage"];
                    DbUsers.Add(foundUser);
                }
            }
            catch (Exception message)
            {
                TempData["ErrorMessage"] = message;
                return RedirectToAction("Error");
            }
            finally
            {
                cnn.Close();
            }
            return View(DbUsers);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User Calvin, HttpPostedFileBase Blob)    
        {
            try
            {
                    //Upload Image
                    Blob.SaveAs(Path.Combine(HttpContext.Server.MapPath("~/Content/Images/"), Blob.FileName));

                    //Craeted a User
                    User NewUser = new User();
                    NewUser.StudentName = Calvin.StudentName;
                    NewUser.StudentDegree = Calvin.StudentDegree;
                    //Added an image location
                    NewUser.StudentImage = "~/Content/Images/" + Blob.FileName;

                    //Insert Command
                    SqlCommand addStudent = new SqlCommand("Insert Into Students values('" + NewUser.StudentName + "','" + NewUser.StudentDegree + "','" + NewUser.StudentImage + "')", cnn);

                    //Open our Connection

                    cnn.Open();

                    //Execute Command
                    addStudent.ExecuteNonQuery(); // Insert Command "Execute NonQuery"

            }
            catch (Exception message)
            {
                TempData["ErrorMessage"] = message;
                return RedirectToAction("Error");
            }
            finally
            {
                cnn.Close();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int studentId)
        {
            //Locate in List --> Lambda
            User FindCalvin;
            try
            {
                FindCalvin = DbUsers.Where(x => x.StudentId == studentId).FirstOrDefault();
                if (FindCalvin == null)
                {
                    TempData["ErrorMessage"] = "Cannot find User";
                    return RedirectToAction("Error");
                }
            }
            catch(Exception message)
            {
                TempData["ErrorMessage"] = message;
                return RedirectToAction("Error");
            }

            return View(FindCalvin);
        }

        [HttpPost]
        public ActionResult Edit(User Calvin, HttpPostedFileBase Blob)
        {
            try
            {
                User FindCalvin = DbUsers.Where(user => user.StudentId == Calvin.StudentId).First();

                if (Blob != null)
                {
                    //Upload Image
                    Blob.SaveAs(Path.Combine(HttpContext.Server.MapPath("~/Content/Images/"), Blob.FileName));
                    FindCalvin.StudentImage = "~/Content/Images/" + Blob.FileName;
                }
                else
                {
                    FindCalvin.StudentImage = FindCalvin.StudentImage;
                }
                FindCalvin.StudentName = Calvin.StudentName;
                FindCalvin.StudentDegree = Calvin.StudentDegree;

                //Update Command
                SqlCommand updateStudent = new SqlCommand("Update Students set StudentName = '" + FindCalvin.StudentName + "', StudentDegree = '" + FindCalvin.StudentDegree + "', StudentImage = '" + FindCalvin.StudentImage + "' where StudentId =" + FindCalvin.StudentId, cnn);
                
                //Open our Connection

                cnn.Open();

                //Execute Command
                updateStudent.ExecuteNonQuery(); // Update Command "Execute NonQuery"

            }
            catch(Exception message)
            {
                TempData["ErrorMessage"] = message;
                return RedirectToAction("Error");
            }
            finally
            {
                cnn.Close();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int studentId)
        {
            //Locate in List --> Lambda      
            try
            {
                User FindCalvin = DbUsers.Where(user => user.StudentId == studentId).First();

                //Delete Command
                SqlCommand deleteStudent = new SqlCommand("Delete from Students where StudentId ="+ studentId, cnn);

                //Open our Connection

                cnn.Open();

                //Execute Command
               deleteStudent.ExecuteNonQuery(); // Delete Command "Execute NonQuery"
            }
            catch (Exception message)
            {
                TempData["ErrorMessage"] = message;
                return RedirectToAction("Error");
            }
            finally
            {
                cnn.Close();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Error()
        {
            ViewBag.Message = TempData["ErrorMessage"];
            return View();
        }

    }
}
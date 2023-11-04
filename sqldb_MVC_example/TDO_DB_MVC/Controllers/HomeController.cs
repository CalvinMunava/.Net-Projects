using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
//add
using TDO_DB_MVC.Models;
using System.IO;
using System.Data;
using System.Security.AccessControl;

namespace TDO_DB_MVC.Controllers
{
    public class HomeController : Controller
    {

        //Connection Address
        public static string connnectionstring = "Data Source=SLIMREAPER21;Initial Catalog=WebStore;Integrated Security=True;";

        //Convert to Sql Connection
        SqlConnection cnn = new SqlConnection(connnectionstring);


        //Example login form
        public ActionResult Login()
        {
            //use to load types
            try
            {
                SqlCommand myReadCommand = new SqlCommand("SELECT * FROM ProductTypes", cnn);
                cnn.Open();
                SqlDataReader dataThatIveRead = myReadCommand.ExecuteReader();
                while (dataThatIveRead.Read())
                {
                    ProductType readType = new ProductType();
                    readType.ID = (int)dataThatIveRead["ID"];
                    readType.TypeName = (string)dataThatIveRead["TypeName"];
                    types.Add(readType);
                }

            }
            catch (Exception ex)
            {
                TempData["Message"] = ex;
            }
            finally
            {
                cnn.Close();
            }

            return View();
        }

        //use to store types
        public List<ProductType> types = new List<ProductType>();


        //R Read Objects from Db
        public ActionResult Home()
        {
            //Use to store Products
             List<Product> products = new List<Product>();

            //create our Welcome message 
            TempData["Message"] = "Welcome Admin :)";

            //use for alert BOX MESSAGE ON VIEW
            if (TempData["Message"] == null)
            {
                ViewBag.Message = "Welcome Admin :)";
            }
            else
            {
                ViewBag.Message = TempData["Message"] as string;
            }
            try
            {
                SqlCommand myReadCommand = new SqlCommand("SELECT * FROM Product", cnn);

                cnn.Open();

                SqlDataReader dataThatIveRead = myReadCommand.ExecuteReader();//important 

                while (dataThatIveRead.Read())
                {
                    Product readProduct = new Product();
                    readProduct.ID = (int)dataThatIveRead["ID"];
                    readProduct.Description = (string)dataThatIveRead["Description"];
                    readProduct.Price = (double)dataThatIveRead["Price"];
                    readProduct.Quantity = (int)dataThatIveRead["Quantity"];
                    readProduct.ImagePath = (string)dataThatIveRead["ImagePath"];
                    readProduct.ProductType = (int)dataThatIveRead["ProductType"];
                    products.Add(readProduct);
                   
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex;
            }
            finally
            {
                cnn.Close();
            }

            //Convert ProductType to string you can see on the view 
            foreach(Product checkproduct in products)
            {
                if (checkproduct.ProductType == 1)
                {
                    checkproduct.ProductTypeText = "CPU";
                }
                if (checkproduct.ProductType == 2)
                {
                    checkproduct.ProductTypeText = "RAM";
                }
                if (checkproduct.ProductType == 3)
                {
                    checkproduct.ProductTypeText = "SSD";
                }
            }


            return View(products);
        }

        //C Create object

        public ActionResult AddProduct()
        {
            try
            {
                SqlCommand myReadCommand = new SqlCommand("SELECT * FROM ProductType", cnn);
                cnn.Open();
                SqlDataReader dataThatIveRead = myReadCommand.ExecuteReader();
                while (dataThatIveRead.Read())
                {
                    ProductType readtype = new ProductType();
                    readtype.ID = (int)dataThatIveRead["ID"];
                    readtype.TypeName = (string)dataThatIveRead["TypeName"];
                    types.Add(readtype);
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex;
            }
            finally
            {
                cnn.Close();
            }


            //shaky part 
            ViewBag.Types = new SelectList(types.Select(c => new { c.ID, c.TypeName }), "ID", "TypeName");

            return View(types);
        }

        [HttpPost]
        public ActionResult AddProducttoDb(string prodDesc, string prodPrice,string prodQuantity ,string ID , HttpPostedFileBase prodBlob)
        {
            //Save Image locally in App Folder
            prodBlob.SaveAs(Path.Combine(HttpContext.Server.MapPath("~/Content/ImageLocation/"), prodBlob.FileName));

            //Get ImagePath for Database
            string Imagepath = "~/Content/ImageLocation/"+ prodBlob.FileName;

            //Save Image in the Database
            System.Drawing.Image imag = System.Drawing.Image.FromStream(prodBlob.InputStream);

            try
            {
                SqlCommand myInsertCommand = new SqlCommand("Insert into Product  Values ('" + prodDesc + "','" + Convert.ToDouble(prodPrice) + "','" + prodQuantity + "', @Pic ,'" +  Imagepath + "','" + Convert.ToInt32(ID) + "')", cnn);
                //Convert image to byte array to store image 
                myInsertCommand.Parameters.Add("Pic", SqlDbType.Image, 0).Value = ConvertImageToByteArray(imag, System.Drawing.Imaging.ImageFormat.Jpeg);
                cnn.Open();
                int RowsChanged = myInsertCommand.ExecuteNonQuery();

                TempData["Message"] = "Success:" + " " + RowsChanged + " " + "rows added.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex;
            }
            finally
            {
                cnn.Close();
            }
            return RedirectToAction("Home");
        }

        //U Update Object

        public ActionResult UpdateProduct(int? Id)
        {
            //use to store types
            List<ProductType> types = new List<ProductType>();

            //use to hold object
            Product readProduct = new Product();

            try
            {

                //First Get Types from Product Types TABLE
     
                SqlCommand readProductTypescommand = new SqlCommand("SELECT * FROM ProductType", cnn);
                cnn.Open();
                SqlDataReader readProductTypes = readProductTypescommand.ExecuteReader();
                while (readProductTypes.Read())
                {
                    ProductType readtype = new ProductType();
                    readtype.ID = (int)readProductTypes["ID"];
                    readtype.TypeName = (string)readProductTypes["TypeName"];
                    types.Add(readtype);
                }

                //close connection 
                cnn.Close();

                //Then get the product 
                SqlCommand readProductcommand = new SqlCommand("SELECT Product.ID , Product.Description , Product.Price, Product.Quantity ,Product.ImageBlob, Product.ImagePath, ProductType FROM Product WHERE ID =" + Id, cnn);
                cnn.Open();
                SqlDataReader readProductfromTable = readProductcommand.ExecuteReader();
                while (readProductfromTable.Read())
                {
                    readProduct.ID = (int)readProductfromTable["ID"];
                    readProduct.Description = (string)readProductfromTable["Description"];
                    readProduct.Price = (double)readProductfromTable["Price"];
                    readProduct.Quantity = (int)readProductfromTable["Quantity"];
                    readProduct.ImageBlob = (byte[])readProductfromTable["ImageBlob"];
                    readProduct.ImagePath = (string)readProductfromTable["ImagePath"];
                    readProduct.ProductType = (int)readProductfromTable["ProductType"];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
            }
            finally
            {
                cnn.Close();
            }

            //Change Product Type Text Property 
            //Convert ProductType to string you can see on the view 
                if (readProduct.ProductType == 1)
                {
                    readProduct.ProductTypeText = "CPU";
                }
                if (readProduct.ProductType == 2)
                {
                    readProduct.ProductTypeText = "RAM";
                }
                if (readProduct.ProductType == 3)
                {
                    readProduct.ProductTypeText = "SSD";
                }
        
            //store in viewbag
            ViewBag.Types = new SelectList(types.Select(c => new { c.ID, c.TypeName }), "ID", "TypeName");

            //Send Products Current ProductType ID to Update Action
            TempData["ID"] = readProduct.ProductType;

            return View(readProduct);
        }

        [HttpPost]
        public ActionResult UpdateProductinDb(string prodID,string prodDesc, string prodPrice, string prodQuantity, string ID, HttpPostedFileBase prodBlob)
        {
            //Recieve SEnt ProductType ID 

           int RecievedProductType = Convert.ToInt32(TempData["ID"]);
            
            //check if ProductType contains Value
            if (ID == "")
            {
                ID = Convert.ToString(RecievedProductType);
            }

            //First Check If Image has been Updated 

            if( prodBlob == null)
            {
                try
                {
                   
                    SqlCommand myUpdateCommand = new SqlCommand("UPDATE Product SET Description = '" + prodDesc + "', Price = '" +Convert.ToDouble(prodPrice) + "',Quantity = '" + Convert.ToDouble(prodQuantity) + "', ProductType = '" + Convert.ToInt32(ID) + "' WHERE ID =" + Convert.ToInt32(prodID), cnn);
                    cnn.Open();
                    int RowsChanged = myUpdateCommand.ExecuteNonQuery();

                    TempData["Message"] = "Successfully Updated :" + " " + RowsChanged + " " + "row";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex;
                }
                finally
                {
                    cnn.Close();
                }
            }
            else
            {
                //Save Image locally in App Folder
                prodBlob.SaveAs(Path.Combine(HttpContext.Server.MapPath("~/Content/ImageLocation/"), prodBlob.FileName));

                //Get ImagePath for Database
                string Imagepath = "~/Content/ImageLocation/" + prodBlob.FileName;

                //Convert Image to drawing
                System.Drawing.Image imag = System.Drawing.Image.FromStream(prodBlob.InputStream);
                try
                {
                    SqlCommand myUpdateCommand = new SqlCommand("UPDATE Product SET Description = '" + prodDesc + "', Price = '" + Convert.ToDouble(prodPrice) + "',Quantity = '" + Convert.ToDouble(prodQuantity) + "',ImageBlob = @Pic , ImagePath = '" + Imagepath + "', ProductType = '" + Convert.ToInt32(ID) + "' WHERE ID =" + Convert.ToInt32(prodID), cnn);
                    //Convert image to byte array to store image 
                    myUpdateCommand.Parameters.Add("Pic", SqlDbType.Image, 0).Value = ConvertImageToByteArray(imag, System.Drawing.Imaging.ImageFormat.Jpeg);
                    cnn.Open();
                    int RowsChanged = myUpdateCommand.ExecuteNonQuery();

                    TempData["Message"] = "Successfully Updated :" + " " + RowsChanged + " " + "row";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex;
                }
                finally
                {
                    cnn.Close();
                }
            }

            return RedirectToAction("Home");
        }

        //D Delete object

        public ActionResult DeleteProduct(int Id)
        {
            try
            {
                SqlCommand myReadCommand = new SqlCommand("DELETE FROM Product WHERE ID =" + Id, cnn);
                cnn.Open();
                myReadCommand.ExecuteNonQuery();
                TempData["Message"] = "Product Successfully deleted :)";

            }
            catch (Exception ex)
            {
                TempData["Message"] = ex;
            }
            finally
            {
                cnn.Close();
            }
            return RedirectToAction("Home");
        }

        // add this method to convert image to byte
        private byte[] ConvertImageToByteArray(System.Drawing.Image imageToConvert, System.Drawing.Imaging.ImageFormat formatOfImage)
        {
            byte[] Ret;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, formatOfImage);
                    Ret = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return Ret;
        }

    }
}
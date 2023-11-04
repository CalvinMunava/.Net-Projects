using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDO_DB_MVC.Models;
using System.Data.SqlClient;


namespace TDO_DB_MVC.Controllers
{
    public class FilterController : Controller
    {
        //Connection Address
        public static string connnectionstring = "Data Source=SLIMREAPER21;Initial Catalog=WebStore;Integrated Security=True;";

        //Convert to Sql Connection
        SqlConnection cnn = new SqlConnection(connnectionstring);

        // GET: Filter

        public static List<Product> products = new List<Product>(); //list to store products

        public void CreateProducts()
        {
            try
            {
                SqlCommand myReadCommand = new SqlCommand("SELECT * FROM Product", cnn);

                cnn.Open();

                SqlDataReader dataThatIveRead = myReadCommand.ExecuteReader();

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
                TempData["MessageFilter"] = ex;
            }
            finally
            {
                cnn.Close();
            }

            //Convert ProductType to string you can see on the view 
            foreach (Product checkproduct in products)
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
            
        } //Method to create products

        public ActionResult Filter()
        {
            if (TempData["MessageFilter"] == null)
            {
                ViewBag.Message = "Welcome Admin :)";
            }
            else
            {
                ViewBag.Message = TempData["MessageFilter"] as string;
            }

            if (products.Count() == 0)
            {
               CreateProducts();
               return View(products);

            }

            return View(products);
        }

        [HttpPost]
        public ActionResult FilterSearchRadio(string Type)
        {
            if (Type == null)
            {
                TempData["ProductsFiltered"] = "yes";
                TempData["MessageFilter"] = "No Radio Filter Selected";
            }
            else
            if (products.Count() == 0)
            {
                CreateProducts();
            }
            else
            {
                products.Clear();

                CreateProducts();

                var FindProducts = products.Where(x => x.ProductTypeText == Type).ToList();

                if (FindProducts == null)
                {
                    TempData["ProductsFiltered"] = "yes";
                    TempData["MessageFilter"] = "No Products found";
                }
                if (Type == "All")
                {
                    TempData["ProductsFiltered"] = "yes";
                    TempData["MessageFilter"] = "Returned All Products";
                }
                else
                {
                    products = FindProducts;
                    TempData["ProductsFiltered"] = "yes";
                    TempData["MessageFilter"] = "Returned" + " " + Type + " " + "Products";

                }

            }

            return RedirectToAction("Filter");

        }

        [HttpPost]
        public ActionResult FilterSearchPrice(string minNum , string maxNum)
        {
            if (products.Count() == 0)
            {
                products.Clear();

                CreateProducts();
            }
            else
            {

                var FindProducts = products.Where(x => x.Price >= Convert.ToDouble(minNum) && x.Price <= Convert.ToDouble(maxNum)).ToList();

                if (FindProducts.Count() == 0)
                {
                    TempData["ProductsFiltered"] = "";
                    TempData["MessageFilter"] = "No Products found matching Condition";
                }
                else
                {
                    products = FindProducts;
                    TempData["ProductsFiltered"] = "yes";
                    TempData["MessageFilter"] = "Products in Range " + " " + minNum + " - " + maxNum + " " + "Products";

                }

            }
            return RedirectToAction("Filter");

        }

        [HttpPost]
        public ActionResult FilterSearchQuantity(string maxQuan)
        {
            if (products.Count() == 0)
            {
                products.Clear();

                CreateProducts();
            }
            else
            {

                var FindProducts = products.Where(x => x.Quantity == Convert.ToInt32(maxQuan)).ToList();

                if (FindProducts.Count() == 0)
                {
                    TempData["ProductsFiltered"] = "";
                    TempData["MessageFilter"] = "No Products found matching Condition";
                }
                else
                {
                    products = FindProducts;
                    TempData["ProductsFiltered"] = "yes";
                    TempData["MessageFilter"] = "Products with Quantity " + " " + maxQuan + " " + "Products";

                }

            }
            return RedirectToAction("Filter");

        }


        /*Use This to filter All*/
        [HttpPost]
        public ActionResult FilterSearchAll(string Type ,string minNum, string maxNum,string maxQuan)
        {
            if (Type == null)
            {
                Type = "All";
            }

            if (products.Count() == 0)
            {
                products.Clear();

                CreateProducts();
            }
            else
            {
                if (Type == "All")
                {
                    var FindProducts = products.Where(x => x.Price >= Convert.ToDouble(minNum) && x.Price <= Convert.ToDouble(maxNum) && x.Quantity == Convert.ToInt32(maxQuan)).ToList();
                }
                else
                {
                    var FindProducts = products.Where(x => x.ProductTypeText == Type && x.Price >= Convert.ToDouble(minNum) && x.Price <= Convert.ToDouble(maxNum) && x.Quantity == Convert.ToInt32(maxQuan)).ToList();

                    if (FindProducts.Count() == 0)
                    {
                        TempData["ProductsFiltered"] = "";
                        TempData["MessageFilter"] = "No Products found matching Condition";
                    }
                    else
                    {
                        products = FindProducts;
                        TempData["ProductsFiltered"] = "yes";
                        TempData["MessageFilter"] = "Products Found";
                    }
                }

            }
            return RedirectToAction("Filter");

        }

    }
}
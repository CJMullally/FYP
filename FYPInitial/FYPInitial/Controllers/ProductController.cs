using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using FYPInitial.Models;

// This controller manages the product CRUD functionality 
namespace FYPInitial.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        // Populates the list on the index page
       
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.ProductNameSortParm = String.IsNullOrEmpty(sortOrder) ? "product_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            List<product> productList = new List<product>();
            using (DBModels dbModel = new DBModels())
            {
                productList = dbModel.products.ToList<product>();
                var products = from s in dbModel.products
                               select s;

                if (!String.IsNullOrEmpty(searchString))
                {
                    products = products.Where(s => s.ProductName.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "product_desc":
                        products = products.OrderByDescending(s => s.ProductName);
                        break;
                    case "price_desc":
                        products = products.OrderByDescending(s => s.Price);
                        break;
                    default:
                        products = products.OrderBy(s => s.ProductName);
                        break;
                }
            return View(products.ToList());
           
            }
            
        }

        // GET: Product/Details/5
        // Returns a view containing product details
        public ActionResult Details(int id)
        {
            product productModel = new product();
            using (DBModels dBModel = new DBModels())
            {
                productModel = dBModel.products.Where(x => x.ProductID == id).FirstOrDefault();
            }
                return View(productModel);
        }

        // GET: Product/Create
        // Returns a view where a new product can be added
        public ActionResult Create()
        {
            return View(new product());
        }

        // POST: Product/Create
        // Takes the details from the product create view and posts them to the server
        [HttpPost]
        public ActionResult Create(product productModel)
        {
            using (DBModels dBModel = new DBModels())
            {
                dBModel.products.Add(productModel);
                dBModel.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Product/Edit/5
        // Returns a view where product details can be edited
        public ActionResult Edit(int id)
        {
            product productModel = new product();
            using (DBModels dBModel = new DBModels())
            {
                productModel = dBModel.products.Where(x => x.ProductID == id).FirstOrDefault();
            }
            return View(productModel);
        }

        // POST: Product/Edit/5
        // Takes the new details from the product edit view and posts them to the server
        [HttpPost]
        public ActionResult Edit(product productModel)
        {
            using (DBModels dbModel = new DBModels())
            {
                dbModel.Entry(productModel).State = System.Data.Entity.EntityState.Modified;
                dbModel.SaveChanges();
            }
            return RedirectToAction("Index");
         }

        // GET: Product/Delete/5
        // Returns a view confirming product deletion
        public ActionResult Delete(int id)
        {
            product productModel = new product();
            using (DBModels dBModel = new DBModels())
            {
                productModel = dBModel.products.Where(x => x.ProductID == id).FirstOrDefault();
            }
            return View(productModel);
        }

        // POST: Product/Delete/5
        // Posts confirmation of product deletion to the server and deletes the product
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (DBModels dBModel = new DBModels())
            {
                product productModel = dBModel.products.Where(x => x.ProductID == id).FirstOrDefault();
                dBModel.products.Remove(productModel);
                dBModel.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}

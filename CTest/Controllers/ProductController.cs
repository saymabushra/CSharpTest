using CTest.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Xml;

namespace CTest.Controllers
{
    public class ProductController : Controller
    {
        List<Product> products = new List<Product>();
        List<ProductDetail> productDetails = new List<ProductDetail>();

        public ActionResult Index()
        {
            try
            {
                ProductList();

                return View(products.ToList());
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return View();
            }
        }

        private void ProductList()
        {
            //Load the XML file in XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("~/Data/List.xml"));

            //Loop through the selected Nodes.
            foreach (XmlNode node in doc.SelectNodes("/Store/Products/Product"))
            {
                Product product = new Product
                {
                    //Fetch the Node values and assign it to Model.
                    Id = node.Attributes.GetNamedItem("id").Value,
                    Title = node["Title"].InnerText,
                    Description = node["Description"].InnerText,
                    Image = node["Image"].InnerText,
                    Price = Convert.ToDouble(node["Price"].InnerText, CultureInfo.InvariantCulture)
                };
                //product.Popular = node["Sorting"].InnerText;

                Sorting sorting = new Sorting
                {
                    Popular = node["Sorting"].InnerText
                };

                ProductDetails();
                var availability = productDetails.Where(x => x.Id == product.Id).FirstOrDefault();
                product.Availability = availability.Availability;

                product.Sorting = sorting;
                products.Add(product);
            }
        }

        public ActionResult Availability(string productId)
        {
            try
            {
                ProductDetails();

                var availability = productDetails.Where(x => x.Id == productId).FirstOrDefault();
                string strValue = availability.Availability;

                return Json(strValue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return View();
            }
        }

        private void ProductDetails()
        {
            //Load the XML file in XmlDocument.
            XmlDocument doc1 = new XmlDocument();
            doc1.Load(Server.MapPath("~/Data/Detail.xml"));

            //Loop through the selected Nodes.
            foreach (XmlNode node in doc1.SelectNodes("/Store/Products/Product"))
            {
                ProductDetail productDetail = new ProductDetail
                {
                    //Fetch the Node values and assign it to Model.
                    Id = node.Attributes.GetNamedItem("id").Value,
                    Title = node["Title"].InnerText,
                    Description = node["Description"].InnerText,
                    Image = node["Image"].InnerText,
                    Availability = node["Availability"].InnerText
                };

                List<Specs> specs = new List<Specs>();
                foreach (XmlNode xmlNode in node["Specs"])
                {
                    Specs spec = new Specs
                    {
                        Spec = xmlNode.InnerText
                    };
                    specs.Add(spec);
                }

                productDetail.Specs = specs;
                productDetails.Add(productDetail);
            }
        }

        public ActionResult Filter(string value, string type)
        {
            try
            {
                ProductList();

                if (type == "asc")
                {
                    if (value == "price")
                    {
                        var model = products.OrderBy(x => x.Price);
                        return PartialView("_Filter", model.ToList());
                        //return RedirectToAction("Index", model.ToList());
                    }
                    else if (value == "popularity")
                    {
                        var model = products.OrderBy(x => x.Sorting.Popular);
                        return PartialView("_Filter", model.ToList());
                    }
                    else
                    {
                        return PartialView("_Filter", products.ToList());
                    }
                }
                else if (type == "desc")
                {
                    if (value == "price")
                    {
                        var model = products.OrderByDescending(x => x.Price);
                        return PartialView("_Filter", model.ToList());
                    }
                    else if (value == "popularity")
                    {
                        var model = products.OrderByDescending(x => x.Sorting.Popular);
                        return PartialView("_Filter", model.ToList());
                    }
                    else
                    {
                        return PartialView("_Filter", products.ToList());
                    }
                }
                else
                {
                    return PartialView("_Filter", products.ToList());
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
        }

        public ActionResult Details(string productId)
        {
            ProductDetails();

            var model = productDetails.Where(x => x.Id == productId);

            return View(model.ToList());
        }

        public ActionResult Price(string productId)
        {
            try
            {
                ProductList();

                var availability = products.Where(x => x.Id == productId).FirstOrDefault();
                double strValue = availability.Price;

                return Json(strValue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return View();
            }
        }
    }
}
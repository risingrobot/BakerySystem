using BakerySystem.Models;
using Newtonsoft.Json;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BakerySystem.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Getorder(int id)
        {
            List<GetOrderDetail> bkryList = new List<GetOrderDetail>();
            List<BKRY_ITEMS> lstBKRY_ITEMS = new List<BKRY_ITEMS>();
            List<GetOrderDetail2> bkrList2 = new List<GetOrderDetail2>();
            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                bkryList = db.GetOrderDetails.Where(x => x.OrderId == id).ToList<GetOrderDetail>();
                foreach (var item in bkryList)
                {
                    item.OrderDetails = null;
                    item.address = item.address + " " + item.street + " " + item.postCode;
                    GetOrderDetail2 st = new GetOrderDetail2
                    {
                        add_dtee = Convert.ToDateTime(item.Orderon).ToLongDateString(),
                        OrderId = item.OrderId,
                        Orderon = item.Orderon,
                        personname = item.personname,
                        email  =item.email,
                        address = item.address,
                        Delivered = item.Delivered
    };
                    bkrList2.Add(st);
                }
                return Json(new { data = bkrList2 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetorderDetails(int id)
        {
            List<GetOrderDetail> bkryList = new List<GetOrderDetail>();
            List<BKRY_ITEMS> lstBKRY_ITEMS = new List<BKRY_ITEMS>();

            using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
            {
                bkryList = db.GetOrderDetails.Where(x => x.OrderId == id).ToList<GetOrderDetail>();
                foreach (var item in bkryList)
                {
                    lstBKRY_ITEMS= Newtonsoft.Json.JsonConvert.DeserializeObject<List<BKRY_ITEMS>>(item.OrderDetails);
                    item.address = item.address + " " + item.street + " " + item.postCode;
                }
                return PartialView("~/Views/Order/OrderDetails.cshtml", lstBKRY_ITEMS);
            }
        }
        public FileResult PrintDetails(int id)
        {
            string pdfFilename = "";
            string line = "";
            try
            {
                List<GetOrderDetail> bkryList = new List<GetOrderDetail>();
                List<BKRY_ITEMS> lstBKRY_ITEMS = new List<BKRY_ITEMS>();
                List<GetOrderDetail2> bkrList2 = new List<GetOrderDetail2>();
                using (BKRY_MNGT_SYSEntities db = new BKRY_MNGT_SYSEntities())
                {
                    bkryList = db.GetOrderDetails.Where(x => x.OrderId == id).ToList<GetOrderDetail>();
                    foreach (var item in bkryList)
                    {
                        line += "Bakery System " + " \n";
                        line += "*****************************************************" + " \n";
                        line += "Order # : " + item.OrderId + " \n";
                        line += "Person Name : " + item.personname + " \n";
                        line += "Address : " + item.address + " \n";
                        line += "Street : " + item.street + " \n";
                        line += "PostCode : " + item.postCode + " \n";
                        line += "Email  : " + item.email + " \n";
                        line += "Order On  : " + Convert.ToDateTime(item.Orderon).ToLongDateString() + " \n";
                        line += "*****************************************************" + " \n";
                        line += "Item Details" + "\n";
                        decimal pp = 0;
                        foreach (var Oitem in JsonConvert.DeserializeObject<List<BKRY_ITEMS>>(item.OrderDetails))
                        {                            
                            line += "Product : " + Oitem.name + " Quantity : " + Oitem.quantity + " Price : " + Oitem.price + " \n";                           
                            pp += (Convert.ToDecimal(Oitem.price) * Convert.ToDecimal(Oitem.quantity));
                        }
                        line += "*****************************************************" + " \n";
                        line += "Total : " + pp + " \n ";
                    }
                }               
                int yPoint = 0;
                PdfDocument pdf = new PdfDocument();
                pdf.Info.Title = "Bakery System Order No #"+ id;
                PdfPage pdfPage = pdf.AddPage();
                XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                XFont font = new XFont("Verdana", 8, XFontStyle.Regular);
                //graph.DrawString(line, font, XBrushes.Black, pdfPage.Width.Point, pdfPage.Height.Point);
                foreach (var item in line.Split('\n'))
                {
                    graph.DrawString(item, font, XBrushes.Black, new XRect(40, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    yPoint = yPoint + 10;
                }                
             
                
                pdfFilename = Server.MapPath("~/Temp/txttopdf"+ DateTime.Now.ToString("mmddyyyyhhMMss") + ".pdf");
                pdf.Save(pdfFilename);            
                               
            }
            catch (Exception ex)
            {
               
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(pdfFilename);
            string fileName = "txttopdf"+ DateTime.Now.ToString("mmddyyyyhhMMss") + ".pdf";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);           
        }

    }
}
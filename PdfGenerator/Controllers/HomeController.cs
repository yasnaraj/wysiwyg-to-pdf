using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jsreport.AspNetCore;
using jsreport.Types;
using Microsoft.AspNetCore.Mvc;
using PdfGenerator.Models;

namespace PdfGenerator.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            return View();
        }

        [HttpPost]
        public ActionResult Index(HomeViewModel model)
        {
            return View(model);
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult GeneratePdf([FromBody] string pdfContents)
        {
            var htmlTop = @"<!DOCTYPE html><html><head><meta charset = ""utf-8""/>" +
                        "<title>PdfGenerator</title>" +
                        @"<link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"" integrity=""sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u"" crossorigin=""anonymous"">" +
                        @"</head><body style=""font-size: 20px"">";

            var htmlBottom = "</body></html>";

            pdfContents = htmlTop + pdfContents + htmlBottom;

            byte[] byteVal = Encoding.ASCII.GetBytes(pdfContents);
            var contentDisposition = "attachment; filename=\"myReport.pdf\"";

            HttpContext.Response.ContentType = "text/html; charset=utf-8";

            HttpContext.Response.Body.Write(byteVal, 0, byteVal.Length);

            HttpContext.JsReportFeature().Recipe(Recipe.PhantomPdf)
                .OnAfterRender((r) => HttpContext.Response.Headers["Content-Disposition"] = contentDisposition);

            return new ContentResult();

        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

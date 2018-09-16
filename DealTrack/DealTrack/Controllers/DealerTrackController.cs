using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using DealTrack.IServices;
using DealTrack.ViewModels;

namespace DealTrack.Controllers
{
    public class DealerTrackController : Controller
    {
        private ICsvFileService _csvService;


        public DealerTrackController(ICsvFileService csvService)
        {
            this._csvService = csvService;
        }
        public ActionResult Index()
        {
            return View(new List<DealerTrackIndexVM>());
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {

            List<DealerTrackIndexVM> dealTrackModel = new List<DealerTrackIndexVM>();

            if (ModelState.IsValid)
            {
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    string filePath = string.Empty;
                    if (postedFile != null)
                    {
                        string extension = Path.GetExtension(postedFile.FileName);

                        if (extension != ".csv")
                        {
                            ViewBag.Error = "File format is not supported";
                            return View(dealTrackModel);
                        }
                        string path = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        filePath = path + Path.GetFileName(postedFile.FileName);

                        if (System.IO.File.Exists(filePath + extension))
                        {
                            System.IO.File.Delete(filePath + extension);
                        }

                        postedFile.SaveAs(filePath);

                        DataTable dt = _csvService.ReadCsvFile(filePath);

                        foreach (DataRow row in dt.Rows)
                        {
                            DealerTrackIndexVM deal = new DealerTrackIndexVM();

                            deal.DealNumber = Convert.ToInt32(row["DealNumber"]);
                            deal.CustomerName = Convert.ToString(row["CustomerName"]);
                            deal.DealershipName = Convert.ToString(row["DealershipName"]);
                            deal.Vehicle = Convert.ToString(row["Vehicle"]);

                            string strPrice = row["Price"].ToString().Replace("\"", "");

                            if (int.TryParse(strPrice, NumberStyles.AllowThousands,
                                             CultureInfo.InvariantCulture, out int PriceNum))
                            {
                                deal.Price = Convert.ToInt32(PriceNum);
                            }

                            if (DateTime.TryParse(row["Date"].ToString(), out DateTime DealDate))
                            {
                                deal.Date = Convert.ToDateTime(DealDate);
                            }

                            dealTrackModel.Add(deal);
                        }
                    }
                }

                else
                {
                    ViewBag.Error = "Please Upload Your file";
                }
            }

            ViewBag.PopularModel = dealTrackModel.GroupBy(x => x.Vehicle)
                                       .Where(g => g.Count() > 1)
                                       .Select(y => y.Key).OrderByDescending(g => g.Count())
                                       .ToList().FirstOrDefault();


            return View(dealTrackModel);

        }

    }
}
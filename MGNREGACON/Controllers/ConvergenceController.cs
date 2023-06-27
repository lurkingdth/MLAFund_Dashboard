using MLAFund.Models;
using MLAFund.Models.Data;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI;
using System.Runtime.Remoting.Contexts;
using System.Web.Routing;

namespace Grievance.Controllers
{
    public class ConvergenceController : Controller
    {

        public RuralWorksData data = new RuralWorksData();
        // GET: CompletedScheme

        public string result;

        public ActionResult Create()
        {
            var user = User.Identity.Name;
            var user_data = User.UserData();
            //ViewData["userid"] = user;
            return View();
        }

        public ActionResult UploadFiles()
        {
            string fname = "";

            if (Request.Files.Count > 0)
            {
                try
                {
                    string txtname = Request.Files["txtname"].ToString();

                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {

                        HttpPostedFileBase file = files[i];

                        string extension = Path.GetExtension(file.FileName);

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {

                            fname = txtname.Trim() + file.FileName;
                        }
                        else
                        {
                            fname = txtname.Trim() + file.FileName;
                        }
                        var dir = Server.MapPath("~/Content/convergence/");
                        DirectoryInfo info = new DirectoryInfo(dir);
                        if (!info.Exists) info.Create();
                        var fpathAndfile = Path.Combine(dir, fname);
                        file.SaveAs(fpathAndfile);

                    }

                    return Json(new { status = "OK", Message = "File Uploaded Successfully!", file = "~/Content/convergence/" + fname });
                }
                catch (Exception ex)
                {
                    return Json(new { status = "failed", Message = "Error occurred. Error details: " + ex.Message });
                }
            }
            else
            {
                return Json(new { status = "Failed", Message = "No files selected." });
            }
        }

       
        public ActionResult Index()
        {
            var result = data.convergence.ToList();
            return View(result);
        }

        public ActionResult WorkProgress()
        {
            //Nishant - Work Progress Entry - Start
            //Comments : This would fetch the record from the workprogress table where
            //           for the user which is logged in.
            var userLoggedIn = User.Identity.Name;
            var result = data.workprogress
                .OrderByDescending(y => y.date)
                .Where(i => i.submittedby == userLoggedIn)
                .ToList()
                .FirstOrDefault();
            return View(result);

            //Nishant - Work Progress Entry - End
        }

        
     
        public ActionResult MLAFund_Utilization_Report()
        {
            var dis = User.GetUserDistrict();
            var userLoggedIn = User.Identity.Name;

            //var selected_district = sno;

            
            var assembly_name = data.tbl_Users
                .Where(x => x.username == userLoggedIn)
                .Select(o => o.assembly_name).SingleOrDefault();


            if (dis == "State")
            {

                var result = data.mla_expenses
                    .ToList()
                    .AsQueryable();

                result = result.OrderBy(k => k.assembly_name);
                return View(result);


            }

            else
            {
                var result = data.mla_expenses
                .Where(i => i.assembly_name == assembly_name)
                .ToList()
                .AsQueryable();
                result = result.OrderBy(k => k.financial_year);
                return View(result);

            }

        }

        //Nishant - Added Report for Selecting Completed Convergence Records
        public ActionResult CompleteConvergenceReport()
        {

            var dis = User.GetUserDistrict();
            var userLoggedIn = User.Identity.Name;
            var user_dpt_name = data.tbl_Users
                .Where(x => x.username == userLoggedIn);
               // .Select(o => o.dpt_name).SingleOrDefault();

            if (dis == "State")
            {
                var result = data.convergence.Include(o => o.convergencedpt)
               .Where(i => i.status == "complete")
               .ToList()
               .AsQueryable();

                result = result.OrderBy(k => k.cid);

                return View(result);

            }
            else
            {
                var result = data.convergence.Include(o => o.convergencedpt)
                .Where(i => i.district == dis && i.status == "complete")
                .ToList()
                .AsQueryable();

                result = result.OrderBy(k => k.cid);
                return View(result);
            }


        }

        //Nishant - Added code for saving Convergence Fund details from Add Convergence Fund PopUp - Start
        [HttpPost]
        public ActionResult AddConvergenceFund(int popup_cid, int popup_convergenceFund, string popup_convergenceDeptName, string popup_convergenceDeptScheme)
        {
            //Selecting the record from the ConvergenceDpt table
            var updateConvergenceDpt = data.convergencedpt.Where(x => x.cid == popup_cid && x.convergencedptname == popup_convergenceDeptName
            && x.convergencedptscheme == popup_convergenceDeptScheme).FirstOrDefault();
            updateConvergenceDpt.convergencefund = popup_convergenceFund;
            data.Entry(updateConvergenceDpt).State = System.Data.Entity.EntityState.Modified;
            data.SaveChanges();

            //Selecting the record from Convergence table to update the status to complete
            var updateConvergence = data.convergence.Where(x => x.cid == popup_cid).FirstOrDefault();
            updateConvergence.status = "Complete";
            data.Entry(updateConvergence).State = System.Data.Entity.EntityState.Modified;
            data.SaveChanges();


            return Json(new { success = true, responseText = "Saved successfuly ", responseid = 1 }, JsonRequestBehavior.AllowGet);

        }
        //Nishant - Added code for saving Convergence Fund details from Add Convergence Fund PopUp - End

        public ActionResult dptReport()
        {
            var userLoggedIn = User.Identity.Name;
            var result = data.workprogress
                .Where(i => i.submittedby == userLoggedIn)
                .ToList();
            return View(result);
        }

        //Nishant - State Department Report
        //Comments: This would genereate a report for the Departments and the number of Pending Convergence Cases.
        
        public ActionResult stateDptReport()
        {

            var result = data.convergence
                        .Join(data.convergencedpt, converge => converge.cid, convergedpt => convergedpt.cid, (converge, convergedpt) => new { converge, convergedpt })
                        //.Where(o =>  o.converge.status == "pending")
                        .GroupBy(x => new { x.convergedpt.convergencedptname })
                        .Select(g => new stateDeptReport()
                        {
                            convergenceDptName = g.Key.convergencedptname,
                            pending = g.Sum(t=>t.converge.status == "Pending" ?1:0  ),
                            completed = g.Sum(t => t.converge.status == "Complete" ? 1 : 0),
                        }).ToList();

            return View(result);
        }

        public ActionResult WorkProgressReport()
        {
            var dis = User.GetUserDistrict();
            var userLoggedIn = User.Identity.Name;
            var user_dpt_name = data.tbl_Users
                .Where(x => x.username == userLoggedIn);
                //.Select(o => o.dpt_name).SingleOrDefault();

            if (dis == "State")
            {
                var result = data.convergence
                    .Join(data.workprogress_app, converge => converge.cid, wrkprg => wrkprg.cid, (converge, wrkprg) => new { converge, wrkprg })
                    .Select(g => new wrkPrgReport()
                    {
                        district = g.converge.district,
                        block= g.converge.block,
                        panchayat=g.converge.panchayat,
                        village=g.converge.village,
                        nameofscheme=g.converge.nameofscheme,
                        fundofdpt=g.converge.fundofdpt,
                        fundofmgnrega=g.converge.fundofmgnrega,
                        latitude=g.wrkprg.latitude,
                        longitude=g.wrkprg.longitude,
                        photo1 = g.wrkprg.photo1,
                        photo2 = g.wrkprg.photo2,
                        photo3 = g.wrkprg.photo3,
                        address =g.wrkprg.address,
                    }).ToList();
                return View(result);

            }
            else
            {
               return RedirectToAction("Index","Home");
            }

        }

    }
}
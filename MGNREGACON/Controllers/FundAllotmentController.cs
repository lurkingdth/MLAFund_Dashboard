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
    public class FundAllotmentController : Controller
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

        public ActionResult CreateExpense(string financial_year)
        {
            ViewBag.financial_year = financial_year;
            return View();
        }

        public ActionResult MLAFundAllotmentReport()
        {
            //Nishant - Fund Allotment Report - Start
            //Comments : This would fetch the record from the fund Allotment table where
            //           for the user which is logged in.
            //          This would only work for State login.

            var dis = User.GetUserDistrict();
            if(dis == "State") {
                var result = data.fund_allotment
                    .OrderByDescending(y => y.fundallotmentid)
                    .ToList()
                    .AsQueryable();
                return View(result);
            }
            return View();

            //Nishant - Fund Allotment Report - End
        }


        public ActionResult MLAFAssemblyExpenseReport()
        {
            var dis = User.GetUserDistrict();
            if (dis == "State")
            {
                var result = data.mla_expenses
                      .GroupBy(x => new { x.assembly_name, x.financial_year })
                      .Select(g => new mla_expenses_temp()
                      {
                          assembly_name = g.Key.assembly_name,
                          financial_year = g.Key.financial_year,
                          total_expense_amount = g.Sum(t => t.expense_amount),
                      }).ToList();
                return View(result);
            }
           
                return View(result);

        }

        public ActionResult MLAFund_Utilization_Report(string financial_year)
        {
            var dis = User.GetUserDistrict();
            var userLoggedIn = User.Identity.Name;

            //var selected_district = sno;


            var assembly_name = data.tbl_Users
                .Where(x => x.username == userLoggedIn)
                .Select(o => o.assembly_name).SingleOrDefault();

            var mla_name = data.tbl_Users
                            .Where(x => x.assembly_name == assembly_name)
                             .Select(o => o.mla_name).SingleOrDefault();

            ViewBag.AssemblyName = assembly_name;
            ViewBag.MLAName = mla_name;

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
                .Where(i => i.assembly_name == assembly_name && i.financial_year == financial_year)
                .ToList()
                .AsQueryable();
                result = result.OrderBy(k => k.financial_year);
                return View(result);
            }

        }

        //Nishant - Added code for Expense details report for State - Code Start
        public ActionResult DetailedExpenseReport(string assembly_name, string financial_year)
        {
            var dis = User.GetUserDistrict();
            if(dis == "State")
            {
                var result = data.mla_expenses
                    .Where(x =>x.assembly_name == assembly_name && x.financial_year == financial_year)
                    .OrderBy(y =>y.expensesid)
                    .ToList()
                    .AsQueryable();

                return View(result);
            }

            return View();  

        }
        //Nishant - Added code for Expense details report for State - Code End


        public ActionResult Assembly_FundAllotment()
        {
            var userLoggedIn = User.Identity.Name;

            var assembly_name = data.tbl_Users
                .Where(x => x.username == userLoggedIn)
                .Select(o => o.assembly_name).SingleOrDefault();

            IQueryable<fund_allotment> result = data.fund_allotment
                .Where(x => x.assembly_name == assembly_name)
                .OrderBy(y => y.financial_year)
                .ToList()
                .AsQueryable();

            return View(result);
        }

        public void updateTotalExpenditure(string financial_year, string assembly_name)
        {
            var temp_data = false;
            if (!temp_data)
            {
                var total_expense = data.mla_expenses
                    .Where(x => x.assembly_name == assembly_name && x.financial_year == financial_year)
                    .Sum(x => x.expense_amount).ToString();
                //select the record from Fund Allocated table
                var record = data.fund_allotment.Where(x => x.assembly_name == assembly_name && x.financial_year == financial_year).FirstOrDefault();
                record.total_expenditure = int.Parse(total_expense);
                data.Entry(record).State = EntityState.Modified;
                data.SaveChanges();
            }
        }


        //Nishant - Added code for Uploading Expenses doc  : Code Start
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
                        var dir = Server.MapPath("~/Content/FundAllotedDocs/");
                        DirectoryInfo info = new DirectoryInfo(dir);
                        if (!info.Exists) info.Create();
                        var fpathAndfile = Path.Combine(dir, fname);
                        file.SaveAs(fpathAndfile);

                    }

                    return Json(new { status = "OK", Message = "File Uploaded Successfully!", file = "~/Content/FundAllotedDocs/" + fname });
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
        //Nishant - Added code for Uploading Expenses doc  : Code End


    }
}
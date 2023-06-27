using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using MLAFund.Models;
using MLAFund.Models.Data;
using Microsoft.Ajax.Utilities;

namespace MLAFund.Controllers
{
    public class HomeController : Controller
    {
        private Models.Data.RuralWorksData data = new Models.Data.RuralWorksData();
        [Authorize]
        public ActionResult Index()
        {
            var dis = User.GetUserDistrict();
            var userLoggedIn = User.Identity.Name;
            var user_assembly_name = data.tbl_Users
                .Where(x => x.username == userLoggedIn)
                .Select(o => o.assembly_name).SingleOrDefault();

            var current_year = DateTime.Now.Year;

            // ViewBag.tot_roadcompleted_scheme = data.oldschems.Where(x => x.status=="").Count().ToString();
            if (dis == "State")
            {

                //Nishant - Getting Active/Pending Count - Start
                // This function would count the number of Active Schemes and Pending Schemes
                // from tbl.fund_details and return the count
                // Input: User.Identity.Name
                // Output: Count of records where status = Active
                //          Count of records where status = Pending

                ViewBag.fundAlloted = data.fund_details.Where(x => x.status == "Active").Count().ToString();
                ViewBag.fundUtilized = data.fund_details.Where(x => x.status == "Pending").Count().ToString();

                /**Calling the Function to call Chart data**/
                ViewBag.chartData = getChartData();

                //Nishant - Getting Active/Pending Count - End

            }
            else
            {
                //Nishant - Getting Active/Pending Count - Start
                // This function would count the number of Active Schemes and Pending Schemes
                // from tbl.fund_details and return the count where Assembly_name is not State
                // Input: User.Identity.Name
                // Output: Count of records where status = Active
                //          Count of records where status = Pending

                ViewBag.fundAlloted = 0;
                ViewBag.fundUtilized = 0;

                //var fundAlloted = data.fund_allotment.Where(x => x.assembly_name == user_assembly_name).Select(o => o.allotted_fund).SingleOrDefault();

                var tempAllotment = checkAllotment();
                if (tempAllotment != 0)
                {
                    var allotment = data.fund_allotment
                    .Where(x => x.assembly_name == user_assembly_name && x.financial_year.StartsWith(current_year.ToString()))
                    .Select(o => o.allotted_fund).SingleOrDefault();

                    ViewBag.fundAlloted = allotment.ToString();

                }
                else
                {
                    ViewBag.fundAlloted = 0;
                }

                var tempExpense = checkExpenditure();
                

                if (tempExpense != 0)
                {
                    var total_expense = data.mla_expenses
                    .Where(x => x.assembly_name == user_assembly_name && x.financial_year.StartsWith(current_year.ToString()))
                    .Sum(x => x.expense_amount).ToString();
                    ViewBag.fundUtilized = total_expense.ToString();
                }
                else
                {
                    ViewBag.fundUtilized = 0;

                }



                //Nishant - Getting Active/Pending Count - End

                //ViewBag.pendingCases = data.convergence.Where(x=>x.status == "pending" && x.district == dis).Count().ToString();

                /** ViewBag.pendingCases = (from converge in data.convergence
                                         join convergedpt in data.convergencedpt on converge.cid equals convergedpt.cid
                                         where (converge.district == dis && converge.status == "pending" && convergedpt.convergencedptname == user_dpt_name)
                                         select new { converge }).Distinct().Count().ToString();

                ViewBag.completedCases = (from converge in data.convergence
                                           join convergedpt in data.convergencedpt on converge.cid equals convergedpt.cid
                                           where (converge.district == dis && converge.status == "complete" && convergedpt.convergencedptname == user_dpt_name)
                                           select new { converge }).Distinct().Count().ToString(); **/

                /**Calling the Function to call Chart data**/
                ViewBag.chartData =  getChartData();

            }
            return View();
        }
        // Function to Get Chart data

        public List<chartData> getChartData()
        {
            List<int> activeSchemes = new List<int>();
            List<int> pendingSchemes= new List<int>();
            List<string> assembly_name = new List<string>();

            /**The below commented code puts the data to Charts.**/
            var result = data.fund_details
                        .GroupBy(x => new { x.assembly_name })
                        .Select(g => new chartData()
                        {
                            assembly_name = g.Key.assembly_name,
                            pendingSchemes = g.Sum(t => t.status == "Pending" ? 1 : 0),
                            activeSchemes = g.Sum(t => t.status == "Active" ? 1 : 0),
                        }).ToList();

            foreach(var item in result)
            {
                //Check here tommorow
                activeSchemes.Add(item.activeSchemes);
                pendingSchemes.Add(item.pendingSchemes);
                assembly_name.Add(item.assembly_name);
            }

            ViewBag.Completed = activeSchemes.ToList() ;
            ViewBag.Pending = pendingSchemes.ToList();
            ViewBag.AssemblyName = assembly_name.ToList() ;

            return result;
        }

        public List<mla_expenses_temp> getChartDataTest()
        {
            List<string> assembly_name = new List<string>();
            List<string> financial_year = new List<string>();
            List<int> total_expense_amount = new List<int>();

            /**The below commented code puts the data to Charts.**/
            var result = data.mla_expenses
                        .GroupBy(x => new { x.assembly_name, x.financial_year })
                        .Select(g => new mla_expenses_temp()
                        {
                            assembly_name = g.Key.assembly_name,
                            financial_year = g.Key.financial_year,
                            total_expense_amount = g.Sum(t => t.expense_amount),
                        }).ToList();

            foreach (var item in result)
            {
                //Check here tommorow
                assembly_name.Add(item.assembly_name);
                financial_year.Add(item.financial_year);
                total_expense_amount.Add(item.total_expense_amount);
            }

            ViewBag.Completed = financial_year.ToList();
            ViewBag.Pending = total_expense_amount.ToList();
            ViewBag.AssemblyName = assembly_name.ToList();

            return result;
        }

        //Nishant - Added ActionMethod for Add NewUser screen - Start

        public ActionResult AddNewUser()
        { return View(); }
        //Nishant - Added ActionMethod for Add NewUser screen - End

        //Nishant - Code Start - Check if any Expense
        //To check if there are any expenditure for the current year 
        //Must return 0 or the SUM of expenditure

        public int checkExpenditure()
        {
            var userLoggedIn = User.Identity.Name;
            var user_assembly_name = data.tbl_Users
                .Where(x => x.username == userLoggedIn)
                .Select(o => o.assembly_name).SingleOrDefault();

            var current_year = DateTime.Now.Year.ToString();

            var expenditureCount = data.mla_expenses
                .Where(x => x.assembly_name == user_assembly_name && x.financial_year.StartsWith(current_year)).Count();

            return expenditureCount;

        }

        //Nishant - Code End - Check if any Expense

        public int checkAllotment()
        {
            var userLoggedIn = User.Identity.Name;
            var user_assembly_name = data.tbl_Users
                .Where(x => x.username == userLoggedIn)
                .Select(o => o.assembly_name).SingleOrDefault();

            var current_year = DateTime.Now.Year.ToString();

            var allotmentCount = data.fund_allotment
                .Where(x=>x.assembly_name == user_assembly_name && x.financial_year.StartsWith(current_year)).Count();

            return allotmentCount;

        }
    }
}
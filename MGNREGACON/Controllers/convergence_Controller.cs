using MLAFund.Models;
using MLAFund.Models.Data;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using System.Web.Http.Description;

namespace MLAFund.Controllers
{
    public class grievance_Controller : ApiController
    {
        //private Models.Data.RuralWorksData data = new Models.Data.RuralWorksData();
        private Models.Data.RuralWorksData data = new Models.Data.RuralWorksData();

        //Nishant - Added code for getting Entire tbl_Users table - Code Start
        [HttpGet]
        [Route("api/user_api")]
        public IQueryable<tbl_Users> Getuser_tbl()
        {
            return data.tbl_Users;
        }
        //Nishant - Added code for getting Entire tbl_Users table - Code End


        [HttpPost]
        [Route("api/Convergence/Save")]
        public object Save(convergence model)
        {
            data.Configuration.LazyLoadingEnabled = false;
            data.Configuration.ProxyCreationEnabled = false;
            //model.status = "Pending for compliance";
            try
            {
                // new
                if (model.cid == 0)
                {
                    //model.status
                 
                    data.convergence.Add(model);
                }
                else
                {
                    //Edit
                }
                data.SaveChanges();
                return new { status = "OK", payload = model, Message = "Data Saved" };
            }
            catch (Exception ex)
            {
                return new { status = "Failed", payload = model, Message = "Data Saved failed : " + ex.Message };
            }
        }

        //Nishant - Added code for Saving data to WorkProgress table - Start
        // Api to save the workprogress data to the workprogress table
        [HttpPost]
        [Route("api/WorkProgress/Save")]
        public object SaveWorkProgress(workprogress model)
        {
            data.Configuration.LazyLoadingEnabled = false;
            data.Configuration.ProxyCreationEnabled = false;
            //model.status = "Pending for compliance";
            try
            {
                // new
                if (model.cid == 0)
                {
                    //model.status

                    data.workprogress.Add(model);
                }
                else
                {
                    //Edit
                }
                data.SaveChanges();
                return new { status = "OK", payload = model, Message = "Data Saved" };
            }
            catch (Exception ex)
            {
                return new { status = "Failed", payload = model, Message = "Data Saved failed : " + ex.Message };
            }
        }

        //Nishant - Added code for Saving data to WorkProgress table - End

        //Nishant - Added code for Saving data to WorkProgress_App table - Start
        // Api to save the workprogress data to the workprogress_App table
        [HttpPost]
        [Route("api/WorkProgressApp/Save")]
        public object SaveWorkProgressApp(workprogress_app model)
        {
            data.Configuration.LazyLoadingEnabled = false;
            data.Configuration.ProxyCreationEnabled = false;
            //model.status = "Pending for compliance";
            try
            {
                // new
                if (model.cid == 0)
                {
                    //model.status

                    data.workprogress_app.Add(model);
                }
                else
                {
                    //Edit
                }
                data.SaveChanges();
                return new { status = "OK", payload = model, Message = "Data Saved" };
            }
            catch (Exception ex)
            {
                return new { status = "Failed", payload = model, Message = "Data Saved failed : " + ex.Message };
            }
        }

        //Nishant - Added code for Saving data to WorkProgress_App table - End

        //Nishant - Added code for Saving data to Tbl_Users table - Start
        [HttpPost]
        [Route("api/SaveNewUser/Save")]
        public object SaveNewUser(tbl_Users model)
        {
            data.Configuration.LazyLoadingEnabled = false;
            data.Configuration.ProxyCreationEnabled = false;
            //model.status = "Pending for compliance";
            try
            {
                // new
                if (model.id == 0)
                {
                    //model.status

                    data.tbl_Users.Add(model);
                }
                else
                {
                    //Edit
                }
                data.SaveChanges();
                return new { status = "OK", payload = model, Message = "Data Saved" };
            }
            catch (Exception ex)
            {
                return new { status = "Failed", payload = model, Message = "Data Saved failed : " + ex.Message };
            }
        }
        //Nishant - Added code for Saving data to Tbl_Users table - End


        //Nishant - Added code for Getting data from tbl_Users - Start

        [Route("api/Convergence/getUsersPost/{username}/{password}")]
        public IQueryable<tbl_Users> GetAllUsers(string username, string password)
        {
            return data.tbl_Users
                .Where(i => i.username == username && i.password == password)
                .ToList()
                .AsQueryable();

        }
        //Nishant - Added code for Getting data from tbl_Users - End


        //Nishant - Added code for getting data from the convergence table - start
        [Route("api/Convergence/getconvergenceList/{district}")]
        public IQueryable<convergence> getconvergenceList(string district)
        {
            var dis = district;

            if (dis == "State")
            {
                return data.convergence.Include(o => o.convergencedpt)
                .Where(i => i.status == "complete")
                .ToList()
                .AsQueryable();
            }
            else
            {
                return data.convergence.Include(o => o.convergencedpt)
               .Where(i => i.district == dis && i.status == "complete")
               .ToList()
               .AsQueryable();
            }

        }

        //Nishant - Added code for getting data from the convergence table - end



        // POST: api/user_api
        [HttpPost]
        [Route("api/user_login")]
        public IHttpActionResult Postuser_tbl(tbl_Users cls)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tbl_Users tbl_Users = data.tbl_Users.Where(x => x.username == cls.username && x.password == cls.password).FirstOrDefault();
            if (tbl_Users == null)
            {
                return NotFound();
            }
            //  return CreatedAtRoute("DefaultApi", new { id = cls.userid }, cls);
            return Ok(tbl_Users);

            }
       



    }
}

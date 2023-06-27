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
using System.Web.UI.WebControls;

namespace MLAFund.Controllers
{
    public class FundAllotment_Controller : ApiController
    {
        //private Models.Data.RuralWorksData data = new Models.Data.RuralWorksData();
        private Models.Data.RuralWorksData data = new Models.Data.RuralWorksData();


        [HttpPost]
        [Route("api/FundAllotment/Save")]
        public object Save(fund_allotment model)
        {
            data.Configuration.LazyLoadingEnabled = false;
            data.Configuration.ProxyCreationEnabled = false;
            //model.status = "Pending for compliance";
            try
            {
                // new
                if (model.fundallotmentid == 0)
                {
                    //model.status
                 
                    data.fund_allotment.Add(model);
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

        [HttpPost]
        [Route("api/MLAExpense/Save")]
        public object Save(mla_expenses model)
        {
            data.Configuration.LazyLoadingEnabled = false;
            data.Configuration.ProxyCreationEnabled = false;
            //model.status = "Pending for compliance";
            try
            {
                // new
                if (model.expensesid == 0)
                {
                    //model.status

                    data.mla_expenses.Add(model);
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
    }
}

using MLAFund.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace MLAFund.Controllers
{
    public class CommonController : ApiController
    {
        private Models.Data.RuralWorksData data = new Models.Data.RuralWorksData();

        [HttpGet]
        [Route("api/common/District")]
        public object GetDistricts()
        {
            return data.master_districts.Select(o => new { o.district_name }).Distinct();
        }
        [HttpGet]
        [Route("api/common/Block")]
        public object GetBlocks(string district)
        {
            return data.master_districts.Where(i => i.district_name == district).Select(o => new { o.block_name }).Distinct();
        }


        [HttpGet]
        [Route("api/common/Panchayat")]
        public object GetPanchayats(string district, string block)
        {
            return data.master_districts.Where(i => i.district_name == district && i.block_name == block)
                .Select(o => new { o.panchayat_name }).Distinct();
        }
        [HttpGet]
        [Route("api/common/Village")]
        public object GetVillages(string district, string block, string panchayat)
        {
            return data.master_districts.Where(i => i.district_name == district && i.block_name == block && i.panchayat_name == panchayat)
                .Select(o => new { o.village_name }).Distinct();
        }

        //Nishant Added code for Fetching Assembly List : Code Start

        [HttpGet]
        [Route("api/common/GetAssemblyList")]

        public object GetAssemblyList()
        {
            return data.tbl_Users.Select(o => new { o.assembly_name }).Distinct();
        }

        //Nishant Added code for Fetching Assembly List : Code End

        

        //Nishant Added code for Fetching Corresponding MLA Names : Code Start
        [HttpGet]
        [Route("api/Common/FillMlaName")]
        public object FillMlaNames(string assemblyName)
        {
            return data.tbl_Users.Where(i => i.assembly_name == assemblyName).Select(o => new { o.mla_name }).Distinct();
        }

        //Nishant Added code for Fetching Corresponding MLA Names : Code End


        //Nishant Added code for Fetching Assembly Name : Code Start

        [HttpGet]
        [Route("api/common/GetAssemblyDetails")]

        public object GetAssemblyDetails() 
        {
            var userLoggedIn = User.Identity.Name;
            return data.tbl_Users.Where(i => i.username == userLoggedIn).Select(o => new { o.assembly_name, o.mla_name, o.district }).Distinct();
        }

        //Nishant Added code for Fetching Assembly Name : Code End

        //Nishant Added code for Inserting data for MLA Fund - Code Start//
        /**
         * API for Inserting data from Mobile App to fund_details table 
         * on the MLAFund database
         * Api Link: http://localhost:56574/api/SaveFundDetails/Save/ 
         * 
         **/

        [HttpPost]
        [Route("api/SaveFundDetails/Save")]
        public object SaveFundDetails(fund_details model)
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

                    data.fund_details.Add(model);
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
        //Nishant Added code for Inserting data for MLA Fund - Code End//


    }
}

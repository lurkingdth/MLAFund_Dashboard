using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;

namespace MLAFund.Models.Data
{
    [Table("tbl_Users")]
    public class tbl_Users
    {
        [Key]
        public int id    { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string assembly_name { get; set; }
        public int user_role { get; set; }
        public string status { get; set; }
        public string district { get; set; }
        public string remarks { get; set; }
        public string mla_name { get; set; }
    }
    //

    [Table("fund_details")]

    public class fund_details
    {
        [Key]
        public int id { get; set; }
        public string assembly_name { get; set; }
        public string mla_name { get; set; }
        public string scheme_name { get; set; }
        public int estimated_cost { get; set; }
        public int total_expenditure { get; set; }
        public DateTime date_of_entry { get; set; }
        public string geotag_pic_1 { get; set; }
        public string geotag_pic_2 { get; set; }
        public string geotag_pic_3 { get; set; }
        public string status { get; set; }
        public string scheme_status { get; set; }
        public string remarks { get; set; }
        public string financial_year { get; set; }
        public int scheme_code { get; set; }

    }

    [Table("fund_allotment")]

    public class fund_allotment
    {
        [Key]
        public int fundallotmentid { get; set; }
        public string assembly_name { get; set; }
        public string mla_name { get; set; }
        public int allotted_fund{ get; set; }
        public int total_expenditure { get; set; }
        public string financial_year { get; set; }
        public string remarks { get; set; }

    }

    [Table("mla_expenses")]

    public class mla_expenses
    {
        [Key]
        public int expensesid { get; set; }
        public string assembly_name { get; set; }
        public string district { get;set; }
        public string mla_name { get;set; }
        public string expense_head { get; set; }
        public string expense_detail { get; set; }
        public int expense_amount { get; set; }
        public int pending_fund { get; set; }
        public string expense_doc { get; set; }
        public string financial_year { get; set; }
    }

    public class master_district
    {
        [Key]
        public int id { get; set; }
        public int district_code { get; set; }
        public string district_name { get; set; }
        public int block_code { get; set; }
        public string block_name { get; set; }
        public int panchayat_code { get; set; }
        public string panchayat_name { get; set; }
        public int village_code { get; set; }
        public string village_name { get; set; }
    }


    //
    [Table("convergence")]
    public class convergence
    {
        [Key]
        public int    cid { get; set; }
        public string district { get; set; }
        public string block { get; set; }
        public string panchayat { get; set; }
        public string village { get; set; }
        public string nameofscheme { get; set; }
        public int fundofdpt { get; set; }
        public int fundofmgnrega { get; set; }
        public int totalfund { get; set; }
        public DateTime dateofsanctioned { get; set; }
        public string timelimit { get; set; }
        public int totalfundutilized { get; set; }
        public string timespent { get; set; }
        public string remarks { get; set; }
        public DateTime dateofentry { get; set; }
        public string status { get; set; }
        public string action { get; set; }
        public string submittedby { get; set; }
        public string convergence_doc { get; set; }
        

        //Added ICollection for updating Department and Scheme details to convergencedpt table
        public ICollection<convergencedpt> convergencedpt { get; set; }


    }

    //Nishant - Added  "convergencedpt" table properties
    [Table("convergencedpt")]
    public class convergencedpt
    {
        [Key]
        public int convergencedptid { get; set; }
        [ForeignKey("convergence")]
        public int cid { set; get; }
        public string convergencedptname { get; set; }
        public string convergencedptscheme { get; set; }
        public int convergencefund { get; set; }

        public  convergence convergence { get; set; }
    }

    //Nishant - Added "workprogress_app" table properties
    [Table ("workprogress_app")]
    public class workprogress_app
    {
        [Key]
        public int pid { get; set;}
        public int cid {  get; set; }
        public string latitude {  get; set; }
        public string longitude { get; set; }
        public DateTime date { get; set; }
        public string submitted_by { get; set; }
        public string photo1 { get; set;}
        public string photo2 { get; set; }
        public string photo3 { get; set; }
        public string remarks { get; set; }
        public string address { get; set; }
        public string userid { get; set; }
        public string status { get; set; }
        public DateTime datetime { get; set; }
    }
    //

    [Table("workprogress")]
    public class workprogress
    {
        [Key]
        public int pid { get; set; }
        public int cid { get; set; }
        public int fundutilizeddept { get; set; }
        public int fundutilizedmgnrega { get; set; }
        public int fundutilized { get; set; }
        public string geotagphoto { get; set; }
        public string workstatus { get; set; }
        public int mandaysgenerated { get; set; }
        public string remarks { get; set; }
        public DateTime dateofutilization { get; set; }
        public string submittedby { get; set; }
        public DateTime date { get; set; }

    }

    

    //Nishant - Added "master_department" table properties
    [Table("master_department")]
    public class master_department
    {
        [Key]
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }

        public int SchemeID { get; set;}
        public string SchemeName { get; set; }
    }

    //Nishant - Added class for stateDeptReprot
    public class stateDeptReport
    {
        public string convergenceDptName { get; set; }
        public int pending { get; set; }
        public int completed { get; set; }
    }

    //Nishant - Added class for wrkPrgReport
    public class wrkPrgReport
    {
        public string district { get; set; }
        public string block { get; set; }
        public string panchayat { get; set; }
        public string village { get; set; }
        public string nameofscheme { get; set; }
        public int fundofdpt { get; set; }
        public int fundofmgnrega { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }   
        public string photo1 { get; set; } //change string to Image
        public string photo2 { get; set; }
        public string photo3 { get; set; }
        public string address { get; set; }


    }

    //Nishant - Added class for chartData
    public class chartData
    {
        public int pendingSchemes { get; set; }
        public int activeSchemes { get; set; }
        public string assembly_name { get; set; }
    }

    public class mla_expenses_temp
    {
        public string assembly_name { get; set; }
        public string financial_year { get; set; }
        public int total_expense_amount { get; set; }
        //public int alloted_fund { get; set; }

    }

    

}

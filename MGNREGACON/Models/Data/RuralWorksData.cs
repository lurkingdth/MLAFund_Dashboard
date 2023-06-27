using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLAFund.Models.Data
{
    public class RuralWorksData :DbContext
    {
        public DbSet<tbl_Users> tbl_Users { get; set; }
        public DbSet<fund_details> fund_details { get; set; }
        public DbSet<master_district> master_districts { get; set; }
        public DbSet<convergence> convergence { get; set; }
        public DbSet<workprogress> workprogress { get; set; }
        public DbSet<master_department> master_department { get; set; }
        public DbSet<convergencedpt> convergencedpt { get; set; }
        public DbSet<workprogress_app> workprogress_app { get; set; }
        public DbSet<fund_allotment> fund_allotment { get; set; }
        public DbSet<mla_expenses> mla_expenses { get; set; }
    }
}
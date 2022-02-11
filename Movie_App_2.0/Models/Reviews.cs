using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Movie_App_2._0.Models
{
    public class Reviews
    {
        [Key]
        public int ReviewID { get; set; }
        public string ReviewTitle { get; set; }
        public bool AlreadyPosted { get; set; }
    }
}
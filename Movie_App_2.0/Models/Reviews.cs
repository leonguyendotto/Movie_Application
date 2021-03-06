using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie_App_2._0.Models
{
    public class Reviews
    {
        [Key]
        public int ReviewID { get; set; }
        public string ReviewTitle { get; set; }
        public bool AlreadyPosted { get; set; }
    }

    public class ReviewDto
    {
        public int ReviewID { get; set; }
        public string ReviewTitle { get; set; }
        public bool AlreadyPosted { get; set; }

    }
}
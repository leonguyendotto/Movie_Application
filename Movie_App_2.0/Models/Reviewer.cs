using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Movie_App_2._0.Models
{
    public class Reviewer
    {
        [Key]
        public int ReviewerID { get; set; }
        public string ReviewerFirstName { get; set; }
        public string ReviewerLastName { get; set; }

        //A reviewer can watch many movies
        public ICollection<Movie> Movies { get; set; }
    }
}
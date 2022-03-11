using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie_App_2._0.Models
{
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }
        public string MovieTitle { get; set; }
        public string MovieOrigin { get; set; }

        //data needed for keeping track of poster images uploaded
        //images deposited into /Content/Images/Posters/{id}.{extension}
        public bool MovieHasPic { get; set; }
        public string PicExtension { get; set; }


        //A review belongs to one movie
        //A movie can have many reviews 

        [ForeignKey("Reviews")]
        public int ReviewID { get; set; }
        public virtual Reviews Reviews { get; set; }

        //A movie can be watched by many reviewers
        public ICollection<Reviewer> Reviewers { get; set; }
    }

    public class MovieDto
    {
        public int MovieID { get; set; }
        public string MovieTitle { get; set; }
        public string MovieOrigin { get; set; }
        public string ReviewTitle { get; set; }

        public int ReviewID { get; set; }

        //data needed for keeping track of poster images uploaded
        //images deposited into /Content/Images/Posters/{id}.{extension}
        public bool MovieHasPic { get; set; }
        public string PicExtension { get; set; }
    }
}
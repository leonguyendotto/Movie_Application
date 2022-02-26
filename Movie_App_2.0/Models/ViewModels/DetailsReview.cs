using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie_App_2._0.Models.ViewModels
{
    public class DetailsReview
    {

        //the review itself that we want to display
        public ReviewDto SelectedReview { get; set; }

        //all of the related movies to that particular reviews
        public IEnumerable<MovieDto> RelatedMovies { get; set; }
    }
}
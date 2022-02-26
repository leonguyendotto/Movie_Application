using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie_App_2._0.Models.ViewModels
{
    public class DetailsReviewer
    {

        public ReviewerDto SelectedReviewer { get; set; }
        public IEnumerable<MovieDto> KeptMovies { get; set; }
    }
}
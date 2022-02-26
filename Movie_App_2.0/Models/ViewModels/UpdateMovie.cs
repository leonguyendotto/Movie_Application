using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie_App_2._0.Models.ViewModels
{
    public class UpdateMovie
    {
        //This viewmodel is a class which stores information that we need to present to /Movie/Update/{}

        //the existing movie information

        public MovieDto SelectedMovie { get; set; }

        // all review to choose from when updating this movie

        public IEnumerable<ReviewDto> ReviewOptions { get; set; }
    }
}
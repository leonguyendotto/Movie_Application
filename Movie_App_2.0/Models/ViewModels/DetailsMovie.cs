using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie_App_2._0.Models.ViewModels
{
    public class DetailsMovie
    {
        public MovieDto SelectedMovie { get; set; }
        public IEnumerable<ReviewerDto> ResponsibleReviewers { get; set; }

        public IEnumerable<ReviewerDto> AvailableReviewers { get; set; }
    }
}
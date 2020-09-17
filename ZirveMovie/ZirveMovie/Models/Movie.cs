using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZirveMovie.Models
{
    [Table("Movie", Schema = "TheMoviedb")]
    public class Movie: IEntityBaseClass
    {
        public double popularity { get; set; }
        public int id { get; set; }
        public bool video { get; set; }
        public int vote_count { get; set; }
        public double vote_average { get; set; }
        public string title { get; set; }
        public DateTime? release_date { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string genre_ids { get; set; }
        public string backdrop_path { get; set; }
        public bool adult { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }

        public class ResponseMovie
        {
            public int page { get; set; }
            public int total_results { get; set; }
            public int total_pages { get; set; }
            public List<Movie> results { get; set; }
        }

    }
}

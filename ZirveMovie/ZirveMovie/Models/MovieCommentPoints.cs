using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZirveMovie.Models
{
    [Table("MovieCommentPoints", Schema = "TheMoviedb")]
    public class MovieCommentPoints : IEntityBaseClass
    {
        public string Comment { get; set; }
        public int MoviePoints { get; set; }
        public int MovieID { get; set; }
    }
    public class MovieandMovieCommentPoints
    {
        public Movie Movie { get; set; }
        public List<MovieCommentPoints> MovieCommentPoints { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ZirveMovie.Models;

namespace ZirveMovie.Token
{
    [Table("Token", Schema = "TheMoviedb")]
    public class Token
    {
        [Key]
        public Int64 AutoID { get; set; }
        public int UserID { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
    }
}

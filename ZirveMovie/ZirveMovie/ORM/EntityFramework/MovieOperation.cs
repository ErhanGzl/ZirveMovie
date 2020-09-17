using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ZirveMovie.Models;
using ZirveMovie.ORM.PureAdo;

namespace ZirveMovie.ORM.EntityFramework
{
    public class MovieOperation : IOperationBaseClass
    {

        public int Insert(IEntityBaseClass entityObj)
        {
            int Result = 1;
            Movie Movies = (Movie)entityObj;
            try
            {
                using (var db = new ZirveMovieContext())
                {
                    db.Movie.Add(Movies);
                    db.SaveChanges();
                }
            }
            catch (Exception exx)
            {
                Result = -1;
            }
            return Result;
        }
        public int Update(IEntityBaseClass entityObj)
        {
            int Result = 1;
            Movie Movies = (Movie)entityObj;
            try
            {
                using (var db = new ZirveMovieContext())
                {
                    db.Movie.Add(Movies);
                    db.SaveChanges();
                }
            }
            catch (Exception exx)
            {
                Result = -1;
            }
            return Result;
        }
        public int Delete(int id)
        {
            int Result = 1;
            try
            {
                using (var db = new ZirveMovieContext())
                {
                    var Movie = db.Movie.SingleOrDefault(t => t.id == id);
                    if (Movie != null)
                    {
                        Movie.DataStatus = 0;
                        Movie.DeletedBy = 676;
                        Movie.DeletedDate = DateTime.Now;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception exx)
            {
                Result = -1;
            }
            return Result;
        }
        public IEntityBaseClass Select(int id)
        {
            using (var db = new ZirveMovieContext())
            {
                return db.Movie.SingleOrDefault(t => t.id == id);
            }

        }
     
        public List<Movie> SelectList()
        {
            using (var db = new ZirveMovieContext())
            {
                return db.Movie.ToList();
            }

        }

    }
}

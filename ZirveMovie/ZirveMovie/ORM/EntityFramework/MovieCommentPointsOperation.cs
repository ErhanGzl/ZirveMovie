using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZirveMovie.Models;

namespace ZirveMovie.ORM.EntityFramework
{
    public class MovieCommentPointsOperation
    {
        public int Insert(IEntityBaseClass entityObj)
        {
            int Result = 1;
            MovieCommentPoints MovieCommentPoint = (MovieCommentPoints)entityObj;
            try
            {
                using (var db = new ZirveMovieContext())
                {
                    db.MovieCommentPoints.Add(MovieCommentPoint);
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
            MovieCommentPoints MovieCommentPoint = (MovieCommentPoints)entityObj;
            try
            {
                using (var db = new ZirveMovieContext())
                {
                    db.MovieCommentPoints.Add(MovieCommentPoint);
                    db.SaveChanges();
                }
            }
            catch (Exception exx)
            {
                Result = -1;
            }
            return Result;
        }
        public int Delete(int AutoID)
        {
            int Result = 1;
            try
            {
                using (var db = new ZirveMovieContext())
                {
                    var Movie = db.MovieCommentPoints.SingleOrDefault(t => t.AutoID == AutoID);
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
                return db.MovieCommentPoints.SingleOrDefault(t => t.AutoID == id);
            }

        }
        public List<MovieCommentPoints> SelectList(int id)
        {
            using (var db = new ZirveMovieContext())
            {
                return db.MovieCommentPoints.Where(t => t.MovieID == id).ToList();
            }

        }
        public List<MovieCommentPoints> SelectList()
        {
            using (var db = new ZirveMovieContext())
            {
                return db.MovieCommentPoints.ToList();
            }

        }
    }
}

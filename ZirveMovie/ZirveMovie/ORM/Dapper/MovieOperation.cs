using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ZirveMovie.Models;
using ZirveMovie.ORM.PureAdo;

namespace ZirveMovie.ORM.Dapper
{
    public class MovieOperation : IOperationBaseClass
    {
        public int Insert(IEntityBaseClass entityObj)
        {
            Movie Movies = (Movie)entityObj;
            using (var sqlConnection = new SqlConnection(ConnectionString.MovieConnectionString))
            {
                SqlParameter[] parameters = new SqlParameter[]
                  {
                        new SqlParameter("@ModifiedBy", Movies.ModifiedBy),
                        new SqlParameter("@popularity", Movies.popularity),
                        new SqlParameter("@id", Movies.id),
                        new SqlParameter("@video", Movies.video),
                        new SqlParameter("@vote_count", Movies.vote_count),
                        new SqlParameter("@vote_average", Movies.vote_average),
                        new SqlParameter("@title", Movies.title),
                        new SqlParameter("@release_date", Movies.release_date),
                        new SqlParameter("@original_language", Movies.original_language),
                        new SqlParameter("@original_title", Movies.original_title),
                        new SqlParameter("@genre_ids", Movies.genre_ids),
                        new SqlParameter("@backdrop_path", Movies.backdrop_path),
                        new SqlParameter("@adult", Movies.adult),
                        new SqlParameter("@overview", Movies.overview),
                        new SqlParameter("@poster_path", Movies.poster_path),
                  };
                return sqlConnection.Execute(" [TheMoviedb].[spInsertMovie]", parameters);
            }
        }
        public int Update(IEntityBaseClass entityObj)
        {
            Movie Movies = (Movie)entityObj;
            using (var sqlConnection = new SqlConnection(ConnectionString.MovieConnectionString))
            {
                SqlParameter[] parameters = new SqlParameter[]
            {
                        new SqlParameter("@ModifiedBy", Movies.ModifiedBy),
                        new SqlParameter("@popularity", Movies.popularity),
                        new SqlParameter("@id", Movies.id),
                        new SqlParameter("@video", Movies.video),
                        new SqlParameter("@vote_count", Movies.vote_count),
                        new SqlParameter("@vote_average", Movies.vote_average),
                        new SqlParameter("@title", Movies.title),
                        new SqlParameter("@release_date", Movies.release_date),
                        new SqlParameter("@original_language", Movies.original_language),
                        new SqlParameter("@original_title", Movies.original_title),
                        new SqlParameter("@genre_ids", Movies.genre_ids),
                        new SqlParameter("@backdrop_path", Movies.backdrop_path),
                        new SqlParameter("@adult", Movies.adult),
                        new SqlParameter("@overview", Movies.overview),
                        new SqlParameter("@poster_path", Movies.poster_path),
            };
                return sqlConnection.Execute("[TheMoviedb].[spUpdateMovie]", parameters);
            }
        }
        public int Delete(int AutoID)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString.MovieConnectionString))
            {
                SqlParameter[] parameters = new SqlParameter[]
            {
                        new SqlParameter("@AutoID", AutoID)
            };

                return sqlConnection.Execute("[TheMoviedb].[spDeleteMovie]", parameters);
            }
        }
        public IEntityBaseClass Select(int recordID)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString.MovieConnectionString))
            {
                DynamicParameters ObjParm = new DynamicParameters();
                ObjParm.Add("@id", recordID);
                return ((Movie)sqlConnection.Query<Movie>("[TheMoviedb].[spSelectMovieAutoID]", ObjParm, commandType: CommandType.StoredProcedure));
            }
        }
        public List<Movie> Select()
        {
            using (var sqlConnection = new SqlConnection(ConnectionString.MovieConnectionString))
            {
                return sqlConnection.Query<Movie>(" [TheMoviedb].[spSelectAllMovie]", commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public List<Movie> Select(int PageNumber, int PageSize)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString.MovieConnectionString))
            {
                DynamicParameters ObjParm = new DynamicParameters();
                ObjParm.Add("@PageNumber", PageNumber);
                ObjParm.Add("@PageSize", PageSize);
                return sqlConnection.Query<Movie>("[TheMoviedb].[spSelectAllMoviePage]", ObjParm, commandType: CommandType.StoredProcedure).ToList();
            }
        }

    }
}

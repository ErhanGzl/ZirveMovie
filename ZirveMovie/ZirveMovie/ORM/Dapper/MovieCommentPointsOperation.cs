using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ZirveMovie.Models;

namespace ZirveMovie.ORM.Dapper
{
    public class MovieCommentPointsOperation
    {
        public int Insert(IEntityBaseClass entityObj)
        {
            MovieCommentPoints Movies = (MovieCommentPoints)entityObj;
            using (var sqlConnection = new SqlConnection(ConnectionString.MovieConnectionString))
            {
                var parameters = new DynamicParameters();
                {
                    parameters.Add("@CreatedBy", Movies.CreatedBy);
                    parameters.Add("@Comment", Movies.Comment);
                    parameters.Add("@MoviePoints", Movies.MoviePoints);
                    parameters.Add("@MovieID", Movies.MovieID);
                }
                return sqlConnection.Execute("[TheMoviedb].[spInsertMovieCommentPoints]", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public int Update(IEntityBaseClass entityObj)
        {
            MovieCommentPoints Movies = (MovieCommentPoints)entityObj;
            using (var sqlConnection = new SqlConnection(ConnectionString.MovieConnectionString))
            {
                var parameters = new DynamicParameters();
                {
                    parameters.Add("@CreatedBy", Movies.CreatedBy);
                    parameters.Add("@Comment", Movies.Comment);
                    parameters.Add("@MoviePoints", Movies.MoviePoints);
                    parameters.Add("@MovieID", Movies.MovieID);
                }
                return sqlConnection.Execute("[TheMoviedb].[spUpdateMovieCommentPoints]", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public int Delete(int AutoID)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString.MovieConnectionString))
            {
                var parameters = new DynamicParameters();
                {
                    new SqlParameter("@AutoID", AutoID);
                }
                return sqlConnection.Execute("[TheMoviedb].[spDeleteMovieCommentPoints]", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public IEntityBaseClass Select(int recordID)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString.MovieConnectionString))
            {
                DynamicParameters ObjParm = new DynamicParameters();
                ObjParm.Add("@id", recordID);
                return ((MovieCommentPoints)sqlConnection.Query<MovieCommentPoints>("[TheMoviedb].[spSelectMovieCommentPointsAutoID]", ObjParm, commandType: CommandType.StoredProcedure));
            }
        }
        public List<MovieCommentPoints> Select()
        {

            using (var sqlConnection = new SqlConnection(ConnectionString.MovieConnectionString))
            {
                return sqlConnection.Query<MovieCommentPoints>("[TheMoviedb].[spSelectAllMovieCommentPoints]", commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public List<MovieCommentPoints> SelectID(int id)
        {
            DynamicParameters ObjParm = new DynamicParameters();
            ObjParm.Add("@id", id);
            using (var sqlConnection = new SqlConnection(ConnectionString.MovieConnectionString))
            {
                return sqlConnection.Query<MovieCommentPoints>("[TheMoviedb].[spSelectMovieCommentPointsMovieID]", ObjParm, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}

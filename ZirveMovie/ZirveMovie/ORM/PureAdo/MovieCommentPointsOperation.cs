using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ZirveMovie.Models;

namespace ZirveMovie.ORM.PureAdo
{
    public class MovieCommentPointsOperation : IOperationBaseClass
    {
        public int Insert(IEntityBaseClass entityObj)
        {
            MovieCommentPoints Movies = (MovieCommentPoints)entityObj;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CreatedBy", Movies.CreatedBy),
                new SqlParameter("@Comment", Movies.Comment),
                new SqlParameter("@MoviePoints", Movies.MoviePoints),
                new SqlParameter("@MovieID", Movies.MovieID)
            };

            return SqlClientUtility.ExecuteScalar(CommandType.StoredProcedure, "[TheMoviedb].[spInsertMovieCommentPoints]", parameters);
        }
        public int Update(IEntityBaseClass entityObj)
        {
            MovieCommentPoints Movies = (MovieCommentPoints)entityObj;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ModifiedBy", Movies.ModifiedBy),
                 new SqlParameter("@Comment", Movies.Comment),
                new SqlParameter("@MoviePoints", Movies.MoviePoints),
                new SqlParameter("@AutoID", Movies.AutoID)
            };

            return SqlClientUtility.ExecuteNonQuery(CommandType.StoredProcedure, "[TheMoviedb].[spUpdateMovieCommentPoints]", parameters);
        }
        public int Delete(int AutoID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@AutoID", AutoID)
            };

            return SqlClientUtility.ExecuteNonQuery(CommandType.StoredProcedure, "[TheMoviedb].[spDeleteMovieCommentPoints]", parameters);
        }
        public IEntityBaseClass Select(int recordID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@RecordID", recordID)
            };

            using (SqlDataReader dataReader = SqlClientUtility.ExecuteReader(CommandType.StoredProcedure, "[TheMoviedb].[spSelectMovieCommentPointsAutoID]", parameters))
            {
                if (dataReader.Read())
                {
                    return null;// MapDataReader(dataReader);
                }
                else
                {
                    return null;
                }
            }
        }
        public List<MovieCommentPoints> Select()
        {

            using (SqlDataReader dataReader = SqlClientUtility.ExecuteReader(CommandType.StoredProcedure, "[TheMoviedb].[spSelectAllMovieCommentPoints]", null))
            {
                if (dataReader.Read())
                {
                    return null;// MapDataReader(dataReader);
                }
                else
                {
                    return null;
                }
            }
        }
        public List<MovieCommentPoints> SelectID(int id)
        {
            List<MovieCommentPoints> Liste = new List<MovieCommentPoints>();
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@id", id) };

            using (SqlDataReader dataReader = SqlClientUtility.ExecuteReader(CommandType.StoredProcedure, "[TheMoviedb].[spSelectMovieCommentPointsMovieID]", parameters))
            {
                while (dataReader.Read())
                {
                    Liste.Add(((MovieCommentPoints)MapDataReader(dataReader)));
                }
            }
            return Liste;
        }
        private IEntityBaseClass MapDataReader(SqlDataReader dr)
        {

            MovieCommentPoints temp = new MovieCommentPoints();
            temp.AutoID = Convert.ToInt16(dr["AutoID"]);
            temp.CreatedBy = Convert.ToInt16(dr["CreatedBy"]);
            temp.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
            temp.ModifiedBy = dr["ModifiedBy"] != DBNull.Value ? (Int16?)dr["ModifiedBy"] : (Int16?)null;
            temp.ModifiedDate = dr["ModifiedDate"] != DBNull.Value ? (DateTime?)dr["ModifiedDate"] : (DateTime?)null;
            temp.DeletedBy = dr["DeletedBy"] != DBNull.Value ? (Int16?)dr["DeletedBy"] : (Int16?)null;
            temp.DeletedDate = dr["DeletedDate"] != DBNull.Value ? (DateTime?)dr["DeletedDate"] : (DateTime?)null;
            temp.DataStatus = Convert.ToInt16(dr["DataStatus"]);
            temp.Comment = dr["Comment"].ToString();
            temp.MoviePoints = Convert.ToInt32(dr["MoviePoints"]);
            temp.MovieID = Convert.ToInt32(dr["MovieID"]);
            return temp;
        }
    }
}

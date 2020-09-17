using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ZirveMovie.Models;

namespace ZirveMovie.ORM.PureAdo
{
    public class MovieOperation : IOperationBaseClass
    {
        public int Insert(IEntityBaseClass entityObj)
        {
            Movie Movies = (Movie)entityObj;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CreatedBy", Movies.CreatedBy),
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

            return SqlClientUtility.ExecuteScalar(CommandType.StoredProcedure, "[TheMoviedb].[spInsertMovie]", parameters);
        }
        public int Update(IEntityBaseClass entityObj)
        {
            Movie Movies = (Movie)entityObj;
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

            return SqlClientUtility.ExecuteNonQuery(CommandType.StoredProcedure, " [TheMoviedb].[spUpdateMovie]", parameters);
        }
        public int Delete(int AutoID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@AutoID", AutoID)
            };

            return SqlClientUtility.ExecuteNonQuery(CommandType.StoredProcedure, "[TheMoviedb].[spDeleteMovie]", parameters);
        }
        public IEntityBaseClass Select(int AutoID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@AutoID", AutoID)
            };

            using (SqlDataReader dataReader = SqlClientUtility.ExecuteReader(CommandType.StoredProcedure, "[TheMoviedb].[spSelectMovieAutoID]", parameters))
            {
                if (dataReader.Read())
                {
                    return MapDataReader(dataReader);
                }
                else
                {
                    return null;
                }
            }
        }
        public IEntityBaseClass SelectID(int id)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };

            using (SqlDataReader dataReader = SqlClientUtility.ExecuteReader(CommandType.StoredProcedure, "[TheMoviedb].[spSelectMovieID]", parameters))
            {
                if (dataReader.Read())
                {
                    return ((Movie)MapDataReader(dataReader));
                }
                else
                {
                    return null;
                }
            }
        }
        public List<Movie> Select()
        {
            List<Movie> Result = new List<Movie>();
            using (SqlDataReader dataReader = SqlClientUtility.ExecuteReader(CommandType.StoredProcedure, "[TheMoviedb].[spSelectAllMovie]", null))
            {
                while (dataReader.Read())
                {
                    Result.Add(((Movie)MapDataReader(dataReader)));
                }

            }
            return Result;
        }
        public List<Movie> Select(int PageNumber, int PageSize)
        {
            List<Movie> Result = new List<Movie>();
            SqlParameter[] parameters = new SqlParameter[]
        {
                new SqlParameter("@PageNumber", PageNumber),
                new SqlParameter("@PageSize", PageSize)
        };
            using (SqlDataReader dataReader = SqlClientUtility.ExecuteReader(CommandType.StoredProcedure, "[TheMoviedb].[spSelectAllMovie]", null))
            {
                while (dataReader.Read())
                {
                    Result.Add(((Movie)MapDataReader(dataReader)));
                }
            }
            return Result;
        }
        private IEntityBaseClass MapDataReader(SqlDataReader dr)
        {
            Movie temp = new Movie();
            temp.AutoID = Convert.ToInt16(dr["AutoID"]);
            temp.CreatedBy = Convert.ToInt16(dr["CreatedBy"]);
            temp.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
            temp.ModifiedBy = dr["ModifiedBy"] != DBNull.Value ? (Int32?)dr["ModifiedBy"] : (Int32?)null;
            temp.ModifiedDate = dr["ModifiedDate"] != DBNull.Value ? (DateTime?)dr["ModifiedDate"] : (DateTime?)null;
            temp.DeletedBy = dr["DeletedBy"] != DBNull.Value ? (Int32?)dr["DeletedBy"] : (Int32?)null;
            temp.DeletedDate = dr["DeletedDate"] != DBNull.Value ? (DateTime?)dr["DeletedDate"] : (DateTime?)null;
            temp.DataStatus = Convert.ToInt16(dr["DataStatus"]);
            temp.popularity = Convert.ToDouble(dr["popularity"]);
            temp.id = Convert.ToInt32(dr["id"]);
            temp.vote_count = Convert.ToInt32(dr["vote_count"]);
            temp.vote_average = Convert.ToInt32(dr["vote_average"]);
            temp.title = dr["title"].ToString();
            temp.release_date = dr["release_date"] != DBNull.Value ? (DateTime?)dr["release_date"] : (DateTime?)null;
            temp.original_language = dr["original_language"].ToString();
            temp.original_title = dr["original_title"].ToString();
            temp.genre_ids = dr["genre_ids"].ToString();
            temp.backdrop_path = dr["backdrop_path"].ToString();
            temp.overview = dr["overview"].ToString();
            temp.poster_path = dr["poster_path"].ToString();
            return temp;
        }
    }
}

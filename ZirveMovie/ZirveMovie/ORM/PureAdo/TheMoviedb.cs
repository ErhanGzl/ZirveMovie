using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ZirveMovie.Models;
using static ZirveMovie.Models.Movie;

namespace ZirveMovie.ORM.PureAdo
{
    public class TheMoviedb
    {
      //  private const int TheMoviedbSeknronFilmSayisi = 81;
        public (bool Result, string ResultMessage, List<Movie> FilmListesi) GetMovie(int TheMoviedbSeknronFilmSayisi = 50)
        {
            bool Result = true;
            string ResultMessage = ""; ;
            List<Movie> FilmListesi = new List<Movie>();
            try
            {
                string Token = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["api_key"];
                int PageCounter = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(TheMoviedbSeknronFilmSayisi) / Convert.ToDouble(20))); // Kaç sayfada çekileceğini hesaplar. bir sayfada 20 adet film bulunmaktadır.
                for (int i = 1; i <= PageCounter; i++)
                {
                    var client = new RestClient(string.Format("https://api.themoviedb.org/3/discover/movie?api_key={0}&language=tr-TR&sort_by=popularity.desc&page={1}", Token, i.ToString()));
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AlwaysMultipartFormData = true;
                    IRestResponse response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ResponseMovie Session = JsonConvert.DeserializeObject<ResponseMovie>(response.Content);
                        if (i == PageCounter)// Son sayfaya gelindiğinde tam olarak istenilen film sayısını yakalamk için kesme işlemi yapılmaktadır.
                        {
                            for (int xx = 0; xx < TheMoviedbSeknronFilmSayisi - FilmListesi.Count; xx++)
                            {
                                FilmListesi.Add(Session.results[i]);
                            }
                        }
                        else
                        {
                            FilmListesi.AddRange(Session.results);
                        }

                    }
                }
            }
            catch (Exception exx)
            {
                Result = false;
                ResultMessage = exx.ToString();

            }
            finally
            {
                ResultMessage = "Başarılı";
            }
            return (Result, ResultMessage, FilmListesi);
        }
        public void BulkInsert_Movie(DataTable TableData)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy("Data Source=NBABRA01;Initial Catalog=ZirveMovie; Integrated Security=True;"))
            {
                bulkCopy.DestinationTableName = "[TheMoviedb].[MovieTemp]";
                bulkCopy.ColumnMappings.Add("popularity", "popularity");
                bulkCopy.ColumnMappings.Add("id", "id");
                bulkCopy.ColumnMappings.Add("video", "video");
                bulkCopy.ColumnMappings.Add("vote_count", "vote_count");
                bulkCopy.ColumnMappings.Add("vote_average", "vote_average");
                bulkCopy.ColumnMappings.Add("title", "title");
                bulkCopy.ColumnMappings.Add("release_date", "release_date");
                bulkCopy.ColumnMappings.Add("original_language", "original_language");
                bulkCopy.ColumnMappings.Add("original_title", "original_title");
                bulkCopy.ColumnMappings.Add("genre_ids", "genre_ids");
                bulkCopy.ColumnMappings.Add("backdrop_path", "backdrop_path");
                bulkCopy.ColumnMappings.Add("adult", "adult");
                bulkCopy.ColumnMappings.Add("overview", "overview");
                bulkCopy.ColumnMappings.Add("poster_path", "poster_path");

                try
                {

                    TruncateTemp();
                    bulkCopy.BulkCopyTimeout = 600000;
                    bulkCopy.WriteToServer(TableData);

                }
                catch (Exception exx)
                {
                    // Hata!!!
                }
            }
        }
        public void TruncateTemp()
        {
            SqlConnection sqlConn = new SqlConnection("Data Source=NBABRA01;Initial Catalog=ZirveMovie; Integrated Security=True;");
            try
            {
                if (sqlConn.State != ConnectionState.Open)
                    sqlConn.Open();
                string sqlTrunc = "TRUNCATE TABLE [TheMoviedb].[MovieTemp]";
                SqlCommand cmd = new SqlCommand(sqlTrunc, sqlConn);
                cmd.ExecuteNonQuery();
                sqlConn.Close();
            }
            catch (Exception exx)
            {
                sqlConn.Close();
            }
        }
        public DataSet ASYNC()
        {
            DataSet ds = new DataSet();
            SqlConnection sqlConn = new SqlConnection("Data Source=NBABRA01;Initial Catalog=ZirveMovie; Integrated Security=True;");
            try
            {
                if (sqlConn.State != ConnectionState.Open)
                    sqlConn.Open();
                SqlCommand cmd = new SqlCommand("[TheMoviedb].[spTheMoviedb_ASYNC]", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                sqlConn.Close();
            }
            catch (Exception exx)
            {
                sqlConn.Close();
            }
            return ds;
        }
        public DataSet InsertLog(int MovieCount, int NewMovieCount, int ModifiedMovieCount, DateTime StartDate, DateTime EndDate, int LogStatus, string ErrorText)
        {
            DataSet ds = new DataSet();
            SqlConnection sqlConn = new SqlConnection("Data Source=NBABRA01;Initial Catalog=ZirveMovie; Integrated Security=True;");
            try
            {
                if (sqlConn.State != ConnectionState.Open)
                    sqlConn.Open();
                SqlCommand cmd = new SqlCommand("TheMoviedb.spInsertLog ", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter[] sqlParams = new SqlParameter[] {
                            new SqlParameter("@MovieCount", MovieCount ),
                             new SqlParameter("@NewMovieCount", NewMovieCount ),
                              new SqlParameter("@ModifiedMovieCount", ModifiedMovieCount ),
                               new SqlParameter("@StartDate", StartDate ),
                                new SqlParameter("@EndDate", EndDate ),
                                 new SqlParameter("@LogStatus", LogStatus ),
                                  new SqlParameter("@ErrorText", ErrorText )
                   };
                cmd.Parameters.AddRange(sqlParams);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                sqlConn.Close();
            }
            catch (Exception exx)
            {
                sqlConn.Close();
            }
            return ds;
        }
    }
}

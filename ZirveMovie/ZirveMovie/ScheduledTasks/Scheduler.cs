using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static ZirveMovie.Models.Movie;

namespace ZirveMovie.Models
{
    public class Scheduler : HostedService
    {
        HttpClient restClient;
        public Scheduler()
        {
            restClient = new HttpClient();
        }
        private const int ZamanlanmisGorev = 3000; // Kaç saniyede bir çalışacak.
        protected override async Task ExecuteAsync(CancellationToken cToken)
        {
            while (!cToken.IsCancellationRequested)
            {
                MovieASYNC();
                await Task.Delay(TimeSpan.FromSeconds(ZamanlanmisGorev), cToken);
            }
        }
        private void MovieASYNC()
        {
            ZirveMovie.ORM.PureAdo.TheMoviedb TmD = new ORM.PureAdo.TheMoviedb();
            DateTime BaslangicTarihi = DateTime.Now;
            int YeniEklenenKayitSayisi = 0, GuncellenenKayitSayisi = 0;
            try
            {
               
                (bool Result, string ResultMessage, List<Movie> FilmListesi) = TmD.GetMovie();// Filim verilerinin alınması
                if (Result == true)
                {
                    DataTable MovieList = ToDataTable(FilmListesi);  // Bulk insert için List to DataTable
                    TmD.BulkInsert_Movie(MovieList);// Çekilen verilerin geçici tabloya aktarımı.
                    DataSet ResultData = TmD.ASYNC();// Geçici tablo ile gerçek tablonun birleştirilmesi.
                    if (ResultData != null && ResultData.Tables.Count > 0 && ResultData.Tables[0].Rows.Count > 0)
                    {
                        YeniEklenenKayitSayisi = Convert.ToInt32(ResultData.Tables[0].Rows[0]["YeniEklenenKayitSayisi"]);
                        GuncellenenKayitSayisi = Convert.ToInt32(ResultData.Tables[0].Rows[0]["GuncellenenKayitSayisi"]);
                    }
                    TmD.InsertLog(FilmListesi.Count, YeniEklenenKayitSayisi, GuncellenenKayitSayisi, BaslangicTarihi, DateTime.Now, 1, "");
                }
                else
                {
                    TmD.InsertLog(FilmListesi.Count, YeniEklenenKayitSayisi, GuncellenenKayitSayisi, BaslangicTarihi, DateTime.Now, 1, ResultMessage);
                }
            }
            catch (Exception exx)
            {
                TmD.InsertLog(0, YeniEklenenKayitSayisi, GuncellenenKayitSayisi, BaslangicTarihi, DateTime.Now, 1, exx.ToString());
            }
        }
        private DataTable ToDataTable<T>(IEnumerable<T> data) // Generic List Data table çevirmek için fonksiyon
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                if (prop.Name != "genre_ids")
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                else
                    table.Columns.Add(prop.Name, typeof(string));
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    if (prop.Name != "genre_ids")
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    else
                        row[prop.Name] = string.Join(",", ((int[])prop.GetValue(item)));
                table.Rows.Add(row);
            }
            return table;
        }
    }
}

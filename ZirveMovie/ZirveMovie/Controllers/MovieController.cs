using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZirveMovie.Models;

namespace ZirveMovie.Controllers
{
    [Authorize]
    [Route("api/{controller}/{action}")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Movie> GetMovieByPage([Range(1, 5000, ErrorMessage = "Sayfa Numarası En Az 1 olmalı.")] int PageNumber, int PageSize)
        {
            #region EntityFramework
            ZirveMovieContext Zmc = new ZirveMovieContext();
            return Zmc.Movie.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
            #endregion
            #region Dapper
            //ZirveMovie.ORM.Dapper.MovieOperation Zmcd = new ORM.Dapper.MovieOperation();
            //return Zmcd.Select(PageNumber, PageSize);
            #endregion
            #region PureAdo
            //ZirveMovie.ORM.Dapper.MovieOperation Zmcp = new ORM.Dapper.MovieOperation();
            //return Zmcp.Select(PageNumber, PageSize);
            #endregion
        }

        [HttpPost]
        public IActionResult SetMoviePointsComment([Required] int id, [Range(0, 10, ErrorMessage = "Film puanları {1} ve {2} sayıları arasında olmalıdır.")] int Points, string Comment)
        {
            int UserID = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "UserID").Value);
            var MovCommentPoints = new MovieCommentPoints { CreatedBy = UserID, CreatedDate = DateTime.Now, DataStatus = 1, MovieID = id, Comment = Comment, MoviePoints = Points };
            #region EntityFramework
            ZirveMovie.ORM.EntityFramework.MovieCommentPointsOperation Pop = new ORM.EntityFramework.MovieCommentPointsOperation();
            Pop.Insert(MovCommentPoints);
            #endregion
            #region Dapper
            //ZirveMovie.ORM.Dapper.MovieCommentPointsOperation Popd = new ORM.Dapper.MovieCommentPointsOperation();
            //Popd.Insert(MovCommentPoints);
            #endregion
            #region PureAdo
            //ZirveMovie.ORM.PureAdo.MovieCommentPointsOperation Popp = new ORM.PureAdo.MovieCommentPointsOperation();
            //Popp.Insert(MovCommentPoints);
            #endregion
            this.StatusCode(StatusCodes.Status200OK, "Başarılı");
            return new ObjectResult("Başarılı");
        }
        [HttpGet]
        public IActionResult GetMovieDetailCommentPoints(int id)
        {
            #region EntityFramework
            MovieandMovieCommentPoints mvc = new MovieandMovieCommentPoints();
            ZirveMovie.ORM.EntityFramework.MovieOperation mov = new ORM.EntityFramework.MovieOperation();
            mvc.Movie = ((Movie)mov.Select(id));
            ZirveMovie.ORM.EntityFramework.MovieCommentPointsOperation mcpo = new ORM.EntityFramework.MovieCommentPointsOperation();
            mvc.MovieCommentPoints = mcpo.SelectList(id);
            return new ObjectResult(mvc);
            #endregion
            #region Dapper
            //MovieandMovieCommentPoints mvcd = new MovieandMovieCommentPoints();
            //ZirveMovie.ORM.Dapper.MovieOperation movd = new ORM.Dapper.MovieOperation();
            //mvcd.Movie = ((Movie)movd.Select(id));
            //ZirveMovie.ORM.Dapper.MovieCommentPointsOperation mcpod = new ORM.Dapper.MovieCommentPointsOperation();
            //mvcd.MovieCommentPoints = mcpod.SelectID(id);
            //return new ObjectResult(mvcd);
            #endregion
            #region PureAdo
            //MovieandMovieCommentPoints mvcp = new MovieandMovieCommentPoints();
            //ZirveMovie.ORM.PureAdo.MovieOperation mov = new ORM.PureAdo.MovieOperation();
            //mvcp.Movie = ((Movie)mov.SelectID(id));
            //ZirveMovie.ORM.PureAdo.MovieCommentPointsOperation mcpo = new ORM.PureAdo.MovieCommentPointsOperation();
            //mvcp.MovieCommentPoints = mcpo.SelectID(id);
            //return new ObjectResult(mvcp);
            #endregion
        }
        [HttpPost]
        public IActionResult SendMail([Required] int id, string MailAdress)
        {
            string FromMailUser = HttpContext.User.Claims.First(c => c.Type == "FullName").Value;
            ZirveMovie.ORM.EntityFramework.MovieOperation Pop = new ORM.EntityFramework.MovieOperation();
            Movie Mv = ((Movie)Pop.Select(id));
            if (Mv != null)
            {
                SmtpClient sc = new SmtpClient();
                sc.Port = 587;
                sc.Host = "smtp.live.com";
                sc.EnableSsl = true;
                sc.Credentials = new NetworkCredential("Zirvemovie@hotmail.com", "Zirve.123456");
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("Zirvemovie@hotmail.com", "Zirve Film");

                mail.To.Add(MailAdress);
                mail.Subject = "Film Tavsiye";
                mail.IsBodyHtml = true;


                StringBuilder sbBody = new StringBuilder();
                sbBody.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                sbBody.Append("<head>");
                sbBody.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" />");
                sbBody.Append("<title>Untitled Document</title>");
                sbBody.Append("<style type=\"text/css\">");
                sbBody.Append("<!--");
                sbBody.Append(".style3 {");
                sbBody.Append("color: #FFFFFF;");
                sbBody.Append("font-family: Verdana, Arial, Helvetica, sans-serif;");
                sbBody.Append("font-weight: bold;");
                sbBody.Append("font-size: 18px;");
                sbBody.Append("}");
                sbBody.Append(".style4 {");
                sbBody.Append("color: #FFFFFF;");
                sbBody.Append("font-weight: bold;");
                sbBody.Append("font-size: 14px;");
                sbBody.Append("}");
                sbBody.Append(".style5 {");
                sbBody.Append("color: #FF0000;");
                sbBody.Append("font-weight: bold;");
                sbBody.Append("font-family: Calibri;font-size: 12px;font-weight: bold;");
                sbBody.Append("}");
                sbBody.Append(".style6 {");
                sbBody.Append("color: #0033FF;");
                sbBody.Append("font-weight: bold;font-size: 12px;");
                sbBody.Append("font-family: Calibri;");
                sbBody.Append("}");
                sbBody.Append(".style8 {font-family: Calibri;font-size: 12px;font-weight: bold;}");
                sbBody.Append(".style9 {font-family: Calibri; font-weight: bold; }");
                sbBody.Append("-->");
                sbBody.Append("</style></head>");
                sbBody.Append("");
                sbBody.Append("<body>");
                sbBody.Append("<table width=\"650\" height=\"176\" border=\"1\" align=\"center\">");
                sbBody.Append("<tr>");
                sbBody.Append("<td height=\"39\" colspan=\"2\" bgcolor=\"#33adff\"><div align=\"center\"><span class=\"style3\">Zirve Film Tavsiye</span></div></td>");
                sbBody.Append("</tr>");


                sbBody.Append("<tr>");
                sbBody.Append("<td width=\"100\" height=\"23\" bgcolor=\"#FFFFFF\"><div align=\"left\" class=\"style9\">Filmi Tavsiye Eden</div></td>");
                sbBody.Append("<td width=\"550\"><div align=\"left\" class=\"style8\">" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + FromMailUser + " </div></td>");
                sbBody.Append("</tr>");

                sbBody.Append("<tr>");
                sbBody.Append("<td width=\"100\" height=\"23\" bgcolor=\"#FFFFFF\"><div align=\"left\" class=\"style9\">Filmin Adı</div></td>");
                sbBody.Append("<td width=\"550\"><div align=\"left\" class=\"style8\">" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Mv.title + " </div></td>");
                sbBody.Append("</tr>");

                sbBody.Append("<tr>");
                sbBody.Append("<td width=\"100\" height=\"23\" bgcolor=\"#FFFFFF\"><div align=\"left\" class=\"style9\">Filmin Tarihi</div></td>");
                sbBody.Append("<td width=\"550\"><div align=\"left\" class=\"style8\">" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Mv.release_date.Value.ToString("dd.MM.yyyy") + " </div></td>");
                sbBody.Append("</tr>");

                sbBody.Append("<tr>");
                sbBody.Append("<td width=\"100\" height=\"23\" bgcolor=\"#FFFFFF\"><div align=\"left\" class=\"style9\">Filmin Konusu</div></td>");
                sbBody.Append("<td width=\"550\"><div align=\"left\" class=\"style8\">" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Mv.overview + " </div></td>");
                sbBody.Append("</tr>");



                sbBody.Append("<tr>");
                sbBody.Append("<td width=\"100\" height=\"23\" bgcolor=\"#FFFFFF\"><div align=\"left\" class=\"style9\">Filmin Afişi</div></td>");
                sbBody.Append("<td width=\"550\"><div align=\"left\" class=\"style8\">" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <img src=\"" + "https://image.tmdb.org/t/p/w500/" + Mv.backdrop_path.ToString() + "\" alt = \"alternatetext\"> </div></td>");
                sbBody.Append("</tr>");


                sbBody.Append("<td height=\"29\" colspan=\"2\" bgcolor=\"#33adff\"><div align=\"center\" class=\"style4\">Copyright &copy; Zirve Film </div>      </td>");
                sbBody.Append("</tr>");
                sbBody.Append("</table>");
                sbBody.Append("</body>");
                sbBody.Append("</html>");


                mail.Body = sbBody.ToString();
                sc.Send(mail);
            }
            this.StatusCode(StatusCodes.Status200OK, "Başarılı");
            return new ObjectResult("Başarılı");
        }

    }

}

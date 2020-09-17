using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ZirveMovie.Models;

namespace ZirveMovie.Controllers
{

    [Route("api/{controller}/{action}")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        readonly ZirveMovieContext _context;
        readonly IConfiguration _configuration;
        public LoginController(ZirveMovieContext content, IConfiguration configuration)
        {
            _context = content;
            _configuration = configuration;
        }
        [HttpPost]
        public bool CreateLogin([Required(ErrorMessage = "Ad Alanının Girilmesi Zorunludur.")] string Name,
            [Required(ErrorMessage = "Soyad Alanının Girilmesi Zorunludur.")] string Surname,
            [Required(ErrorMessage = "Email Alanının Girilmesi Zorunludur.")][DataType(DataType.EmailAddress)][EmailAddress] string Email,
            [Required(ErrorMessage = "Şifre Alanının Girilmesi Zorunludur.")][DataType(DataType.Password)][StringLength(100, ErrorMessage = "Şifreniz en az 8 karakterden oluşmalıdır.", MinimumLength = 8)]
            [Display(Name = "Password")][RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Parolalar en az 8 karakter olmalı ve büyük harf (A-Z), küçük harf (a-z), sayı (0-9) ve özel karakter (e.g. !@#$%^&*)")] string Password)
        {
            var Users = new User { CreatedBy = 676, CreatedDate = DateTime.Now, DataStatus = 1, Name = Name, Surname = Surname, Email = Email, Password = Password };
            _context.Users.Add(Users);
            _context.SaveChanges();    

            return true;
        }
        [HttpPost]
        public Token.Token Login(string Email, string Password)
        {
            User user = _context.Users.FirstOrDefault(x => x.Email == Email && x.Password == Password);
            if (user != null)
            {
                //Token üretiliyor.
                ZirveMovie.Token.TokenHandler tokenHandler = new ZirveMovie.Token.TokenHandler(_configuration);
                Token.Token token = tokenHandler.CreateAccessToken(user);
                token.UserID = user.AutoID;
                _context.Token.Add(token);
                //Refresh token Users tablosuna işleniyor.
                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenEndDate = token.Expiration.AddMinutes(3);
                _context.SaveChanges();

                return token;
            }
            return null;
        }
    }
}

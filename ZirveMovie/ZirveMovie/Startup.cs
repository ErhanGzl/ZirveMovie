using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using ZirveMovie.Models;
using ZirveMovie.ORM;

namespace ZirveMovie
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Token:Issuer"],
                    ValidAudience = Configuration["Token:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:SecurityKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
            //            Audience
            //Oluşturulacak token değerini kimlerin / hangi originlerin / sitelerin kullanacağını belirlediğimiz alandır
            //Issuer
            //Oluşturulacak token değerini kimin dağıttığını ifade edeceğimiz alandır.Örneğin; “www.myapi.com”
            //LifeTime
            //Oluşturulan token değerinin süresini kontrol edecek olan doğrulamadır.
            //SigningKey
            //Üretilecek token değerinin uygulamamıza ait bir değer olduğunu ifade eden security key verisinin doğrulamasıdır.
            //ClockSkew
            //Üretilecek token değerinin expire süresinin belirtildiği değer kadar uzatılmasını sağlayan özelliktir. Örneğin; kullanılabilirlik süresi 5 dakika olarak ayarlanan token değerinin ClockSkew değerine 3 dakika verilirse eğer ilgili 
            //token 5 + 3 = 8 dakika kullanılabilir olacaktır.Bunun nedeni, aralarında zaman farkı olan farklı lokasyonlardaki sunucularda yayın yapan bir uygulama üzerinde elde edilen ortak token değerinin saati ileride olan
            //sunucuda geçerliliğini daha erken yitirmemesi için ClockSkew propertysi sayesinde aradaki fark kadar zamanı tokena eklememiz gerekmektedir.Böylece kullanım süresi uzatılmış ve tüm sunucularda token değeri adil
            //    kullanılabilir hale getirilmiş olunacaktır.

            services.AddControllers();
            services.AddAuthentication();
            services.AddSingleton<IHostedService, Scheduler>();
            //   services.AddSingleton<IHostedService, TheMoviedbCollectorService>();
            services.AddDbContext<ZirveMovieContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            ConnectionString.MovieConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

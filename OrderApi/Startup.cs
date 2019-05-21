using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Routing;
using AkkaUtilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderApi.Controllers;

namespace OrderApi
{
    public class Startup
    {
        private ActorSystem akka;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            akka = Bootstrap.CreateActorSystem();
            //var router = akka.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "router");

            var router = akka.CreateOrderShardProxy();

            services.AddSingleton<IActorRef>(svc => router);
            services.AddTransient<IOrderProcessing, OrderProcessing>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}

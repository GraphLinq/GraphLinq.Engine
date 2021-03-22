using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace NodeBlock.Engine.API
{
    public class InternalApi
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private string[] _args;
        private string _listenHost { get; set; }
        private int _listenPort { get; set; }
        public InternalApi(string[] args,
            string listenHost,  int listenPort)
        {
            this._args = args;
            this._listenHost = listenHost;
            this._listenPort = listenPort;
        }

        public void InitApi()
        {
            DotNetEnv.Env.Load();
            CreateHostBuilder(this._args).Build().Run();
        }

        public int GetListenPort()
        {
            return _listenPort;
        }
        
        public IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>()
                       .UseUrls(string.Format("http://{0}:{1}", this._listenHost, this._listenPort));
               });
        }
}

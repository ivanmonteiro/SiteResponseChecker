using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.NHibernate;
using SharpArchContrib.Data.NHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Configuration;

namespace SiteResponseChecker.Desktop.Util
{
    public static class Initializer
    {
        public static void Init()
        {

            var dicConfig = new Dictionary<string, string>();
            dicConfig.Add("connection.connection_string", ConfigurationManager.ConnectionStrings["connString"].ConnectionString);
            dicConfig.Add("connection.provider", ConfigurationManager.AppSettings["connection.provider"]);
            dicConfig.Add("connection.driver_class", ConfigurationManager.AppSettings["connection.driver_class"]);            
            dicConfig.Add("dialect", ConfigurationManager.AppSettings["dialect"]);
            dicConfig.Add("query.substitutions", ConfigurationManager.AppSettings["query.substitutions"]);
            dicConfig.Add("show_sql", ConfigurationManager.AppSettings["show_sql"]);
            
            var configuration = NHibernateSession.Init(new ThreadSessionStorage(),
                new string[] { "SiteResponseChecker.NhRepository" }, dicConfig);
        }
    }
}

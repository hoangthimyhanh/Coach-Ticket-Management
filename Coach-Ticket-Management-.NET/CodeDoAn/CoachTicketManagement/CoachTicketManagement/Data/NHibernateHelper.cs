﻿using CoachTicketManagement.Models;
using CoachTicketManagement.Utility;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachTicketManagement.Data
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                    InitializeSessionFactory(); return _sessionFactory;
            }
        }
        private static void InitializeSessionFactory()
        {
            _sessionFactory = Fluently.Configure()

             .Database(MsSqlConfiguration.MsSql2012.ConnectionString(ConnectionString.Instance.getConnectionString()).ShowSql())

             .Mappings(m => m.FluentMappings
                 .AddFromAssemblyOf<Account>()
                 .AddFromAssemblyOf<Employee>()
                 .AddFromAssemblyOf<Client>()
                 .AddFromAssemblyOf<Driver>()
                 .AddFromAssemblyOf<Trip>()
             )
             .ExposeConfiguration(cfg => new SchemaExport(cfg)
             .Create(false, false))
             .BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}

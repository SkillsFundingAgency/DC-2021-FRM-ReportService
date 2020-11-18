using System;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using Autofac;
using ESFA.DC.FRM.ReportService.Data;
using ESFA.DC.FRM.ReportService.Data.ReferenceData;
using ESFA.DC.FRM.ReportService.Interfaces.Configuration;
using ESFA.DC.FRM.ReportService.Interfaces.DataProvider;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;
using ESFA.DC.ILR2021.DataStore.EF;
using ESFA.DC.ILR2021.DataStore.EF.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.FRM.ReportService.Modules
{
    public class DataProviderModule : Module
    {
        private readonly IReportServiceConfiguration _reportServiceConfiguration;
        private readonly string sqlFuncParameterName = "sqlConnectionFunc";

        public DataProviderModule(IReportServiceConfiguration reportServiceConfiguration)
        {
            _reportServiceConfiguration = reportServiceConfiguration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var orgSqlFunc = new Func<SqlConnection>(() => new SqlConnection(_reportServiceConfiguration.OrgConnectionString));
            var fcsSqlFunc = new Func<SqlConnection>(() => new SqlConnection(_reportServiceConfiguration.FCSConnectionString));
            var larsSqlFunc = new Func<SqlConnection>(() => new SqlConnection(_reportServiceConfiguration.LarsConnectionString));
            var postcodesSqlFunc = new Func<SqlConnection>(() => new SqlConnection(_reportServiceConfiguration.PostcodesConnectionString));

            builder.RegisterType<OrganisationDataProvider>()
                .WithParameter(sqlFuncParameterName, orgSqlFunc)
                .As<IDataProvider<IOrganisation>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<McaGlaLookupProvider>()
                .WithParameter(sqlFuncParameterName, postcodesSqlFunc)
                .As<IDataProvider<IMcaGlaSofLookup>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<McaDevolvedContractsProvider>()
                .WithParameter(sqlFuncParameterName, fcsSqlFunc)
                .As<IDataProvider<IMcaDevolvedContract>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<LARSLearningDeliveryProvider>()
                .WithParameter(sqlFuncParameterName, larsSqlFunc)
                .As<IDataProvider<ILARSLearningDelivery>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<LearnerProvider>().As<IDataProvider<Learner>>();

            builder.RegisterType<ReportDataProvider>().As<IReportDataProvider>().InstancePerLifetimeScope();

            builder.RegisterType<ILR2021_DataStoreEntities>().As<IIlr2021Context>();
            builder.Register(container => new DbContextOptionsBuilder<ILR2021_DataStoreEntities>()
                .UseSqlServer(_reportServiceConfiguration.ILR2021DataStoreConnectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(600))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options).As<DbContextOptions<ILR2021_DataStoreEntities>>().SingleInstance();
        }
    }
}

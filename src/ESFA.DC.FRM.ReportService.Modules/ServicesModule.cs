﻿using Autofac;
using ESFA.DC.ExcelService;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Reports.Services;

namespace ESFA.DC.FRM.ReportService.Modules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ExcelFileService>().As<IExcelFileService>().InstancePerLifetimeScope();

            builder.RegisterType<FileNameService>().As<IFileNameService>().InstancePerLifetimeScope();
            builder.RegisterType<ReportZipService>().As<IReportZipService>().InstancePerLifetimeScope();
        }
    }
}

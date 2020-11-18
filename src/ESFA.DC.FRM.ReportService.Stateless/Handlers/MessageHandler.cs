using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.Configuration;
using ESFA.DC.FRM.ReportService.Modules;
using ESFA.DC.FRM.ReportService.Reports;
using ESFA.DC.FRM.ReportService.Stateless.Configuration;
using ESFA.DC.FRM.ReportService.Stateless.Context;
using ESFA.DC.IO.AzureStorage.Config.Interfaces;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.JobContextManager.Model.Interface;
using ESFA.DC.Logging.Interfaces;
using ExecutionContext = ESFA.DC.Logging.ExecutionContext;

namespace ESFA.DC.FRM.ReportService.Stateless.Handlers
{
    public class MessageHandler : IMessageHandler<JobContextMessage>
    {
        private readonly ILifetimeScope _parentLifeTimeScope;
        private readonly StatelessServiceContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHandler"/> class.
        /// Simple constructor for use by AutoFac testing, don't want to have to fake a @see StatelessServiceContext.
        /// </summary>
        /// <param name="parentLifeTimeScope">AutoFac scope.</param>
        /// <param name="jobContextMessageKeysMutator">jobContextMessageKeysMutator.</param>
        public MessageHandler(ILifetimeScope parentLifeTimeScope)
        {
            _parentLifeTimeScope = parentLifeTimeScope;
            _context = null;
        }

        public MessageHandler(ILifetimeScope parentLifeTimeScope, StatelessServiceContext context)
        {
            _parentLifeTimeScope = parentLifeTimeScope;
            _context = context;
        }

        public async Task<bool> HandleAsync(JobContextMessage jobContextMessage, CancellationToken cancellationToken)
        {
            try
            {
                using (var childLifeTimeScope = GetChildLifeTimeScope(jobContextMessage))
                {
                    var executionContext = (ExecutionContext)childLifeTimeScope.Resolve<IExecutionContext>();
                    executionContext.JobId = jobContextMessage.JobId.ToString();
                    var logger = childLifeTimeScope.Resolve<ILogger>();

                    logger.LogDebug("Started FRM Report Service");

                    var entryPoint = childLifeTimeScope.Resolve<EntryPoint>();
                    var result = await entryPoint.Callback(cancellationToken);

                    logger.LogDebug($"Completed FRM Report Service with result-{result}");
                    return result;
                }
            }
            catch (OutOfMemoryException oom)
            {
                Environment.FailFast("Report Service Out Of Memory", oom);
                throw;
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceMessage(_context, "Exception-{0}", ex.ToString());
                throw;
            }
        }

        public ILifetimeScope GetChildLifeTimeScope(JobContextMessage jobContextMessage)
        {
            return _parentLifeTimeScope.BeginLifetimeScope(c =>
            {
                c.RegisterInstance(jobContextMessage).As<IJobContextMessage>();
                c.RegisterType<ReportServiceContext>().As<IReportServiceContext>();

                c.RegisterType<EntryPoint>().InstancePerLifetimeScope();

                var reportServiceConfiguration = _parentLifeTimeScope.Resolve<IReportServiceConfiguration>();

                c.RegisterModule<ServicesModule>();
                c.RegisterModule<ReportsModule>();
                c.RegisterModule(new DataProviderModule(reportServiceConfiguration));

                var azureBlobStorageOptions = _parentLifeTimeScope.Resolve<IAzureStorageOptions>();
                c.RegisterInstance(new AzureStorageKeyValuePersistenceConfig(
                        azureBlobStorageOptions.AzureBlobConnectionString,
                        jobContextMessage.KeyValuePairs[JobContextMessageKey.Container].ToString()))
                    .As<IAzureStorageKeyValuePersistenceServiceConfig>();
            });
        }
    }
}

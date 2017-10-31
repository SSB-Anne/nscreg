using System;
using System.Linq;
using System.Threading;
using nscreg.Data;
using nscreg.Server.Common.Services.Contracts;
using nscreg.Server.Common.Services.StatUnit;
using nscreg.ServicesUtils.Interfaces;
using nscreg.Utilities.Configuration.DBMandatoryFields;
using nscreg.Utilities.Configuration.StatUnitAnalysis;

namespace nscreg.AnalysisService.Jobs
{
    /// <summary>
    /// Класс работы анализа
    /// </summary>
    internal class AnalysisJob : IJob
    {
        private readonly NSCRegDbContext _ctx;
        public int Interval { get; }
        private readonly IStatUnitAnalyzeService _analysisService;

        public AnalysisJob(NSCRegDbContext ctx, StatUnitAnalysisRules analysisRules, DbMandatoryFields dbMandatoryFields, int dequeueInterval)
        {
            _ctx = ctx;
            _analysisService = new AnalyzeService(ctx, analysisRules, dbMandatoryFields);
            Interval = dequeueInterval;
        }

        /// <summary>
        /// Analysis start
        /// </summary>
        /// <param name="cancellationToken"></param>
        public void Execute(CancellationToken cancellationToken)
        {
            var analysisQueue = _ctx.AnalysisQueues.LastOrDefault(aq => aq.ServerEndPeriod == null);
            if (analysisQueue == null) return;

            _analysisService.AnalyzeStatUnits(analysisQueue);
        }

        /// <summary>
        /// Метод обработчик исключений
        /// </summary>
        public void OnException(Exception e)
        {
            throw new NotImplementedException();
        }
    }
}

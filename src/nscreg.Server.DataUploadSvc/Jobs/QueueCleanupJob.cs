﻿using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using nscreg.Data;
using nscreg.Services.DataSources;
using nscreg.ServicesUtils.Interfaces;

namespace nscreg.Server.DataUploadSvc.Jobs
{
    public class QueueCleanupJob : IJob
    {
        public int Interval { get; }

        private readonly int _timeout;
        private readonly ILogger _logger;
        private readonly QueueService _queueSvc;

        public QueueCleanupJob(NSCRegDbContext ctx, int dequeueInterval, int timeout, ILogger logger)
        {
            Interval = dequeueInterval;
            _queueSvc = new QueueService(ctx);
            _timeout = timeout;
            _logger = logger;
        }

        public async void Execute(CancellationToken cancellationToken)
        {
            _logger.LogInformation("cleaning up...");
            await _queueSvc.ResetDequeuedByTimeout(_timeout);
        }

        public void OnException(Exception e)
        {
            _logger.LogError("cleaning up exception {0}", e);
            throw new NotImplementedException();
        }
    }
}

using Maersk.Sorting.Api.Utility;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.BackgroundTask
{
    public class SortService : BackgroundService
    {
        private readonly ILogger<SortService> _logger;
        private readonly ISortJobProcessor _sortJobProcessor;

        public SortService(ILogger<SortService> logger, ISortJobProcessor sortJobProcessor)
        {
            _logger = logger;
            _sortJobProcessor = sortJobProcessor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Sort Service starting");
            stoppingToken.Register(() =>
            _logger.LogDebug($" Sort Service background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                if(MemoryQueue.GetPendingQueue().Count>0)
                {
                    foreach (SortJob pendingJob in MemoryQueue.GetPendingQueue())
                    {
                        SortJob completedJob=await _sortJobProcessor.Process(pendingJob);
                        MemoryQueue.UpdateQueue(completedJob);
                    }
                }
                await Task.Delay(5000, stoppingToken);
            }

            _logger.LogDebug("Sort Service background task is stopping.");
        }
    }
}

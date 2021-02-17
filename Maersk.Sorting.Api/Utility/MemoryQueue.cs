using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.Utility
{
    public static class MemoryQueue
    {
        private static List<SortJob> jobs;

        static MemoryQueue()
        {
            jobs = new List<SortJob>();
        }

        public static List<SortJob> GetAllQueue()
        {
            return jobs;
        }

        public static List<SortJob> GetPendingQueue()
        {
            return jobs.Where(i => i.Status == SortJobStatus.Pending).ToList();
        }

        public static SortJob GetJob(Guid id)
        {
            SortJob job = jobs.Where(i => i.Id == id).FirstOrDefault();
            return job;
        }

        public static void InsertToQueue(SortJob sortJob)
        {
            jobs.Add(sortJob);
        }

        public static void UpdateQueue(SortJob sortJob)
        {
            SortJob job = jobs.Where(i => i.Id == sortJob.Id).FirstOrDefault();
            if (job != null)
            {
                jobs.Remove(job);
                jobs.Add(sortJob);
            }
        }

        public static void RemoveFromQueue(Guid Id)
        {
            SortJob job = jobs.Where(i => i.Id == Id).FirstOrDefault();
            if(job!=null)
            {
                jobs.Remove(job);
            }
        }
    }
}

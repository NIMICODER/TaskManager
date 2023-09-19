using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager_Models.Configuration
{
    public class QueueConfiguration
    {
        public string NotificationQueueUrl { get; set; } = null!;
        public int TimeOutInSeconds { get; set; }
        public int VisibilityInSeconds { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager_Services.Domains.ServiceFactory
{
    public interface IServiceFactory
    {
        T GetService<T>() where T : class;
    }
}

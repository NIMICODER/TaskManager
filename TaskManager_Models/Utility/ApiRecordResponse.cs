using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager_Models.Utility
{
    public record ApiRecordResponse<T>(bool IsSuccessful, string? Message, T? Data) where T : BaseRecord;

    public record ApiResponse<T>(bool IsSuccessful, string? Message, T? Data) where T : class;

    public record ApiResponse(bool IsSuccessful, string? Message);
}

namespace TaskManager_Models.Utility
{
    public record PaginationResponse<T>(int PageSize, int CurrentPage, int TotalPages, int TotalRecords, IEnumerable<T> Records) : BaseRecord where T : BaseRecord;
}

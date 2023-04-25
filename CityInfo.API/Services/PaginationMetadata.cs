namespace CityInfo.API.Services
{
    public class PaginationMetadata
    {
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; set; }
        public int PageSize { get; set; }
        public int CurrenPage { get; set; }

        public PaginationMetadata(int totalItemCount, int pageSize, int currentPage)
        {
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            CurrenPage = currentPage;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)PageSize);
        }
    }
}

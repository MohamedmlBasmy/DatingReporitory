namespace DatingApp.API.Models
{
    public class UserParams
    {
        public int Id { get; set; }
        public int PageNumber { get; set; } = 1;
        private int pageSize = 6;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 90;
        public string SortType { get; set; }
        public bool Likees { get; set; }
        public bool Likers { get; set; }

    }
}
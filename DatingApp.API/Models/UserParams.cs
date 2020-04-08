namespace DatingApp.API.Models
{
    public class UserParams
    {

        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

    }
}
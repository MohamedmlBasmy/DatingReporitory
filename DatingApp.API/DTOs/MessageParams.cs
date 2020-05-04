namespace DatingApp.API.DTOs
{
    public class MessageParams
    {
        public int Id { get; set; }
        public int PageNumber { get; set; } = 1;
        private int pageSize = 6;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        public string MessageType { get; set; }
    }
}
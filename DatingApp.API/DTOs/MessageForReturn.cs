using System;

namespace DatingApp.API.DTOs
{
    public class MessageForReturn
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderKnownAs { get; set; }
        public int RecipientId { get; set; }
        public string RecipientKnownAs { get; set; }
        public string Content { get; set; }
        public DateTime MessageSent { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public bool IsRecepeientDeleted { get; set; }
        public bool IsSenderDeleted { get; set; }
    }
}
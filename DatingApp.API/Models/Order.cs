using DatingApp.API.Data;

namespace DatingApp.API.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public User UserOrder { get; set; }
    }
}
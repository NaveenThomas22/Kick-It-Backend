namespace practise.Models
{
    public class Cart
    { 
        public Guid CartId { get; set; }
        public Guid userId { get; set; }
        public virtual  User User { get; set; }
        public virtual List <CartItems>  cartItems { get; set; }

    }
}

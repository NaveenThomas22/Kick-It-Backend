namespace practise.Models
{
    public class WishList
    {
        public Guid  WishlistId{ get; set; }

        public Guid UserID { get; set; }

        public  Guid ProdectId { get; set; }

        public User User{ get; set; }

    }

}

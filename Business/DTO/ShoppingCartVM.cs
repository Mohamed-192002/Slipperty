namespace Business.DTO
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCartDTO>? shoppingCartDTOs { get; set; }
        public IEnumerable<UserAddressDTO>? UserAddressDTOs { get; set; }
        public IEnumerable<UserPaymentMethodDTO>? UserPaymentMethodDTOs { get; set; }

    }
}

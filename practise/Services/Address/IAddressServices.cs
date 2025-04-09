using practise.DTO.Address;

namespace practise.Services.Address
{
    public interface IAddressServices
    {
        public Task<bool> CreateAddress(AddressCreateDTO  newAddress , Guid userid);
        public Task<List<AddressResDTO>> GetAllAddress(Guid userid);

        public Task<bool> DeleteAddress(Guid userid,Guid AddressId);
    }
}

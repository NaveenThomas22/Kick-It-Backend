using AutoMapper;
using Microsoft.EntityFrameworkCore;
using practise.Data;
using practise.DTO.Address;

namespace practise.Services.Address
{
    public class AddressServices : IAddressServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AddressServices(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
     

        }

        public async Task<bool> CreateAddress(AddressCreateDTO newAddress, Guid userid)
        {
            try
            {
                if (userid == Guid.Empty)
                {
                    throw new ArgumentException("user not found");
                }

                if (newAddress == null)
                {
                    throw new ArgumentException("The addrss cannot be null");
                }

                var userAddress = await _context.Address
                     .Where(s => s.Userid == userid)
                     .ToListAsync();

                if (userAddress.Count == 5)
                {
                    throw new ArgumentException(" minimus address limit is 5");
                }

                var address = new practise.Models.Address
                {
                    Userid = userid,
                    FullName = newAddress.FullName,
                    PhoneNumber = newAddress.PhoneNumber,
                    Pincode = newAddress.Pincode,
                    HouseName = newAddress.HouseName,
                    Place = newAddress.Place,
                    PostOffice = newAddress.PostOffice,
                    LandMark = newAddress.LandMark,
                };

                await _context.AddAsync(address);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }

        public async Task<bool> DeleteAddress(Guid userid ,Guid AddressId)
        {
            try
            {

         if (userid == null)
         {
                throw new ArgumentException("user not found ");
         }

           var address = await _context.Address
           .FirstOrDefaultAsync(u => u.AddressId == AddressId && u.Userid == userid);

            _context.Address.Remove(address);
            await _context.SaveChangesAsync();
            return true;
                
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<AddressResDTO>> GetAllAddress(Guid userid)
        {
            try
            {

            if (userid == Guid.Empty)
            {
                throw new ArgumentException("user not found ");
            }
            var address = await _context.Address
                .Where(a => a.Userid == userid)
                .ToListAsync();
            if (address != null)
            {
                return _mapper.Map<List<AddressResDTO>>(address);

            }
            return new List<AddressResDTO>();
            } catch (Exception ex)
            {
                  throw new Exception (ex.Message);
            }
        }
    }
}
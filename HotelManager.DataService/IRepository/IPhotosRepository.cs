using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.Photo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DataService.IRepository
{
    public interface IPhotosRepository : IGenericRepository<Photos>
    {
        Task<bool> AddPhoto(Room room, IFormFile image);
        Task<bool> UpdatePhotos(UpdatePhoto request);
        Task<bool> DeletePhotos(List<DeletePhotoRequest> request);
    }
}

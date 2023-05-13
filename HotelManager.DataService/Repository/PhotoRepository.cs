using HotelManager.DataService.Data;
using HotelManager.DataService.IRepository;
using HotelManager.Entities.DbSet;
using HotelManager.Entities.Dto.Incoming.Photo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace HotelManager.DataService.Repository
{
    public class PhotoRepository : GenericRepository<Photos>, IPhotosRepository
    {
        public PhotoRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {

        }

        public async Task<bool> DeletePhotos(List<DeletePhotoRequest> request)
        {
            try
            {
                if (request?.Count > 0)
                {
                    foreach (var image in request)
                    {
                        var exsting = await _dbSet.FindAsync(image.Id);

                        if(exsting != null)
                        {
                             _dbSet.Remove(exsting);
                            System.IO.File.Delete(exsting.PhotoUrl);
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(PhotoRepository)} CreatePhoto Method has generated an error");
                return false;
            }
        }

        public async Task<bool> UpdatePhotos(UpdatePhoto request)
        {
            try
            {
                if (request.Images != null && request.id > 0)
                {
                    var existing = await _context.Rooms.FindAsync(request.id);
                    
                    if(existing == null)
                    {
                        return false;
                    }

                    foreach (var image in request.Images)
                    {
                        var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PhotosRoom");

                        var fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
                        var file = Path.Combine(directory, fileName);

                        using (var streamFile = new FileStream(file, FileMode.Create))
                        {
                            await image.CopyToAsync(streamFile);
                        }
                        var photo = new Photos()
                        {
                            RoomId = existing.Id,
                            PhotoName = "/Contents/" + "PhotosRoom/" + fileName,
                            PhotoUrl = file
                        };

                        var isCreaate = await _dbSet.AddAsync(photo);
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(PhotoRepository)} CreatePhoto Method has generated an error");
                return false;
            }
        }

        public async Task<bool> AddPhoto(Room room, IFormFile image)
        {
            try
            {
                if (image != null && room != null)
                {
                    var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PhotosRoom");

                    var fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
                    var file = Path.Combine(directory, fileName);

                    using (var streamFile = new FileStream(file, FileMode.Create))
                    {
                        await image.CopyToAsync(streamFile);
                    }
                   var photo = new Photos()
                   {
                       Room = room,
                       PhotoName = "/Contents/" + "PhotosRoom/" + fileName,
                       PhotoUrl = file
                   };

                    var isCreaate = await _dbSet.AddAsync(photo);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(PhotoRepository)} CreatePhoto Method has generated an error");
                return false;
            }
        }

    }
}

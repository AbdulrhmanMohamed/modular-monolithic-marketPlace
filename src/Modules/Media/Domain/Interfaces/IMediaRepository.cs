namespace Modules.Media.Domain.Interfaces;

using Modules.Media.Domain.Entities;

public interface IMediaRepository
{
    Task<MediaFile> UploadAsync(MediaFile file);
    Task<MediaFile?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
}
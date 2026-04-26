namespace Modules.Media.Application.Results;

public record MediaResult(
    bool Success,
    MediaDto? Media,
    string? Error
)
{
    public static MediaResult Ok(MediaDto media) => new(true, media, null);
    public static MediaResult Bad(string error) => new(false, null, error);
};

public record MediaDto(
    int Id,
    string FileName,
    string Url,
    string ContentType,
    long Size,
    int? ProductId
);
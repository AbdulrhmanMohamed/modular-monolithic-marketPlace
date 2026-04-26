namespace Modules.Media.Application.Handlers;

using MediatR;
using Modules.Media.Application.Commands;
using Modules.Media.Application.Queries;
using Modules.Media.Application.Results;
using Modules.Media.Domain.Entities;
using Modules.Media.Domain.Interfaces;

public class UploadMediaHandler : IRequestHandler<UploadMediaCommand, MediaResult>
{
    private readonly IMediaRepository _repository;

    public UploadMediaHandler(IMediaRepository repository)
    {
        _repository = repository;
    }

    public async Task<MediaResult> Handle(UploadMediaCommand request, CancellationToken cancellationToken)
    {
        var file = new MediaFile
        {
            FileName = request.FileName,
            Url = $"/uploads/{request.FileName}",
            ContentType = request.ContentType,
            Size = request.Size,
            ProductId = request.ProductId
        };

        var created = await _repository.UploadAsync(file);

        var dto = new MediaDto(
            created.Id,
            created.FileName,
            created.Url,
            created.ContentType,
            created.Size,
            created.ProductId
        );

        return MediaResult.Ok(dto);
    }
}

public class DeleteMediaHandler : IRequestHandler<DeleteMediaCommand, bool>
{
    private readonly IMediaRepository _repository;

    public DeleteMediaHandler(IMediaRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteMediaCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.MediaId);
    }
}

public class GetMediaByIdHandler : IRequestHandler<GetMediaByIdQuery, MediaResult>
{
    private readonly IMediaRepository _repository;

    public GetMediaByIdHandler(IMediaRepository repository)
    {
        _repository = repository;
    }

    public async Task<MediaResult> Handle(GetMediaByIdQuery request, CancellationToken cancellationToken)
    {
        var file = await _repository.GetByIdAsync(request.MediaId);

        if (file is null)
            return MediaResult.Bad("Media not found");

        var dto = new MediaDto(
            file.Id,
            file.FileName,
            file.Url,
            file.ContentType,
            file.Size,
            file.ProductId
        );

        return MediaResult.Ok(dto);
    }
}
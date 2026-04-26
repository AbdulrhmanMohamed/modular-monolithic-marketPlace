namespace Modules.Media.Application.Commands;

using MediatR;
using Modules.Media.Application.Results;

public record UploadMediaCommand(
    string FileName,
    string ContentType,
    long Size,
    int? ProductId
) : IRequest<MediaResult>;

public record DeleteMediaCommand(
    int MediaId
) : IRequest<bool>;
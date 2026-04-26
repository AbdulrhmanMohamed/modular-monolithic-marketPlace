namespace Modules.Media.Application.Queries;

using MediatR;
using Modules.Media.Application.Results;

public record GetMediaByIdQuery(
    int MediaId
) : IRequest<MediaResult>;
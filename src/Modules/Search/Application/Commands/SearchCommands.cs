namespace Modules.Search.Application.Commands;

using MediatR;
using Modules.Search.Application.Results;
using Modules.Search.Domain.Entities;
using Modules.Search.Domain.Interfaces;

public record SaveSearchHistoryCommand(
    int UserId,
    string Query
) : IRequest<SearchHistoryResult>;

public class SaveSearchHistoryHandler : IRequestHandler<SaveSearchHistoryCommand, SearchHistoryResult>
{
    private readonly ISearchRepository _repository;

    public SaveSearchHistoryHandler(ISearchRepository repository)
    {
        _repository = repository;
    }

    public async Task<SearchHistoryResult> Handle(SaveSearchHistoryCommand request, CancellationToken cancellationToken)
    {
        var history = new SearchHistory
        {
            UserId = request.UserId,
            Query = request.Query
        };

        var created = await _repository.CreateAsync(history);

        var dto = new SearchHistoryDto(created.Id, created.UserId, created.Query, created.CreatedAt);

        return SearchHistoryResult.Ok(dto);
    }
}
namespace Modules.Search.Application.Queries;

using MediatR;
using Modules.Search.Application.Results;

public record SearchProductsQuery(
    string Query,
    int Page,
    int PageSize
) : IRequest<SearchResult>;

public record GetSearchHistoryQuery(
    int UserId
) : IRequest<SearchHistoryListResult>;
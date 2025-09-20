namespace GestaoLivros.Application;

public record PaginatedResponse<T>  where T : class
{
    public IList<T> Data { get; init; } = [];
    public int? PageSize  { get; init; }
    public int? Page  { get; init; }
    public int? TotalItems  { get; init; }
    public int? TotalPages  { get; init; }
}
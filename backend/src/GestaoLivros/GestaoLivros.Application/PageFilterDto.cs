namespace GestaoLivros.Application;

public class PageFilterDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string Search { get; set; } = string.Empty;
}
namespace Filters;

public class SortModel
{
    public string ColId { get; set; }
    public SortType Sort { get; set; }
}

public enum SortType
{
    Asc,
    Desc,
}
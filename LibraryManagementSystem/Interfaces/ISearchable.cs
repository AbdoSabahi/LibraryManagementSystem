namespace LibraryManagementSystem.Interfaces
{
    public interface ISearchable
    {
        bool MatchesQuery(string query);
    }
}
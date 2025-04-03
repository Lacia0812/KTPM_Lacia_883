using Lab1_THKTPM.Navigation;

namespace Lab1_THKTPM
{
    public interface INavigationCacheOperations
    {
        Task<NavigationMenu> GetNavigationCacheAsync();
        Task CreateNavigationCacheAsync();
    }
}

namespace Entities.Helpers
{
    using System.Linq;
    
    public interface ISortHelper<T>
    {
         IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString);
    }
}
using System.Collections.Generic;

namespace PetShop.Core.Search
{
    public interface ISearchEngine

    {
        List<T> Search<T>(List<T> searchList, string searchTitle) where T: ISearchAble;
    }
}

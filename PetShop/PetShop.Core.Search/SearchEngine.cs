using System;
using System.Collections.Generic;

namespace PetShop.Core.Search
{
    public class SearchEngine : ISearchEngine
    {
        public List<T> Search<T>(List<T> searchList, string searchTitle) where T : ISearchAble
        {
            string[] searchTerms = searchTitle.ToLower().Split('%');
            List<T> matches = new List<T>();

            foreach (T entity in searchList)
            {
                int size = 0;
                if (!searchTitle.StartsWith('%'))
                {
                    if (!entity.searchValue().ToLower().StartsWith(searchTerms[0]))
                    {
                        continue;
                    }
                }
                else
                {
                    size++;
                }

                String entityTitle = entity.searchValue().ToLower();
                bool candidateMeetsCriteria = true;

                for (int i = size; i < searchTerms.Length; i++)
                {

                    if (entityTitle.Contains(searchTerms[i]))
                    {
                        int index = entityTitle.IndexOf(searchTerms[i]);
                        entityTitle = entityTitle.Substring(index + searchTerms[i].Length);
                    }
                    else
                    {
                        candidateMeetsCriteria = false;
                        break;
                    }
                }
                    if (searchTitle.EndsWith("%") && candidateMeetsCriteria)
                    {
                        if (entityTitle.Length > 0)
                        {
                            matches.Add(entity);
                        }
                    }
                    else
                    {
                        if (entityTitle.Length == 0 && candidateMeetsCriteria)
                        {
                            matches.Add(entity);
                        }
                    }
            }
            return matches;
        }
    }
}

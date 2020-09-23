using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using PetShop.Core.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PetShop.Core.ApplicationService.Impl
{
    public class ColorService : IColorService
    {
        private IColorRepository ColorRepository;
        private ISearchEngine SearchEngine;

        public ColorService(IColorRepository colorRepository, ISearchEngine searchEngine)
        {
            this.ColorRepository = colorRepository;
            this.SearchEngine = searchEngine;
        }

        public Color CreateColor(string color)
        {
            if (string.IsNullOrEmpty(color))
            {
                throw new ArgumentException("Entered color description too short");
            }

            return new Color { ColorDescription = color };
        }

        public Color AddColor(Color color)
        {
            if (color != null)
            {
                return ColorRepository.AddColor(color);
            }
            return null;
        }

        public List<Color> GetAllColors()
        {
            return ColorRepository.ReadColors().ToList();
        }

        public List<Color> GetColorsFilterSearch(Filter filter)
        {
            if (filter.CurrentPage < 0 || filter.ItemsPrPage < 0)
            {
                throw new InvalidDataException("Page or items per page must be above zero");
            }

            IEnumerable<Color> colors = ColorRepository.ReadColorsFilterSearch(filter);

            if (!string.IsNullOrEmpty(filter.Name))
            {
                colors = SearchEngine.Search<Color>(colors.ToList(), filter.Name);
            }

            if (filter.CurrentPage > 0 && filter.ItemsPrPage > 0)
            {
                colors = colors.Skip((filter.CurrentPage - 1) * filter.ItemsPrPage).Take(filter.ItemsPrPage);
                if (colors.Count() == 0)
                {
                    throw new InvalidDataException("Index out of bounds");
                }
            }

            return colors.ToList();
        }

        public Color GetColorByID(int ID)
        {
            return ColorRepository.GetColorByID(ID);
        }

        public Color UpdateColor(Color color, int ID)
        {
            if (GetColorByID(ID) == null)
            {
                throw new ArgumentException("No color with such ID found");
            }
            if (color == null)
            {
                throw new ArgumentException("Updating color does not excist");
            }
            color.ID = ID;
            return ColorRepository.UpdateColor(color);
        }

        public Color DeleteColor(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentException("Incorrect ID entered");
            }
            if (GetColorByID(ID) == null)
            {
                throw new ArgumentException("No color with such ID found");
            }
            return ColorRepository.DeleteColor(ID);
        }
    }
}

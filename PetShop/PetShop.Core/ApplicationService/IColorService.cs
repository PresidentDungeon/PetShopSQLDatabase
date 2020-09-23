using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.ApplicationService
{
    public interface IColorService
    {
        Color CreateColor(string color);

        Color AddColor(Color color);

        List<Color> GetAllColors();

        List<Color> GetColorsFilterSearch(Filter filter);

        Color GetColorByID(int ID);

        Color UpdateColor(Color color, int ID);

        Color DeleteColor(int ID);
    }
}

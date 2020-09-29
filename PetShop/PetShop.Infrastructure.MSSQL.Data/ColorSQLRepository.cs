using Microsoft.EntityFrameworkCore;
using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetShop.Infrastructure.SQLLite.Data
{
    public class ColorSQLRepository : IColorRepository
    {
        private PetShopContext ctx;

        public ColorSQLRepository(PetShopContext ctx)
        {
            this.ctx = ctx;
        }

        public Color AddColor(Color color)
        {
            ctx.Attach(color).State = EntityState.Added;
            ctx.SaveChanges();
            return color;
        }
        public IEnumerable<Color> ReadColors()
        {
            return ctx.Colors.AsEnumerable();
        }
        public IEnumerable<Color> ReadColorsFilterSearch(Filter filter)
        {
            IQueryable<Color> colors = ctx.Colors.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                colors = from x in colors where x.ColorDescription.Contains(filter.Name) select x;
            }
            if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("asc"))
            {
                colors = from x in colors orderby x.ColorDescription select x;
            }
            else if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("desc"))
            {
                colors = from x in colors orderby x.ColorDescription descending select x;
            }

            return colors;
        }
        public Color GetColorByID(int ID)
        {
            return ctx.Colors.AsNoTracking().FirstOrDefault(x => x.ID == ID);
        }
        public Color UpdateColor(Color color)
        {
            ctx.Attach(color).State = EntityState.Modified;
            ctx.SaveChanges();
            return color;
        }
        public Color DeleteColor(int ID)
        {
            var deletedColor = ctx.Colors.Remove(GetColorByID(ID));
            ctx.SaveChanges();
            return deletedColor.Entity;
        }
    }
}

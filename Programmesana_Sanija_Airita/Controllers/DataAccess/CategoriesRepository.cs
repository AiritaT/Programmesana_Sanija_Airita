using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Programmesana_Sanija_Airita.Models;

namespace Programmesana_Sanija_Airita.Controllers.DataAccess
{
    public class CategoriesRepository : ConnectionClass
    {
        public CategoriesRepository(): base()
        {

        }
        public IQueryable<Category> GetCategories()
        {
            return Entity.Categories;
        }
    }
}
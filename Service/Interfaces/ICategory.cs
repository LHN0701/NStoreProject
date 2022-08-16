using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ICategory
    {
        GetAllCategory GetAll();

        Category GetById(int id);

        CategoryModel GetProductCategory(int id);
    }
}
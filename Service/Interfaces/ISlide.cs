using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ISlide
    {
        List<Slide> ListSlide(bool quantri = false);

        Slide SlideInfo(int id);

        int Create(SlideModel.Output.CreateSlide input);

        int Update(SlideModel.Output.UpdateSlide input);

        int Delete(SlideModel.Input.DeleteSlide input);
    }
}
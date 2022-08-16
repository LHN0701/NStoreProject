using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IPromotion
    {
        List<Promotion> GetAll();

        PromotionModel.PromotionBase GetById(int id);

        PromotionModel.Output.GetPromotionClient GetPromotionClient(PromotionModel.Input.GetPromotionClient request);

        NotetiModel CreatePromotion(PromotionModel.PromotionBase request);

        NotetiModel UpdatePromotion(PromotionModel.PromotionBase request);

        NotetiModel DeletePromotion(PromotionModel.Input.DeletePromotion request);
    }
}
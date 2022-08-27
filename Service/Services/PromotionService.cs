using Service.Interfaces;
using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class PromotionService : IPromotion
    {
        private readonly NStoreContext _context;

        public PromotionService(NStoreContext context)
        {
            _context = context;
        }

        public List<Promotion> GetAll()
        {
            var result = _context.Promotions.ToList();
            return result;
        }

        public PromotionModel.PromotionBase GetById(int id)
        {
            var promotion = _context.Promotions.FirstOrDefault(x => x.Id.Equals(id));
            return new PromotionModel.PromotionBase()
            {
                Id = promotion.Id,
                ApplyForAll = promotion.ApplyForAll,
                DiscountAmount = promotion.DiscountAmount,
                DiscountPercent = promotion.DiscountPercent,
                FromDate = promotion.FromDate,
                ToDate = promotion.ToDate,
                Name = promotion.Name
            };
        }

        public PromotionModel.Output.GetPromotionClient GetPromotionClient(PromotionModel.Input.GetPromotionClient request)
        {
            var promotion = _context.Promotions.FirstOrDefault(x => x.Name.Equals(request.Name) && x.ApplyForAll.Equals(true) && x.DiscountAmount != 0);

            if (promotion == null)
            {
                return new PromotionModel.Output.GetPromotionClient()
                {
                    IsSuccess = false,
                    Noteti = "Can not find code discount!"
                };
            }

            var member = _context.Members.FirstOrDefault(x => x.Id.Equals(request.IdMember));

            if (member != null && member.Promotions != null)
            {
                var listIdPromotion = member.Promotions.Split(new string[] { "," },
                                            StringSplitOptions.RemoveEmptyEntries).ToList();
                if (listIdPromotion.Contains(promotion.Id.ToString()))
                {
                    return new PromotionModel.Output.GetPromotionClient()
                    {
                        IsSuccess = false,
                        Noteti = "Your account used discount!"
                    };
                }
            }

            if (DateTime.Compare(DateTime.Now, promotion.ToDate) > 0 || promotion == null)
            {
                return new PromotionModel.Output.GetPromotionClient()
                {
                    IsSuccess = false,
                    Noteti = "Promotion expire!"
                };
            }

            var result = new PromotionModel.Output.GetPromotionClient()
            {
                IsSuccess = true,
                Noteti = $"Discount {promotion.DiscountPercent}% ",
                DiscountPercent = promotion.DiscountPercent,
                Id = promotion.Id
            };
            return result;
        }

        public NotetiModel CreatePromotion(PromotionModel.PromotionBase request)
        {
            var result = new NotetiModel();
            var promotion = new Promotion()
            {
                ApplyForAll = request.ApplyForAll,
                DiscountAmount = request.DiscountAmount,
                DiscountPercent = request.DiscountPercent,
                FromDate = request.FromDate,
                Name = request.Name,
                ToDate = request.ToDate
            };
            _context.Promotions.Add(promotion);
            var change = _context.SaveChanges();
            if (change > 0)
            {
                result.IsSuccess = true;
                result.Noteti = "Create promotion success!";
            }
            else
            {
                result.IsSuccess = false;
                result.Noteti = "Create promotion fail!";
            }

            return result;
        }

        public NotetiModel UpdatePromotion(PromotionModel.PromotionBase request)
        {
            var promotion = _context.Promotions.FirstOrDefault(x => x.Id.Equals(request.Id));

            var result = new NotetiModel();

            promotion.ApplyForAll = request.ApplyForAll;
            promotion.DiscountAmount = request.DiscountAmount;
            promotion.DiscountPercent = request.DiscountPercent;
            promotion.FromDate = request.FromDate;
            promotion.Name = request.Name;
            promotion.ToDate = request.ToDate;

            var change = _context.SaveChanges();
            if (change > 0)
            {
                result.IsSuccess = true;
                result.Noteti = "Update promotion success!";
            }
            else
            {
                result.IsSuccess = false;
                result.Noteti = "Update promotion fail!";
            }

            return result;
        }

        public NotetiModel DeletePromotion(PromotionModel.Input.DeletePromotion request)
        {
            var result = new NotetiModel();
            var promotion = _context.Promotions.FirstOrDefault(x => x.Id.Equals(request.Id));
            if (promotion != null)
            {
                _context.Promotions.Remove(promotion);
            }
            var change = _context.SaveChanges();
            if (change > 0)
            {
                result.IsSuccess = true;
                result.Noteti = "Delete promotion success!";
            }
            else
            {
                result.IsSuccess = false;
                result.Noteti = "Delete promotion fail!";
            }

            return result;
        }
    }
}
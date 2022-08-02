using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Service.Interfaces;
using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class SlideService : ISlide
    {
        private readonly NStoreContext _context;
        private readonly IHostEnvironment _hostingEnvironment;

        public SlideService(NStoreContext context, IHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public List<Slide> ListSlide(bool Quantri = false)
        {
            List<Slide> slideBanners;
            if (Quantri)
            {
                slideBanners = _context.Slides.ToList();
            }
            else
            {
                slideBanners = _context.Slides.Where(x => x.Active).ToList();
            }
            return slideBanners;
        }

        public Slide SlideInfo(int id)
        {
            return _context.Slides.FirstOrDefault(x => x.Id.Equals(id));
        }

        public int Create(SlideModel.Output.CreateSlide input)
        {
            if (input.ThumbnailImage != null)
            {
                var filename = input.ThumbnailImage.FileName;
                var filepath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot\\Picture\\Slides", filename);
                var imagepath = "/Picture/Slides/" + filename;
                var slide = new Slide()
                {
                    Name = input.Name,
                    Url = input.Url,
                    Active = input.Active,
                    Picture = imagepath
                };
                input.ThumbnailImage.CopyTo(new FileStream(filepath, FileMode.Create));
                _context.Slides.Add(slide);
                _context.SaveChanges();
                return slide.Id;
            }
            return 0;
        }

        public int Update(SlideModel.Output.UpdateSlide input)
        {
            var slide = _context.Slides.FirstOrDefault(x => x.Id.Equals(input.Id));
            if (input != null)
            {
                slide.Name = input.Name;
                slide.Url = input.Url;
                slide.Active = input.Active;

                var filepath = "";
                if (input.ThumbnailImage != null)
                {
                    var filename = input.ThumbnailImage.FileName;
                    filepath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot\\Picture\\Slides", filename);
                    var imagepath = "/Picture/Slides/" + filename;
                    slide.Picture = imagepath;
                }

                if (filepath != "") input.ThumbnailImage.CopyTo(new FileStream(filepath, FileMode.Create));
                _context.SaveChanges();
                return slide.Id;
            }
            return 0;
        }

        public int Delete(SlideModel.Input.DeleteSlide input)
        {
            var slide = _context.Slides.FirstOrDefault(x => x.Id.Equals(input.Id));
            if (slide != null)
            {
                var filePath = _hostingEnvironment.ContentRootPath + "\\wwwroot" + slide.Picture.Replace("/", "\\");

                //Garbage Collection issue.
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                File.Delete(filePath);

                if (filePath != "") System.IO.File.Delete(filePath);
                _context.Slides.Remove(slide);
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }
    }
}
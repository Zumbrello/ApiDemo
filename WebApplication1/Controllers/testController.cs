using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

    [ApiController]
    [Route("[controller]/[action]")]
public class testController:Controller
{
    [HttpPost]
    public IActionResult addImage(IFormFile file)
    {
        Stream stream = System.IO.File.Open("stream", FileMode.OpenOrCreate);
        stream.Close();
        MemoryStream memoryStream = new MemoryStream();
        file.CopyToAsync(memoryStream);
        byte[] arrBytes = memoryStream.ToArray();
        using (var context = new TestDatabaseContext())
        {
            Newtable image = new Newtable()
            {
                Identify = Guid.NewGuid().ToString("N"),
                Image = arrBytes
            };
            context.Newtables.Add(image);
            context.SaveChanges();
            
        }

        return Ok("Изображение добавлено");
    }

    [HttpGet]
    public IActionResult getImage(int id)
    {
        using (var context = new TestDatabaseContext())
        {
            var image = context.Newtables.FirstOrDefault(i => i.Id == id).Image;
            if (image != null)
            {
                var imageData = image;
                return File(imageData, "image/png");
            }
            else
            {
                return Ok("Изображениен не найдено");
            }
        }
        
    }

}
﻿using codepulse.API.Modells.Domain;
using System.Net;

namespace codepulse.API.Repositories.Interface
{
    public interface IImageRepo
    {
       Task<BlogImage> Upload(IFormFile file, BlogImage blogimage);
    }
}

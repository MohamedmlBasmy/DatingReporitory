using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Route("api/[Controller]/{id}/upload")]
    [ApiController]
    [Authorize]
    public class PhotosController : ControllerBase
    {
        private readonly DatingRepository datingRepository;
        private readonly IOptions<CloudinarySettings> _cloudinarySettings;

        private Cloudinary _cloudinary;
        private readonly IMapper _mapper;

        public PhotosController(DatingRepository datingRepository,
        IOptions<CloudinarySettings> cloudinarySettings, IMapper mapper)
        {
            this.datingRepository = datingRepository;

            this._cloudinarySettings = cloudinarySettings;
            this._mapper = mapper;
            Account account = new Account(
                _cloudinarySettings.Value.AccountName,
                _cloudinarySettings.Value.ApiKey,
                _cloudinarySettings.Value.ApiSecrets
            );

            this._cloudinary = new Cloudinary(account);
        }

        //public Cloudinary Cloudinary { get => _cloudinary; set => _cloudinary = value; }

        [HttpPost]
        public async Task<IActionResult> Upload(int userId, PhotoForUpload photoForUpload)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                Unauthorized("Sorry, You are not Authorized");
            }

            var userFromRepo = await this.datingRepository.GetUser(userId);

            var file = photoForUpload.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var parameters = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream)
                    };
                    uploadResult = this._cloudinary.Upload(parameters);
                }
            }
            photoForUpload.Url = uploadResult.Uri.ToString();
            photoForUpload.PublicId = uploadResult.PublicId;

            var photo =  this._mapper.Map<Photo>(photoForUpload);

            if (!userFromRepo.Photos.Any(photo => photo.IsMain))
            {
                photo.IsMain = true;
            }
            userFromRepo.Photos.Add(photo);

            if(await datingRepository.SaveAll())
            {
                // return CreatedAtRoute()
                return Ok();
            }else{
                return BadRequest("Could not add the photo");
            }
            
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HumanResource.Data;
using HumanResource.DTOS;
using HumanResource.Halpers;
using HumanResource.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HumanResource.Controllers
{
    [Authorize]
    [Route("api/users/{userid}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudanarySettings> _cloudanaryConfig;
        private readonly Cloudinary _cloudanary;

        public PhotosController(IDatingRepository repo, IMapper mapper,
            IOptions<CloudanarySettings> cloudanaryConfig)
        {
            _repo = repo;
            _mapper = mapper;
            _cloudanaryConfig = cloudanaryConfig;

            Account acc = new Account(
                _cloudanaryConfig.Value.CloudName,
                _cloudanaryConfig.Value.ApiKey,
                _cloudanaryConfig.Value.ApiSecret
                );

            _cloudanary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name ="GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);

        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, 
            [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            var file = photoForCreationDto.File;

            var uploadresult = new ImageUploadResult();

            if(file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500)
                        .Crop("fill").Gravity("face")
                    };

                    uploadresult = _cloudanary.Upload(uploadParams);

                };
            }

            photoForCreationDto.Url = uploadresult.Uri.ToString();
            photoForCreationDto.PublicId = uploadresult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);

            if (!userFromRepo.Photos.Any(u => u.IsMain))
                photo.IsMain = true;

            userFromRepo.Photos.Add(photo);

            if(await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _repo.GetUser(userId);

            if (!user.Photos.Any(p => p.Id == id))
                 return Unauthorized();

            var photFromRepo = await _repo.GetPhoto(id);

            if (photFromRepo.IsMain)
                return BadRequest("This is already the main photo");

            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);

            currentMainPhoto.IsMain = false;

            photFromRepo.IsMain = true;

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("Could not set photo to main");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _repo.GetUser(userId);

            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized();

            var photFromRepo = await _repo.GetPhoto(id);

            if (photFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo");

            if(photFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photFromRepo.PublicId);

                var result = _cloudanary.Destroy(deleteParams);

                if(result.Result == "ok")
                {
                    _repo.Delete(photFromRepo);
                }
            }

            if(photFromRepo.PublicId == null)
            {
                _repo.Delete(photFromRepo);
            }

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the photo");

        }

    }
}
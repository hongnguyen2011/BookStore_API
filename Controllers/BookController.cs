using BookStore_API.Data;
using BookStore_API.Models;
using BookStore_API.Models.Dto;
using BookStore_API.Services;
using BookStore_API.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookStore_API.Controllers
{
    [Route("api/Book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;
        public BookController(ApplicationDbContext db, IBlobService blobService) 
        {
            _db = db;
            _blobService = blobService;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            _response.Result = _db.BooksList;
            _response.StatusCode=HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name= "GetBook")]
        public async Task<IActionResult> GetBook(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            Book book = _db.BooksList.FirstOrDefault(u => u.Id == id);

            if (book == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.Result = book;
            _response.StatusCode=HttpStatusCode.OK;
            return Ok(_response);

        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm] BookCreateDTO BookCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (BookCreateDTO.File == null || BookCreateDTO.File.Length == 0)
                    {
                        return BadRequest();
                    }
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(BookCreateDTO.File.FileName)}";
                    Book BookToCreate = new()
                    {
                        Name = BookCreateDTO.Name,
                        Price = BookCreateDTO.Price,
                        Category = BookCreateDTO.Category,
                        SpecialTag = BookCreateDTO.SpecialTag,
                        Description = BookCreateDTO.Description,
                        Image = await _blobService.UploadBlob(fileName, SD.SD_Storage_Container, BookCreateDTO.File)
                    };
                    _db.BooksList.Add(BookToCreate);
                    _db.SaveChanges();
                    _response.Result = BookToCreate;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetMenuItem", new { id = BookToCreate.Id }, _response);

                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }

            return _response;
        }

    }
}

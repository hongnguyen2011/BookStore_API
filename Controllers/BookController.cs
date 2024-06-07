using BookStore_API.Data;
using BookStore_API.Models;
using BookStore_API.Models.Dto;
using BookStore_API.Services;
using BookStore_API.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static BookStore_API.Models.Dto.BookUpdateDTO;

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
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            Book book = _db.BooksList.FirstOrDefault(u => u.Id == id);

            if (book == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
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
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
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


        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateBook(int id, [FromForm] BookUpdateDTO BookUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (BookUpdateDTO == null || id != BookUpdateDTO.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    Book BookFromDb = await _db.BooksList.FindAsync(id);
                    if (BookFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }

                    BookFromDb.Name = BookUpdateDTO.Name;
                    BookFromDb.Price = BookUpdateDTO.Price;
                    BookFromDb.Category = BookUpdateDTO.Category;
                    BookFromDb.SpecialTag = BookUpdateDTO.SpecialTag;
                    BookFromDb.Description = BookUpdateDTO.Description;

                    if (BookUpdateDTO.File != null && BookUpdateDTO.File.Length > 0)
                    {
                        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(BookUpdateDTO.File.FileName)}";
                        await _blobService.DeleteBlob(BookFromDb.Image.Split('/').Last(), SD.SD_Storage_Container);
                        BookFromDb.Image = await _blobService.UploadBlob(fileName, SD.SD_Storage_Container, BookUpdateDTO.File);
                    }

                    _db.BooksList.Update(BookFromDb);
                    _db.SaveChanges();
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);

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


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteBook(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                Book menuItemFromDb = await _db.BooksList.FindAsync(id);
                if (menuItemFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }
                await _blobService.DeleteBlob(menuItemFromDb.Image.Split('/').Last(), SD.SD_Storage_Container);
                int milliseconds = 2000;
                Thread.Sleep(milliseconds);

                _db.BooksList.Remove(menuItemFromDb);
                _db.SaveChanges();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
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

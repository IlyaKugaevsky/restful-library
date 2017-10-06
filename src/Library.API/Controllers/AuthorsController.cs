using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library.API.Services;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthorsController : Controller
    {
        private ILibraryRepository _libraryRepository;

        public AuthorsController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }
        public IActionResult GetAuthors()
        {
            var authors = _libraryRepository.GetAuthors();
            return new JsonResult(authors);
        }
    }
}

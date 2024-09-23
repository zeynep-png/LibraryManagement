using LibraryManagement.Entities;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers
{
    public class AuthorController : Controller
    {
        // A static list of authors for demo purposes
        public static List<AuthorEntity> _authors = new List<AuthorEntity>()
        {
            new AuthorEntity { Id = 1, FirstName = "Harper", LastName = "Lee", DateOfBirth = new DateTime(1926, 4, 28) },
            new AuthorEntity { Id = 2, FirstName = "George", LastName = "Orwell", DateOfBirth = new DateTime(1903, 6, 25) },
            new AuthorEntity { Id = 3, FirstName = "F. Scott", LastName = "Fitzgerald", DateOfBirth = new DateTime(1896, 9, 24) },
            new AuthorEntity { Id = 4, FirstName = "Herman", LastName = "Melville", DateOfBirth = new DateTime(1819, 8, 1) },
            new AuthorEntity { Id = 5, FirstName = "Jane", LastName = "Austen", DateOfBirth = new DateTime(1775, 12, 16) },
            new AuthorEntity { Id = 6, FirstName = "J.D.", LastName = "Salinger", DateOfBirth = new DateTime(1919, 1, 1) },
            new AuthorEntity { Id = 7, FirstName = "J.R.R.", LastName = "Tolkien", DateOfBirth = new DateTime(1892, 1, 3) },
            new AuthorEntity { Id = 8, FirstName = "Ray", LastName = "Bradbury", DateOfBirth = new DateTime(1920, 8, 22) },
            new AuthorEntity { Id = 9, FirstName = "Aldous", LastName = "Huxley", DateOfBirth = new DateTime(1894, 7, 26) }
        };

        // Lists all authors
        public IActionResult List()
        {
            var viewModel = _authors.Select(x => new AuthorListViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth,
            }).ToList();

            return View(viewModel);
        }

        // Displays details of a specific author
        public IActionResult Details(int id)
        {
            var author = _authors.FirstOrDefault(x => x.Id == id);
            var viewModel = new AuthorDetailsViewModel
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                DateOfBirth = author.DateOfBirth,
            };
            return View(viewModel);
        }

        // Displays the form for creating a new author
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Authors = _authors;
            return View();
        }

        // Handles the form submission for creating a new author
        [HttpPost]
        public IActionResult Create(AuthorCreateViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View(formData);
            }
            int maxId = _authors.Max(x => x.Id);

            var newAuthor = new AuthorEntity()
            {
                Id = maxId + 1,
                FirstName = formData.FirstName,
                LastName = formData.LastName,
                DateOfBirth = formData.DateOfBirth,
            };
            _authors.Add(newAuthor);
            return RedirectToAction("List");
        }

        // Displays the form for editing an existing author
        public IActionResult Edit(int id)
        {
            var author = _authors.Find(x => x.Id == id);
            var viewModel = new AuthorEditViewModel()
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                DateOfBirth = author.DateOfBirth,
            };

            ViewBag.Authors = _authors;
            return View(viewModel);
        }

        // Handles the form submission for editing an existing author
        [HttpPost]
        public IActionResult Edit(AuthorEditViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            var author = _authors.Find(x => x.Id == formData.Id);
            author.FirstName = formData.FirstName;
            author.LastName = formData.LastName;
            author.DateOfBirth = formData.DateOfBirth;

            return RedirectToAction("List");
        }

        // Handles the deletion of an author
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var author = _authors.Find(x => x.Id == id);
            if (author != null)
            {
                _authors.Remove(author); // Removes the selected author
                return RedirectToAction("List");
            }
            return NotFound(); // Returns 404 if author is not found
        }
    }
}

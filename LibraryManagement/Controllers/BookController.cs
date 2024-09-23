using LibraryManagement.Entities;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryManagement.Controllers
{
    public class BookController : Controller
    {
        public static List<BookEntity> _books = new List<BookEntity>()
        {
            // Sample books initialized
            new BookEntity { Id = 1, Title = "To Kill a Mockingbird", AuthorId = 1, AuthorName = "Harper Lee", Genre = "Fiction", PublishDate = new DateTime(1960, 7, 11), ISBN = "978-0-06-112008-4", CopiesAvailable = 5 },
            new BookEntity { Id = 2, Title = "1984", AuthorId = 2, AuthorName = "George Orwell", Genre = "Dystopian", PublishDate = new DateTime(1949, 6, 8), ISBN = "978-0-452-28423-4", CopiesAvailable = 3 },
            new BookEntity { Id = 3, Title = "The Great Gatsby", AuthorId = 3, AuthorName = "F. Scott Fitzgerald", Genre = "Classic", PublishDate = new DateTime(1925, 4, 10), ISBN = "978-0-7432-7356-5", CopiesAvailable = 7 },
            new BookEntity { Id = 4, Title = "Moby Dick", AuthorId = 4, AuthorName = "Herman Melville", Genre = "Adventure", PublishDate = new DateTime(1851, 10, 18), ISBN = "978-0-14-243724-7", CopiesAvailable = 2 },
            new BookEntity { Id = 5, Title = "Pride and Prejudice", AuthorId = 5, AuthorName = "Jane Austen", Genre = "Romance", PublishDate = new DateTime(1813, 1, 28), ISBN = "978-0-19-953556-9", CopiesAvailable = 6 },
            new BookEntity { Id = 6, Title = "The Catcher in the Rye", AuthorId = 6, AuthorName = "J.D. Salinger", Genre = "Fiction", PublishDate = new DateTime(1951, 7, 16), ISBN = "978-0-316-76948-0", CopiesAvailable = 4 },
            new BookEntity { Id = 7, Title = "The Hobbit", AuthorId = 7, AuthorName = "J.R.R. Tolkien", Genre = "Fantasy", PublishDate = new DateTime(1937, 9, 21), ISBN = "978-0-618-00221-3", CopiesAvailable = 8 },
            new BookEntity { Id = 8, Title = "Fahrenheit 451", AuthorId = 8, AuthorName = "Ray Bradbury", Genre = "Dystopian", PublishDate = new DateTime(1953, 10, 19), ISBN = "978-1-4516-7331-9", CopiesAvailable = 3 },
            new BookEntity { Id = 9, Title = "Brave New World", AuthorId = 9, AuthorName = "Aldous Huxley", Genre = "Science Fiction", PublishDate = new DateTime(1932, 8, 30), ISBN = "978-0-06-085052-4", CopiesAvailable = 4 },
            new BookEntity { Id = 10, Title = "The Lord of the Rings", AuthorId = 7, AuthorName = "J.R.R. Tolkien", Genre = "Fantasy", PublishDate = new DateTime(1954, 7, 29), ISBN = "978-0-618-00222-0", CopiesAvailable = 9 }
        };

        public static List<AuthorEntity> _authors = new List<AuthorEntity>()
        {
            // Sample authors initialized
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

        // Action to display the list of books
        public IActionResult BookList()
        {
            var viewModel = _books.Where(x => x.IsDeleted == false)
                .Select(book => new BookListViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    AuthorName = book.AuthorName,  // Author name is taken directly from the book model
                    Genre = book.Genre,
                    PublishDate = book.PublishDate,
                    ISBN = book.ISBN,
                    CopiesAvailable = book.CopiesAvailable
                })
                .ToList();

            return View(viewModel);
        }

        // Action to display the details of a specific book
        public IActionResult Details(int id)
        {
            var book = _books.FirstOrDefault(x => x.Id == id);

            var viewModel = new BookDetailsViewModel
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,
                Genre = book.Genre,
                PublishDate = book.PublishDate,
                CopiesAvailable = book.CopiesAvailable,
                ISBN = book.ISBN,
            };

            return View(viewModel);
        }

        // GET action to display the create book form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST action to handle the creation of a new book
        [HttpPost]
        public IActionResult Create(BookCreateViewModel formData)
        {
            // Check model validation
            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            // Generate a new Id for the book (getting max Id from existing books and incrementing)
            int maxId = _books.Max(x => x.Id);

            // Create a new book entity
            var newBook = new BookEntity
            {
                Id = maxId + 1, // Assign new Id
                Title = formData.Title, // Title from the form
                Genre = formData.Genre, // Genre from the form
                PublishDate = formData.PublishDate, // Publish date
                ISBN = formData.ISBN, // ISBN
                CopiesAvailable = formData.CopiesAvailable, // Copies available
                AuthorId = formData.AuthorId,
                AuthorName = formData.AuthorName // Author assigned based on form data
            };

            // Add the new book to the book list
            _books.Add(newBook);

            // Redirect to the book list
            return RedirectToAction("BookList");
        }

        // Action to display the edit form for a specific book
        public IActionResult Edit(int id)
        {
            var book = _books.Find(x => x.Id == id);

            var viewModel = new BookEditViewModel()
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,
                Genre = book.Genre,
                AuthorName = book.AuthorName
            };
            ViewBag.Authors = _authors; // Provide the list of authors to the view
            return View(viewModel);
        }

        // Action to handle the deletion of a specific book
        public IActionResult Delete(int id)
        {
            var book = _books.Find(x => x.Id == id);
            if (book != null)
            {
                book.IsDeleted = true; // Mark the book as deleted
            }

            return RedirectToAction("BookList"); // Redirect to the book list
        }
    }
}

using LibraryManagement.Entities;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    public class HomeController : Controller
    {
        public static List<BookEntity> _books = new List<BookEntity>()
        {
            new BookEntity { Id = 1, Title = "To Kill a Mockingbird", AuthorId = 1, Genre = "Fiction", PublishDate = new DateTime(1960, 7, 11), ISBN = "978-0-06-112008-4", CopiesAvailable = 5 },
            new BookEntity { Id = 2, Title = "1984", AuthorId = 2, Genre = "Dystopian", PublishDate = new DateTime(1949, 6, 8), ISBN = "978-0-452-28423-4", CopiesAvailable = 3 },
            new BookEntity { Id = 3, Title = "The Great Gatsby", AuthorId = 3, Genre = "Classic", PublishDate = new DateTime(1925, 4, 10), ISBN = "978-0-7432-7356-5", CopiesAvailable = 7 },
            new BookEntity { Id = 4, Title = "Moby Dick", AuthorId = 4, Genre = "Adventure", PublishDate = new DateTime(1851, 10, 18), ISBN = "978-0-14-243724-7", CopiesAvailable = 2 },
            new BookEntity { Id = 5, Title = "Pride and Prejudice", AuthorId = 5, Genre = "Romance", PublishDate = new DateTime(1813, 1, 28), ISBN = "978-0-19-953556-9", CopiesAvailable = 6 },
            new BookEntity { Id = 6, Title = "The Catcher in the Rye", AuthorId = 6, Genre = "Fiction", PublishDate = new DateTime(1951, 7, 16), ISBN = "978-0-316-76948-0", CopiesAvailable = 4 },
            new BookEntity { Id = 7, Title = "The Hobbit", AuthorId = 7, Genre = "Fantasy", PublishDate = new DateTime(1937, 9, 21), ISBN = "978-0-618-00221-3", CopiesAvailable = 8 },
            new BookEntity { Id = 8, Title = "Fahrenheit 451", AuthorId = 8, Genre = "Dystopian", PublishDate = new DateTime(1953, 10, 19), ISBN = "978-1-4516-7331-9", CopiesAvailable = 3 },
            new BookEntity { Id = 9, Title = "Brave New World", AuthorId = 9, Genre = "Science Fiction", PublishDate = new DateTime(1932, 8, 30), ISBN = "978-0-06-085052-4", CopiesAvailable = 4 },
            new BookEntity { Id = 10, Title = "The Lord of the Rings", AuthorId = 7, Genre = "Fantasy", PublishDate = new DateTime(1954, 7, 29), ISBN = "978-0-618-00222-0", CopiesAvailable = 9 }
        };

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

        // GET: Home/Index
        public IActionResult Index()
        {
            // Fetching books that are not marked as deleted and joining with authors to create a view model
            var books = _books.Where(x => x.IsDeleted == false)
                .Join(
                    _authors,
                    book => book.AuthorId,
                    author => author.Id,
                    (book, author) => new BookListViewModel
                    {
                        Id = book.Id,
                        Title = book.Title,
                        AuthorId = book.AuthorId,
                        Genre = book.Genre,
                        AuthorName = book.AuthorName,
                    })
                .ToList();

            // Creating a list of author details using AuthorDetailsViewModel
            var authors = _authors.Select(a => new AuthorDetailsViewModel
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName
            }).ToList();

            // Creating a tuple to hold books and authors and passing to the view
            var viewModel = Tuple.Create(books, authors);
            return View(viewModel);
        }

        // GET: Home/About
        public IActionResult About()
        {
            // Returning the About view
            return View();
        }
    }
}

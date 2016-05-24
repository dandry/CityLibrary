using CityLibrary.Models.Library;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CityLibrary.DAL
{
    //public class LibraryInitializer : DropCreateDatabaseAlways<LibraryContext>
    //{
    //    protected override void Seed(LibraryContext context)
    //    {
    //        var users = new List<LibraryUser>()
    //        {
    //            new LibraryUser() {FirstName = "Daniel", LastName = "Andraszewski", DateOfBirth = DateTime.Parse("08-09-1992"), RegistrationDate = DateTime.Parse("30-04-2016") },
    //            new LibraryUser() {FirstName = "Zuzanna", LastName = "Białek", DateOfBirth = DateTime.Parse("13-05-1995"), RegistrationDate = DateTime.Parse("04-05-2016") },
    //            new LibraryUser() {FirstName = "Jan", LastName = "Kowalski", DateOfBirth = DateTime.Parse("01-02-1971"), RegistrationDate = DateTime.Parse("04-05-2016") },
    //            new LibraryUser() {FirstName = "Anna", LastName = "Mucha", DateOfBirth = DateTime.Parse("25-11-1987"), RegistrationDate = DateTime.Parse("03-05-2016") },
    //            new LibraryUser() {FirstName = "Rżąca", LastName = "Beata", DateOfBirth = DateTime.Parse("19-08-1971"), RegistrationDate = DateTime.Parse("02-05-2016") },
    //            new LibraryUser() {FirstName = "Zawadzki", LastName = "Mirosław", DateOfBirth = DateTime.Parse("28-06-1964"), RegistrationDate = DateTime.Parse("02-05-2016") },
    //            new LibraryUser() {FirstName = "Wyga", LastName = "Maciej", DateOfBirth = DateTime.Parse("28-01-1998"), RegistrationDate = DateTime.Parse("02-05-2016") }
    //        };

    //        users.ForEach(s => context.LibraryUsers.Add(s));
    //        context.SaveChanges();

    //        var bookCollections = new List<BookCollection>()
    //        {
    //            new BookCollection() { Name = "Historyczne" },
    //            new BookCollection() { Name = "Fantasy" },
    //            new BookCollection() { Name = "Horror" },
    //            new BookCollection() { Name = "Science Fiction" },
    //            new BookCollection() { Name = "Przygodowa" },
    //            new BookCollection() { Name = "Religijna" },
    //            new BookCollection() { Name = "Romans" },
    //            new BookCollection() { Name = "Satyra" },
    //            new BookCollection() { Name = "Literatura faktu" },
    //        };

    //        bookCollections.ForEach(bc => context.BookCollections.Add(bc));
    //        context.SaveChanges();

    //        var books = new List<Book>()
    //        {
    //            new Book() {Title = "Historia bez cenzury", Author = "Wojciech Drewniak", ISBN = "125-85-1655-241-1", CollectionId = 1 },
    //            new Book() {Title = "D-Day", Author = "Stephen E. Ambrose", ISBN = "554-24-0548-141-1", CollectionId = 1 },
    //            new Book() {Title = "Sługa Boży", Author = "Jacek Piekara", ISBN = "245-23-9710-123-1", CollectionId = 2 },
    //            new Book() {Title = "Miech Aniołów", Author = "Jacek Piekara", ISBN = "245-35-3663-236-0", CollectionId = 2 },
    //            new Book() {Title = "Lśnienie", Author = "Stephen King", ISBN = "954-54-7815-139-5", UserId = 5, CollectionId = 3 },
    //            new Book() {Title = "Miasteczko Salem", Author = "Stephen King", ISBN = "123-53-2147-365-9", CollectionId = 3 },
    //            new Book() {Title = "Miasteczko Salem", Author = "Stephen King", ISBN = "978-83-7510-540-7", BorrowDate = DateTime.Parse("03-05-2016"), ReturnDate = DateTime.Parse("2016-06-02"), UserId = 6, CollectionId = 3 },
    //            new Book() {Title = "Miasteczko Salem", Author = "Stephen King", ISBN = "245-23-9710-123-1", BorrowDate = DateTime.Parse("27-04-2016"), ReturnDate = DateTime.Parse("26-05-2016"), UserId = 1, CollectionId = 3 },
    //            new Book() {Title = "Metro 2033", Author = "Dmitrij Głuchowski", CollectionId = 4, ISBN = "677-21-0057-157-1" },
    //            new Book() {Title = "Tomek w kraninie kangurów", Author = "Alfred Szklarski", CollectionId = 5, ISBN = "256-62-1352-875-1" },
    //            new Book() {Title = "W pustyni i w puszczy", Author = "Henryk Sienkiewicz", CollectionId = 5, ISBN = "178-85-1657-158-8" },
    //            new Book() {Title = "Mój brat Pier Giorgio. Wiara", Author = "Luciana Frassati", CollectionId = 6, ISBN = "569-12-1257-132-8" },
    //            new Book() {Title = "Dieta Alleluja leczy raka i...", Author = "George Malkmus", CollectionId = 6, ISBN = "968-55-4521-152-7"},
    //            new Book() {Title = "Pamiętnik", Author = "Nicholas Sparks", CollectionId = 7, ISBN = "875-85-4125-254-1" },
    //            new Book() {Title = "Dzieje Tristana i Izoldy", Author = "Autor nieznany", CollectionId = 7, ISBN = "178-28-1586-357-6" },
    //            new Book() {Title = "Kot w stanie czystym", Author = "Terry Pratchett", CollectionId = 8, ISBN = "145-45-1549-124-8" },
    //            new Book() {Title = "Myśli nieuczesane wszystkie", Author = "Stanisław Jerzy Lec", CollectionId = 8, ISBN = "457-15-1575-211-2"},
    //            new Book() {Title = "Służby specjalne. Podwójna przykrywka", Author = "Patryk Vega", ISBN = "154-08-6045-215-8", CollectionId = 9 },
    //            new Book() {Title = "Saga Puszczy Białowieskiej", Author = "Kossak Simona", ISBN = "154-15-1545-162-8",  CollectionId = 9 },

    //        };

    //        books.ForEach(b => context.LibraryBooks.Add(b));
    //        context.SaveChanges();
    //    }
    //}
}
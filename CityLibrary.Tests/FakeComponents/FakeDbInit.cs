using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLibrary.Tests.FakeComponents
{
    public static class FakeDbInit
    {
        public static void InitDummyDb()
        {
            var fUow = new FakeUnitOfWork();

            var dummyCollections = FakeCollections.GetCollections();    // 9 collections
            var dummyUsers = FakeUsers.GetUsers();  // 9 users
            var dummyBooks = FakeBooks.GetBooks();  // 9 books

            // deleting existing DB records
            var existingBooks = fUow.BookRepository.Get();
            var existingUsers = fUow.UserRepository.Get();
            var existingCollections = fUow.BookCollectionRepository.Get();

            if (existingBooks.Count() != 0)
            {
                foreach (var book in existingBooks)
                    fUow.BookRepository.Delete(book);
            }
            if (existingUsers.Count() != 0)
            {
                foreach (var user in existingUsers)
                    fUow.UserRepository.Delete(user);
            }
            if (existingCollections.Count() != 0)
            {
                foreach (var collection in existingCollections)
                    fUow.BookCollectionRepository.Delete(collection);
            }
            fUow.Save();

            // inserting new dummy DB records
            foreach (var collection in dummyCollections)
            {
                fUow.BookCollectionRepository.Insert(collection);
            }
            fUow.Save();
            foreach (var user in dummyUsers)
            {
                //fUow.UserRepository.Insert(user);
            }
            fUow.Save();

            var collectionIdStartNumber = fUow.BookCollectionRepository.Get().FirstOrDefault().CollectionId;
            var userIdStartNumber = 1;
            foreach (var book in dummyBooks)
            {
                if (book.BorrowDate != null)
                    book.UserId = userIdStartNumber;

                book.CollectionId = collectionIdStartNumber;
                fUow.BookRepository.Insert(book);
                collectionIdStartNumber++;
            }
            fUow.Save();
        }
    }
}

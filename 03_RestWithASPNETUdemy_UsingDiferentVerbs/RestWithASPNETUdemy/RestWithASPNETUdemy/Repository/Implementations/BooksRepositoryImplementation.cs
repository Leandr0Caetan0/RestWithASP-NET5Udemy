using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace RestWithASPNETUdemy.Repository.Implementations
{
    public class BooksRepositoryImplementation : IBooksRepository
    {
        private MySQLContext _context;

        public BooksRepositoryImplementation(MySQLContext context) 
        {
            _context = context;
        }

        public List<Books> FindAll()
        {
            return _context.Books.ToList();
        }

        public Books FindByID(long id)
        {
            return _context.Books.SingleOrDefault(b => b.Id.Equals(id));
        }
        public Books Create(Books books)
        {
            try
            {
                _context.Add(books);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
            return books;
        }

        public Books Update(Books books)
        {
            if (!Exists(books.Id))
            {
                return null;
            }

            var oldBook = _context.Books.SingleOrDefault(b => b.Id.Equals(books.Id));

            try
            {
                _context.Entry(oldBook).CurrentValues.SetValues(books);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
            return books;
        }

        public void Delete(long id)
        {
            var bookToDelete = _context.Books.SingleOrDefault(b => b.Id.Equals(id));

            if (bookToDelete != null)
            {
                try
                {
                    _context.Books.Remove(bookToDelete);
                    _context.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public bool Exists(long id)
        {
            return _context.Books.Any(b => b.Id.Equals(id));
        }
    }
}

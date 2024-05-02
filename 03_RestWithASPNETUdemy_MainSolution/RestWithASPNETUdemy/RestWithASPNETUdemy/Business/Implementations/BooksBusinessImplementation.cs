using RestWithASPNETUdemy.Data.Converter.Contract;
using RestWithASPNETUdemy.Data.Converter.Implementations;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Hypermedia.Utils;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Repository;
using System.Collections.Generic;

namespace RestWithASPNETUdemy.Business.Implementations
{
    public class BooksBusinessImplementation : IBookBusiness
    {
        private readonly IRepository<Book> _repository;
        private readonly BooksConverter _converter;
        /*private readonly IParser<BooksVO, Books> _converterVO;
        private readonly IParser<Books, BooksVO> _converterBooks;*/

        public BooksBusinessImplementation(IRepository<Book> repository/*, IParser<BooksVO, Books> converterVO, IParser<Books, BooksVO> converterBooks*/)
        {
            _repository = repository;
            _converter = new BooksConverter();
            /*_converterVO = converterVO;
            _converterBooks = converterBooks*/
        }

        public List<BookVO> FindAll()
        {
            var booksEntity = _repository.FindAll(); // Vai ao repositório com entidade Books e lista todos os livros, depois joga na variável booksEntity
            return _converter.Parse(booksEntity); // chama o método Parse do BooksConverter para converter o Books em BooksVO, uma lista de livros e retorna o valor
        }

        public PagedSearchVO<BookVO> FindWithPagedSearch(string title, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrWhiteSpace(sortDirection)) && !sortDirection.Equals("desc") ? "asc" : "desc";
            var size = (pageSize < 1) ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            string query = @"SELECT * FROM books B WHERE 1 = 1";
            if (!string.IsNullOrWhiteSpace(title)) query = query + $" AND B.title LIKE '%{title}%' ";
            query += $" ORDER BY B.title {sort} limit {size} offset {offset}";

            string countQuery = @"SELECT COUNT(*) FROM books B WHERE 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(title)) countQuery = countQuery + $" AND B.title LIKE '%{title}%' ";

            var books = _repository.FindWithPagedSearch(query);
            int totalResults = _repository.GetCount(countQuery);

            return new PagedSearchVO<BookVO>
            {
                CurrentPage = page,
                List = _converter.Parse(books),
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }

        public BookVO FindByID(long id)
        {
            var bookEntity = _repository.FindByID(id); // Vai ao repositório com entidade Books e lista o livro com o id enviado no parametro, depois joga na variável bookEntity
            return _converter.Parse(bookEntity); // chama o método Parse do BooksConverter para converter o Books em BooksVO, apenas um livro e retorna o valor;
        }

        public BookVO Create(BookVO booksVO)
        {
            // É necessário converter a Entidade antes de persistir no banco de dados.
            var bookEntity = _converter.Parse(booksVO); // chama o método Parse para converter o BooksVO em Books, salva na variável bookEntity.
            bookEntity = _repository.Create(bookEntity); // chama o método Create com a entidade Books, salva o Books criado na variável bookEntity.
            return _converter.Parse(bookEntity); // chama o método Parse passando o Books bookEntity como parametro para converte-lo, e retorna o BooksVO.
        }

        public BookVO Update(BookVO booksVO)
        {
            var bookEntity = _converter.Parse(booksVO);
            bookEntity = _repository.Update(bookEntity);
            return _converter.Parse(bookEntity);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}

using RestWithASPNETUdemy.Data.VO;
using System.Collections.Generic;

namespace RestWithASPNETUdemy.Business
{
    public interface IBookBusiness
    {
        BookVO Create(BookVO bookVO);
        BookVO FindByID(long id);
        List<BookVO> FindAll();
        BookVO Update(BookVO booksVO);
        void Delete(long id);
    }
}

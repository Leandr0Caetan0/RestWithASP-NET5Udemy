﻿using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Hypermedia.Utils;
using System.Collections.Generic;

namespace RestWithASPNETUdemy.Business
{
    public interface IBookBusiness
    {
        BookVO Create(BookVO bookVO);
        BookVO FindByID(long id);
        List<BookVO> FindAll();
		// teste git
        PagedSearchVO<BookVO> FindWithPagedSearch(string title, string sortDirection, int pageSize, int page);
        BookVO Update(BookVO booksVO);
        void Delete(long id);
    }
}

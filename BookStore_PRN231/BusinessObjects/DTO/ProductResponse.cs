﻿using BusinessObjects.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class ProductResponse : PagingResponsse
    {
        public List<BookDto>? Product { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eob_Web.Core.Helpers
{
    public class Pagination
    {
        public int Page_Number { get; set; } = 1;
        public int Page_Size { get; set; } = 10;
    }

    public static class Pagination_Helper
    {

        public static List<T> Paginate<T>(this List<T>? source, Pagination pagination)
        {
            if (source is null)
            {
                throw new InvalidOperationException("Null argument");
            }

            return source
                .Skip((pagination.Page_Number - 1) * pagination.Page_Size)
                .Take(pagination.Page_Size)
                .ToList();
        }

        public static int Get_Page(int index, Pagination pagination)
        {
            return (int)Math.Ceiling(decimal.Divide(index + 1, pagination.Page_Size));
        }
    }
}

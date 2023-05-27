using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudOperation
{
    public class StatusCodes
    {
        public const int HTTP_OK = 200;
        public const int HTTP_CREATED = 201;
        public const int HTTP_DATANOTMATCHED = 204;
        public const int HTTP_INTERNAL_SERVER_ERROR = 500;
        public const int HTTP_UNAUTHORIZED = 401;
        public const int HTTP_FORBIDDEN = 403;
        public const int HTTP_NOT_FOUND = 403;
        public const int HTTP_ALREADYEXISTS = 409;
        public const int HTTP_LOCKED = 423;
        public const int HTTP_BAD_REQUEST = 400;
    }
}

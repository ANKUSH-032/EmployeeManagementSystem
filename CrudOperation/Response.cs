using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudOperation
{
    public class Response
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public string? Data { get; set; }
    }
    public class ResponseRunTimeError
    {
        public bool Status { get; set; }
        public string? UserMessage { get; set; }
        public string? DeveloperMessage { get; set; }
        public string? Data { get; set; }
    }
    public class Response<T>
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }

    public class ResponseList<T>
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public List<T>? Data { get; set; }
        public int TotalRecords { get; set; }
        public int RecordsFiltered { get; set; }
    }
}

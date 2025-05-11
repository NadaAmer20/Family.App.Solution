using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Repository.Response
{

    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
        public object? Meta { get; set; }


        public Pagination? Pagination { get; set; }

        public bool IsSuccess => Errors == null || !Errors.Any();

        //public bool IsSuccess
        //{
        //    get { return Errors == null || Errors.Count == 0; }
        //}
        public static ApiResponse<T> Success(T data, string message = "Success", object? meta = null)
          => new ApiResponse<T> { Data = data, Message = message, Meta = meta };

        public static ApiResponse<T> Fail(string message, List<string>? errors = null)
            => new ApiResponse<T> { Message = message, Errors = errors ?? new List<string> { message } };
    }

    public class Pagination
    {

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}

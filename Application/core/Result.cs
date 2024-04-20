using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.core
{
    public class Result <T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string error { get; set; }
        public static Result<T> Success (T Value) => new  Result<T> {IsSuccess = true, Value = Value};
        public static Result<T> Failure (string error ) => new  Result<T> {IsSuccess = false, error = error};

               
        
        
    }
}
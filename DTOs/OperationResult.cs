using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FifoApi.DTOs.User;
using FifoApi.Models;

namespace FifoApi.DTOs
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; private set; }
        public string? ErrorMessage { get; private set; }
        public object? Data { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public object? Errors { get; private set; }
        private OperationResult(
            bool isSuccess,
            HttpStatusCode statusCode,
            T? data = default,
            string? errorMessage = null,
            object? errors = null
        )
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Data = data;
            ErrorMessage = errorMessage;
            Errors = errors;
        }

        public static OperationResult<T> Ok(T? data = default)
        {
            return new OperationResult<T>(true, HttpStatusCode.OK, data);
        }

        public static OperationResult<T> Unauthorized(string message = "Unauthorized")
        {
            return new OperationResult<T>(false, HttpStatusCode.Unauthorized, default, message);
        }

        public static OperationResult<T> Forbidden(string message = "Forbidden")
        {
            return new OperationResult<T>(false, HttpStatusCode.Forbidden, default, message);
        }

        public static OperationResult<T> BadRequest(string message = "Bad Request", object? errors = null)
        {
            return new OperationResult<T>(false, HttpStatusCode.BadRequest, default, message, errors);
        }

        public static OperationResult<T> NotFound(string message = "Not Found")
        {
            return new OperationResult<T>(false, HttpStatusCode.NotFound, default, message);
        }

        public static OperationResult<T> InternalServerError(string message = "Unknown Error Occurred")
        {
            return new OperationResult<T>(false, HttpStatusCode.InternalServerError, default, message);
        }
    }
}
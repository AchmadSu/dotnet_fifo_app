using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FifoApi.Extensions.Controllers
{
    public static class ControllerExtensions
    {
        public static IActionResult ToActionResult<T>(this ControllerBase controller, OperationResult<T> result)
        {
            var response = new Dictionary<string, object>
            {
                { "success", result.IsSuccess }
            };

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                response.Add("message", result.ErrorMessage);
            }

            if (result.StatusCode == System.Net.HttpStatusCode.OK && result.Data != null)
            {
                response.Add("data", result.Data);
            }

            if (result.StatusCode != System.Net.HttpStatusCode.OK && result.Errors != null)
            {
                response.Add("errors", result.Errors);
            }

            return controller.StatusCode((int)result.StatusCode, response);
        }

        public static IActionResult ToErrorActionResult(this ControllerBase controller, string message = "Internal Server Error")
        {
            return controller.StatusCode(500, new
            {
                success = false,
                message
            });
        }
    }
}
using FOODProject.Model.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Shopping.Middleware
{
    public class ErrorHandler
    {
       private readonly RequestDelegate _next;
        public ErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Result result = new Result();
            var hasError = false;
            try
            {
                await _next(context);
            }
            catch (ArgumentException e)
            {
                hasError = true;
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.warning,
                };
            }
            catch (Exception e)
            {
                hasError = true;
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.warning,
                };
            }
            finally
            {
                if(hasError)
                {
                    var errorJson = JsonConvert.SerializeObject(result);
                    await context.Response.WriteAsync(errorJson);
                }
            }
        }
    }
}


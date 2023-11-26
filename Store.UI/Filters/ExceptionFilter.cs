using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Store.UI.Models;

namespace Store.UI.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;
    private readonly IWebHostEnvironment _environment;

    public ExceptionFilter(ILogger<ExceptionFilter> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public void OnException(ExceptionContext context)
    {
        if (_environment.IsDevelopment())
        {
            return;
        }

        _logger.LogError(context.Exception.Message);

        if (context.Exception.TargetSite?.Name == "ThrowNoElementsException")
        {
            context.Result = new ViewResult()
            {
                ViewName = "NotFound",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = new ErrorViewModel()
                    {
                        Message = context.Exception.Message
                    }
                },
                StatusCode = 404
            }; 
        }
    }
}
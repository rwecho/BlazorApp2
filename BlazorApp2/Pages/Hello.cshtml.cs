using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorApp2.Pages
{
    public class HelloModel : PageModel
    {
        private IServiceProvider ServiceProvider { get; }

        public HelloModel(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public void OnGet()
        {
            Debug.WriteLine($"{nameof(Index)} ->" + this.ServiceProvider.GetHashCode());

            var field = this.ServiceProvider.GetType().GetField("_disposed", BindingFlags.NonPublic |
                                                                             BindingFlags.Instance);

            var isDisposed = (bool) field.GetValue(this.ServiceProvider);
            Debug.Assert(!isDisposed);
        }
    }
}
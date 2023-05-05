using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage
{
    public static partial class Extensions
    {
        public static Task GoToRootAsync(this Shell shell, string route) => shell.GoToAsync("//" + route);
    }
}

﻿using System.Web;
using System.Web.Mvc;

namespace Coticula2Face
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

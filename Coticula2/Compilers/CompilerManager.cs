using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coticula2.Compilers
{
    public static class CompilerManager
    {
        public static ICompiler CreateCompiler(string language)
        {
            if (language != "FPC")
                throw new NotImplementedException();

            string pascalInvokeParameter = "ppc386 %1 -WC -Ci-o-r-t- -Xs -Sdgich -Se10 -l- -vwnh";

            var defaultCompiler = new DefaultCompiler(pascalInvokeParameter);
            defaultCompiler.Language = language;
            return defaultCompiler;
        }
    }
}

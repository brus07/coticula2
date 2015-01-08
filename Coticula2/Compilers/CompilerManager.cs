﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coticula2.Compilers
{
    public class CompilerManager
    {
        private string compilersPath;

        public CompilerManager(string compilersPath)
        {
            this.compilersPath = compilersPath;

            const string internalFpcFolderPath = @"fpc_2.6.4\bin\i386-win32";
            if (Directory.Exists(Path.Combine(compilersPath, internalFpcFolderPath)))
            {
                this.compilersPath = Path.Combine(compilersPath, internalFpcFolderPath);
            }
        }
        public ICompiler CreateCompiler(string language)
        {
            if (language != "FPC")
                throw new NotImplementedException();

            string pascalInvokeParameter = "ppc386 %1 -WC -Ci-o-r-t- -Xs -Sdgich -Se10 -l- -vwnh";

            string invokeParameters = Path.Combine(compilersPath, pascalInvokeParameter);
            var defaultCompiler = new DefaultCompiler(invokeParameters);
            defaultCompiler.Language = language;
            return defaultCompiler;
        }
    }
}

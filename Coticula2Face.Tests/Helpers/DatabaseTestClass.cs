using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coticula2Face.Tests.Helpers
{
    [TestFixture]
    public abstract class DatabaseTestClass
    {
        [SetUp]
        public void Setup()
        {
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = Path.Combine(System.IO.Path.GetDirectoryName(executable), "App_Data");
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
        }
    }
}

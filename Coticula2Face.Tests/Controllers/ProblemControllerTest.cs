using Coticula2Face.Controllers;
using Coticula2Face.Models.Coticula;
using Coticula2Face.Tests.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Coticula2Face.Tests.Controllers
{
    [TestFixture]
    public class ProblemControllerTest : DatabaseTestClass
    {
        #region Index action
        [Test]
        public void Index()
        {
            // Arrange
            ProblemController controller = new ProblemController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        #endregion

        #region Details action
        [Test]
        public void Details()
        {
            ProblemController controller = new ProblemController();

            ActionResult actionResult = controller.Details(1) as ActionResult;
            Assert.IsNotNull(actionResult);

            var view = actionResult as ViewResult;
            Assert.IsNotNull(view);

            var problem = view.Model as Problem;
            Assert.IsNotNull(problem);
            Assert.AreEqual(1, problem.Id);
            Assert.AreEqual("Swap", problem.Name);
        }

        [Test]
        public void DetailsWithInvalidId()
        {
            ProblemController controller = new ProblemController();

            ActionResult actionResult = controller.Details(123456789) as ActionResult;
            Assert.IsNotNull(actionResult);

            Assert.IsTrue(actionResult is HttpNotFoundResult);
        }

        [Test]
        public void DetailsWithInvalidNegativeId()
        {
            ProblemController controller = new ProblemController();

            ActionResult actionResult = controller.Details(-1) as ActionResult;
            Assert.IsNotNull(actionResult);

            Assert.IsTrue(actionResult is HttpNotFoundResult);
        }

        [Test]
        public void DetailsWithInvalidZeroId()
        {
            ProblemController controller = new ProblemController();

            ActionResult actionResult = controller.Details(-1) as ActionResult;
            Assert.IsNotNull(actionResult);

            Assert.IsTrue(actionResult is HttpNotFoundResult);
        }
        #endregion

        #region Create action
        [Test]
        public void Create()
        {
            ProblemController controller = new ProblemController();

            ActionResult actionResult = controller.Create() as ActionResult;
            Assert.IsNotNull(actionResult);

            var view = actionResult as ViewResult;
            Assert.IsNull(view.Model);
        }

        [Test]
        public void CreateGetTestAutorizeAttribute()
        {
            var controller = new ProblemController();
            var type = controller.GetType();
            var methodInfo = type.GetMethod("Create", new Type[] { });
            var attributes = methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            Assert.IsTrue(attributes.Any(), "No AuthorizeAttribute found on Create() method");
        }

        [Test]
        public void CreatePostTestAutorizeAttribute()
        {
            var controller = new ProblemController();
            var type = controller.GetType();
            var methodInfo = type.GetMethod("Create", new Type[] { typeof(Problem) });
            var attributes = methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            Assert.IsTrue(attributes.Any(), "No AuthorizeAttribute found on Create(Problem) method");
        }

        [Test]
        public void CreateProblem()
        {
            ProblemController controller = new ProblemController();

            Problem problem = new Problem();
            problem.Name = "Problem for Unit test";
            problem.Description = "Description of problem for Unit test.";

            ActionResult actionResult = controller.Create(problem) as ActionResult;
            Assert.IsNotNull(actionResult);

            var redirect = actionResult as RedirectToRouteResult;
            Assert.IsNotNull(redirect);
            Assert.IsTrue(redirect.RouteValues.ContainsValue("Index"));

            actionResult = controller.Details(problem.Id) as ActionResult;
            Assert.IsNotNull(actionResult);

            var view = actionResult as ViewResult;
            Assert.IsNotNull(view);

            var receivedProblem = view.Model as Problem;
            Assert.IsNotNull(problem);
            Assert.AreEqual(problem.Id, receivedProblem.Id);
            Assert.AreEqual(problem.Name, receivedProblem.Name);
            Assert.AreEqual(problem.Description, receivedProblem.Description);
        }
        #endregion

        #region Edit action

        [Test]
        public void Edit()
        {
            ProblemController controller = new ProblemController();

            ActionResult actionResult = controller.Edit(1) as ActionResult;
            Assert.IsNotNull(actionResult);

            var view = actionResult as ViewResult;
            Assert.IsNotNull(view);

            var problem = view.Model as Problem;
            Assert.IsNotNull(problem);
            Assert.AreEqual(1, problem.Id);
            Assert.AreEqual("Swap", problem.Name);
        }

        [Test]
        public void EditPost()
        {
            Problem problem = new Problem();
            problem.Name = "Problem for Unit test";
            problem.Description = "Description of problem for Unit test.";

            ProblemController controller = new ProblemController();

            ActionResult actionResult = controller.Create(problem) as ActionResult;
            Assert.IsNotNull(actionResult);

            var redirect = actionResult as RedirectToRouteResult;
            Assert.IsNotNull(redirect);
            Assert.IsTrue(redirect.RouteValues.ContainsValue("Index"));

            actionResult = controller.Details(problem.Id) as ActionResult;
            Assert.IsNotNull(actionResult);

            problem.Name = "Edited name of problem;";
            actionResult = controller.Edit(problem) as ActionResult;
            Assert.IsNotNull(actionResult);
            redirect = actionResult as RedirectToRouteResult;
            Assert.IsNotNull(redirect);
            Assert.IsTrue(redirect.RouteValues.ContainsValue("Index"));

            actionResult = controller.Details(problem.Id) as ActionResult;
            Assert.IsNotNull(actionResult);

            var view = actionResult as ViewResult;
            Assert.IsNotNull(view);

            var receivedProblem = view.Model as Problem;
            Assert.IsNotNull(problem);
            Assert.AreEqual(problem.Id, receivedProblem.Id);
            Assert.AreEqual(problem.Name, receivedProblem.Name);
            Assert.AreEqual(problem.Description, receivedProblem.Description);
        }

        #endregion

        #region Delete action

        [Test]
        public void Delete()
        {
            ProblemController controller = new ProblemController();

            ActionResult actionResult = controller.Delete(1) as ActionResult;
            Assert.IsNotNull(actionResult);

            var view = actionResult as ViewResult;
            Assert.IsNotNull(view);

            var problem = view.Model as Problem;
            Assert.IsNotNull(problem);
            Assert.AreEqual(1, problem.Id);
            Assert.AreEqual("Swap", problem.Name);
        }

        [Test]
        public void DeletePost()
        {
            Problem problem = new Problem();
            problem.Name = "Problem for Unit test";
            problem.Description = "Description of problem for Unit test.";

            ProblemController controller = new ProblemController();

            ActionResult actionResult = controller.Create(problem) as ActionResult;
            Assert.IsNotNull(actionResult);

            var redirect = actionResult as RedirectToRouteResult;
            Assert.IsNotNull(redirect);
            Assert.IsTrue(redirect.RouteValues.ContainsValue("Index"));

            actionResult = controller.Details(problem.Id) as ActionResult;
            Assert.IsNotNull(actionResult);

            problem.Name = "Edited name of problem;";
            actionResult = controller.DeleteConfirmed(problem.Id) as ActionResult;
            Assert.IsNotNull(actionResult);
            redirect = actionResult as RedirectToRouteResult;
            Assert.IsNotNull(redirect);
            Assert.IsTrue(redirect.RouteValues.ContainsValue("Index"));

            actionResult = controller.Details(problem.Id) as ActionResult;
            Assert.IsNotNull(actionResult);

            var notFound = actionResult as HttpNotFoundResult;
            Assert.IsNotNull(notFound);
        }

        #endregion
    }
}

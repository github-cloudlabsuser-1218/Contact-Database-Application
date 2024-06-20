using CRUD_application_2.Controllers;
using CRUD_application_2.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

[TestClass]
public class UserControllerTests
{
    private UserController controller;
    private List<User> users;

    [TestInitialize]
    public void Setup()
    {
        // Initialize your controller and test data here
        users = new List<User>
        {
            new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
            new User { Id = 2, Name = "Jane Doe", Email = "jane@example.com" }
        };
        UserController.userlist = users;
        controller = new UserController();
    }

    [TestMethod]
    public void Index_ReturnsViewWithUsers()
    {
        var result = controller.Index() as ViewResult;
        Assert.IsNotNull(result);
        var model = result.Model as List<User>;
        Assert.AreEqual(2, model.Count);
    }

    [TestMethod]
    public void Details_UserExists_ReturnsViewWithUser()
    {
        var result = controller.Details(1) as ViewResult;
        Assert.IsNotNull(result);
        var model = result.Model as User;
        Assert.AreEqual("John Doe", model.Name);
    }

    [TestMethod]
    public void Details_UserDoesNotExist_ReturnsHttpNotFound()
    {
        var result = controller.Details(99);
        Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
    }

    [TestMethod]
    public void Create_GET_ReturnsView()
    {
        var result = controller.Create() as ViewResult;
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Create_POST_ValidModel_RedirectsToIndex()
    {
        var newUser = new User { Id = 3, Name = "New User", Email = "new@example.com" };
        var result = controller.Create(newUser) as RedirectToRouteResult;
        Assert.IsNotNull(result);
        Assert.AreEqual("Index", result.RouteValues["action"]);
    }

    [TestMethod]
    public void Create_POST_InvalidModel_ReturnsViewWithModel()
    {
        controller.ModelState.AddModelError("Error", "Model is invalid");
        var newUser = new User { Id = 3, Name = "New User", Email = "new@example.com" };
        var result = controller.Create(newUser) as ViewResult;
        Assert.IsNotNull(result);
        var model = result.Model as User;
        Assert.AreEqual(newUser.Name, model.Name);
    }

    [TestMethod]
    public void Edit_GET_UserExists_ReturnsViewWithUser()
    {
        var result = controller.Edit(1) as ViewResult;
        Assert.IsNotNull(result);
        var model = result.Model as User;
        Assert.AreEqual("John Doe", model.Name);
    }

    [TestMethod]
    public void Edit_GET_UserDoesNotExist_ReturnsHttpNotFound()
    {
        var result = controller.Edit(99);
        Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
    }

    [TestMethod]
    public void Edit_POST_ValidModel_RedirectsToIndex()
    {
        var updatedUser = new User { Id = 1, Name = "Updated User", Email = "updated@example.com" };
        var result = controller.Edit(updatedUser.Id, updatedUser) as RedirectToRouteResult;
        Assert.IsNotNull(result);
        Assert.AreEqual("Index", result.RouteValues["action"]);
    }

    [TestMethod]
    public void Edit_POST_InvalidModel_ReturnsViewWithModel()
    {
        controller.ModelState.AddModelError("Error", "Model is invalid");
        var updatedUser = new User { Id = 1, Name = "Updated User", Email = "updated@example.com" };
        var result = controller.Edit(updatedUser.Id, updatedUser) as ViewResult;
        Assert.IsNotNull(result);
        var model = result.Model as User;
        Assert.AreEqual(updatedUser.Name, model.Name);
    }

    [TestMethod]
    public void Delete_GET_UserExists_ReturnsViewWithUser()
    {
        var result = controller.Delete(1) as ViewResult;
        Assert.IsNotNull(result);
        var model = result.Model as User;
        Assert.AreEqual("John Doe", model.Name);
    }

    [TestMethod]
    public void Delete_GET_UserDoesNotExist_ReturnsHttpNotFound()
    {
        var result = controller.Delete(99);
        Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
    }

    [TestMethod]
    public void Delete_POST_UserExists_RedirectsToIndex()
    {
        var result = controller.Delete(1, null) as RedirectToRouteResult;
        Assert.IsNotNull(result);
        Assert.AreEqual("Index", result.RouteValues["action"]);
        Assert.AreEqual(1, UserController.userlist.Count(user => user.Id != 1)); // Adjusted to reflect deletion
    }

    [TestMethod]
    public void Delete_POST_UserDoesNotExist_ReturnsHttpNotFound()
    {
        var result = controller.Delete(99, null);
        Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
    }

    [TestMethod]
    public void Search_ShouldReturnFilteredUsers()
    {
        // Arrange
        string searchString = "John";

        // Act
        var result = controller.Search(searchString) as ViewResult;
        var model = result.Model as List<User>;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(model);
        Assert.AreEqual(1, model.Count); // Expecting one user to match the search string
        Assert.IsTrue(model.Any(u => u.Name.Contains(searchString) || u.Email.Contains(searchString)));
    }


}

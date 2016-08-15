using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System.Web.Mvc;


namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTest
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            //Arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P1" };

            Cart target = new Cart();

            //Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            Cart.CartLine[] result = target.Lines.ToArray();

            //Assert
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Product, p1);
            Assert.AreEqual(result[1].Product, p2);
        }

        [TestMethod]
        public void Can_Add_Quantity_To_Existing_Line()
        {
            //Arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P1" };

            Cart target = new Cart();

            //Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            Cart.CartLine[] result = target.Lines.ToArray();

            //Assert
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Product, p1);
            Assert.AreEqual(result[1].Product, p2);
            Assert.AreEqual(result[0].Quantity, 11);
            Assert.AreEqual(result[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //Arrange -creat the mock repo
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { new Product { ProductID = 1, Name = "P1", Category = "Apples" }, }.AsQueryable());

            //Arrange - creat a cart
            Cart cart = new Cart();

            //Arrange - creat the controller
            CartController target = new CartController(mock.Object);

            //Act - add a product to the cart
            target.AddToCart(cart, 1, null);

            //Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
                
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_tO_Cart_Screen()
        {
            //Arrange -creat the mock repo
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { new Product { ProductID = 1, Name = "P1", Category = "Apples" }, }.AsQueryable());

            //Arrange - creat a cart
            Cart cart = new Cart();

            //Arrange - creat the controller
            CartController target = new CartController(mock.Object);

            //Act - add a product to the cart
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");


            //Assert
            Assert.AreEqual(result.RouteValues["action"],"Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");

        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {

            //Arrange - creat a cart
            Cart cart = new Cart();

            //Arrange - creat the controller
            CartController target = new CartController(null);

            //Act - add a product to the cart
            CartIndexViewModel result = (CartIndexViewModel) (target.Index(cart, "myUrl").ViewData.Model);


            //Assert
            Assert.AreEqual(result.cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");

        }
    }
}

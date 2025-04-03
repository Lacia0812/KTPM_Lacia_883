using System;
using Lab1_THKTPM.Configuration;
using Lab1_THKTPM.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using ASC.Utilities;
using ASC.Tests.TestUtilities;

namespace ASC.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<IOptions<ApplicationSettings>> optionsMock;
        private readonly Mock<ILogger<HomeController>> loggerMock;
        private readonly Mock<HttpContext> mockHttpContext;
        private readonly FakeSession fakeSession;

        public HomeControllerTests()
        {
            optionsMock = new Mock<IOptions<ApplicationSettings>>();
            loggerMock = new Mock<ILogger<HomeController>>();
            mockHttpContext = new Mock<HttpContext>();
            fakeSession = new FakeSession();

            // Cấu hình giá trị ApplicationSettings giả lập
            optionsMock.Setup(ap => ap.Value).Returns(new ApplicationSettings
            {
                ApplicationTitle = "ASC"
            });

            // Cấu hình HttpContext giả lập với Session giả lập
            mockHttpContext.SetupGet(p => p.Session).Returns(fakeSession);
        }

        [Fact]
        public void HomeController_Index_View_Test()
        {
            var controller = new HomeController(loggerMock.Object, optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void HomeController_Index_NoModel_Test()
        {
            var controller = new HomeController(loggerMock.Object, optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
            Assert.Null(result.ViewData.Model);
        }

        [Fact]
        public void HomeController_Index_Validation_Test()
        {
            var controller = new HomeController(loggerMock.Object, optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(0, result.ViewData.ModelState.ErrorCount);
        }

        [Fact]
        public void HomeController_Index_Session_Test()
        {
            var controller = new HomeController(loggerMock.Object, optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            // Lưu giá trị vào Session
            var settings = new ApplicationSettings { ApplicationTitle = "Test ASC" };
            fakeSession.SetSession("Test", settings);

            // Kiểm tra xem có lấy được dữ liệu từ Session không
            var retrievedSettings = fakeSession.GetSession<ApplicationSettings>("Test");

            Assert.NotNull(retrievedSettings);
            Assert.Equal("Test ASC", retrievedSettings.ApplicationTitle);
        }
    }
}

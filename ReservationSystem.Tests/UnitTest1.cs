using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Controllers;

namespace ReservationSystem.Tests
{
    public class UnitTest1
    {
        
        [Fact]
        public void ReservationRequestTest_UserCreateView()
        {
            var controller = new ReservationController();

            var result = controller.ReservationRequest() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void Index_ReturnHomeControllerView()
        {
            var controller = new HomeController();

            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void Index_TableControllerView()
        {
            var controller = new TableController();

            var result = controller.Index();

            Assert.NotNull(result);
        }
    }
}
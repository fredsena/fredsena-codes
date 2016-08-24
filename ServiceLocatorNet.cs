
namespace ServiceLocatorNet
{
    public class EmailService: IOrderSender
    {
        public void SendOrder(string file)
        {
            Console.WriteLine("Order {0} sent by e-mail!");
        }
    }
}

namespace ServiceLocatorNet
{
    public interface IOrderFacade
    {
        void Process(Order order);
    }
}


namespace ServiceLocatorNet
{
    public interface IOrderSender
    {
        void SendOrder(string file);
    }
}
?
namespace ServiceLocatorNet
{
    public interface IServiceLocator
    {
        T Resolve<T>();
    }
}


namespace ServiceLocatorNet
{
    public class MailService : IOrderSender
    {
        public void SendOrder(string file)
        {
            Console.WriteLine("Order {0} sent by MAIL!");
        }
    }
}

namespace ServiceLocatorNet
{
    public class Order
    {
        public int OrderNumber { get; set; }
        public string OrderDescription { get; set; }
        

    }
}

namespace ServiceLocatorNet
{
    public class OrderFacade : IOrderFacade
    {
        private readonly IServiceLocator serviceLocator;

        public OrderFacade(IServiceLocator serviceLocator)
        {
            if (serviceLocator == null)
            {
                throw new ArgumentException("serviceLocator null");
            }

            this.serviceLocator = serviceLocator;
        }

        public void Process(Order order)
        {
            //TODO: build a validator
            //var validator = this.serviceLocator.Resolve<IOrderValidator>();
            //if (validator.Validate(order))
            //{
            //    //include code here after validation
            //}

            var sender = this.serviceLocator.Resolve<IOrderSender>();

            sender.SendOrder(String.Format("{0}:{1}", order.OrderNumber, order.OrderDescription));
        }
    }
}

namespace ServiceLocatorNet
{
    public class ServiceLocator : IServiceLocator
    {
        private readonly Dictionary<Type, Func<object>> services;

        public ServiceLocator()
        {
            this.services = new Dictionary<Type, Func<object>>();       
     
            //services.Add(typeof(IOrderSender), () => (new EmailService()));
            //services.Add(typeof(IOrderSender), () => (new MailService()));            
        }

        public void Register<T>(Func<T> resolver)
        {
            this.services[typeof(T)] = () => resolver();
        }

        public T Resolve<T>()
        {
            return (T)this.services[typeof(T)]();
        }
    }
}

using System;
using Xunit;
using ServiceLocatorNet;

namespace ServiceLocator.Tests
{
    public class UnitTest_ServiceLocator
    {
        [Fact]
        public void IsCorrectTypeService()
        {
            ServiceLocatorNet.ServiceLocator serviceLocator = new ServiceLocatorNet.ServiceLocator();

            serviceLocator.Register<IOrderSender>(() => (new MailService()));

            var sender = serviceLocator.Resolve<IOrderSender>();

            Assert.IsType(typeof(MailService), sender);
        }

        [Fact]
        public void NotCorrectTypeService()
        {
            ServiceLocatorNet.ServiceLocator serviceLocator = new ServiceLocatorNet.ServiceLocator();

            serviceLocator.Register<IOrderSender>(() => (new MailService()));

            var sender = serviceLocator.Resolve<IOrderSender>();

            Assert.IsNotType(typeof(EmailService), sender);
        }


        [Fact]
        public void Execute_Order_with_Success()
        {
            var order = new Order { OrderNumber = 985632, OrderDescription = "Intel Core 7 micro" };            

            ServiceLocatorNet.ServiceLocator serviceLocator = new ServiceLocatorNet.ServiceLocator();

            serviceLocator.Register<IOrderSender>(() => (new MailService()));

            var orderFacade = new OrderFacade(serviceLocator);

            orderFacade.Process(order);

            Assert.IsType(typeof(MailService), serviceLocator.Resolve<IOrderSender>());
        }
    }
}

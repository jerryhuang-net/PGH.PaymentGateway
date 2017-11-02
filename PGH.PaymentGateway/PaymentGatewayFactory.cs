using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGH.PaymentGateway
{
    public class PaymentGatewayFactory
    {
        public static IPaymentGateway CreateInstance(string gatewayShortname)
        {
            var type = string.Format("PGH.PaymentGateway.Gateways.{0}Gateway", gatewayShortname);
            Type t = null;
            t = Type.GetType(type);
            if (t == null)
            {
                var a = AppDomain.CurrentDomain.Load("PGH.PaymentGateway");
                if (a == null)
                    throw new Exception("Cannot load PGH.PaymentGateway assembly.");
                t = a.GetType(type);
            }

            if (t == null)
                throw new ArgumentException(string.Format("type '{0}' not found."));
            return (IPaymentGateway)Activator.CreateInstance(t);
        }
    }
}

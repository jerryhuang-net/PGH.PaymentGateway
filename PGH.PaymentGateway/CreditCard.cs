using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGH.PaymentGateway
{
    public class CreditCard
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int Month { get; set; }
        /// <summary>
        /// year in 4 digits
        /// </summary>
        public int Year { get; set; }
        public string ExpiryDate
        {
            get { return string.Format("{0}{1}", Year.ToString("D4"), Month.ToString("D2")); }
        }
        public string CVV { get; set; }

        public string Brand { get; set; }
        public string Token { get; set; }
    }
}

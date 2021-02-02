using System;

namespace Contoso.MotorPolicy.Models
{
    public class Policy
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string PolicyNumber { get; set; }
        public string ProductName { get; set; }
        public string Status { get; set; }
        public decimal Premium { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime RenewalDate { get; set; }
    }

}

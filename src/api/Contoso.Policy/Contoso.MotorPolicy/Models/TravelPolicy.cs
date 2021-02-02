using System;

namespace Contoso.MotorPolicy.Models
{
    public class TravelPolicy
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string PolicyNo { get; set; }
        public string ProductName { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime CoverStarts { get; set; }
        public DateTime CoverEnds { get; set; }
    }

}


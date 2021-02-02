using System;
using System.Runtime.Serialization;

namespace Contoso.TravelPolicy.Models
{
    [DataContract]
    public class Policy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public string PolicyNo { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public DateTime CoverStarts { get; set; }
        [DataMember]
        public DateTime CoverEnds { get; set; }
    }

}


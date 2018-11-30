using System;
using System.Collections.Generic;

namespace MewsQaInterview.Objects.Response
{

    public class Configuration
    {
        public Enterprise Enterprise { get; set; }
        public DateTime NowUtc { get; set; }
    }

    public class Enterprise
    {
        public Address Address { get; set; }
        public string ChainId { get; set; }
        public string CoverImageId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public List<AcceptedCurrency> Currencies { get; set; }
        public string DefaultLanguageCode { get; set; }
        public string EditableHistoryInterval { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public string LegalEnvironmentCode { get; set; }
        public string LogoImageId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string TimeZoneIdentifier { get; set; }
        public string WebsiteUrl { get; set; }
    }

    public class Address
    {
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string PostalCode { get; set; }
    }

    public class AcceptedCurrency
    {
        public string Currency { get; set; }
        public bool IsDefault { get; set; }
        public bool IsEnabled { get; set; }
    }

}

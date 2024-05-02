using BankLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary.Services
{

    public static class CountryMapper
    {
        private static readonly Dictionary<Country, CountryCode> CountryMap = new Dictionary<Country, CountryCode>
        {
            { Country.Sweden, CountryCode.SE },
            { Country.Norway, CountryCode.NO },
            { Country.Denmark, CountryCode.DK },
            { Country.Finland, CountryCode.FI }
        };

        public static string GetCountryCode(Country country)
        {
            return Enum.GetName(typeof(CountryCode), CountryMap[country])!;
        }
        public static string GetCountryCode(string country)
        {
            var countryEnum = (Country)Enum.Parse(typeof(Country), country);
            return Enum.GetName(typeof(CountryCode), CountryMap[countryEnum])!;
        }

        public static string GetCountry(Country country)
        {
            return Enum.GetName(typeof(Country), country)!;
        }
        public static Country GetCountry(string country)
        {
            return (Country)Enum.Parse(typeof(Country), country);
        }
        public static string GetTelephoneCode(string country)
        {
            switch (country)
            {
                case "Sweden":
                    return "46";
                case "Norway":
                    return "47";
                case "Denmark":
                    return "45";
                case "Finland":
                    return "358";
            }
            return string.Empty;
        }
    }
}

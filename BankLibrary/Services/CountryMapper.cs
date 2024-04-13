using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary.Services
{
    public enum Country
    {
        Choose = 0,
        Sweden = 1,
        Norway = 2,
        Denmark = 3,
        Finland = 4
    }
    public enum CountryCode
    {
        NA = 0,
        SE = 1,
        NO = 2,
        DK = 3,
        FI = 4
    }
    public class CountryMapper
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

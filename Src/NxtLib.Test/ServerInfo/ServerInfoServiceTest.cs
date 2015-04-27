using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;
using NxtLib.Internal;
using NxtLib.ServerInfo;

namespace NxtLib.Test.ServerInfo
{
    class ServerInfoServiceTest
    {
        private readonly IServerInfoService _serverInfoService;
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal ServerInfoServiceTest(IServerInfoService serverInfoService)
        {
            _serverInfoService = serverInfoService;
        }

        internal void RunAllTests()
        {
            GetConstantsTest();
        }

        void GetConstantsTest()
        {
            var getConstantsReply = _serverInfoService.GetConstants().Result;
            var expected = Enum.GetValues(typeof (CurrencyType)).Cast<CurrencyType>().ToList();
            CheckCurrencyTypeCount(getConstantsReply, expected);
            expected.ForEach(e => CheckCurrencyTypeValues(e, getConstantsReply));
        }

        private static void CheckCurrencyTypeValues(CurrencyType expectedCurrencyType, GetConstantsReply getConstantsReply)
        {
            long value;
            var currencyName = expectedCurrencyType
                .GetType()
                .GetTypeInfo()
                .GetDeclaredField(expectedCurrencyType.ToString())
                .GetCustomAttribute<DescriptionAttribute>().Name;

            if (!getConstantsReply.CurrencyTypes.TryGetValue(currencyName, out value))
            {
                Logger.Error("Could not find expected currency type {0}", currencyName);
            }
            else if (value != (int) expectedCurrencyType)
            {
                Logger.Error("Currency type {0} was found but with wrong value. Expected {1}, but actual was {2}",
                    currencyName, (int) expectedCurrencyType, value);
            }
        }

        private static void CheckCurrencyTypeCount(GetConstantsReply getConstantsReply, List<CurrencyType> expected)
        {
            if (getConstantsReply.CurrencyTypes.Count != expected.Count())
            {
                Logger.Error("Incorrect number of currency types, expected {0}, but got {1}", expected.Count,
                    getConstantsReply.CurrencyTypes.Count);
            }
        }
    }
}

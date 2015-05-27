using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NxtLib.Networking;
using NxtLib.ServerInfo;
using NxtLib.VotingSystem;

namespace NxtLib.Test.ServerInfo
{
    internal class GetConstantsTest : TestBase
    {
        private readonly IServerInfoService _serverInfoService;
        private GetConstantsReply _getConstantsReply;
        private static readonly Dictionary<TransactionSubType, TransactionType> TransactionTypes = new Dictionary<TransactionSubType, TransactionType>();

        static GetConstantsTest()
        {
            TransactionTypes.Add(TransactionSubType.PaymentOrdinaryPayment, new TransactionType {CanHaveRecipient = true, IsPhasingSafe = true, MustHaveRecipient = true});
            TransactionTypes.Add(TransactionSubType.MessagingArbitraryMessage, new TransactionType {CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.MessagingAliasAssignment, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.MessagingPollCreation, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.MessagingVoteCasting, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.MessagingHubTerminalAnnouncement, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.MessagingAccountInfo, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.MessagingAliasSell, new TransactionType {CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.MessagingAliasBuy, new TransactionType {CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = true});
            TransactionTypes.Add(TransactionSubType.MessagingAliasDelete, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.MessagingPhasingVoteCasting, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.ColoredCoinsAssetIssuance, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            TransactionTypes.Add(TransactionSubType.ColoredCoinsAssetTransfer, new TransactionType { CanHaveRecipient = true, IsPhasingSafe = true, MustHaveRecipient = true });
            TransactionTypes.Add(TransactionSubType.ColoredCoinsAskOrderPlacement, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            TransactionTypes.Add(TransactionSubType.ColoredCoinsBidOrderPlacement, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            TransactionTypes.Add(TransactionSubType.ColoredCoinsAskOrderCancellation, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            TransactionTypes.Add(TransactionSubType.ColoredCoinsBidOrderCancellation, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            TransactionTypes.Add(TransactionSubType.ColoredCoinsDividendPayment, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            TransactionTypes.Add(TransactionSubType.DigitalGoodsListing, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            TransactionTypes.Add(TransactionSubType.DigitalGoodsDelisting, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            TransactionTypes.Add(TransactionSubType.DigitalGoodsPriceChange, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false });
            TransactionTypes.Add(TransactionSubType.DigitalGoodsQuantityChange, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false });
            TransactionTypes.Add(TransactionSubType.DigitalGoodsPurchase, new TransactionType { CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = true });
            TransactionTypes.Add(TransactionSubType.DigitalGoodsDelivery, new TransactionType { CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = true });
            TransactionTypes.Add(TransactionSubType.DigitalGoodsFeedback, new TransactionType { CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = true });
            TransactionTypes.Add(TransactionSubType.DigitalGoodsRefund, new TransactionType { CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = true });
            TransactionTypes.Add(TransactionSubType.AccountControlEffectiveBalanceLeasing, new TransactionType { CanHaveRecipient = true, IsPhasingSafe = true, MustHaveRecipient = true });
            TransactionTypes.Add(TransactionSubType.MonetarySystemCurrencyIssuance, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.MonetarySystemReserveIncrease, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.MonetarySystemReserveClaim, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.MonetarySystemCurrencyTransfer, new TransactionType { CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = true });
            TransactionTypes.Add(TransactionSubType.MonetarySystemPublishExchangeOffer, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.MonetarySystemExchangeBuy, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.MonetarySystemExchangeSell, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.MonetarySystemCurrencyMinting, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.MonetarySystemCurrencyDeletion, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.TaggedDataUpload, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            TransactionTypes.Add(TransactionSubType.TaggedDataExtend, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
        }

        public GetConstantsTest(IServerInfoService serverInfoService)
        {
            _serverInfoService = serverInfoService;
        }

        public void RunAllTests()
        {
            _getConstantsReply = _serverInfoService.GetConstants().Result;
            CheckConstants();
            CheckCurrencyTypes();
            CheckHashAlgorithms();
            CheckMinBalanceModels();
            CheckPeerStates();
            CheckVotingModels();
            CheckTransactionTypes();
        }

        private void CheckConstants()
        {
            using (Logger = new TestsessionLogger())
            {
                const ulong genesisAccountId = 1739068987193023818;
                const ulong genesisBlockId = 2680262203532249785;
                const int maxArbitraryMessageLength = 1000;
                const int maxBlockPayloadLength = 44880;
                const int maxTaggedDataDataLength = 43008;

                AssertEquals(genesisAccountId, _getConstantsReply.GenesisAccountId, "GenesisAccountId");
                AssertEquals(genesisBlockId, _getConstantsReply.GenesisBlockId, "GenesisBlockId");
                AssertEquals(maxArbitraryMessageLength, _getConstantsReply.MaxArbitraryMessageLength,
                    "MaxArbitraryMessageLength");
                AssertEquals(maxBlockPayloadLength, _getConstantsReply.MaxBlockPayloadLength, "MaxBlockPayloadLength");
                AssertEquals(maxTaggedDataDataLength, _getConstantsReply.MaxTaggedDataDataLength, "MaxTaggedDataDataLength");
            }
        }

        private void CheckCurrencyTypes()
        {
            using (Logger = new TestsessionLogger())
            {
                var expected = Enum.GetValues(typeof (CurrencyType)).Cast<CurrencyType>().ToList();
                CheckEnumCount(expected.Count, _getConstantsReply.CurrencyTypes.Count, "currency types");
                expected.ForEach(e => CheckEnumValues(e, _getConstantsReply.CurrencyTypes));
            }
        }

        private void CheckHashAlgorithms()
        {
            using (Logger = new TestsessionLogger())
            {
                var expected = Enum.GetValues(typeof (HashAlgorithm)).Cast<HashAlgorithm>().ToList();
                CheckEnumCount(expected.Count(), _getConstantsReply.HashAlgorithms.Count, "hash algorithms");
                expected.ForEach(e => CheckEnumValues(e, _getConstantsReply.HashAlgorithms));
            }
        }

        private void CheckMinBalanceModels()
        {
            using (Logger = new TestsessionLogger())
            {
                var expected = Enum.GetValues(typeof (MinBalanceModel)).Cast<MinBalanceModel>().ToList();
                CheckEnumCount(expected.Count, _getConstantsReply.MinBalanceModels.Count, "min balances");
                expected.ForEach(e => CheckEnumValues(e, _getConstantsReply.MinBalanceModels));
            }
        }

        private void CheckPeerStates()
        {
            using (Logger = new TestsessionLogger())
            {
                var expected = Enum.GetValues(typeof (PeerInfo.PeerState)).Cast<PeerInfo.PeerState>().ToList();
                CheckEnumCount(expected.Count, _getConstantsReply.PeerStates.Count, "peer states");
                expected.ForEach(e => CheckEnumValues(e, _getConstantsReply.PeerStates));
            }
        }

        private void CheckVotingModels()
        {
            using (Logger = new TestsessionLogger())
            {
                var expected = Enum.GetValues(typeof (VotingModel)).Cast<VotingModel>().ToList();
                CheckEnumCount(expected.Count, _getConstantsReply.VotingModels.Count, "voting models");
                expected.ForEach(e => CheckEnumValues(e, _getConstantsReply.VotingModels));
            }
        }

        private void CheckTransactionTypes()
        {
            using (Logger = new TestsessionLogger())
            {
                var expectedMainTypes =
                    Enum.GetValues(typeof (TransactionMainType)).Cast<TransactionMainType>().ToList();
                CheckEnumCount(expectedMainTypes.Count, _getConstantsReply.TransactionTypes.Count,
                    "main transaction types");

                var expectedSubTypes = Enum.GetValues(typeof (TransactionSubType)).Cast<TransactionSubType>().ToList();
                CheckEnumCount(expectedSubTypes.Count,
                    _getConstantsReply.TransactionTypes.SelectMany(t => t.Value.Values).Count(), "sub transaction types");

                expectedSubTypes.ForEach(CheckTransactionTypeValues);
            }
        }

        private void CheckTransactionTypeValues(TransactionSubType expectedSubType)
        {
            var expectedName = GetAttributeName(expectedSubType);

            var actual = _getConstantsReply.TransactionTypes
                [TransactionTypeMapper.GetMainTypeByte(expectedSubType)]
                [TransactionTypeMapper.GetSubTypeByte(expectedSubType)];

            if (actual == null)
            {
                Logger.Fail(string.Format("Did not find expected transaction type {0}", expectedName));
                return;
            }
            if (!actual.Name.Equals(expectedName))
            {
                Logger.Fail(string.Format("Transaction type name mismatch, expected: {0}, actual: {1}", expectedName, actual.Name));
            }
            else
            {
                var expectedValues = TransactionTypes[expectedSubType];
                AssertEquals(expectedValues.CanHaveRecipient, actual.CanHaveRecipient, "CanHaveRecipient");
                AssertEquals(expectedValues.IsPhasingSafe, actual.IsPhasingSafe, "IsPhasingSafe");
                AssertEquals(expectedValues.MustHaveRecipient, actual.MustHaveRecipient, "MustHaveRecipient");
            }
        }

        private static void CheckEnumCount(int expected, int actual, string typeName)
        {
            if (expected != actual)
            {
                Logger.Fail(string.Format("Incorrect number of {0}, expected: {1}, actual: {2}", typeName, expected, actual));
            }
        }

        private static void CheckEnumValues(Enum expected, IReadOnlyDictionary<string, sbyte> actuals)
        {
            sbyte value;
            var enumName = GetAttributeName(expected);

            var expectedValue = (int)Convert.ChangeType(expected, typeof(int));

            if (!actuals.TryGetValue(enumName, out value))
            {
                Logger.Fail(string.Format("Could not find expected currency type {0}", enumName));
            }
            else if (expectedValue != value)
            {
                Logger.Fail(string.Format("Currency type {0} was found but with wrong value. Expected {1}, but actual was {2}",
                    enumName, expectedValue, value));
            }
        }

        private static string GetAttributeName(Enum expected)
        {
            return expected
                .GetType()
                .GetTypeInfo()
                .GetDeclaredField(expected.ToString())
                .GetCustomAttribute<NxtApiAttribute>().Name;
        }
    }
}
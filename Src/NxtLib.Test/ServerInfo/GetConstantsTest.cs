using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NxtLib.Networking;
using NxtLib.ServerInfo;
using NxtLib.VotingSystem;

namespace NxtLib.Test.ServerInfo
{
    public interface IGetConstantsTest
    {
        void RunAllTests();
    }

    public class GetConstantsTest : TestBase, IGetConstantsTest
    {
        private readonly IServerInfoService _serverInfoService;
        private readonly ILogger _logger;
        private GetConstantsReply _getConstantsReply;
        private readonly Dictionary<TransactionSubType, TransactionType> _transactionTypes = new Dictionary<TransactionSubType, TransactionType>();

        public GetConstantsTest(IServerInfoService serverInfoService, ILogger logger)
        {
            _serverInfoService = serverInfoService;
            _logger = logger;

            _transactionTypes.Add(TransactionSubType.PaymentOrdinaryPayment, new TransactionType {CanHaveRecipient = true, IsPhasingSafe = true, MustHaveRecipient = true});
            _transactionTypes.Add(TransactionSubType.MessagingArbitraryMessage, new TransactionType {CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.MessagingAliasAssignment, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.MessagingPollCreation, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.MessagingVoteCasting, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.MessagingHubTerminalAnnouncement, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.MessagingAccountInfo, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.MessagingAliasSell, new TransactionType {CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.MessagingAliasBuy, new TransactionType {CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = true});
            _transactionTypes.Add(TransactionSubType.MessagingAliasDelete, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.MessagingPhasingVoteCasting, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.ColoredCoinsAssetIssuance, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            _transactionTypes.Add(TransactionSubType.ColoredCoinsAssetTransfer, new TransactionType { CanHaveRecipient = true, IsPhasingSafe = true, MustHaveRecipient = true });
            _transactionTypes.Add(TransactionSubType.ColoredCoinsAskOrderPlacement, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            _transactionTypes.Add(TransactionSubType.ColoredCoinsBidOrderPlacement, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            _transactionTypes.Add(TransactionSubType.ColoredCoinsAskOrderCancellation, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            _transactionTypes.Add(TransactionSubType.ColoredCoinsBidOrderCancellation, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            _transactionTypes.Add(TransactionSubType.ColoredCoinsDividendPayment, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            _transactionTypes.Add(TransactionSubType.ColoredCoinsAssetDelete, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            _transactionTypes.Add(TransactionSubType.DigitalGoodsListing, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            _transactionTypes.Add(TransactionSubType.DigitalGoodsDelisting, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = true, MustHaveRecipient = false });
            _transactionTypes.Add(TransactionSubType.DigitalGoodsPriceChange, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false });
            _transactionTypes.Add(TransactionSubType.DigitalGoodsQuantityChange, new TransactionType { CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false });
            _transactionTypes.Add(TransactionSubType.DigitalGoodsPurchase, new TransactionType { CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = true });
            _transactionTypes.Add(TransactionSubType.DigitalGoodsDelivery, new TransactionType { CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = true });
            _transactionTypes.Add(TransactionSubType.DigitalGoodsFeedback, new TransactionType { CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = true });
            _transactionTypes.Add(TransactionSubType.DigitalGoodsRefund, new TransactionType { CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = true });
            _transactionTypes.Add(TransactionSubType.AccountControlEffectiveBalanceLeasing, new TransactionType { CanHaveRecipient = true, IsPhasingSafe = true, MustHaveRecipient = true });
            _transactionTypes.Add(TransactionSubType.MonetarySystemCurrencyIssuance, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.MonetarySystemReserveIncrease, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.MonetarySystemReserveClaim, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.MonetarySystemCurrencyTransfer, new TransactionType { CanHaveRecipient = true, IsPhasingSafe = false, MustHaveRecipient = true });
            _transactionTypes.Add(TransactionSubType.MonetarySystemPublishExchangeOffer, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.MonetarySystemExchangeBuy, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.MonetarySystemExchangeSell, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.MonetarySystemCurrencyMinting, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.MonetarySystemCurrencyDeletion, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.TaggedDataUpload, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
            _transactionTypes.Add(TransactionSubType.TaggedDataExtend, new TransactionType {CanHaveRecipient = false, IsPhasingSafe = false, MustHaveRecipient = false});
        }

        public void RunAllTests()
        {
            _getConstantsReply = _serverInfoService.GetConstants().Result;
            CheckConstants();
            CheckCurrencyTypes();
            CheckHashAlgorithms();
            CheckMintingHashAlgorithms();
            CheckMinBalanceModels();
            CheckPhasingHashAlgorithms();
            CheckPeerStates();
            CheckRequestTypes();
            CheckVotingModels();
            CheckTransactionTypes();
        }

        private void CheckConstants()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                const ulong genesisAccountId = 1739068987193023818;
                const ulong genesisBlockId = 2680262203532249785;
                const long epochBeginning = 1385294400000;
                const int maxArbitraryMessageLength = 1000;
                const int maxBlockPayloadLength = 44880;
                const int maxTaggedDataDataLength = 43008;

                AssertEquals(epochBeginning, _getConstantsReply.EpochBeginning, "EpochBeginning");
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
            using (Logger = new TestsessionLogger(_logger))
            {
                var expected = Enum.GetValues(typeof (CurrencyType)).Cast<CurrencyType>().ToList();
                CheckEnumCount(expected.Count, _getConstantsReply.CurrencyTypes.Count, "currency types");
                expected.ForEach(e => CheckEnumValues(e, _getConstantsReply.CurrencyTypes));
            }
        }

        private void CheckHashAlgorithms()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var expected = Enum.GetValues(typeof (HashAlgorithm)).Cast<HashAlgorithm>().ToList();
                CheckEnumCount(expected.Count, _getConstantsReply.HashAlgorithms.Count, "hash algorithms");
                expected.ForEach(e => CheckEnumValues(e, _getConstantsReply.HashAlgorithms));
            }
        }

        private void CheckMintingHashAlgorithms()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var expected = Enum.GetValues(typeof(MintingHashAlgorithm)).Cast<MintingHashAlgorithm>().ToList();
                CheckEnumCount(expected.Count, _getConstantsReply.MintingHashAlgorithms.Count, "minting hash algorithms");
                expected.ForEach(e => CheckEnumValues(e, _getConstantsReply.MintingHashAlgorithms));
            }
        }

        private void CheckPhasingHashAlgorithms()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var expected = Enum.GetValues(typeof(PhasingHashAlgorithm)).Cast<PhasingHashAlgorithm>().ToList();
                CheckEnumCount(expected.Count, _getConstantsReply.PhasingHashAlgorithms.Count, "phasing hash algorithms");
                expected.ForEach(e => CheckEnumValues(e, _getConstantsReply.PhasingHashAlgorithms));
            }
        }

        private void CheckMinBalanceModels()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var expected = Enum.GetValues(typeof (MinBalanceModel)).Cast<MinBalanceModel>().ToList();
                CheckEnumCount(expected.Count, _getConstantsReply.MinBalanceModels.Count, "min balances");
                expected.ForEach(e => CheckEnumValues(e, _getConstantsReply.MinBalanceModels));
            }
        }

        private void CheckPeerStates()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var expected = Enum.GetValues(typeof (PeerInfo.PeerState)).Cast<PeerInfo.PeerState>().ToList();
                CheckEnumCount(expected.Count, _getConstantsReply.PeerStates.Count, "peer states");
                expected.ForEach(e => CheckEnumValues(e, _getConstantsReply.PeerStates));
            }
        }

        private void CheckVotingModels()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var expected = Enum.GetValues(typeof (VotingModel)).Cast<VotingModel>().ToList();
                CheckEnumCount(expected.Count, _getConstantsReply.VotingModels.Count, "voting models");
                expected.ForEach(e => CheckEnumValues(e, _getConstantsReply.VotingModels));
            }
        }

        private void CheckRequestTypes()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var expectedRequestTypes = new List<string>
                {
                    "addPeer",
                    "approveTransaction",
                    "blacklistPeer",
                    "broadcastTransaction",
                    "buyAlias",
                    "calculateFullHash",
                    "cancelAskOrder",
                    "cancelBidOrder",
                    "canDeleteCurrency",
                    "castVote",
                    "clearUnconfirmedTransactions",
                    "createPoll",
                    "currencyBuy",
                    "currencyMint",
                    "currencyReserveClaim",
                    "currencyReserveIncrease",
                    "currencySell",
                    "decodeFileToken",
                    "decodeHallmark",
                    "decodeQRCode",
                    "decodeToken",
                    "decryptFrom",
                    "deleteAlias",
                    "deleteAssetShares",
                    "deleteCurrency",
                    "dgsDelisting",
                    "dgsDelivery",
                    "dgsFeedback",
                    "dgsListing",
                    "dgsPriceChange",
                    "dgsPurchase",
                    "dgsQuantityChange",
                    "dgsRefund",
                    "dividendPayment",
                    "downloadTaggedData",
                    "dumpPeers",
                    "encodeQRCode",
                    "encryptTo",
                    "eventRegister",
                    "eventWait",
                    "extendTaggedData",
                    "fullHashToId",
                    "fullReset",
                    "generateFileToken",
                    "generateToken",
                    "getAccount",
                    "getAccountAssetCount",
                    "getAccountAssets",
                    "getAccountBlockCount",
                    "getAccountBlockIds",
                    "getAccountBlocks",
                    "getAccountCurrencies",
                    "getAccountCurrencyCount",
                    "getAccountCurrentAskOrderIds",
                    "getAccountCurrentAskOrders",
                    "getAccountCurrentBidOrderIds",
                    "getAccountCurrentBidOrders",
                    "getAccountExchangeRequests",
                    "getAccountId",
                    "getAccountLedger",
                    "getAccountLedgerEntry",
                    "getAccountLessors",
                    "getAccountPhasedTransactionCount",
                    "getAccountPhasedTransactions",
                    "getAccountPublicKey",
                    "getAccountTaggedData",
                    "getAlias",
                    "getAliasCount",
                    "getAliases",
                    "getAliasesLike",
                    "getAllAssets",
                    "getAllBroadcastedTransactions",
                    "getAllCurrencies",
                    "getAllExchanges",
                    "getAllOpenAskOrders",
                    "getAllOpenBidOrders",
                    "getAllPrunableMessages",
                    "getAllTaggedData",
                    "getAllTrades",
                    "getAllWaitingTransactions",
                    "getAskOrder",
                    "getAskOrderIds",
                    "getAskOrders",
                    "getAsset",
                    "getAssetAccountCount",
                    "getAssetAccounts",
                    "getAssetIds",
                    "getAssetPhasedTransactions",
                    "getAssets",
                    "getAssetsByIssuer",
                    "getAssetTransfers",
                    "getBalance",
                    "getBidOrder",
                    "getBidOrderIds",
                    "getBidOrders",
                    "getBlock",
                    "getBlockchainStatus",
                    "getBlockchainTransactions",
                    "getBlockId",
                    "getBlocks",
                    "getBuyOffers",
                    "getChannelTaggedData",
                    "getConstants",
                    "getCurrencies",
                    "getCurrenciesByIssuer",
                    "getCurrency",
                    "getCurrencyAccountCount",
                    "getCurrencyAccounts",
                    "getCurrencyFounders",
                    "getCurrencyIds",
                    "getCurrencyPhasedTransactions",
                    "getCurrencyTransfers",
                    "getDataTagCount",
                    "getDataTags",
                    "getDataTagsLike",
                    "getDGSExpiredPurchases",
                    "getDGSGood",
                    "getDGSGoods",
                    "getDGSGoodsCount",
                    "getDGSGoodsPurchaseCount",
                    "getDGSGoodsPurchases",
                    "getDGSPendingPurchases",
                    "getDGSPurchase",
                    "getDGSPurchaseCount",
                    "getDGSPurchases",
                    "getDGSTagCount",
                    "getDGSTags",
                    "getDGSTagsLike",
                    "getECBlock",
                    "getExchanges",
                    "getExchangesByExchangeRequest",
                    "getExchangesByOffer",
                    "getExpectedAskOrders",
                    "getExpectedAssetTransfers",
                    "getExpectedBidOrders",
                    "getExpectedBuyOffers",
                    "getExpectedCurrencyTransfers",
                    "getExpectedExchangeRequests",
                    "getExpectedOrderCancellations",
                    "getExpectedSellOffers",
                    "getExpectedTransactions",
                    "getForging",
                    "getGuaranteedBalance",
                    "getInboundPeers",
                    "getLastExchanges",
                    "getLastTrades",
                    "getLog",
                    "getMintingTarget",
                    "getMyInfo",
                    "getOffer",
                    "getOrderTrades",
                    "getPeer",
                    "getPeers",
                    "getPhasingPoll",
                    "getPhasingPolls",
                    "getPhasingPollVote",
                    "getPhasingPollVotes",
                    "getPlugins",
                    "getPoll",
                    "getPollResult",
                    "getPolls",
                    "getPollVote",
                    "getPollVotes",
                    "getPrunableMessage",
                    "getPrunableMessages",
                    "getSellOffers",
                    "getStackTraces",
                    "getState",
                    "getTaggedData",
                    "getTaggedDataExtendTransactions",
                    "getTime",
                    "getTrades",
                    "getTransaction",
                    "getTransactionBytes",
                    "getUnconfirmedTransactionIds",
                    "getUnconfirmedTransactions",
                    "getVoterPhasedTransactions",
                    "hash",
                    "hexConvert",
                    "issueAsset",
                    "issueCurrency",
                    "leaseBalance",
                    "longConvert",
                    "luceneReindex",
                    "markHost",
                    "parseTransaction",
                    "placeAskOrder",
                    "placeBidOrder",
                    "popOff",
                    "publishExchangeOffer",
                    "readMessage",
                    "rebroadcastUnconfirmedTransactions",
                    "requeueUnconfirmedTransactions",
                    "retrievePrunedData",
                    "rsConvert",
                    "scan",
                    "searchAccounts",
                    "searchAssets",
                    "searchCurrencies",
                    "searchDGSGoods",
                    "searchPolls",
                    "searchTaggedData",
                    "sellAlias",
                    "sendMessage",
                    "sendMoney",
                    "setAccountInfo",
                    "setAlias",
                    "setLogging",
                    "shutdown",
                    "signTransaction",
                    "startForging",
                    "stopForging",
                    "transferAsset",
                    "transferCurrency",
                    "trimDerivedTables",
                    "uploadTaggedData",
                    "verifyPrunableMessage",
                    "verifyTaggedData"
                };

                var requestTypes = _getConstantsReply.RequestTypes.ToList();

                foreach (var requestType in requestTypes)
                {
                    if (expectedRequestTypes.All(expected => expected != requestType.Name))
                    {
                        Logger.Fail($"Missing request type: {requestType.Name}");
                    }
                }
                foreach (var expectedRequestType in expectedRequestTypes)
                {
                    if (requestTypes.All(rt => rt.Name != expectedRequestType))
                    {
                        Logger.Fail($"Missing request type: {expectedRequestType}");
                    }
                }
            }
        }

        private void CheckTransactionTypes()
        {
            using (Logger = new TestsessionLogger(_logger))
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
                Logger.Fail($"Did not find expected transaction type {expectedName}");
                return;
            }
            if (!actual.Name.Equals(expectedName))
            {
                Logger.Fail($"Transaction type name mismatch, expected: {expectedName}, actual: {actual.Name}");
            }
            else
            {
                var expectedValues = _transactionTypes[expectedSubType];
                AssertEquals(expectedValues.CanHaveRecipient, actual.CanHaveRecipient, "CanHaveRecipient");
                AssertEquals(expectedValues.IsPhasingSafe, actual.IsPhasingSafe, "IsPhasingSafe");
                AssertEquals(expectedValues.MustHaveRecipient, actual.MustHaveRecipient, "MustHaveRecipient");
            }
        }

        private static void CheckEnumCount(int expected, int actual, string typeName)
        {
            if (expected != actual)
            {
                Logger.Fail($"Incorrect number of {typeName}, expected: {expected}, actual: {actual}");
            }
        }

        private static void CheckEnumValues(Enum expected, IReadOnlyDictionary<string, sbyte> actuals)
        {
            sbyte value;
            var enumName = GetAttributeName(expected);

            var expectedValue = (int)Convert.ChangeType(expected, typeof(int));

            if (!actuals.TryGetValue(enumName, out value))
            {
                Logger.Fail($"Could not find expected currency type {enumName}");
            }
            else if (expectedValue != value)
            {
                Logger.Fail($"Currency type {enumName} was found but with wrong value. Expected {expectedValue}, but actual was {value}");
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
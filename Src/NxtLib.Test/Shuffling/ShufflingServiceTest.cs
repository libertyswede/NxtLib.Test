using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using NxtLib.Shuffling;

namespace NxtLib.Test.Shuffling
{
    public interface IShufflingServiceTest : ITest
    {
    }

    public class ShufflingServiceTest : TestBase, IShufflingServiceTest
    {
        private readonly ILogger _logger;
        private readonly IShufflingService _shufflingService;
        private readonly ulong _knownShufflingId = 17626955119804441745;

        public ShufflingServiceTest(ILogger logger, IShufflingService shufflingService)
        {
            _logger = logger;
            _shufflingService = shufflingService;
        }

        public void RunAllTests()
        {
            TestGetAccountShufflings();
            TestGetAllShufflings();
            TestGetAssignedShufflings();
            TestGetHoldingShufflings();
            TestGetShufflers();
            TestGetShuffling();
            TestGetShufflingParticipants();
            //TestShufflingCancel();
            TestShufflingProcess();
        }

        private void TestGetAccountShufflings()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var shufflings = _shufflingService.GetAccountShufflings(TestSettings.Account1, true).Result;

                var knownShuffling = shufflings.Shufflings.Single(s => s.ShufflingId == _knownShufflingId);

                AssertEquals(3, shufflings.Shufflings.Count(), "Shufflings.Count()");
                AssertKnownShuffling(knownShuffling);
            }
        }

        private static void AssertKnownShuffling(ShufflingData knownShuffling)
        {
            AssertIsNull(knownShuffling.AssigneeId, nameof(knownShuffling.AssigneeId));
            AssertIsNull(knownShuffling.AssigneeRs, nameof(knownShuffling.AssigneeRs));
            AssertEquals(1234, knownShuffling.Amount.Nxt, nameof(knownShuffling.Amount));
            AssertEquals(0, knownShuffling.BlocksRemaining, nameof(knownShuffling.BlocksRemaining));
            AssertEquals(0, knownShuffling.HoldingId, nameof(knownShuffling.HoldingId));
            AssertEquals(HoldingType.Nxt, knownShuffling.HoldingType, nameof(knownShuffling.HoldingType));
            AssertEquals(5873880488492319831, knownShuffling.IssuerId, nameof(knownShuffling.IssuerId));
            AssertEquals("NXT-XK4R-7VJU-6EQG-7R335", knownShuffling.IssuerRs, nameof(knownShuffling.IssuerRs));
            AssertEquals(5, knownShuffling.ParticipantCount, nameof(knownShuffling.ParticipantCount));
            AssertEquals(5, knownShuffling.RecipientPublicKeys.Count, nameof(knownShuffling.RecipientPublicKeys));
            AssertEquals("9164b2334a869ff4db6795777d4afd6aaaff4217dc47cb1baa42f43530ee9e93",
                knownShuffling.ShufflingFullHash.ToHexString(), nameof(knownShuffling.ShufflingFullHash));
            AssertEquals("cb8c1482560052b87ff992e567cfdc7bf13a1f069a1df3f81de98183baf8906e",
                knownShuffling.ShufflingStateHash.ToHexString(), nameof(knownShuffling.ShufflingStateHash));
            AssertEquals(ShufflingStage.Done, knownShuffling.Stage, nameof(knownShuffling.Stage));
        }

        private void TestGetAllShufflings()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                // Just testing for exceptions here
                // ReSharper disable once UnusedVariable
                var shufflings = _shufflingService.GetAllShufflings(true, true, 0, 100).Result;
            }
        }

        private void TestGetAssignedShufflings()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                // Just testing for exceptions here
                // ReSharper disable once UnusedVariable
                var test = _shufflingService.GetAssignedShufflings(TestSettings.Account1).Result;
            }
        }

        private void TestGetHoldingShufflings()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var holdingId = 9836948456938609454;
                var shufflings = _shufflingService.GetHoldingShufflings(holdingId, includeFinished: true).Result;

                foreach (var shuffling in shufflings.Shufflings)
                {
                    AssertEquals(holdingId, shuffling.HoldingId, nameof(shuffling.HoldingId));
                    AssertEquals(HoldingType.Asset, shuffling.HoldingType, nameof(shuffling.HoldingType));
                    AssertIsNull(shuffling.HoldingInfo, nameof(shuffling.HoldingInfo));
                }

                var allShufflings = _shufflingService.GetHoldingShufflings(includeFinished: true, firstIndex: 0, lastIndex: 100).Result;
                AssertIsLargerThanZero(allShufflings.Shufflings.Count(), nameof(allShufflings.Shufflings));
            }
        }

        private void TestGetShufflers()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                // Just testing for exceptions here
                // ReSharper disable once UnusedVariable
                var test = _shufflingService.GetShufflers().Result;
            }
        }

        private void TestGetShuffling()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var knownShuffling = _shufflingService.GetShuffling(_knownShufflingId).Result;
                AssertKnownShuffling(knownShuffling);
            }
        }

        private void TestGetShufflingParticipants()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var participants = _shufflingService.GetShufflingParticipants(_knownShufflingId).Result;
                var list = participants.Participants.ToList();
                var me = list.Single(p => p.AccountRs == TestSettings.Account1.AccountRs);

                AssertEquals(5, list.Count, nameof(participants.Participants));
                list.ForEach(p => AssertEquals(ShufflingParticipantState.Verified, p.State, nameof(p.State)));
                AssertEquals(TestSettings.Account1.AccountId, me.AccountId, nameof(me.AccountId));
                AssertEquals("NXT-EVHD-5FLM-3NMQ-G46NR", me.NextAccountRs, nameof(me.NextAccountRs));
                AssertEquals(16992224448242675179, me.NextAccountId, nameof(me.NextAccountId));
                AssertEquals(_knownShufflingId, me.ShufflingId, nameof(me.ShufflingId));
            }
        }

        private void TestShufflingCancel()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                throw new NotImplementedException();
            }
        }

        private void TestShufflingProcess()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var expectedAmount = Amount.CreateAmountFromNxt(10);
                const int expectedParticipantCount = 3;
                const int expectedRegistrationPeriod = 720;

                var create = _shufflingService.ShufflingCreate(expectedAmount, expectedParticipantCount, expectedRegistrationPeriod, 
                    CreateTransaction.CreateTransactionByPublicKey()).Result;

                var attachment = (ShufflingCreationAttachment) create.Transaction.Attachment;

                AssertEquals(expectedAmount.Nqt, attachment.Amount.Nqt, nameof(attachment.Amount));
                AssertEquals(0, attachment.HoldingId, nameof(attachment.HoldingId));
                AssertEquals((int)HoldingType.Nxt, (int)attachment.HoldingType, nameof(attachment.HoldingType));
                AssertEquals(expectedParticipantCount, attachment.ParticipantCount, nameof(attachment.ParticipantCount));
                AssertEquals(expectedRegistrationPeriod, attachment.RegistrationPeriod, nameof(attachment.RegistrationPeriod));
            }
        }
    }
}

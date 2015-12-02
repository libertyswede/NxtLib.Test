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

        public ShufflingServiceTest(ILogger logger, IShufflingService shufflingService)
        {
            _logger = logger;
            _shufflingService = shufflingService;
        }

        public void RunAllTests()
        {
            TestGetAccountShufflings();
            TestGetAllShufflings();
        }

        private void TestGetAllShufflings()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var shufflings = _shufflingService.GetAccountShufflings(TestSettings.Account1, true).Result;
            }
        }

        private void TestGetAccountShufflings()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var shufflings = _shufflingService.GetAccountShufflings(TestSettings.Account1, true).Result;

                var knownShuffling = shufflings.Shufflings.Single(s => s.ShufflingId == 17626955119804441745);

                AssertEquals(2, shufflings.Shufflings.Count(), "Shufflings.Count()");
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
                AssertEquals("9164b2334a869ff4db6795777d4afd6aaaff4217dc47cb1baa42f43530ee9e93", knownShuffling.ShufflingFullHash.ToHexString(), nameof(knownShuffling.ShufflingFullHash));
                AssertEquals("cb8c1482560052b87ff992e567cfdc7bf13a1f069a1df3f81de98183baf8906e", knownShuffling.ShufflingStateHash.ToHexString(), nameof(knownShuffling.ShufflingStateHash));
                AssertEquals(ShufflingStage.Done, knownShuffling.Stage, nameof(knownShuffling.Stage));
            }   
        }
    }
}

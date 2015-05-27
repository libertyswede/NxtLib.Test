using NxtLib.TaggedData;

namespace NxtLib.Test.TaggedData
{
    class TaggedDataServiceTest : TestBase
    {
        private readonly TaggedDataService _taggedDataService;

        internal TaggedDataServiceTest()
        {
            _taggedDataService = (TaggedDataService)TestSettings.ServiceFactory.CreateTaggedDataService();
        }

        internal void RunAllTests()
        {
            UploadTaggedData();
        }

        internal void UploadTaggedData()
        {
            using (Logger = new TestsessionLogger())
            {
                const string name = "testname";
                const string data = "abc123";
                const string description = "description";
                const string tags = "tag1,tag2";
                const string channel = "channel?";
                const string type = "type?";
                const string filename = "test.txt";

                var transaction = _taggedDataService.UploadTaggedData(name, data, CreateTransaction.CreateTransactionByPublicKey(),
                        description, tags, channel, type, true, filename).Result.Transaction;
                var attachment = (TaggedDataUploadAttachment)transaction.Attachment;

                AssertEquals(name, attachment.Name, "Name");
                AssertEquals(data, attachment.Data, "Data");
                AssertEquals(description, attachment.Description, "Description");
                AssertEquals(tags, attachment.Tags, "Tags");
                AssertEquals(channel, attachment.Channel, "Channel");
                AssertEquals(type, attachment.DataType, "DataType");
                AssertIsTrue(attachment.IsText, "IsText");
                AssertEquals(filename, attachment.Filename, "Filename");
            }
        }
    }
}

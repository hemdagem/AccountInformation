using System.Collections.Generic;
using System.Data;
using Moq;

namespace Accounts.Tests.Unit.TestHelpers
{
    public static class DataReaderTestHelper
    {
        public static IDataReader Reader(Dictionary<string, object> dictionary)
        {
            var moq = new Mock<IDataReader>();
            var count = 0;

            moq.Setup(x => x.Read()).Returns(() => count < 1).Callback(() => count++);

            foreach (KeyValuePair<string, object> keyValuePair in dictionary)
            {
                var keyValuePairItem = keyValuePair;
                moq.Setup(x => x[keyValuePairItem.Key]).Returns(() => keyValuePairItem.Value);
            }
            return moq.Object;
        } 

        public static IDataReader MultipleResultsReader(Dictionary<string, object> dictionary, Queue<bool> readQueue)
        {
            var moq = new Mock<IDataReader>();

            moq.Setup(x => x.Read()).Returns(readQueue.Dequeue);

            foreach (KeyValuePair<string, object> keyValuePair in dictionary)
            {
                var keyValuePairItem = keyValuePair;
                moq.Setup(x => x[keyValuePairItem.Key]).Returns(() => keyValuePairItem.Value);
            }

            return moq.Object;
        }


    }
}

using FsTask.AcceptanceTests.StepDefinitions.Drivers;

namespace FsTask.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class SensorDataStepDefinitions
    {
        private readonly ISensorDataDriver _driver;

        public SensorDataStepDefinitions()
        {
            _driver = new SensorDataApiDriver();
        }

        
        [When(@"An event just received from a sensor at timestamp '([^']*)' as follow")]
        public async Task WhenAnEventJustReceivedFromASensorAtTimestampAsFollow(string timestamp, Table eventTable)
        {
            await _driver.SendData(timestamp, eventTable);
        }

        [Then(@"The event data will be persist as follow")]
        public async Task ThenTheEventDataWillBePersistAsFollow(Table expectedEventTable)
        {
            await _driver.Assert(expectedEventTable);
        }


        [Given(@"An event received from a sensor at timestamp '([^']*)'")]
        public async Task GivenAnEventReceivedFromASensorAtTimestamp(string timestamp)
        {
            await _driver.SendData(timestamp);
        }

        [When(@"An event arrived from a sensor at with timestamp '([^']*)'")]
        public async Task WhenAnEventArrivedFromASensorAtWithTimestamp(string timestamp)
        {
            await _driver.SendData(timestamp);
        }

        [Then(@"The events data will be show in an ordered manner based on timestamp")]
        public async Task ThenTheEventsDataWillBeShowInAnOrderedMannerBasedOnTimestamp(Table table)
        {
            await _driver.AssertTheOrderOfEvents(table);
        }

    }
}

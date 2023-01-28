
using FsTask.AcceptanceTests.StepDefinitions.Drivers;
using FsTask.ApplicationServices;

namespace FsTask.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class SensorDataStepDefinitions
    {
        private readonly ISensorDataDriver _driver;
        private DashboardReportViewModel _result;

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

        #region
        [Given(@"These human environmental statistics was received from sensors")]
        public async Task GivenTheseHumanEnvironmentalStatisticsWasReceivedFromSensors(Table table) 
            => await _driver.StoreSensorData(table);

        [Given(@"These position x of human environmental statistics was received from sensors")]
        public async Task GivenThesePositionXOfHumanEnvironmentalStatisticsWasReceivedFromSensors(Table table)
            => await _driver.StoreSensorDataPosX(table);

        [Given(@"These position y of human environmental statistics was received from sensors")]
        public async Task GivenThesePositionYOfHumanEnvironmentalStatisticsWasReceivedFromSensors(Table table)
            => await _driver.StoreSensorDataPosY(table);


        [When(@"Fetching human environmental statistics based on time and human between '([^']*)' and '([^']*)'")]
        public async Task WhenFetchingHumanEnvironmentalStatisticsBasedOnTimeAndHumanBetweenAnd(long @from, long @to) =>
            _result = await _driver.FetchSensorData(from, to, ReportFilter.Human);

        [When(@"Fetching human environmental statistics based on time and x position between '([^']*)' and '([^']*)'")]
        public async Task WhenFetchingHumanEnvironmentalStatisticsBasedOnTimeAndXPositionBetween(long @from, long @to) =>
            _result = await _driver.FetchSensorData(from, to, ReportFilter.PositionX);


        [Then(@"The event data will be fetched as follow")]
        public async Task ThenTheEventDataWillBePersistAsFollows(Table table)
        => await _driver.Assert(_result, table);

        [Then(@"The event data of position x series will be fetched as follow")]
        public async Task ThenTheEventsDataOfPositionXSeriesWillBePersistAsFollows(Table table)
        => await _driver.AssertPositionXSeries(_result, table);
        

        [When(@"Fetching human environmental statistics based on time and y position between '([^']*)' and '([^']*)'")]
        public async Task WhenFetchingHumanEnvironmentalStatisticsBasedOnTimeAndYPositionBetweenAnd(long from, long to)
        {
            _result = await _driver.FetchSensorData(from, to, ReportFilter.PositionY);
        }

        [Then(@"The event data of position y series will be fetched as follow")]
        public async Task ThenTheEventDataOfPositionYSeriesWillBeFetchedAsFollow(Table table)
            => await _driver.AssertPositionYSeries(_result, table);



        #endregion
    }
}

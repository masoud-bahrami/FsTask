# FsTask

FaTask is a playground asp net core application that simulate wow to send different data from different sensors to an API. This API then adds the data to a queue for processing.


Received events are queued and then processed asynchronously using *PullingSensorDataFromQueueHostedService*. Events are added to a append-only storage. These events can be stored in a time series database.

These events can then be retrieved based on when they occurred. When fetching, hyperlinks are also sent that will help you.


# How to run
You need dotnet core 6. Also you can run the app using docker file.

# Integration tests
You can use integration test to import data to the [Quest DB](https://questdb.io/). 

Use *FilteredDataHuman.json* file(exists in the *IntegrationTestProject*)  as a template to prepare the test data used in the integration test.

# Time-series DB
In the production environment, the data received from the sensors is stored in the []*Quest DB* database

# Acceptance tests
There are two acceptance schenario you can view in the FsTask.AcceptanceTests project.


```
@receiving_data
Scenario: Adjusting the timestamp of events
	Given An event received from a sensor at timestamp '1662896469284'
	When An event arrived from a sensor at with timestamp '1662896469200'
	Then The events data will be show in an ordered manner based on timestamp
		| timestamp     | order |
		| 1662896469200 | 0     |
		| 1662896469284 | 1     |
```

```
receiving_data
Scenario: Receiving a single event from a sensos
	When An event just received from a sensor at timestamp '1662896469284' as follow
		| human_id                 | pos_x | pos_y | vel_x | vel_y |
		| 631dad359fbc895818809423 | 15.32 | 8.75  | 0     | 0     |
	Then The event data will be persist as follow
		| timestamp     | human_id                 | pos_x | pos_y | vel_x | vel_y |
		| 1662896469284 | 631dad359fbc895818809423 | 15.32 | 8.75  | 0     | 0     |
```

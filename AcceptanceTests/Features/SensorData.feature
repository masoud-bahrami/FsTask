Feature: Sensor Data


@receiving_data
Scenario: Receiving a single event from a sensos
	When An event just received from a sensor at timestamp '1662896469284' as follow
		| human_id                 | pos_x | pos_y | vel_x | vel_y |
		| 631dad359fbc895818809423 | 15.32 | 8.75  | 0     | 0     |
	Then The event data will be persist as follow
		| timestamp     | human_id                 | pos_x | pos_y | vel_x | vel_y |
		| 1662896469284 | 631dad359fbc895818809423 | 15.32 | 8.75  | 0     | 0     |

@receiving_data
Scenario: Adjusting the timestamp of events
	Given An event received from a sensor at timestamp '1662896469284'
	When An event arrived from a sensor at with timestamp '1662896469200'
	Then The events data will be show in an ordered manner based on timestamp
		| timestamp     | order |
		| 1662896469200 | 0     |
		| 1662896469284 | 1     |

@receiving_data @fetchEvents_human_time
Scenario: Fetch sensor data based on time and human
	Given These human environmental statistics was received from sensors
		| timestamp     | human1   | human2   | human3   |
		| 1662896469284 | detected |          | detected |
		| 1662896469280 | detected | detected | detected |
		| 1662896469270 | detected |          |          |
		| 1662896469260 |          | detected |          |
		| 1662896469272 |          | detected |          |
	When Fetching human environmental statistics based on time and human between '1662896469270' and '1662896469280'
	Then The event data will be fetched as follow
		| timestamp     | human1   | human2   | human3   |
		| 1662896469270 | detected |          |          |
		| 1662896469272 |          | detected |          |
		| 1662896469280 | detected | detected | detected |

@receiving_data @fetchEvents_positionX_time
Scenario: Fetch sensor data based on time and position x
	Given These position x of human environmental statistics was received from sensors
		| timestamp     | human1                      | human2                       | human3                       |
		| 1662896469284 | detected at postion 1.5-2.1 |                              | detected at postion 10.2-2.1 |
		| 1662896469280 | detected at postion 5.2-2.1 | detected at postion 1.4-2.1  | detected at postion 10.2-2.1 |
		| 1662896469270 | detected at postion 5.0-2.6 |                              |                              |
		| 1662896469260 |                             | detected at postion 10.2-2.1 |                              |
		| 1662896469272 |                             | detected at postion 12.9-2.1 |                              |
	When Fetching human environmental statistics based on time and x position between '1662896469270' and '1662896469280'
	Then The event data of position x series will be fetched as follow
		| timestamp     | human1's x position | human2's x position | human3's x position |
		| 1662896469270 | 5.0                 |                     |                     |
		| 1662896469272 |                     | 12.9                |                     |
		| 1662896469280 | 5.2                 | 1.4                 | 10.2                |


@receiving_data @fetchEvents_positionX_time
Scenario: Fetch sensor data based on time and position y
	Given These position y of human environmental statistics was received from sensors
		| timestamp     | human1                      | human2                       | human3                       |
		| 1662896469284 | detected at postion 1.5-2.1 |                              | detected at postion 10.2-2.1 |
		| 1662896469280 | detected at postion 5.2-2.1 | detected at postion 1.4-2.1  | detected at postion 10.2-2.1 |
		| 1662896469270 | detected at postion 5.0-2.6 |                              |                              |
		| 1662896469260 |                             | detected at postion 10.2-2.1 |                              |
		| 1662896469272 |                             | detected at postion 12.9-2.1 |                              |
	When Fetching human environmental statistics based on time and y position between '1662896469270' and '1662896469280'
	Then The event data of position y series will be fetched as follow
		| timestamp     | human1's y position | human2's y position | human3's y position |
		| 1662896469270 | 2.6                 |                     |                     |
		| 1662896469272 |                     | 2.1                 |                     |
		| 1662896469280 | 2.1                 | 2.1                 | 2.1                 |

		

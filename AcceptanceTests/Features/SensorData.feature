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
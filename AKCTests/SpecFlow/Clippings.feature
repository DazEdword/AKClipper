Feature: Clippings
	Testing behaviour of base clipping classes. 

Scenario: Null clip handling. 
	When I use a null clipping item.
	Then Class has to return "null"

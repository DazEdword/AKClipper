Feature: parsing_entry
	In order to have useful information
	As a user
	I want to parse an entry

Background:
	Given a text to parse
	"""
	WHY CAN’T WE FALL ASLEEP?  (newyorker.com)
	- Your Highlight on Location 24-25 | Added on Saturday, July 18, 2015 8:54:04 AM
	
	If you’re “out of phase” from typical bedtimes due to circadian disruption, for example, your melatonin levels are off:
	==========
	﻿Freethinkers: A History of American Secularism (Jacoby, Susan)
	- Your Highlight on page 3 | Location 90-91 | Added on Wednesday, August 12, 2015 8:54:41 PM
	
	Bush’s very presence in the pulpit attested powerfully to the erosion of America’s secularist tradition;
	==========
	Freethinkers: A History of American Secularism (Jacoby, Susan)
	- Your Highlight on page 33 | Location 557-560 | Added on Wednesday, August 12, 2015 10:03:09 PM
	
	The absurdity of the claim that the framers somehow overlooked, or misunderstood, the political and religious implications of leaving God out of the nation’s founding document is borne out not only by Washington’s matter-of-fact assumption of the distinction between religious affiliation and citizenship but by the intensity and clarity of the public debate that preceded ratification of the Constitution.
	==========
	"""
	When I parse it
		
Scenario: Delimiters
Scenario: number of entries
Scenario: catch the name of the book
	Given a text to parse
	"""
	WHY CAN’T WE FALL ASLEEP?  (newyorker.com)
	- Your Highlight on Location 24-25 | Added on Saturday, July 18, 2015 8:54:04 AM
	
	If you’re “out of phase” from typical bedtimes due to circadian disruption, for example, your melatonin levels are off:
	==========
	"""
	When I parse it
	Then the book name should be "WHY CAN’T WE FALL ASLEEP?"



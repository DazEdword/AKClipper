Feature: the parser should recognise the language of the entry
	In order to correctly parse the entries
	As a parser
	I want to be able to recognise the language


Scenario: Recognise an entry in English
	Given I have entered an entry in English
	When I parse it
	Then the language recognised should be English

Scenario: Recognise an entry in Spanish
	Given I have entered an entry in Spanish
	When I parse it
	Then the language recognised should be Spanish
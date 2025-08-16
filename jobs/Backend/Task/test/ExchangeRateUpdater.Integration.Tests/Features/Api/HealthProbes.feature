Feature: HealthProbes

@HappyPath
Scenario: Liveness responds with 'Healthy'
	When I call the 'health' endpoint
	Then HttpStatus 200 is returned
	And response is a string like 'Healthy'

@HappyPath
Scenario: Readiness responds with 'Healthy'
	When I call the 'ready' endpoint
	Then HttpStatus 200 is returned
	And response is a string like 'Healthy'
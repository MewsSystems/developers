# Mews QA Interview
In this project you can find couple of tests for Mews API. For creating requests, a `RestSharp` library from Nuget is used and for assertion of the data we are using NUnit.

## Setup
1. Clone
2. Build Solution
3. If not opened, Click on Test > Windows > Test Explorer.
4. For easier readability, in the Test Explorer, uncheck the `Show test hierarchy` and choose `Group by class`.
5. Run all tests by clicking on the `Run All` button, or separate tests by right-clicking on them and choosing `Run Selected Tests` (works for the classes as well, if you want to run all tests for specific endpoint).

Using NUnit TestAdapter 3 we should be able to view all test methods (marked with `[Test]` attribute) as separate tests in the Test Explorer window. All Asserts have a message in case of failure that should help to quickly analyze, why the test failed.

## Info
I'm using helper methods that I developed earlier to simplify the creation of requests and reusability for more endpoints. I'm also using POCO model for mapping the json request and response bodies as c# classes for easier access when asserting. 

The response code 200 is implicitly tested inside the method that creates request, so therefore, no separate test for response code 200 is present. When expecting other than 200, it can be specified in the `CreateRequest` method (seen in action in `When_GetConfigInvalid_Expect_400` test).
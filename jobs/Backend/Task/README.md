# Solutions

Given the instructions for the backend test I've chosen to implement a Web API to solve the task.
At high level, this API returns the exchange rates with CZK currency for the input currencies (in JSON format as body for a POST request).

As the instructions indicated to look for the Czech National Bank as source for the exchange rates and this source only provides exchange rates for CZK currency, I've asummed that this will always be source currency.
The source currency could be changed if another exchange rate source is used.

# Notes

As a production-ready API, I've decided to include a simple Dockerfile that can be used to containerize the application and deploy it in Azure, AWS, Kubernetes, etc.
I've also included Postman collections to test several HTTP POST requests.
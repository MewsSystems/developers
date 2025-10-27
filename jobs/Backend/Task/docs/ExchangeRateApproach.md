# Approach 1

## System Design
![alt text](Approach1.png?raw=true)


The idea of this approach is to use the API provided by Czech National Bank to retrieve the exchange data in the form of XML.

By creating a XML template file that stores an example output of the XML response it can be used in different ways.
Implementing a class associated with this XML format we can determine the expected response and map the elements and attributes. This also offers a mock response that can be used in the case of testing, by using the model of the response we can see that the expected functioning of the program. 

The logic in relation to the getting the associated xml attributes is done in the ExchangeRateprovider file. The goal was to read the model based on the XML response and then deserialise into a string. An issue became present in which instead of reading the xml file we could just read it directly from the API stream. Reading directly from the XML file is only useful for files that are already store in the machine or in use in testing. This quickly changed the approach in what would be more efficient.


# Approach 2
## System Design
![alt text](Approach2.png?raw=true)

Based off of Approach 1, instead of using an XML model response file for receiving the data, we could directly extract the information from the stream itself. This lead to using stream async to parse the content in the response. Once the http call has been made, and the response is successful it is then serliasided based off the XML class format allowing to receive different attributes and elements of the response. Once the response is then deserliased, it then can be returned for use in the GetExchangeRates() method.

The GetExchangeRates() method is where the logic is to perform the extraction, filtering and conversion of the currency types. 

The main flow is calls the http method which using async Task allows for the asynchronous call instead of waiting for a response it can move onto the next step, this is an advantage if the API takes a while to call and receive a response, especially with a large response. The flow proceeds to create a list to store the new format of the exchange rate. After this it iterates through the XML response based on the “Tabulka.Radek” item which corresponds to the line for each currency based off the elements in the XML response. To filter the data a lock is implemented as a boolean to determine what currencies to return based on the defined currencies and its associated three char code. For the replace of “,” for “.” It uses the Replace() method which replaces the commas for a full stop to represent a decimal place. From here the conversion happens where we can see how much CZK is to the defined currencies. Based off this the data is retuned in a new structure in the format of the CZK / “Currency” and value defined in the public override ToString() method.

# Testing
The test that could be used is to read the mocked XML response instead of calling the http method. It would test to ensure that the data is being properly read and processed according to main implementation.

# Output
Successfully retrieved 5 exchange rates:
CZK/EUR=24.325
CZK/JPY=0.13671
CZK/THB=0.6394
CZK/TRY=0.49848
CZK/USD=20.897

# Challenges 

* Instead of using a reader to read based off the model of the XML file, stream was used instead as it provided direct extraction from the response itself.

* Task was used for the GetExchangeRates() method as Task<IEnumerable<ExhangeRate>> does not contain a public instance or extension definition for GetEnumerator. 

* To keep the program in line with asynchronous processing, aysnc was used in the Main() method but .NET 6 doesn’t support the use of async for the Main() method so GetAwaiter() and GetResult() were used. By using these methods the call could be made to produce the output but the disadvantage is that it waits for a response which is slower for real time applications and use in production.

* Format of the output was an issue with the use of commas as base on the API response it used commas. To display the value accurately decimal point had to be used as the decimal point would be in a different location leading to an inaccurate output. By using the built in Replace() method, the commas were replaced with decimal points which allowed the ToDecimal() method to convert the string representation to decimal format accurately.

* For the designated test I wasnt able to get working in a test folder due to it not being read by project.

# Exchange Rate Client

*a Mews Frontend test for [the job](Job.md)*

## Prerequisites

- Node.js & npm
- Git bash or alternative

## Setup

To install all project dependencies, run `npm install` in the root folder.

## Your task

You should start with creating a fork of this repository. When you're finished with the task create a pull request.

In `server` folder there is a simple currency exchange rate server configured, with a few API calls. It gives a random exchange rate updates for currency pairs. Your task is to write a client application that will periodically request updates from the server and display them to users. There is a skeleton app prepared in the `app` folder which you can use. The application should be composed of two main components:
- Currency pairs selector - Allows the user to filter displayed currency pairs.
- Currency pairs rate list - Displays shortcut name, current value and trend for each of the selected currency pair. Shortcut is defined as `{name1}/{name2}`. Trend is defined as:
    - growing, if `prevValue < nextValue`
    - declining, if `prevValue > nextValue`
    - stagnating, if `prevValue == nextValue`

When the aplication is loaded, it should download the configuration from `/configuration`. It should take a list of available exchange rates returned from the API call and use it to display controls. Then start periodical requests for rates update through the `/rates` API call, and display the results.

Filtering of currency pairs should be done client-side, so if filter changes between the updates, the change should be immediate.

Beware that the server is slow and not very reliable. The initial time to load the configuration can take some time (simulated by timeout in response). Also, when requesting the updates, there is a slight chance that the request will fail (simulated by random 500 HTTP status code response). Your client should handle both situations properly.

The visual side of the application is not the primary goal of task, however you should use at least some minimal css styling.

### Bonus points

- Use React & Redux.
- Use any kind of modular approach to css (i.e. https://github.com/css-modules/css-modules).
- Keep the configuration and user filters saved between application reloads.

## Starting the server and the app

- To start the server, run `npm run start-server` in the root folder. The server will start listening on the localhost at port 3000.
- To start the app, run `npm start` in the root folder. This will run `webpack-dev-server` on localhost:8080. It also watches files and does incremental updates, so you can keep it running when you do any changes in your app.

## Api calls

#### Get Configuration

**Request** `[endpoint]/configuration`
```
{ }
```

**Response**
```
{
    currencyPairs: {
        id1: [{ code: 'EUR', name: 'Euro' }, { code: 'USD', name: 'US Dollar' }],
        id2: [{ name: 'GBP', name: 'British Pound' }, { code: 'JPY', name: 'Japanese Yen' }],
        ...
    }
}
```

| Property | Type | | Description |
| --- | --- | --- | --- |
| currencyPairs | object of pairs | required | An object of available currencies.

#### Get rates

**Request** `[endpoint]/rates`
```
{
    currencyPairIds: [ 'id2' ]
}
```

| Property | Type | | Description |
| --- | --- | --- | --- |
| currencyPairIds | array of numbers | required | An array of requested currency pairs rates.

**Response**
```
{
    rates: {
        id2: 1.0345 
    }
}
```

| Property | Type | | Description |
| --- | --- | --- | --- |
| rates | object | required | An object of new exchange rates for every requested currencyPairId.

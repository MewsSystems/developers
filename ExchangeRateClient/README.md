# Exchange Rate Client

*a Mews Frontend test*

## Prerequisites

- Node.js & npm
- Git bash or alternative

## Setup

To install all project dependencies, run `npm install` in the root folder.

## Project description

In `server` folder there is a simple currency exchange rate server configured, with few api calls. It gives random exchange rate updates for a few currency pairs, and localized news messages at random time. Your task is to write a simple client app, that will periodically request updates from the server and display them to a user. There is a skeleton app prepared in the `app` folder.

### Api cals

#### Get Configuration

**Request** `[endpoint]/configuration`
```
{ }
```

**Response**
```
{
    currencyPairs: [
        { name: 'EUR/GBP', id: 'id1' },
        { name: 'USD/JPY', id: 'id2' },
        ...
    ],
    languages: [
        { name: 'English', code: 'en-US' },
        { name: 'Czech', code: 'cs-CZ' },
        ...
    ]
}
```

| Property | Type | | Description |
| currencyPairs | array of currencies | required | An array of available currencies.

#### Get rates

**Request** `[endpoint]/rates`
```
{
    currencyPairIds: [ 'id2' ]
}
```

| Property | Type | | Description |
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
| rates | object | required | An object of new exchange rates for every requested currencyPairId.

## Your task

You should start with creating a fork of the repository. When you're finished with the task, you should create a pull request.

Your task is to write a client application, that will periodically request updates from the server and display them to a user. There is a skeleton app in the `app` folder you can use. The application should be composed from three main components:
- Currency pairs selector - Allows user to filter displayed currency pairs.
- Currency pairs rate list - Displays name, current value and trend for each selected currency pair. Trend is defined as:
    - growing, when `prevValue < nextValue`
    - declining, when `prevValue > nextValue`
    - stagnating, when `prevValue == nextValue`
- News list - Displays last 5 news localized to the language user has selected.

When is the aplication loaded, it should downloads the configuration from the `/configuration` api. From the configuration, it should take a list of available exchange rates and a list of available language codes and use this to display the proper controls to the user. After that, start the periodical requests for updates, with proper parameters, and display the results to the user.

The graphical side of the application is not the primary focus of task, however you should use at least some minimal css styling. 

### Requirements

- The filtering should be done on client-side, meaning if filter changes between the updates, the change should be immediate.
- The same applies to localization, it should change the localized parts without a reload of page.

### Bonus points

- Use React & Redux.
- Use some kind of modular approach to css (i.e. https://github.com/css-modules/css-modules).
- Keep the configuration and user filters saved between application reloads.

## Starting the server and the app

- To start the server, run `npm run start-server` in the root folder. The server will start listening on the localhost at port 3000.
- To start the app, run `npm start` in the root folder. This will run `webpack-dev-server` on the localhost at port 8080. It also watches the app files and does incremental updates, so you can keep it running when you do changes in the app.

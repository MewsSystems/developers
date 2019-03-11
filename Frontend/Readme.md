# Mews frontend developer task

Before starting, make sure you have Node.js, npm and Git bash or alternative. To install all project dependencies, run `npm install` in the root folder.

## Your task

You should start with creating a fork of the repository. When you're finished with the task, you should create a pull request.

In `server` folder there is a simple currency exchange rate server configured, with few api calls. It gives random exchange rate updates for a few currency pairs. Your task is to write a client application, that will periodically request updates from the server and display them to a user. There is a skeleton app prepared in the `app` folder that you can use. The application should be composed from two main components:
- Currency pairs selector - Allows user to filter displayed currency pairs.
- Currency pairs rate list - Displays shortcut name, current value and trend for each selected currency pair. Shortcut is defined as `{name1}/{name2}`. Trend is defined as:
    - growing, when `prevValue < nextValue`
    - declining, when `prevValue > nextValue`
    - stagnating, when `prevValue == nextValue`

When is the aplication loaded, it should downloads the configuration from the `/configuration` api. From the configuration, it should take a list of available exchange rates and use it to display the proper controls to the user. After that, start the periodical requests for updates through `/rates` api, with proper parameters, and display the results to the user.

The filtering of currency pairs should be done on client-side, meaning if filter changes between the updates, the change should be immediate.

Beware that the server is slow and not very reliable. The initial time to load the configuration can take some time (simulated by timeout in response). Also, when requesting the updates, there is a slight chance that the request will fail (simulated by random 500 HTTP status code response). Your client should handle both situations properly.

The graphical side of the application is not the primary focus of task, however you should use at least some minimal css styling. 

### Bonus points

- Use React & Redux.
- Use some kind of modular approach to css (i.e. https://github.com/css-modules/css-modules).
- Keep the configuration and user filters saved between application reloads.

## Starting the server and the app

- To start the server, run `npm run start-server` in the root folder. The server will start listening on the localhost at port 3000.
- To start the app, run `npm start` in the root folder. This will run `webpack-dev-server` on the localhost at port 8080. It also watches the app files and does incremental updates, so you can keep it running when you do changes in the app.

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
        id2: [{ code: 'GBP', name: 'British Pound' }, { code: 'JPY', name: 'Japanese Yen' }],
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

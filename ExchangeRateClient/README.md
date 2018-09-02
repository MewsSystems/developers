# Exchange Rate Client

*a Mews Frontend test for [the job](Job.md)*

## Prerequisites

- Node.js & npm
- Git bash or alternative

## Setup

To install all project dependencies, run `npm install` in the root folder.

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

## Implemented task

### Implementation

Most used libraries:
* react
* redux
* react-redux
* reselect
* ramda, ramda-adjunct, ramda-extension
* reactstrap

The task is built with React library using Redux architecture. Almost all of the utility functions for processing data 
in actions and reducers are handled through functional programming with 'ramda' library and its other extensions. 
Selectors were created with 'reselect' which has a memoization functionality that prevents unnecessary value 
recalculations. 'redux-thunk' has been used for async actions as well as for creating fat actions. The redux 
architecture for this task has 'thin reducers, fat action creators' approach whereby most of the business logic is 
handled in the action creators. Persistence of applications state is handled by 'redux-persist' which saves the 
application state in local storage. 


Most of the design is from Bootstrap 4 with React wrappers provided by 'reactstrap' library. The only custom styled 
component in the application are the tracking checkboxes which have been styled with the help of 'styled-components' library.

Directories:
* **actions:** Redux actions, 
* **api:** API calls that return promises
* **components:** Components that are used across multiple containers
* **containers:** Bigger components that are connected to the store and are composed of smaller components 
* **constants:** Constants used in applications
* **reducers:** Redux reducers
* **selectors:** Redux selectors mostly created with 'reselect' library
* **store:** Contains store configuration as well as root reducer and initial state
* **utils:** Utility functions used across the code
* **validations:** Validation functions for forms

The application state has the following structure:
```
{
    config: { endpoint, interval },
    currencyPairs: {
        byId: {
            currencyPairId1: {
                currentRate: 3.333,
                growthRate: 2,
                pair: [{ code: 'EUR', name: 'Euro' }, { code: 'USD', name: 'US Dollar' }],
                previousRate: 1.111
                track: true,
                trend: 'growing',
            },
            currencyPairId2: ...,
            ...
        },
        allIds: [currencyPairId1, currencyPairId2, ...],
    },
    errorMessages: {
        errorMessageId1: ...,
        errorMessageId2: ...,
    },
    isFetching {
            currencyPairs: false,
            rates: false,
        },
    ratesHistory: {
        currencyPairId1: [{ timestamp: ..., rate: ... }, ...],
        currencyPairId2: [{ timestamp: ..., rate: ... }, ...],
    },
    uiControl: {
        showCountdown: false,
        tables: {
            currencyPairs: {
                currentPage: 1,
            },
            ratesHistory: {
                currentPage: 1,
            },
        },
    },
}
```

### UI

The main page of the application consists of the currency table that is structured as follows:

| ID | Currency pair | Current rate | Trend | Growth rate | Track? |
| --- | --- | --- | --- | --- | --- |
| Id of currency pair | Currency pair string in the form of `${curr1.code} - ${curr1.name} ↔ ${curr2.code} - ${curr2.name}` | Current rate, also shows previous rate if available as `${previousRate} → ${currentRate}` | One of DECLINING, STAGNATING or GROWING | Growth rate shown in percentage to four decimal places | Checkbox that tracks or untracks rate updates of given currency pair

Clicking on the ID cell of any row opens another row beneath that shows data about the current currency pair such as 
lowest rate, highest rate and their respective timestamps. Furthermore, it also contains another table that shows the 
timestamp of its rate everytime it has been updated. The table is structured as follows:

| Timestamp | Rate |
| --- | --- |
| Timestamp when the currency pair's rate was updated | The updated value of the currency pair's rate |

The main table has toolbars above it:

* **Row filter textfield:** Filters the rows on the basis of the value typed on the form which is then matched with the 
currency pair names
* **Sort toolbar:** Four buttons that sort the table according to currency pair name, current rate, trend or growth 
rate. On first click it will sort the table in ascending order.
* **Track toolbar:** In addition to tracking/untracking the currency pairs from their respective rows the toolbar 
further allows to track/untrack all the currency pairs or only currency pairs on the current page.
* **Number of rows selectbox:** Handles the number of rows displayed on one page.
 
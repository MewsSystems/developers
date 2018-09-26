# Exchange Rate Client

_a Mews Frontend test for [the job](Job.md)_

## Prerequisites

- Node.js & yarn
- Git bash or alternative

## Setup

```sh
yarn && cd client-app && yarn
```

## Starting the server and the app

For starting both client and server run :

```sh
yarn dev
```

## Tests Eslint Flow

client

```sh
cd client-app
yarn test-ci
```

## Api cals

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

| Property      | Type            |          | Description                        |
| ------------- | --------------- | -------- | ---------------------------------- |
| currencyPairs | object of pairs | required | An object of available currencies. |

#### Get rates

**Request** `[endpoint]/rates`

```
{
    currencyPairIds: [ 'id2' ]
}
```

| Property        | Type             |          | Description                                 |
| --------------- | ---------------- | -------- | ------------------------------------------- |
| currencyPairIds | array of numbers | required | An array of requested currency pairs rates. |

**Response**

```
{
    rates: {
        id2: 1.0345
    }
}
```

| Property | Type   |          | Description                                                         |
| -------- | ------ | -------- | ------------------------------------------------------------------- |
| rates    | object | required | An object of new exchange rates for every requested currencyPairId. |

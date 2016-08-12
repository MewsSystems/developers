# Exchange Rate Client

*a Mews Frontend test*

## Prerequisites

- Node.js & npm
- Git bash or alternative. Windows Cmd will work too...

## Task

In `server` folder there is a simple exhange rate server configured, giving random rate updates for few currency pairs. Your task is to write simple client app, that will periodically request rates from the server and displays them to a user. There is a skeleton app prepared in `app` folder. Feel free to use any additional technology, library or framework you want!

## Starting the server and the app

First you have to install all project dependencies if you haven't done so yet, with `npm install` run in the root folder.

- To start the server, run `npm run start-server` in the root folder. The server will start listening on the localhost at port 3000.
- To start the app, run `npm start` in the root folder. This will run `webpack-dev-server` on the localhost at port 8080. It also watches the app files and does incremental updates, so you can keep it running when you do changes in the app.

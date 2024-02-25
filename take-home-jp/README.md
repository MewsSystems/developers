# Getting Started with Create React App

This project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app).

## Available Scripts

In the project directory, you can run:

### `npm start`

Runs the app in the development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in the browser.

The page will reload if you make edits.\
You will also see any lint errors in the console.

### `npm test`

Launches the test runner in the interactive watch mode.\
See the section about [running tests](https://facebook.github.io/create-react-app/docs/running-tests) for more information.

## What I would do to make production ready
#### Move all the text to copy files
- Move the text of buttons and banners to a copy file that way text can be centralised and text can be reused
- This would make it easier to translate the app to other languages if this app had to support multipl regions
#### Stricter linting rules
- Enforce strict coding rules to improve code quaity
#### Better use of logging
- Use logs to give an understanding of the application behaviour, particularly on network calls
- Store events and logs to a centralised (Logstash, Cloudwatch etc)
#### Centralised error handling
- When there are errors in the application, it would be good also to collect the errors and have alerting when they occurs (incorporate Sentry, Cloudwatch etc)
#### UI/UX improvements
- Improve the designs
- Add search to url so user can navigate straight to the search term
#### Additional forms of testing 
- More unit tests on the components to validate their behaviour
- Integration tests on pages and large components to ensure the application works as expected


import { hot } from 'react-hot-loader/root';
import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';

const HotWrapper = process.env.NODE_ENV === 'production' ? App : hot(App);

ReactDOM.render(<HotWrapper />, document.querySelector('.root'));

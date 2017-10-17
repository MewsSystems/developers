import React from 'react';
import ReactDom from 'react-dom';
import { endpoint, interval } from './config';
import {getRates, getConfig} from './utils/api';
import './styles/common.less';

getConfig()
  .then(config => getRates(Object.keys(config.currencyPairs)))
  .then(data => console.log(data))
  .catch(e => console.log(e));

ReactDom.render(
  <h1 className='test'>Build test</h1>,
  document.getElementById('root')
);

import React from 'react';
import ReactDom from 'react-dom';
import { endpoint, interval } from './config';
import './styles/common.less';

ReactDom.render(
  <h1 className='test'>Build test</h1>,
  document.getElementById('root')
);

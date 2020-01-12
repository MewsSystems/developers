import React from 'react';

import Filter from './Filter';
import Rows from './Rows';


const RateListView = () => (
  <table>
    <thead>
      <Filter />
    </thead>
    <tbody>
      <Rows />
    </tbody>
  </table>
);


export default RateListView;

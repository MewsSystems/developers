import React from 'react';

import Card from '../../../../atoms/Card/Card';
import Table from '../../../../components/Table/Table';
import Filter from './Filter';
import Rows from './Rows';

import StyledRateListView from './styles/StyledRateListView';


const RateListView = () => (
  <StyledRateListView>
    <Card>
      <Table>
        <Filter />
        <Rows />
      </Table>
    </Card>
  </StyledRateListView>
);


export default RateListView;

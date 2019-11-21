import React, {useState} from 'react';
import {useSelector, useDispatch} from 'react-redux';
import apiCall from '../api/apiCall';
import {addTableData} from '../actions/pairsActions';
import {interval, tableColumns} from '../../src/config';
import {Title, Table} from '../ui';
import {useInterval} from '../utils';

const RatesTable = () => {
  const rows = useTableRows ();

  return (
    <React.Fragment>
      <Title>Rates</Title>
      <Table rows={rows} />
    </React.Fragment>
  );
};

export default RatesTable;

function useTableRows () {
  const newSelectionKey = useSelector (state => state.pairs.selectionKey);
  const [selectionKey, setSelectionKey] = useState (newSelectionKey);
  const selectedPairs = useSelector (state => state.pairs.selectedPairs);
  const tableData = useSelector (state => state.pairs.tableData);
  const dispatch = useDispatch ();

  const fetchData = () => {
    apiCall ('/rates', {
      currencyPairIds: selectedPairs.map (e => e.id),
    }).then (data => dispatch (addTableData (data)));
  };
  const selectionChanged = newSelectionKey !== selectionKey;

  if (selectionChanged) {
    setSelectionKey (newSelectionKey);
    fetchData ();
  }
  useInterval (fetchData, interval, [selectionKey]);

  const columns = getTableColumns (tableData);
  const rowHeader = [
    'Currency pair / Time',
    ...columns.map (t => t && <TimeStamp time={t} />),
    'Tendency',
  ];
  const rowsBody = selectedPairs.map (pair => {
    const values = columns.map (c => tableData[c] && tableData[c][pair.id]);
    return [pair.code, ...values, <Tendency values={values} />];
  });

  return [rowHeader, ...rowsBody];
}

function getTableColumns (tableData) {
  const columns = Object.keys (tableData).sort ();

  const difference = tableColumns - columns.length;
  const columnsToAdd = Math.max (difference, 0);
  const missingColumns = new Array (columnsToAdd).fill ();

  return [
    ...missingColumns, // adding blank columns
    ...columns,
  ];
}
const keys = {
  UP: 'growing ↑',
  DOWN: 'declining ↓',
  SAME: 'stagnating →',
};

const Tendency = ({values}) => {
  const lastValue = values[values.length - 1];
  const preLastValue = values[values.length - 2];

  if (!lastValue || !preLastValue) return 'unknow';

  const relation = lastValue / preLastValue;
  const isGrowing = relation > 1;
  const isSame = relation === 1;
  const direction = isSame ? 'SAME' : isGrowing ? 'UP' : 'DOWN';
  const tendency = keys[direction];

  if (relation === 1) return tendency;

  const change = Math.floor ((relation - 1) * 10000) / 100 + '%';
  return tendency + change;
};

const TimeStamp = ({time}) => {
  const date = new Date (Number (time));
  const format = method => ('0' + date[method] ()).substr (-2);

  const hours = format ('getHours');
  const minutes = format ('getMinutes');
  const seconds = format ('getSeconds');

  return `${hours}:${minutes}:${seconds} ⏱`;
};

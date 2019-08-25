import React from 'react';
import TableHead from 'Components/RatesTable/TableHead';
import TableBody from 'Components/RatesTable/TableBody';
import styles from 'Components/RatesTable/Table.module.css';

const RatesTable = ({ data }) => {
  return (
    <table className={styles.rates_table}>
      <TableHead />
      <TableBody data={data} />
    </table>
  );
}

export default RatesTable;

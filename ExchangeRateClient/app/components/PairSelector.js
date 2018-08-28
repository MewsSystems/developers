import React from 'react';

const PairSelector = ({ pairs, onChange, style }) => (
  <select onChange={onChange} style={style}>
      {Object.keys(pairs).map(key => {
          let pair = `${pairs[key][0].name} / ${pairs[key][1].name}`;
          return <option key={key} value={key}>{pair}</option>
      })}
  </select>
);

export default PairSelector
import React, { Component, } from 'react';
import { func, } from 'prop-types';


class RowsView extends Component {
  componentDidMount() {
    const { startRatesInterval, } = this.props;

    startRatesInterval();
  }


  render() {
    return (
      <tbody>
        <tr>
          <td>ahoj</td>
        </tr>
      </tbody>
    );
  }
}


RowsView.propTypes = {
  startRatesInterval: func.isRequired,
};


export default RowsView;

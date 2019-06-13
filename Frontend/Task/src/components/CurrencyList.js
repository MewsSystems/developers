import React, { Component } from 'react';
import { getShortcut } from 'helpers';
import { Loading } from './index';

class CurrencyList extends Component {
  getPrevious = (id) => {
    const { rates } = this.props;
    return id && rates.prevData[id] ? rates.prevData[id].toFixed(4) : '-';
  };

  getCurrent = (id) => {
    const { rates } = this.props;
    return id && rates.data[id] ? rates.data[id].toFixed(4) : '-';
  };

  getDifference = (id) => {
    const { rates } = this.props;
    return id && rates.data[id] && rates.prevData[id] ? (rates.data[id] - rates.prevData[id]).toFixed(4) : '-';
  };

  getTrend = (id) => {
    const { rates } = this.props;
    return id && rates.data[id] && rates.prevData[id] ?
      rates.data[id] === rates.prevData[id] ?
        'fas fa-minus' : rates.data[id] > rates.prevData[id] ?
          'fas fa-caret-up' : 'fas fa-caret-down' : 'fas fa-minus';
  };

  render() {
    const { configuration, rates, filter } = this.props;
    let _conf = filter.length ? Object.keys(configuration.data).filter(x => configuration.data[x].some(y => filter.indexOf(y.code) >= 0)) : Object.keys(configuration.data);
    return (
      <table className="currency-table">
        <thead>
          <tr>
            <th>
              <div>
                <span>Trend</span>
              </div>
            </th>
            <th>
              <div>
                <span>Name</span>
              </div>
            </th>
            <th>Previous Value</th>
            <th>Current Value</th>
            <th>Difference</th>
          </tr>
        </thead>
        <tbody>
          {configuration.loading ?
            <tr>
              <td colSpan="5">
                <Loading className="text-green" />
              </td>
            </tr>
            :
            _conf && _conf.length ?
              _conf.map(x => (
                <tr key={x}>
                  <td><i className={this.getTrend(x)} /></td>
                  <td>{getShortcut(configuration.data[x])}</td>
                  <td>{!rates.prevData && rates.loading ? <Loading className="text-small" /> : this.getPrevious(x)}</td>
                  <td>{!rates.prevData && rates.loading ? <Loading className="text-small" /> : this.getCurrent(x)}</td>
                  <td>{!rates.prevData && rates.loading ? <Loading className="text-small" /> : this.getDifference(x)}</td>
                </tr>
              ))
              :
              <tr>
                <td colSpan="5" className="text-center">No record found</td>
              </tr>
          }
        </tbody>
      </table>
    );
  };
};

export default CurrencyList;
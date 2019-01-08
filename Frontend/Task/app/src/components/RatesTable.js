import React, {Component} from 'react';
import {connect} from 'react-redux';
import apiCall from '../api/apiCall';
import {addTableData} from '../actions/pairsActions';
import {interval} from '../../src/config';

const getTendency = ({values, e}) => {
  const last = values[values.length - 1];
  const preLast = values[values.length - 2];
  let tendency = 'unknown', rel;
  if (last && preLast) {
    rel = last / preLast;
    if (rel === 1) tendency = 'stagnating';
    else {
      const percentageChange = Math.floor ((rel - 1) * 10000) / 100;
      tendency = rel > 1 ? 'growing ' : 'declining ';
      tendency += percentageChange + '%';
      tendency = [
        tendency,
        rel === 1
          ? '→'
          : <span
              className={rel > 1 ? 'green' : 'red'}
              key={percentageChange + e.pairCode}
            >
              {rel > 1 ? '↑' : '↓'}
            </span>,
      ];
    }
  }
  return tendency;
};

class RatesTable extends Component {
  componentDidMount () {
    this.props.fetchTableData (this);
  }

  componentDidUpdate (prevProps, prevState) {
    /** 
	   * we do fetching only if the number of 
	   * selected filters has increased
	  */
    if (this.props.selectedPairs.length > prevProps.selectedPairs.length)
      this.props.fetchTableData (this, false);
  }

  render () {
    const {selectedPairs, tableData} = this.props;
    let columns = Object.keys (tableData);
    columns.sort ();

    if (columns.length < 5) {
      columns = [
        ...new Array (5 - columns.length).fill (), // making blank columns
        ...columns,
      ];
    }

    return (
      <div>
        <h1>Rates</h1>
        <div className="container">
          <div className="table">
            <div className="table-header">
              {[
                'Currency pair / Time',
                ...columns.map ((timeStamp, index) => {
                  if (!timeStamp) return false;
                  var date = new Date (Number (timeStamp));
                  var hours = '0' + date.getHours ();
                  var minutes = '0' + date.getMinutes ();
                  var seconds = '0' + date.getSeconds ();
                  return (
                    hours.substr (-2) +
                    ':' +
                    minutes.substr (-2) +
                    ':' +
                    seconds.substr (-2) +
                    ' ⏱'
                  );
                }),
                'Tendency',
              ].map (e => (
                <div className={'header__item'} key={Math.random ()}>
                  {e || '-'}
                </div>
              ))}
            </div>
            <div className="table-content">
              {selectedPairs.map (e => (
                <div className="table-row" key={e.pairCode}>
                  {(() => {
                    const values = columns.map (column => {
                      const data = tableData[column];
                      return data && data[e.id];
                    });
                    return [
                      e.pairCode,
                      ...values,
                      getTendency ({values, e}),
                    ].map (e => (
                      <div className={'table-data'} key={Math.random ()}>
                        {e || '-'}
                      </div>
                    ));
                  }) ()}
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    );
  }
}

const mapStateToProps = state => {
  return {
    selectedPairs: state.pairsReducer.selectedPairs,
    tableData: state.pairsReducer.tableData,
    selectionKey: state.pairsReducer.selectionKey,
  };
};

const mapDispatchToProps = dispatch => {
  return {
    fetchTableData: (component, loopOnSuccess = true) => {
      const {selectedPairs, fetchTableData, selectionKey} = component.props;
      const ids = selectedPairs.map (e => e.id);

      apiCall (
        '/rates',
        (err, res) => {
          if (selectionKey !== component.props.selectionKey) {
            // oops, this response was meant for the prev arrangement of selected keys
            return;
          }
          if (err || !res) {
            //immediate re-fetching due to a fail
            return fetchTableData (component);
          }
          dispatch (addTableData (res));
          if (loopOnSuccess)
            setTimeout (() => fetchTableData (component), interval);
        },
        {
          currencyPairIds: ids,
        }
      );
    },
  };
};

export default connect (mapStateToProps, mapDispatchToProps) (RatesTable);

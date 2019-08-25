import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { isEmpty, applySpec } from 'ramda';
import NotificationsSystem from 'reapop';
import theme from 'reapop-theme-wybo';

// logic
import { interval } from 'App/config';
import { getCurrencyPairs, getRates } from 'Actions';
import {
  getPairs,
  getPairsIds,
  getRatesData,
  fetchingCurrenyPairs,
  getSortedTableData
} from 'Selectors';

// styles
import styles from 'Containers/App/App.module.css';

// Containers
import Filters from 'Containers/Filters';

// components
import Navbar from 'Components/Navbar';
import RatesTable from 'Components/RatesTable';
import Loader from 'Components/Loader';

class App extends React.Component {
  constructor(props) {
    super(props);
  }

  componentDidMount() {
    const { getCurrencyPairs } = this.props;
    getCurrencyPairs().then(() => this.startPolling());
  }

  componentWillUnmount() {
    clearInterval(this.refreshId);
  }

  startPolling() {
    const { getRates, pairsIds } = this.props;
    getRates(pairsIds);
    this.refreshId = setInterval(() => {
      getRates(pairsIds);
    }, interval);
  }

  render() {
    const { pairs, isFetchingPairs, data } = this.props;

    if (isFetchingPairs) {
      return <Loader />;
    }

    return (
      <div className={styles.app}>
        <NotificationsSystem theme={theme} />
        <div className={styles.container}>
          <Navbar />
          <Filters />
          <RatesTable data={data} />
        </div>
      </div>
    );
  }
}

const mapStateToProps = applySpec({
  pairs: getPairs,
  pairsIds: getPairsIds,
  isFetchingPairs: fetchingCurrenyPairs,
  data: getSortedTableData
});

const mapDispatchToProps = dispatch =>
  bindActionCreators(
    {
      getCurrencyPairs,
      getRates
    },
    dispatch
  );

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(App);

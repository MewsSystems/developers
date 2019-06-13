import React, { Component } from 'react';
import { connect } from 'react-redux';
import actions from 'store/actions';
import { CurrencyFilter, CurrencyList, Button, Timer } from 'components';

class App extends Component {
  searchTimeout = null;

  componentDidMount() {
    const filter = localStorage.getItem('filter');
    if (filter) this.props.updateFilter(JSON.parse(filter));
    this.props.getConfiguration().then(() => {
      this.getRates();
    });
  };

  getRates = async () => {
    const _this = this;
    this.props.getRates().then(() => {
      clearTimeout(this.searchTimeout);
      this.searchTimeout = setTimeout(() => {
        _this.getRates();
      }, 5000);
    }).catch(() => {
      clearTimeout(this.searchTimeout);
      this.searchTimeout = setTimeout(() => {
        _this.getRates();
      }, 5000);
    });
  };

  render() {
    const { configuration, rates, currencies, filter, updateFilter } = this.props;
    return (
      <div className="App">
        <div className="content">
          <div className="text-right mb-10">
            <CurrencyFilter currencies={currencies} filter={filter} updateFilter={updateFilter} />
            <Button icon="fas fa-sync" loading={configuration.loading} onClick={this.getRates}>Refresh</Button>
          </div>
          <div>
            <CurrencyList configuration={configuration} rates={rates} filter={filter} />
          </div>
          {rates.error &&
            <div className="error-message">
              <div><i className="fas fa-exclamation-triangle" /> Couldn't get the latest rates. ({rates.error || 'Unknown Error'})</div>
              <div>Refreshing in <Timer sec={5} />... (<button className="btn-link text-small" onClick={this.getRates}>Refresh Now</button>)</div>
            </div>
          }
        </div>
      </div>
    );
  };
};

const mapStateToProps = ({ configuration, rates, currencies, filter }) => ({ configuration, rates, currencies, filter });
const mapDispatchToProps = (dispatch) => ({
  getConfiguration: () => dispatch(actions.getConfiguration()),
  getRates: () => dispatch(actions.getRates()),
  updateFilter: (data) => dispatch(actions.updateFilter(data))
});
export default connect(mapStateToProps, mapDispatchToProps)(App);

import React from 'react';
import {connect} from 'react-redux';
import {getPairs, setIntervalFetchValues, filterPairs, getValues} from './actions'
import PairRow from './PairRow';

class Dashboard extends React.Component {

  constructor(props) {
    super(props);
    this.refreshInterval = null;
    this.searchInputTimeout = null;
  }

  componentDidMount() {
    this.props.getPairs();
    this.refreshInterval = this.props.setIntervalFetchValues();
  }

  componentWillUnmount () {
    clearInterval(this.refreshInterval);
  }

  searchPairs(e, type) {
    let query = e.target.value || '';

    this.props.filterPairs(query, type);

    clearTimeout(this.searchInputTimeout);
    clearInterval(this.refreshInterval);
    this.searchInputTimeout = setTimeout(()=>{
      this.props.getValues();
      this.refreshInterval = this.props.setIntervalFetchValues();
    }, 200);
  }

  render() {
    let footerContent = <td colSpan="4"></td>;
    if(this.props.error_values){
      footerContent = <td colSpan="4" className="error-message">Cant update values{this.props.error_message_values !== '' && (
        <span> :{this.props.error_message_values} ... retrying in 10 secs.</span>
      )}</td>;
    }

    let html = <tr></tr>;
    if(this.props.loading_pairs){
      html = <tr><td colSpan="4">Loading Pairs</td></tr>;
    }else if(Object.keys(this.props.pairs).length > 0){
      html = Object.keys(this.props.pairs).map((key) => {
        let [first, second] = this.props.pairs[key];
        return (
          <PairRow  key={key}
                    pairKey={key}
                    name1={first.name}
                    name2={second.name}
                    code1={first.code}
                    code2={second.code}
          />
        )
      });
    }else if(this.props.error_pairs){
      html = <tr><td colSpan="4" className="error-message">Error loading rates{this.props.error_message_pairs !== '' && (
          <span>: {this.props.error_message_pairs}</span>
        )}</td></tr>;
    }

    return(
      <table className="table table-hover">
        <thead>
          <tr>
            <th scope="col">Pair names
              <br />
              <input type="text" value={this.props.query} onChange={(e) => this.searchPairs(e, 'names')} />
            </th>
            <th scope="col">Pair codes
              <br />
              <input type="text" value={this.props.query_codes} onChange={(e) => this.searchPairs(e, 'codes')} />
            </th>
            <th scope="col">Value</th>
            <th scope="col">Trend</th>
          </tr>
        </thead>
        <tbody>
          {html}
        </tbody>
        <tfoot>
          <tr>
            {footerContent}
          </tr>
        </tfoot>
      </table>

    )
  }
}

function mapStateToProps(state, ownProps) {
  return {
    pairs: state.app.pairs,
    loading_pairs: state.app.loading_pairs,
    error_pairs: state.app.error_pairs,
    query: state.app.query,
    query_codes: state.app.query_codes,
    error_message_pairs: state.app.error_message_pairs,
    error_values: state.app.error_values,
    error_message_values: state.app.error_message_values,
  }
}

const mapDispatchToProps = {
  getPairs, setIntervalFetchValues, filterPairs, getValues
}

export default connect(mapStateToProps, mapDispatchToProps)(Dashboard);

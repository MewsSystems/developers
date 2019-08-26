import * as React from 'react';
import PropTypes from 'prop-types';
import Filter from '../Filter';

import styles from './list.css';

class List extends React.Component {
	constructor(props) {
    super(props);
		this.state = {
			rateToShow: '',
		};
  }

	handleChange = (key) => {
		const { rates } = this.props;
		if (!key) {
			this.setState({ rateToShow: '' });
		} else {
			const rate = (rates).filter((item) => item.id === key);
			this.setState({ rateToShow: rate });
		}
	}

	renderContent = () => {
		const { rates } = this.props;
    return (
      <div>
        {rates ? this.renderRates() : this.renderLoading()}
      </div>
    );
  }

	renderError = () => {
	const { rates } = this.props;
		if (!rates) {
			return (
  <div>Something Went Wrong with the fetch API.</div>
			);
		}
		return (
  <div>
    <h2>
		Something Went Wrong with the fetch API,
		showing you the old rates if we have any...
    </h2>
    {this.renderRates()}
  </div>
		);
	}

  renderRates = () => {
		const { rates } = this.props;
		const { rateToShow } = this.state;
		return (
  <>
    <Filter selectOptions={rates} onSelectOptions={this.handleChange} />
    <table className={styles.table}>
      <thead>
        <tr className={styles.tr}>
          <th className={styles.th}>Name</th>
          <th className={styles.th}>Old Value</th>
          <th className={styles.th}>Value</th>
          <th className={styles.th}>Type</th>
        </tr>
      </thead>
      <tbody>
        {rateToShow ? this.renderRate(rateToShow) : this.renderRate(rates)}
      </tbody>
    </table>
  </>
	   );
  }

	renderRate = (rates) => rates.map((rate) => (
  <tr key={rate.id}>
    <td className={styles.td}>{rate.name}</td>
    <td className={styles.td}>{rate.oldValue}</td>
    <td className={styles.td}>{rate.value}</td>
    <td className={styles.td}>{rate.type}</td>
  </tr>
		))

  renderLoading = () => {
		const { status } = this.props;
	  return (
  <div>
    {status !== 200 ? this.renderError() : <div>Loading Rates...</div>}
  </div>
	  );
  }

  render() {
		const { isLoadingRates, status, isLoadingConfiguration } = this.props;
    return (
      <div>
        <div>
          {(isLoadingRates === true && isLoadingConfiguration === true) || status !== 200
						? this.renderLoading() : this.renderContent()}
        </div>
      </div>
    );
  }
}

List.propTypes = {
  rates: PropTypes.array,
	isLoadingConfiguration: PropTypes.bool,
	isLoadingRates: PropTypes.bool,
	status: PropTypes.number,
};

List.defaultProps = {
  rates: [],
	status: 200,
	isLoadingConfiguration: true,
	isLoadingRates: true,
};

export default List;

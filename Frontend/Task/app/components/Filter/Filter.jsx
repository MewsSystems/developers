import * as React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';

import styles from './filter.css';

class Filter extends React.Component {
  handleChange = (event) => {
		const { onSelectOptions } = this.props;
		const { selectedIndex } = event.target.options;
		const key = event.target.options[selectedIndex].getAttribute('data-key');
    onSelectOptions(key);
  }

  render() {
		const { selectOptions, rateId } = this.props;

    return (
      <select
        onChange={this.handleChange}
        data-test="filterComponent"
        className={styles.select}
        multiple={false}
        defaultValue={typeof rateId !== 'string' ? '0' : rateId}
      >
        <option value="0">Please Select A Pair To Filter</option>
        {
            (selectOptions).map((item) => (
              <option
                key={item.id}
                data-key={item.id}
                value={item.id}
              >
                {item.name}
              </option>
            ))
				}
      </select>
    );
  }
}
const mapStateToProps = (state) => ({
	rateId: state.CurrencyReducer.rateId,
});

Filter.propTypes = {
  selectOptions: PropTypes.array,
	onSelectOptions: PropTypes.func,
};

Filter.defaultProps = {
  selectOptions: [],
};

export default connect(mapStateToProps)(Filter);

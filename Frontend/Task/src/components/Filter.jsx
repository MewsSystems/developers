import * as React from 'react';
import PropTypes from 'prop-types';

class Filter extends React.Component {
  constructor(props) {
    super(props);
    this.state = { value: '' };
  }

  handleChange = (event) => {
		const { onSelectOptions } = this.props;
		const { selectedIndex } = event.target.options;
		const key = event.target.options[selectedIndex].getAttribute('data-key');
    onSelectOptions(key);
		this.setState({ value: event.target.value });
  }

  render() {
		const { selectOptions } = this.props;
		const { value } = this.state;
    return (
      <select
        value={value}
        onChange={this.handleChange}
      >
        <option value="0">Please Select A Pair To Filter</option>
        {
            (selectOptions).map((item) => (
              <option
                key={item.id}
                data-key={item.id}
                value={item.value}
              >
                {item.name}
              </option>
            ))
				}
      </select>
    );
  }
}
Filter.propTypes = {
  selectOptions: PropTypes.array,
	onSelectOptions: PropTypes.func,
};

Filter.defaultProps = {
  selectOptions: [],
};

export default Filter;

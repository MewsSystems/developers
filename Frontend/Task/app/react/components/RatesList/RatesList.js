import React, { PureComponent } from 'react';
import { connect } from 'react-redux';

class RatesList extends PureComponent {
  render() {
    const { rates } = this.props;
    return (
      <div className='rates--list'>
        <ul>
          {
            rates.map(rate => {
              return (
                <li key={rate.id}>{rate.label} {rate.rate} {rate.dynamic}</li>
              );
            })
          }
        </ul>
      </div>
    );
  }
}

const mapStateToProps = state => ({
  rates: state.currencies.rates
});

export default connect(mapStateToProps, null)(RatesList);

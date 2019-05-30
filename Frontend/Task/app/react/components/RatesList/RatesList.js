import React, { PureComponent } from 'react';
import { connect } from 'react-redux';

class RatesList extends PureComponent {
  render() {
    const { ratesList } = this.props;
    return (
      <div className='rates__list'>
        <ul>
          {
            ratesList.map(rate => {
              return (
                <li>rate</li>
              );
            })
          }
        </ul>
      </div>
    );
  }
}

const mapStateToProps = state => ({
  ratesList: state.currencies.ratesList
});

export default connect(mapStateToProps, null)(RatesList);

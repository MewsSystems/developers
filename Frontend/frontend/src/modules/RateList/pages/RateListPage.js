import React, { Component, } from 'react';
import { func, } from 'prop-types';
import { connect, } from 'react-redux';
import { bindActionCreators, } from 'redux';

import { getRatesConfigurationAction, } from '../../Main/dataActions/ratesConfigurationActions';
import RateListView from '../components/RateList/RateListView';


class RateListPage extends Component {
  componentDidMount() {
    const { getRatesConfiguration, } = this.props;

    getRatesConfiguration();
  }


  render() {
    return (
      <RateListView />
    );
  }
}


const mapDispatchToProps = (dispatch) => ({
  getRatesConfiguration: bindActionCreators(getRatesConfigurationAction, dispatch),
});


RateListPage.propTypes = {
  getRatesConfiguration: func.isRequired,
};


export default connect(null, mapDispatchToProps)(RateListPage);

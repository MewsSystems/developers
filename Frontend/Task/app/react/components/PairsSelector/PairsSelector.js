import React, {PureComponent} from 'react';
import {connect} from 'react-redux';
import { bindActionCreators } from 'redux';
import Select from 'react-select';

import { fetchConfig } from '../../actions/currencies';

class PairsSelector extends PureComponent {
  componentDidMount() {
    const { actions } = this.props;
    actions.fetchConfig();
  }
  
  render() {
    return (
      <div>
        <Select />
      </div>
    );
  }
}

const mapStateToProps = state => ({});

const mapDispatchToProps = dispatch => ({
  actions: bindActionCreators({ fetchConfig }, dispatch)
});

export default connect(mapStateToProps, mapDispatchToProps)(PairsSelector);

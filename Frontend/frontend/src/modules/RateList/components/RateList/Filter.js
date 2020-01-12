import React, { Component, } from 'react';
import { func, shape, object, } from 'prop-types';
import { connect, } from 'react-redux';
import { bindActionCreators, } from 'redux';

import { FILTER_REFRESH_TIMEOUT, } from '../../../../globals';
import { changeFilterValueAction, } from '../../actions/rateListUIActions';
import FilterView from './FilterView';


class Filter extends Component {
  constructor(props) {
    super(props);

    this.filterChangeTimeoutId = null;

    this.state = {
      actualFilter: {
        name: '',
      },
    };
  }


  componentWillUnmount() {
    this.clearFilterChangeTimeout();
  }


  handleChangeSort = (name, order) => {
    const { actualFilter, } = this.state;
    const { changeFilterValue, } = this.props;

    this.clearFilterChangeTimeout();
    changeFilterValue({
      values: actualFilter,
      sort: {
        name,
        order,
      },
    });
  }


  handleChangeValue = (name, value) => {
    this.startFilterChangeTimeout();
    this.setState((prevState) => ({
      ...prevState,
      actualFilter: {
        ...prevState.actualFilter,
        [name]: value,
      },
    }));
  }


  startFilterChangeTimeout = () => {
    this.clearFilterChangeTimeout();
    this.filterChangeTimeoutId = setTimeout(this.doneFilterChangeTimeout, FILTER_REFRESH_TIMEOUT);
  }


  doneFilterChangeTimeout = () => {
    const { actualFilter, } = this.state;
    const { filter, changeFilterValue, } = this.props;

    this.clearFilterChangeTimeout();
    changeFilterValue({
      ...filter,
      values: actualFilter,
    });
  }


  clearFilterChangeTimeout = () => {
    if (this.filterChangeTimeoutId !== null) {
      clearInterval(this.filterChangeTimeoutId);
      this.filterChangeTimeoutId = null;
    }
  }


  render() {
    const { actualFilter, } = this.state;
    const { filter: { sort, }, } = this.props;

    return (
      <FilterView
        filter={{
          values: actualFilter,
          sort,
        }}
        onChangeSort={this.handleChangeSort}
        onChangeValue={this.handleChangeValue}
      />
    );
  }
}


const mapStateToProps = (state) => {
  const {
    rateListPage: {
      rateListUIReducer,
    },
  } = state;

  return {
    filter: rateListUIReducer.filter,
  };
};

const mapDispatchToProps = (dispatch) => ({
  changeFilterValue: bindActionCreators(changeFilterValueAction, dispatch),
});


Filter.propTypes = {
  filter: shape({
    sort: object.isRequired,
  }).isRequired,
  changeFilterValue: func.isRequired,
};


export default connect(mapStateToProps, mapDispatchToProps)(Filter);

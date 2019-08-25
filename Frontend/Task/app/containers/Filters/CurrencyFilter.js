import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { setFilterParams } from 'Actions';
import styles from 'Containers/Filters/Filters.module.css';

const CurrencyFilter = (props) => {
  const { setFilterParams } = props;

  return (
    <div className={styles.filterwrapper}>
      <h3>Sort by:</h3>
      <button type="button" className={styles.sortBtn} onClick={() => setFilterParams('codeFrom')}>
        Name
      </button>
      <button type="button" className={styles.sortBtn} onClick={() => setFilterParams('rate')}>
        Rates
      </button>
      <button type="button" className={styles.sortBtn} onClick={() => setFilterParams('trend')}>
        Trend
      </button>
    </div>
  );
};

const mapDispatchToProps = dispatch =>
  bindActionCreators(
    {
      setFilterParams
    },
    dispatch
  );

export default connect(
  null,
  mapDispatchToProps
)(CurrencyFilter);

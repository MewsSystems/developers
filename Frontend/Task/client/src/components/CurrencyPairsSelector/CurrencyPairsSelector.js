import React from 'react';
import { reduxForm } from 'redux-form';
import { Field } from 'redux-form';
import CheckboxGroup from '../CheckboxGroup';
import styles from './CurrencyPairsSelector.module.css';
import { isEmpty } from 'ramda';

const CurrencyPairsSelector = ({handleSubmit, options = [], loading = false, error = null}) => {

    return isEmpty(options) || loading || error ? null : (
        <div
            className={styles['container']}
        >
            <div
                className={styles['form-header']}
            >
                Select Currency Pairs
            </div>
            <form
                onSubmit={handleSubmit}
            >
                <Field
                    type="checkbox"
                    component={CheckboxGroup}
                    name="selectedCurrencyPairsIds"
                    options={options}
                />
            </form>
        </div>
    );
};

export default reduxForm({
    // a unique name for the form
    form: 'currencyPairsSelector',
})(CurrencyPairsSelector);

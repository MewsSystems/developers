import React from "react";
import { connect } from "react-redux";
import PropTypes from 'prop-types';

const mapStateToProps = state => {
    const {
        pairs,
        pairFilter,
        rates: { current, previous, error }
    } = state;

    const filteredRates = pairFilter.map(key => ({
        key,
        text: `${pairs[key][0].code}/${pairs[key][1].code}`,
        currentValue: current[key],
        previousValue: previous[key]
    }));

    return {
        filteredRates,
        error
    };
};

const RatesList = ({ filteredRates, error }) => {
    if (error) {
        return <img src="./robot.gif" />;
    }

    const processRate = ({ currentValue, previousValue }) => {
        let style = "bg-dark";
        let value = currentValue;

        if (!currentValue) {
            value = "Loading"
        }

        if (currentValue && previousValue) {
            if (currentValue > previousValue) {
                style = "bg-success";
            } else if (currentValue < previousValue) {
                style = "bg-danger";
            }
        }

        return [style, value];
    }

    return (
        <div>
            {filteredRates.map(rate => {
                const [style, value] = processRate(rate);
                return (
                    <div key={rate.key} className={`card m-1 p-1 text-white ${style}`}>
                        <h4>{rate.text} - {value}</h4>
                    </div>
                );
            })}
        </div>
    );
};

RatesList.propTypes = {
    filteredRates: PropTypes.arrayOf(PropTypes.object).isRequired,
    error: PropTypes.bool.isRequired
};

export default connect(mapStateToProps)(RatesList);

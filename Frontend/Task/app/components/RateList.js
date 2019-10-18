import React from "react";
import { connect } from "react-redux";

const mapStateToProps = state => {
    const {
        pairs,
        pairFilter,
        rates: { current, previous, message }
    } = state;

    const filteredRates = pairFilter.map(key => ({
        key,
        text: `${pairs[key][0].code}/${pairs[key][1].code}`,
        currentValue: current[key],
        previousValue: previous[key]
    }));

    return {
        filteredRates,
        message
    };
};

const RatesList = ({ filteredRates }) => {
    return (
        <div>
            {filteredRates.map(rate => (
                <div key={rate.key} className="card m-1 p-1 bg-light">
                    <h4>
                        {rate.text} - {rate.currentValue} - {rate.previousValue}
                    </h4>
                </div>
            ))}
        </div>
    );
};

export default connect(mapStateToProps)(RatesList);

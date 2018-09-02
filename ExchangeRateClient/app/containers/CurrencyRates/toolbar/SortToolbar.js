import React from 'react';
import PropTypes from 'prop-types';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Button, ButtonGroup } from 'reactstrap';
import { always, compose, equals, ifElse, map, omit, prop } from 'ramda';
import { alwaysEmptyString } from 'ramda-extension';
import { orderCurrencyPairs } from '../../../actions';

class SortToolbar extends React.Component {

    state = {
        pairArrangement: '',
        currentRateArrangement: '',
        trendArrangement: '',
        growthRateArrangement: '',
    };

    handleClickSort = (sortField) => (event) => {
        event.preventDefault();
        const arrangementState = `${sortField}Arrangement`;
        const otherArrangementStates = omit([arrangementState], this.state);
        const arrangement = compose(
            ifElse(equals('ASC'), always('DESC'), always('ASC')), prop(arrangementState),
        )(this.state);
        this.setState({ [arrangementState]: arrangement, ...map(alwaysEmptyString, otherArrangementStates) });
        this.props.orderCurrencyPairs(arrangement, sortField);
    };

    renderArrangementIcon = (sortField) => {
        switch (this.state[`${sortField}Arrangement`]) {
            case 'ASC': return (<i className="fas fa-arrow-up" />);
            case 'DESC': return (<i className="fas fa-arrow-down" />);
            default: return '';
        }
    };

    render() {
        return (
            <React.Fragment>
                Sort by:
                <ButtonGroup className="ml-2">
                    <Button outline color="secondary" onClick={this.handleClickSort('pair')}>
                        Currency pair
                    </Button>
                    <Button outline color="secondary" onClick={this.handleClickSort('currentRate')}>
                        Current Rate {this.renderArrangementIcon('currentRate')}
                    </Button>
                    <Button outline color="secondary" onClick={this.handleClickSort('trend')}>
                        Trend {this.renderArrangementIcon('trend')}
                    </Button>
                    <Button outline color="secondary" onClick={this.handleClickSort('growthRate')}>
                        Growth Rate {this.renderArrangementIcon('growthRate')}
                    </Button>
                </ButtonGroup>
            </React.Fragment>
        );
    }
}

SortToolbar.propTypes = {
    orderCurrencyPairs: PropTypes.func,
};

const mapDispatchToProps = (dispatch) => bindActionCreators({
    orderCurrencyPairs,
}, dispatch);

export default connect(null, mapDispatchToProps)(SortToolbar);

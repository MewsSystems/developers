import React from 'react';
import PropTypes from 'prop-types';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { applySpec, defaultTo } from 'ramda';
import { ButtonDropdown, DropdownItem, DropdownMenu, DropdownToggle } from 'reactstrap';
import { collectiveToggleTracking } from '../../../actions';
import { getCurrentPage, getNumberOfCurrencyPairs } from '../../../selectors';

class TrackToolbar extends React.Component {

    state = {
        trackDropdownOpen: false,
        untrackDropdownOpen: false,
    };

    toggleTrackDropdown = () => {
        this.setState({ trackDropdownOpen: !this.state.trackDropdownOpen });
    };

    toggleUntrackDropdown = () => {
        this.setState({ untrackDropdownOpen: !this.state.untrackDropdownOpen });
    };

    handleClickTracking = (trackStrategy) => (event) => {
        event.preventDefault();
        const {
            collectiveToggleTracking, orderedCurrencyPairs, currentPage, maxNumberOfRows,
            numberOfCurrencyPairs,
        } = this.props;
        switch (trackStrategy) {
            case 'TRACK_CURRENT_PAGE': {
                collectiveToggleTracking(orderedCurrencyPairs, defaultTo(1, currentPage), maxNumberOfRows, true);
                break;
            }
            case 'UNTRACK_CURRENT_PAGE': {
                collectiveToggleTracking(orderedCurrencyPairs, defaultTo(1, currentPage), maxNumberOfRows, false);
                break;
            }
            case 'TRACK_ALL': {
                collectiveToggleTracking(orderedCurrencyPairs, 1, numberOfCurrencyPairs, true);
                break;
            }
            case 'UNTRACK_ALL': {
                collectiveToggleTracking(orderedCurrencyPairs, 1, numberOfCurrencyPairs, false);
                break;
            }
        }
    };

    render() {
        return (
            <React.Fragment>
                <ButtonDropdown
                    className="mr-2"
                    isOpen={this.state.trackDropdownOpen}
                    toggle={this.toggleTrackDropdown}
                >
                    <DropdownToggle color="primary" caret>Track</DropdownToggle>
                    <DropdownMenu>
                        <DropdownItem onClick={this.handleClickTracking('TRACK_CURRENT_PAGE')}>Current page</DropdownItem>
                        <DropdownItem onClick={this.handleClickTracking('TRACK_ALL')}>All</DropdownItem>
                    </DropdownMenu>
                </ButtonDropdown>
                <ButtonDropdown
                    isOpen={this.state.untrackDropdownOpen}
                    toggle={this.toggleUntrackDropdown}
                >
                    <DropdownToggle color="danger" caret>Untrack</DropdownToggle>
                    <DropdownMenu>
                        <DropdownItem onClick={this.handleClickTracking('UNTRACK_CURRENT_PAGE')}>Current page</DropdownItem>
                        <DropdownItem onClick={this.handleClickTracking('UNTRACK_ALL')}>All</DropdownItem>
                    </DropdownMenu>
                </ButtonDropdown>
            </React.Fragment>
        );
    }
}

TrackToolbar.propTypes = {
    collectiveToggleTracking: PropTypes.func,
    currentPage: PropTypes.number,
    maxNumberOfRows: PropTypes.number,
    numberOfCurrencyPairs: PropTypes.number,
    orderedCurrencyPairs: PropTypes.array,
};

const mapStateToProps = applySpec({
    currentPage: getCurrentPage('currencyRates'),
    numberOfCurrencyPairs: getNumberOfCurrencyPairs,
});

const mapDispatchToProps = (dispatch) => bindActionCreators({
    collectiveToggleTracking,
}, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(TrackToolbar);

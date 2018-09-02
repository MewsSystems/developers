import React from 'react';
import PropTypes from 'prop-types';
import { Popover, PopoverHeader } from 'reactstrap';
import { copyToClipboard, truncate } from '../../../utils';

class IdCell extends React.Component {

    state = {
        showPopover: false,
    };

    _handleMouseOver = () => this.setState({ showPopover: true });
    _handleMouseOut = () => this.setState({ showPopover: false });
    _handleClick = () => copyToClipboard(this.props.id);

    render() {
        const { id, index, handleClick } = this.props;
        return (
            <th
                scope="row"
                id={`cellId-${index}`}
                onMouseOver={this._handleMouseOver}
                onMouseOut={this._handleMouseOut}
                onClick={handleClick}
                style={{ cursor: 'pointer' }}
            >
                {truncate(12)(id)}
                <Popover placement="left" isOpen={this.state.showPopover} target={`cellId-${index}`}>
                    <PopoverHeader>{id}</PopoverHeader>
                </Popover>
            </th>
        );
    }
}

IdCell.propTypes = {
    handleClick: PropTypes.func.isRequired,
    id: PropTypes.string.isRequired,
    index: PropTypes.number.isRequired,
};

export default IdCell;

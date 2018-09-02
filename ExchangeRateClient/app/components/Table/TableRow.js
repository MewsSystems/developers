import React from 'react';
import PropTypes from 'prop-types';
import { always, cond, equals } from 'ramda';
import { isNilOrEmpty } from 'ramda-adjunct';
import { Collapse } from 'reactstrap';
import { CheckboxCell, IdCell, DisplayCell } from './cells';

// eslint-disable-next-line ramda/cond-simplification
const renderTableCell = ({ cellType, cellData }, index) => cond([
    [
        equals('checkbox'),
        // eslint-disable-next-line react/jsx-handler-names
        always(<CheckboxCell key={index} defaultValue={cellData.defaultValue} onChange={cellData.onChange} />),
    ], [
        equals('display'),
        always(<DisplayCell key={index} cellData={cellData} />),
]])(cellType);

class TableRow extends React.Component {

    state = { collapse: false };

    onHandleClick = () => this.setState({ collapse: !this.state.collapse });

    render() {
        const { id, index, rowData, collapseBody } = this.props;
        return (
            <React.Fragment>
                <tr>
                    { !isNilOrEmpty(id) && <IdCell id={id} index={index} handleClick={this.onHandleClick} /> }
                    {rowData.map(renderTableCell)}
                </tr>
                { !isNilOrEmpty(collapseBody) &&
                    <tr>
                        <td style={{ padding: 0 }} colSpan={rowData.length + 1}>
                            <Collapse isOpen={this.state.collapse}>
                                <div className="mt-3 mb-3">{collapseBody}</div>
                            </Collapse>
                        </td>
                    </tr>
                }
            </React.Fragment>

        );
    }
}


TableRow.propTypes = {
    collapseBody: PropTypes.any,
    id: PropTypes.string,
    index: PropTypes.number.isRequired,
    rowData: PropTypes.arrayOf(PropTypes.shape({
        cellType: PropTypes.string,
        cellData: PropTypes.any,
    })).isRequired,
};

export default TableRow;

import React from 'react';
import {connect} from 'react-redux';

class PairRow extends React.Component{
  constructor(props){
    super(props);
    this.prevValue = null;
    this.firstRender = true;
  }

  render() {
    let prevValue = this.prevValue;
    let pairKey = this.props.pairKey;
    let value = this.props.values[pairKey] ? this.props.values[pairKey] : null;
    let to_hide = (this.props.pairs_to_show.indexOf(pairKey) < 0);

    this.prevValue = value ? value : this.prevValue;

    let trend = this.firstRender ? '' : 'stagnating';

    if(parseFloat(prevValue) > parseFloat(value)){
      trend = 'decreasing';
    }else if(parseFloat(prevValue) < parseFloat(value)){
      trend = 'increasing';
    }

    this.firstRender = false;
    return (
      <tr style={{
          display: (to_hide ? 'none' : 'table-row')
        }}>
        <td scope="row">{this.props.name1+'/'+this.props.name2}</td>
        <td>{this.props.code1+'/'+this.props.code2}</td>
        <td>{value ? value : this.prevValue}</td>
        <td>{trend}</td>
      </tr>
    )
  }
}

function mapStateToProps(state, ownProps) {
  return {
    values: state.app.values,
    loading_values: state.app.loading_values,
    pairs_to_show : state.app.pairs_to_show
  }
}

export default connect(mapStateToProps, null)(PairRow);

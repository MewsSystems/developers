import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'
import {actions} from '../actions/configActions'
import PropTypes from 'prop-types'
import React from 'react'
import styles from '../styles/multiselect.css'
import Loader from './Loader'
import { _map } from '../utils/lodash'

class Multiselect extends React.Component {
  constructor(props){
    super(props);
    this.state = {
      opened: false,
    }
  }

  onOpenClose = (disabled) => {
    if(disabled) return;
    this.setState({ opened: !this.state.opened })
  }

  onClick = (e) => {
    this.props.onPairToggle(e)
  }

  listMapper = ({selected, baseCode, secondaryCode}, id) => {
    return(
    <li key={id} onClick={() => this.onClick(id)} className={selected ? styles.selected : styles.unselected}>
      {`${baseCode} ${secondaryCode}`}
    </li>
  )}

  render() {
    const {
      pairs,
      isConfigFetching,
    } = this.props
    return (
      <div className={styles.multiselect}>
        <div 
          className={isConfigFetching ? styles.disabledButton : styles.enabledButton}
          onClick={() => this.onOpenClose(isConfigFetching)}>
          {isConfigFetching ? <Loader /> : 'Select pairs'}
        </div>
        <ul className={this.state.opened? styles.opened : styles.closed}>
          {_map(this.listMapper, pairs)}
        </ul>
      </div>
    )
  }
}
export default Multiselect;
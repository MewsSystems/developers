import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'
import {actions} from '../actions/configActions'
import PropTypes from 'prop-types'
import React from 'react'
import styles from '../styles/multiselect.css'
import Loader from './Loader'

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
          {Object.keys(pairs).map(o => (
            <li key={o} onClick={() => this.onClick(o)} className={pairs[o].selected ? styles.selected : styles.unselected}>
              {`${pairs[o].baseCode} ${pairs[o].secondaryCode}`}
            </li>
          ))}
        </ul>
      </div>
    )
  }
}
export default Multiselect;
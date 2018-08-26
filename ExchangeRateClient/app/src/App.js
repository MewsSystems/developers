import React, {Component} from 'react'
import PairSelector from './containers/PairSelector'
import RatesTableContainer from './containers/RatesTableContainer'
import styles from './styles/app.css'


export default class App extends Component {
    render () {
      return(
        <div className={styles.app} >
          <PairSelector />
          <RatesTableContainer />
        </div>
      )
    }
}
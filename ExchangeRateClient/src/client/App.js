import React, {Component} from 'react'
import CurrencyDashboard from 'client/modules/CurrencyDashboard'

export const EAppViews = [
  CurrencyDashboard: CurrencyDashboard,
]

export default class App extends Component {
  state = {
    view: EAppViews[0],
  }

  render () {
    const {view} = this.state
    const Component = view

    return (
      <Component {...this.props} />
    )
  }
}

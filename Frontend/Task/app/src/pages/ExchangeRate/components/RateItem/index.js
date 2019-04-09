import React, { Component } from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

import { Item, Wrapper, Pair, Rate, Trend } from './styled'

class RateItem extends Component {
  render() {
    const { pair, rate, prevRate } = this.props
    return (
      <Item>
        <Wrapper>
          <Pair>{pair}</Pair>
          <Rate>{rate}</Rate>
          {calculateTrend(rate, prevRate)}
        </Wrapper>
      </Item>
    )
  }
}

function calculateTrend(rate, prevRate) {
  let icon = ''
  let className = ''

  if (rate > prevRate) {
    icon = 'arrow-up'
    className = 'grow'
  } else if (rate < prevRate) {
    icon = 'arrow-down'
    className = 'decline'
  } else icon = 'arrows-alt-h'

  return (
    <Trend className={className}>
      <FontAwesomeIcon icon={icon} />
    </Trend>
  )
}

export { RateItem }

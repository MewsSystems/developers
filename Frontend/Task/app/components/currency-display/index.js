import React, { useEffect, useState } from 'react'
import styled, { keyframes } from 'styled-components'
import { connect } from 'react-redux'
import { oneOf, object } from 'prop-types'

import { selectIconColor, filterData } from '../../utils'
import { getCurrenciesWithRates, getSelectedFilter } from '../../selectors'
import { trend } from '../../constants'

import Icon from '../shared/icon'
import FilterTab from './filter-tab'

const SLIDE_IN = keyframes`
  0% {
    transform: scale(0.5);
    opacity: 0;
  }
  100% {
    transform: scale(1);
    opacity: 1;
  }
`

const DisplayContainer = styled.div`
  animation: ${SLIDE_IN} 0.3s ease-in forwards;
  background: white;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
  height: 70vh;
  overflow: auto;
  width: 55vw;

  ::-webkit-scrollbar {
    width: 10px;
  }

  ::-webkit-scrollbar-track {
    background: lightgrey;
  }

  ::-webkit-scrollbar-thumb {
    background: darkgrey;
  }
`

const List = styled.ul`
  list-style-type: none;
  padding: 0;
  text-align: center;
`

const Item = styled.li`
  align-items: flex-end;
  background: white;
  border-bottom: 1px solid lightgrey;
  display: flex;
  justify-content: space-between;
  padding: 20px 0;
  margin: 0 20px;
`

const RateDisplay = styled.div`
  align-items: flex-end;
  display: flex;
  justify-content: space-around;
  width: 20%;
`

const ErrorMessage = styled.div`
  align-self: center;
  margin-top: 80px;
`

const CurrencyDisplay = ({ currenciesToDisplay, filter }) => {
  const [filteredCurrencies, setCurrenciesFiltered] = useState([])

  const currencyKeys = Object.keys(filteredCurrencies)

  useEffect(() => {
    const filteredData = filterData(currenciesToDisplay, filter)

    setCurrenciesFiltered(filteredData)
  }, [currenciesToDisplay, filter])

  return (
    <DisplayContainer>
      <FilterTab />
      <List>
        {currencyKeys.map(key => {
          const name = currenciesToDisplay[key].name
          const value = currenciesToDisplay[key].value
          const trend = currenciesToDisplay[key].trend
          const iconColor = selectIconColor(trend)

          return (
            <Item key={key}>
              <div>{name}</div>
              <RateDisplay>
                <div>{value}</div>
                <Icon icon={trend} color={iconColor} size={32} />
              </RateDisplay>
            </Item>
          )
        })}
        {currencyKeys.length === 0 && (
          <ErrorMessage>No matching data found</ErrorMessage>
        )}
      </List>
    </DisplayContainer>
  )
}

CurrencyDisplay.propTypes = {
  filter: oneOf(Object.keys(trend)),
  currenciesToDisplay: object.isRequired,
}

const mapStateToProps = state => ({
  currenciesToDisplay: getCurrenciesWithRates(state),
  filter: getSelectedFilter(state),
})

export default connect(mapStateToProps)(CurrencyDisplay)

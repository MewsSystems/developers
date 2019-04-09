import React, { Component } from 'react'
import { connect } from 'react-redux'
import Select from 'react-select'
import makeAnimated from 'react-select/lib/animated'
import ClipLoader from 'react-spinners/ClipLoader'

import config from '../../../config'

import { getConfig } from '../../api/get-config'
import { getRates } from '../../api/get-rates'

import { loadConfig } from '../../store/config/actions'
import { loadRates } from '../../store/rates/actions'
import { addPairs } from '../../store/pairs/actions'

import { theme } from '../../../styles/theme'
import { RateItem } from './components/RateItem'
import { Wrapper, Aside, List, Content, Status } from './styled'

class ExchnageRateView extends Component {
  state = {
    error: null,
    isLoading: Object.values(this.props.config).length === 0,
  }

  async componentDidMount() {
    if (Object.values(this.props.config).length === 0) {
      try {
        const { currencyPairs } = await getConfig()
        this.props.loadConfig(currencyPairs)

        this.setState({ isLoading: false, error: null })
      } catch (e) {
        this.setState({ error: e.toString() })
      }
    }

    await this.fetchRates()

    this.interval = setInterval(
      async () => await this.fetchRates(),
      config.interval
    )
  }

  componentWillUnmount() {
    clearInterval(this.interval)
  }

  async fetchRates() {
    try {
      const { rates } = await getRates(
        Object.entries(this.props.config).map(item => item[0])
      )
      this.props.loadRates(rates)
      this.setState({ error: null })
    } catch (e) {
      this.setState({ error: e.toString() })
    }
  }

  handleChange = values => {
    this.props.addPairs(values)
  }

  render() {
    const { config, rates, pairs } = this.props
    const { error, isLoading } = this.state

    const options = Object.entries(config).map(item => ({
      value: item[0],
      label: `${item[1][0].code}/${item[1][1].code}`,
    }))

    return (
      <Wrapper>
        <Aside>
          <Select
            defaultValue={pairs}
            closeMenuOnSelect={false}
            components={makeAnimated()}
            isMulti
            options={options}
            onChange={this.handleChange}
          />
          <Status>
            {isLoading ? (
              <ClipLoader
                sizeUnit={'rem'}
                size={4}
                color={theme.color.darkGray}
                loading={true}
              />
            ) : (
              error
            )}
          </Status>
        </Aside>
        <Content>
          <List>
            {pairs.length !== 0 &&
              pairs.map(item => (
                <RateItem
                  key={item.value}
                  pair={item.label}
                  rate={rates.rates[item.value]}
                  prevRate={rates.prevRates[item.value]}
                />
              ))}
          </List>
        </Content>
      </Wrapper>
    )
  }
}

const mapStateToProps = ({ config, rates, pairs }) => ({
  config,
  rates,
  pairs,
})

const mapDispatchToProps = {
  loadConfig,
  loadRates,
  addPairs,
}

const ExchnageRate = connect(
  mapStateToProps,
  mapDispatchToProps
)(ExchnageRateView)
export default ExchnageRate

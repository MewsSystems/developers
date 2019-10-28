import React from 'react'
import styled from 'styled-components'
import { connect } from 'react-redux'
import { func, oneOf } from 'prop-types'

import { trend, PURPLE } from '../../constants'
import { selectFilter } from '../../actions'
import { getSelectedFilter } from '../../selectors'

import Icon from '../shared/icon'

const FilterTabContainer = styled.div`
  display: flex;
  padding: 30px;
  justify-content: space-around;
`

const StyledButton = styled.button`
  background: none;
  border: none;
  cursor: pointer;
  outline: none;
`

const FilterTab = ({ selectFilter, filter }) => (
  <FilterTabContainer>
    <StyledButton onClick={() => selectFilter(trend.GROWING)}>
      <Icon
        icon={trend.GROWING}
        color={filter === trend.GROWING ? PURPLE : 'lightgrey'}
        size={56}
      />
    </StyledButton>
    <StyledButton onClick={() => selectFilter(trend.DECLINING)}>
      <Icon
        icon={trend.DECLINING}
        color={filter === trend.DECLINING ? PURPLE : 'lightgrey'}
        size={56}
      />
    </StyledButton>
    <StyledButton onClick={() => selectFilter(trend.STAGNATING)}>
      <Icon
        icon={trend.STAGNATING}
        color={filter === trend.STAGNATING ? PURPLE : 'lightgrey'}
        size={56}
      />
    </StyledButton>
  </FilterTabContainer>
)

FilterTab.propTypes = {
  filter: oneOf(Object.keys(trend)),
  selectFilter: func.isRequired,
}

const mapStateToProps = state => ({
  filter: getSelectedFilter(state),
})

const mapDispatchToProps = dispatch => ({
  selectFilter: selectFilter(dispatch),
})

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(FilterTab)

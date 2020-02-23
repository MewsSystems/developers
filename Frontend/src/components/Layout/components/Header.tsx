import { COLORS } from 'constants/colors'
import { BOX_SHADOW } from 'constants/index'
import { Search } from 'components/Search'
import { Spacing } from 'components/Spacing'
import { StyledInput } from 'components/Input'
import { Navigation, NavigationItemProps } from 'components/Navigation'
import styled from 'styled-components'
import React, { useCallback } from 'react'
import { useTranslation } from 'react-i18next'
import { navigate } from '@reach/router'
import { useThunkDispatch } from 'hooks/useThunkDispatch'
import { setSearchValue } from 'state/actions/search'
import { useSelector } from 'react-redux'
import { State } from 'state/rootReducer'
import { Logo } from 'components/Logo'

const Top = styled(Spacing)`
  display: grid;
  grid-gap: 2rem;
  grid-template-columns: min-content auto;
`

const StyledHeader = styled.div`
  padding: 1.6rem 2rem 2rem;
  background: ${COLORS.BLACK};
  box-shadow: ${BOX_SHADOW.MEDIUM};
`

const StyledNavigation = styled(Navigation)`
  align-self: end;
  justify-self: end;

  * {
    font-size: 1.1rem;
  }
`

const StyledSearch = styled(Search)`
  ${StyledInput} {
    color: ${COLORS.WHITE};
    background: rgba(255, 255, 255, 0.3);

    ::placeholder {
      color: ${COLORS.WHITE};
    }
  }
`

const { PUBLIC_URL } = process.env

export const Header: React.FC = () => {
  const { t } = useTranslation()
  const dispatch = useThunkDispatch()
  const searchValue = useSelector((state: State) => state.search.value)

  const navigationItems: NavigationItemProps[] = [
    {
      title: t('header.navigation.home'),
      to: `${PUBLIC_URL}/`,
      key: 'home',
    },
    {
      title: t('header.navigation.favourite'),
      to: `${PUBLIC_URL}/favourite`,
      key: 'favourite',
    },

    {
      title: t('header.navigation.recomended'),
      to: `${PUBLIC_URL}/recomended`,
      key: 'recomended',
    },
  ]

  const handleSearch = useCallback(
    (value: string) => {
      if (value.length > 0) {
        dispatch(setSearchValue(value))
        navigate(`${PUBLIC_URL}/search`)
      }
    },
    [dispatch]
  )

  const handleClear = useCallback(() => {
    dispatch(setSearchValue(''))
    navigate(`${PUBLIC_URL}/`)
  }, [dispatch])

  return (
    <StyledHeader>
      <Top outer="0 0 1rem">
        <Logo>{t('header.title')}</Logo>
        <StyledNavigation items={navigationItems} />
      </Top>
      <StyledSearch
        value={searchValue}
        onPressEnter={handleSearch}
        onClear={handleClear}
      />
    </StyledHeader>
  )
}

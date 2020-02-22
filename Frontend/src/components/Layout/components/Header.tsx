import { COLORS } from 'constants/colors'
import { BOX_SHADOW } from 'constants/index'
import { Search } from 'components/Search'
import { Spacing } from 'components/Spacing'
import { StyledInput } from 'components/Input'
import { Navigation, NavigationItemProps } from 'components/Navigation'
import { Logo } from '../../Logo'
import styled from 'styled-components'
import React from 'react'
import { useTranslation } from 'react-i18next'

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

  return (
    <StyledHeader>
      <Top outer="0 0 1rem">
        <Logo>{t('header.title')}</Logo>
        <StyledNavigation items={navigationItems} />
      </Top>
      <StyledSearch />
    </StyledHeader>
  )
}

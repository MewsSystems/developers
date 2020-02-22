import { COLORS } from 'constants/colors'
import React from 'react'
import styled from 'styled-components'
import { useTranslation } from 'react-i18next'

export const StyledFooter = styled.div`
  padding: 1rem 2rem;
  color: ${COLORS.WHITE};
  background: ${COLORS.DARK_GRAY};
`

export const Footer: React.FC = () => {
  const { t } = useTranslation()
  const year = new Date().getFullYear()

  return <StyledFooter>{t('footer.copyright', { year })}</StyledFooter>
}

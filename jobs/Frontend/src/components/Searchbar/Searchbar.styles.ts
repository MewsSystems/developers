import styled from 'styled-components'
import { MagnifyingGlassIcon } from '@heroicons/react/24/outline'
import { theme } from '@/theme'

type StyledTypes = { $isMobile: boolean }

export const Wrapper = styled.div`
  position: relative;
  display: flex;
`

export const StyledInput = styled.input<StyledTypes>`
  background: none;
  width: 100%;
  font-size: ${(props) => (props.$isMobile ? '1.9rem' : '2.3rem')};
  padding: 32px 80px;
  padding: ${(props) => (props.$isMobile ? '16px 64px' : '32px 80px')};
  border-top: 1.5px solid ${theme.colors.border.primary};
  border-bottom: 1.5px solid ${theme.colors.border.primary};
  color: ${theme.colors.text.primary};
  outline: none;

  &::placeholder {
    color: ${theme.colors.text.secondary};
    transition: color 0.5s ease;
  }

  &:focus {
    color: ${theme.colors.text.primary};
    transition: color 0.5s ease;

    &::placeholder {
      color: ${theme.colors.text.primary};
    }
  }
`

export const StyledMagnifyingGlassIcon = styled(
  MagnifyingGlassIcon,
)<StyledTypes>`
  position: absolute;
  color: ${theme.colors.text.primary};
  left: 8px;
  width: ${(props) => (props.$isMobile ? '32px' : '48px')};
  height: ${(props) => (props.$isMobile ? '32px' : '48px')};
  top: 50%;
  transform: translateY(-50%);
`

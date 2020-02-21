import { COLORS } from 'constants/colors'
import { BORDER_RADIUS, BOX_SHADOW } from 'constants/index'
import { CircleIcon } from 'components/CircleIcon'
import React from 'react'
import styled from 'styled-components'

export const StyledCircleIcon = styled(CircleIcon)<
  Pick<InputProps, 'inputSize'>
>`
  position: absolute;
  top: ${({ inputSize }) => (inputSize === 'small' ? '0.45rem' : '0.75rem')};
  right: ${({ inputSize }) => (inputSize === 'small' ? '0.5rem' : '0.6rem')};
`

export const Container = styled.div<Pick<InputProps, 'fullWidth'>>`
  position: relative;
  display: inline-block;
  width: ${({ fullWidth }) => fullWidth && '100%'};
`

export const StyledInput = styled.input<
  Pick<InputProps, 'inputSize' | 'fullWidth'>
>`
  width: ${({ fullWidth }) => fullWidth && '100%'};
  min-width: 16rem;
  padding: ${({ inputSize }) =>
    inputSize === 'small' ? '0.45rem 0.5rem' : '0.8rem 1rem'};
  border: 1px solid ${COLORS.GRAY};
  border-radius: ${BORDER_RADIUS.MEDIUM};
  box-shadow: ${BOX_SHADOW.MEDIUM};

  &:focus {
    border-color: ${COLORS.LIGHT_BLUE};
    outline: none;
  }
`

export interface InputProps
  extends React.InputHTMLAttributes<HTMLInputElement> {
  inputSize?: 'small' | 'large'
  allowClear?: boolean
  onClear?: () => void
  fullWidth?: boolean
}

export const Input: React.FC<InputProps> = ({
  inputSize = 'small',
  allowClear = false,
  fullWidth = false,
  onClear,
  className,
  ...rest
}) => {
  return (
    <div className={className}>
      <Container fullWidth={fullWidth}>
        <StyledInput inputSize={inputSize} fullWidth={fullWidth} {...rest} />
        {allowClear && (
          <StyledCircleIcon
            icon="times"
            onClick={onClear}
            inputSize={inputSize}
          />
        )}
      </Container>
    </div>
  )
}

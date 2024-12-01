import { styled } from 'styled-components'

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  children: React.ReactNode
  disabled?: boolean
  onClick?: () => void
}

function Button({ children, disabled, onClick }: ButtonProps) {
  return (
    <StyledButton onClick={onClick} disabled={disabled}>
      {children}
    </StyledButton>
  )
}

const StyledButton = styled.button`
  background-color: var(--secondary-brand-color-100);
  color: white;
  border: none;
  padding: 0.5rem;
  border-radius: 0.8rem;
  cursor: pointer;

  a {
    text-decoration: none;
    color: white;
    font-weight: 600;
  }

  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }
`

export default Button

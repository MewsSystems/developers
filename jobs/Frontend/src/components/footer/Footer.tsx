import { styled } from 'styled-components'

function Footer() {
  return (
    <StyledFooter>
      <p>Made with ❤️ by Raffaele Nicosia</p>
    </StyledFooter>
  )
}

const StyledFooter = styled.footer`
  background-color: var(--primary-brand-color-400);
  padding: 1rem;
  height: 80px;
  flex-shrink: 0;
`

export default Footer

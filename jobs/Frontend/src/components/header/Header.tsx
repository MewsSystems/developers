import { styled } from 'styled-components'

function Header() {
  return <StyledHeader />
}

const StyledHeader = styled.header`
  flex-shrink: 0;
  height: 175px;
  padding: 1rem 1rem 0;
  background-image: url('/images/movie-posters-collage.jpg');
  background-size: cover;
`

export default Header

import { styled } from 'styled-components'
import { ArrowLeftCircleFill } from '../components/ui/icon/Icon'

function NotFound() {
  return (
    <LayoutContainer>
      <a href="/" className="back-link">
        <ArrowLeftCircleFill width={42} height={42} fill="#D50C2F" />
      </a>
      <h1 className="not-found-title">404: Page Not Found</h1>
    </LayoutContainer>
  )
}

const LayoutContainer = styled.main`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  margin: 1rem;
  gap: 1rem;

  .back-link {
    align-self: flex-start;
    cursor: pointer;
  }

  .not-found-title {
    align-self: center;
  }
`

export default NotFound

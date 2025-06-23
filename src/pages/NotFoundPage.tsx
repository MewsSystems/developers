import { Link } from "react-router"
import styled from "styled-components"
import { ROUTES } from "../constants/routes"

const NotFoundContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  padding: 2rem;
  text-align: center;
`

const NotFoundTitle = styled.h1`
  font-size: 6rem;
  font-weight: bold;
  color: ${({ theme }) => theme.colors.primary};
  margin: 0;
  line-height: 1;
`

const NotFoundSubtitle = styled.h2`
  font-size: 2rem;
  color: ${({ theme }) => theme.colors.text};
  margin: 1rem 0 2rem 0;
  font-weight: 600;
`

const NotFoundMessage = styled.p`
  font-size: 1.1rem;
  color: ${({ theme }) => theme.colors.textSecondary};
  margin-bottom: 2rem;
  max-width: 500px;
  line-height: 1.6;
`

const BackButton = styled(Link)`
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem 1.5rem;
  background-color: ${({ theme }) => theme.colors.primary};
  color: white;
  text-decoration: none;
  border-radius: 8px;
  font-weight: 500;
  transition: all 0.2s ease;

  &:hover {
    background-color: ${({ theme }) => theme.colors.primaryHover};
    transform: translateY(-1px);
  }
`

const MovieIcon = styled.span`
  font-size: 4rem;
  margin-bottom: 1rem;
`

export const NotFoundPage = () => {
  return (
    <NotFoundContainer>
      <MovieIcon>🎬</MovieIcon>
      <NotFoundTitle>404</NotFoundTitle>
      <NotFoundSubtitle>Page Not Found</NotFoundSubtitle>
      <NotFoundMessage>
        Oops! The page you're looking for doesn't exist. It might have been moved, deleted, or you
        entered the wrong URL.
      </NotFoundMessage>
      <BackButton to={ROUTES.HOME}>← Back to Movie Search</BackButton>
    </NotFoundContainer>
  )
}

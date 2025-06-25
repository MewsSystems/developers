import { Link } from "react-router"
import styled from "styled-components"

export const NotFoundContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  padding: 2rem;
  text-align: center;
`

export const NotFoundTitle = styled.h1`
  font-size: 6rem;
  font-weight: bold;
  color: ${({ theme }) => theme.colors.primary};
  margin: 0;
  line-height: 1;
`

export const NotFoundSubtitle = styled.h2`
  font-size: 2rem;
  color: ${({ theme }) => theme.colors.text};
  margin: 1rem 0 2rem 0;
  font-weight: 600;
`

export const NotFoundMessage = styled.p`
  font-size: 1.1rem;
  color: ${({ theme }) => theme.colors.textSecondary};
  margin-bottom: 2rem;
  max-width: 500px;
  line-height: 1.6;
`

export const BackButton = styled(Link)`
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

export const MovieIcon = styled.span`
  font-size: 4rem;
  margin-bottom: 1rem;
  color: ${({ theme }) => theme.colors.primary};
`

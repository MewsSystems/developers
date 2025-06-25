import styled from "styled-components"

export const ErrorContainer = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  padding: ${({ theme }) => theme.spacing.lg};
  background-color: ${({ theme }) => theme.colors.surface};
  border: 1px solid ${({ theme }) => theme.colors.error};
  border-radius: ${({ theme }) => theme.borderRadius.lg};
  margin: ${({ theme }) => theme.spacing.lg} 0;
  max-width: 600px;
  margin-left: auto;
  margin-right: auto;
`

export const ErrorIcon = styled.span`
  display: flex;
  font-size: ${({ theme }) => theme.fontSizes.xl};
  margin-right: ${({ theme }) => theme.spacing.md};
  color: ${({ theme }) => theme.colors.error};
`

export const ErrorText = styled.p`
  color: ${({ theme }) => theme.colors.error};
  margin: 0;
  font-size: ${({ theme }) => theme.fontSizes.base};
`

import styled from 'styled-components';

const StyledHeaderWrapper = styled.div`
  display: flex;
  width: 100%;
  align-items: center;
  justify-content: space-between;
`;
const StyledHeaderText = styled.p`
  font-size: 4.5rem;
  display: flex;
  margin: 0;
  @media (max-width: 991px) {
    font-size: 3em;
  }
  @media (max-width: 797px) {
    font-size: 2rem;
  }
`;
const StyledToggle = styled.button`
  display: flex;
  background-color: transparent;
  color: ${({ theme }) => theme.colors.secondary};
  font-size: 30px;
  border: none;
  cursor: pointer;
  transition: transform 200ms ease-in-out;
  &:hover {
    color: ${({ theme }) => theme.colors.hoverSecondary};
    transform: scale(1.1);
  }
`;
const StyledGoBackButtonContent = styled.div`
  display: flex;
  gap: 10px;
  align-items: center;
  justify-content: flex-start;
`;
const StyledGoBackButtonWrapper = styled.div`
  display: flex;
  justify-content: flex-start;
  align-items: center;
`;
const StyledGoBackButtonText = styled.p`
  @media (max-width: 991px) {
    display: none;
  }
`;

export {
  StyledHeaderWrapper,
  StyledHeaderText,
  StyledToggle,
  StyledGoBackButtonContent,
  StyledGoBackButtonText,
  StyledGoBackButtonWrapper,
};

import styled from 'styled-components';

const StyledSearchMovieInput = styled.input`
  width: 300px;
  padding: 10px 16px;
  font-size: 16px;
  border: 1px solid #ccc;
  border-radius: 8px;
  outline: none;
  margin: 20px auto;
  display: flex;
  justify-content: center;
  background-color: ${({ theme }) => theme.colors.surface};
  border-color: ${({ theme }) => theme.colors.primary};
  color: ${({ theme }) => theme.colors.primary};
  transition: width 500ms ease-in-out;
  &:focus {
    border-color: ${({ theme }) => theme.colors.hoverPrimary};
    box-shadow: 0 0 0 2px rgba(0, 112, 243, 0.2);
    width: 350px;
  }
`;

const StyledSearchMovieInputCleanButton = styled.button`
  background-color: transparent;
  color: ${({ theme }) => theme.colors.primary};
  border: none;
  font-size: 30px;
  display: flex;
  align-items: center;
  margin-left: -40px;
  cursor: pointer;
`;

const StyledSearchMovieInputContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
`;

export {
  StyledSearchMovieInput,
  StyledSearchMovieInputCleanButton,
  StyledSearchMovieInputContainer,
};

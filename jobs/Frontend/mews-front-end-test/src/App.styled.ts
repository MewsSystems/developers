import styled from 'styled-components';

const StyledAppContainer = styled.div`
  min-height: 100%;
  width: 50%;
  margin: 0 auto;
`;

const StyledHeader = styled.header`
  margin: 0 auto;
  width: fit-content;
`;

const StyledMain = styled.main`
  margin: 0 auto;
`;

const StyledTitle = styled.h1`
  font-size: 5rem;
  margin: 0;
`;

const StyledSearchBox = styled.input`
  width: 100%;
  font-size: 2rem;
  padding: 1rem 2rem;
  margin: 0 auto 2rem auto;
  border-radius: 1rem;
`;

export {
  StyledAppContainer,
  StyledHeader,
  StyledMain,
  StyledTitle,
  StyledSearchBox,
};

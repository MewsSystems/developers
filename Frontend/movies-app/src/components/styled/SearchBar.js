import styled from 'styled-components';

const SearchBar = styled.div`
  font-size: 24px;
  position: relative;

  input {
    height: 48px;
    width: calc(100% - 64px);
    border: none;
    padding: 0 0 0 64px;
    font-size: 14px;
    outline: none;
    color: ${props => props.theme.text.primary};
    background-color: ${props => props.theme.background.withContent};
  }

  svg {
    width: 16px;
    height: 16px;
    position: absolute;
    top: 50%;
    left: 24px;
    transform: translateY(-50%);
    fill: ${props => props.theme.text.primary};
  }
`;

export default SearchBar;

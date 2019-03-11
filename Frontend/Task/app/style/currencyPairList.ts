import styled from 'styled-components';

export default styled.ul`
  list-style: none;
  li {
    clear: both;
  }
  strong, span {
    display: block;
    float: left;
    padding: 5px;
  }
  strong {
    width: 200px;
  }
  span {
    text-align: center;
    min-width: 100px;
  }
`;

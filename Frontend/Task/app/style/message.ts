import styled from 'styled-components';

export default styled.div`
  font-style: italic;
  color: ${(props: any) => (props.error ? 'red' : 'black')};
  padding: 10px 0;
`;

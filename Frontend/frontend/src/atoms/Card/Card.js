import styled from 'styled-components';


const Card = styled.div`
  background: ${(p) => p.theme.white};
  padding: ${(p) => p.theme.common.paddingSM};
  margin-bottom: ${(p) => p.theme.common.paddingMD};
  box-shadow: 6px 3px 12px 0 rgba(0, 0, 0, 0.05);
`;


export default Card;

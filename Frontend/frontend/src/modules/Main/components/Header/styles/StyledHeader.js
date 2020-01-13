import styled from 'styled-components';


const StyledHeader = styled.div`
  display: flex;
  align-items: center;
  height: 3rem;
  margin-bottom: 3rem;

  background: ${(p) => p.theme.grey.t800};
  color: ${(p) => p.theme.white};
  box-shadow: 0 5px 18px 0 rgba(0, 0, 0, 0.1);

  .header--container {
    padding-left: ${(p) => p.theme.common.paddingMD};
    padding-right: ${(p) => p.theme.common.paddingMD};
  }

  .header--title {
    font-size: 1.25rem;
  }
`;


export default StyledHeader;

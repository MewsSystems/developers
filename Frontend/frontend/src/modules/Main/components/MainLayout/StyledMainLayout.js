import styled from 'styled-components';


const StyledMainLayout = styled.div`
  min-height: 100vh;
  background: ${(p) => p.theme.grey.t200};

  .mainLayout--body {
    padding-left: ${(p) => p.theme.common.paddingMD};
    padding-right: ${(p) => p.theme.common.paddingMD};
  }
`;


export default StyledMainLayout;

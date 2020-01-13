import styled from 'styled-components';


const StyledTrendIcon = styled.span`
  .trendIcon--asc {
    transform: rotate(-40deg);
    color: ${(p) => p.theme.success.t700};
  }

  .trendIcon--des {
    transform: rotate(40deg);
    color: ${(p) => p.theme.error.t700};
  }

  .trendIcon--eql {
    color: ${(p) => p.theme.grey.t700};
  }
`;

export default StyledTrendIcon;

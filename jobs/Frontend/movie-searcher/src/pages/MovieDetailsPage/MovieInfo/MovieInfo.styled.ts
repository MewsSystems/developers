import styled from "styled-components";
import { Col } from "antd";

const ColStyled = styled(Col)`
  margin-left: 24px;
`;

const Title = styled.h2`
  font-size: ${(p) => p.theme.fontSize.h2};
  font-weight: ${(p) => p.theme.fontWeight.bold};
  margin-bottom: 5px;
`;

const SmallTitle = styled.div`
  display: block-inline;
  font-size: ${(p) => p.theme.fontSize[14]};
  font-weight: ${(p) => p.theme.fontWeight.bold};
`;

const OriginalTitle = styled.div`
  color: ${(p) => p.theme.basicStyle.colors.grey[50]};
  font-size: ${(p) => p.theme.fontSize[14]};
  font-style: italic;
`;

const Overview = styled.p`
  line-height: 22px;
`;

const RatingWrapper = styled.div`
  display: flex;
  font-size: 20px;
  font-weight: ${(p) => p.theme.fontWeight.bold};

  & svg {
    fill: ${(p) => p.theme.basicStyle.colors.yellow[90]};
  }
`;

const HeaderWrapper = styled.div`
  display: flex;
  justify-content: space-between;
`;

const Rating = styled.span`
  margin-left: 5px;

  & span {
    color: ${(p) => p.theme.basicStyle.colors.grey[50]};
    font-size: 12px;
  }
`;

const FlexWrapper = styled.div`
  display: flex;
  margin-bottom: 10px;

  & span {
    margin-left: 5px;
  }
`;

const VoteCount = styled.div`
  color: ${(p) => p.theme.basicStyle.colors.grey[50]};
  font-size: 12px;
  text-align: right;
  margin-top: 5px;
`;

export {
  ColStyled,
  Title,
  SmallTitle,
  OriginalTitle,
  Overview,
  RatingWrapper,
  Rating,
  HeaderWrapper,
  FlexWrapper,
  VoteCount,
};

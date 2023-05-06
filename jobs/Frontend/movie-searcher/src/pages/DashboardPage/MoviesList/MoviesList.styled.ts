import { Card } from "antd";
import Meta from "antd/es/card/Meta";
import styled from "styled-components";

const CardStyled = styled(Card)`
  height: 300px;
  overflow: hidden;
  padding: 0;

  & .ant-card-body {
    padding: 0;
  }
`;

const Wrapper = styled.div`
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  grid-template-rows: auto;
  column-gap: 20px;
  row-gap: 20px;
`;
const ImageStyled = styled.img`
  height: 255px;
  width: 100%;
  object-fit: cover;
`;
const MetaStyled = styled(Meta)`
  padding: 5px;
`;

export { CardStyled, Wrapper, ImageStyled, MetaStyled };

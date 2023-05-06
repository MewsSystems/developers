import Meta from "antd/es/card/Meta";
import { Link } from "react-router-dom";
import styled from "styled-components";

const LinkStyled = styled(Link)`
  height: 400px;
  padding: 0;
  overflow: hidden;
  border-radius: 6px;

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
  height: 360px;
  width: 100%;
  object-fit: cover;
`;
const MetaStyled = styled(Meta)`
  padding: 5px;
`;

export { LinkStyled, Wrapper, ImageStyled, MetaStyled };

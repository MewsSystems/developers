import styled from "styled-components";

export const ListCollection = styled.div`
  margin: .5rem 0 1rem 0;
  border: 1px solid #e0e0e0;
  border-radius: 2px;
  overflow: hidden;
  position: relative;
`;

export const ListCollectionItem = styled.span`
  background-color: #fff;
  line-height: 1.5rem;
  padding: 10px 20px;
  margin: 0;
  border-bottom: 1px solid #e0e0e0;
  display: block;
  transition: .25s;
  color: #26a69a;
  &:hover  {
    background-color: #26a69a;
    color: #eafaf9;
    & > * {
      color: #eafaf9;
    }
  }
`;


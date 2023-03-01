import styled from "styled-components";

export const PaginationLayout = styled.div`
  display: flex;
  justify-content: center;
  padding: 2rem;
  box-sizing: border-box;
`;

export const PaginationList = styled.ul`
  display: flex;
  flex-direction: row;
  gap: 0.5rem;
`;

interface PaginationItemProps {
  disabled?: boolean;
  active?: boolean;
}

export const PaginationItem = styled.li<PaginationItemProps>`
  font-size: 1.8rem;
  padding: 0.75rem;
  border: 1px solid #888;
  color: ${p => p.active == true ? '#eee' : '#333'};
  background-color: ${p => p.active == true ? '#333' : '#fff'};
  display: inline-block;
  opacity: ${p => p.disabled == true ? 0.3 : 1};

  &:hover {
    cursor: ${p => p.disabled == true ? 'default' : 'pointer'};
    background-color: ${p => p.active == true ? '#333' : '#eee'};
  }
`;

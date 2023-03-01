import styled from "styled-components";

interface PropertyListProps {
  textColor?: string;
  sideBySide?: boolean;
  padding?: boolean;
}

export const PropertyList = styled.dl<PropertyListProps>`
  display: grid;
  grid-template-columns: ${p => p.sideBySide == true ? '20% 80%' : '100%'};
  color: ${p => p.textColor ? p.textColor : '#222'};
  padding: ${p => p.padding == true ? '1rem' : 'none'};

  & > dt {
    margin-top: ${p => p.sideBySide == true ? 'none' : '2rem'};
  }

  @media (max-width: 900px) {
    grid-template-columns: 100%;
  }
`;

export const PropertyTitle = styled.dt`
  font-weight: bold;
  line-height: 1.8rem;
`;

export const PropertyValue = styled.dd`
  opacity: 0.8;
  line-height: 1.8rem;
`;

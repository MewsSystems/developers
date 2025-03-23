import styled from 'styled-components';

type PageSectionProps = {
  children?: React.ReactNode;
  direction?: 'row' | 'column';
  backgroundColor?: 'none' | '#4d5b9e';
};

const StyledPageSection = styled.div<PageSectionProps>`
  display: flex;
  flex-direction: ${(props) => props.direction || 'column'};
  flex-wrap: wrap;
  max-width: 1250px;
  padding: 2rem;
  justify-content: center;
  align-items: center;
  background-color: ${(props) => props.backgroundColor || 'none'};
`;

export const PageSection = ({
  children,
  direction,
  backgroundColor,
}: PageSectionProps) => {
  return (
    <StyledPageSection direction={direction} backgroundColor={backgroundColor}>
      {children}
    </StyledPageSection>
  );
};

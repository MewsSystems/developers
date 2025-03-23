import styled from 'styled-components';

type PageSectionProps = {
  children?: React.ReactNode;
  direction?: 'row' | 'column';
  backgroundColor?: 'none' | '#e0e0e0';
};

const StyledPageSection = styled.div<PageSectionProps>`
  display: flex;
  flex-direction: ${(props) => props.direction || 'column'};
  flex-wrap: wrap;
  gap: 2rem;
  max-width: 1250px;
  padding: 2rem;
  justify-content: center;
  align-items: center;
  border-radius: 10px;
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

import styled from 'styled-components';

type PageSectionProps = {
  children?: React.ReactNode;
  direction?: 'row' | 'column';
  $backgroundcolor?: 'none' | '#e0e0e0';
};

const StyledPageSection = styled.div<PageSectionProps>`
  display: flex;
  flex-grow: 1;
  flex-direction: ${(props) => props.direction || 'column'};
  flex-wrap: wrap;
  gap: 2rem;
  max-width: 1250px;
  padding: 1rem;
  justify-content: center;
  align-items: center;
  border-radius: 10px;
  background-color: ${(props) => props.$backgroundcolor || 'none'};
`;

export const PageSection = ({
  children,
  direction,
  $backgroundcolor,
}: PageSectionProps) => {
  return (
    <StyledPageSection
      direction={direction}
      $backgroundcolor={$backgroundcolor}
    >
      {children}
    </StyledPageSection>
  );
};

import styled from 'styled-components';

type PageSectionProps = {
  children?: React.ReactNode;
  direction?: 'row' | 'column';
};

const StyledPageSection = styled.div<PageSectionProps>`
  display: flex;
  flex-direction: ${(props) => props.direction || 'column'};
  flex-wrap: wrap;
  max-width: 1250px;
  padding: 2rem;
`;

export const PageSection = ({ children, direction }: PageSectionProps) => {
  return (
    <StyledPageSection direction={direction}>{children}</StyledPageSection>
  );
};

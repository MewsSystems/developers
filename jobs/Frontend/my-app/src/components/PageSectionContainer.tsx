import styled from 'styled-components';

type PageSectionContainerProps = {
  children?: React.ReactNode;
};

const StyledPageSectionContainer = styled.div`
  max-width: 1250px;
  padding: 2rem;
`;

export const PageSectionContainer = ({
  children,
}: PageSectionContainerProps) => {
  return <StyledPageSectionContainer>{children}</StyledPageSectionContainer>;
};

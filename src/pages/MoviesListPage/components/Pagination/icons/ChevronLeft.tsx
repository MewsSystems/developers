import styled from 'styled-components';

const StyledSvg = styled.svg`
  width: 16px;
  height: 16px;
`;

export const ChevronLeft = () => {
  return (
    <StyledSvg
      xmlns="http://www.w3.org/2000/svg"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
    >
      <path d="M11 17l-5-5 5-5M18 17l-5-5 5-5" />
    </StyledSvg>
  );
};

ChevronLeft.displayName = 'ChevronLeft';

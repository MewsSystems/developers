import styled from 'styled-components';

const StyledSvg = styled.svg`
  width: 16px;
  height: 16px;
`;

export default function ChevronRight() {
  return (
    <StyledSvg
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
    >
      <path d="M13 17l5-5-5-5M6 17l5-5-5-5" />
    </StyledSvg>
  );
}

ChevronRight.displayName = 'ChevronRight';

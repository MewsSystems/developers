import styled from "styled-components";

export const DetailsContainer = styled.div`
  display: flex;
  flex-direction: column;
  min-height: 100vh;
`;

interface HeroSectionProps {
  backdrop: string;
}

export const HeroSection = styled.div<HeroSectionProps>`
  position: relative;
  width: 100%;
  height: 60vh;
  background: ${({ backdrop }) =>
    `url(${backdrop}) center center / cover no-repeat`};

  @media (min-width: 768px) {
    height: 70vh;
  }

  &:after {
    content: "";
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: linear-gradient(
      to bottom,
      rgba(0, 0, 0, 0) 0%,
      rgba(0, 0, 0, 0.8) 100%
    );
  }
`;

export const ContentSection = styled.div`
  flex: 1;
  max-width: 800px;
  margin: 0 auto;
  padding: ${({ theme }) => theme.spacing(4)} ${({ theme }) => theme.spacing(2)};
  display: flex;
  flex-direction: column;
  gap: ${({ theme }) => theme.spacing(3)};

  @media (min-width: 768px) {
    padding: ${({ theme }) => theme.spacing(6)}
      ${({ theme }) => theme.spacing(3)};
  }
`;

export const Title = styled.h1`
  margin: 0;
  font-size: 2rem;
  font-weight: 700;
  color: ${({ theme }) => theme.colors.textPrimary};

  @media (min-width: 768px) {
    font-size: 2.5rem;
  }
`;

export const MetaInfo = styled.div`
  display: flex;
  gap: ${({ theme }) => theme.spacing(2)};
  font-size: 1rem;
  color: ${({ theme }) => theme.colors.textSecondary};

  span {
    display: inline-block;
  }
`;

export const Description = styled.p`
  font-size: 1.1rem;
  line-height: 1.6;
  color: ${({ theme }) => theme.colors.textPrimary};
  margin: 0;
`;

export const BackButton = styled.button`
  align-self: flex-start;
  padding: ${({ theme }) => theme.spacing(1)} ${({ theme }) => theme.spacing(2)};
  background: rgba(255, 255, 255, 0.1);
  border: none;
  border-radius: 4px;
  color: ${({ theme }) => theme.colors.textPrimary};
  font-size: 0.9rem;
  cursor: pointer;

  &:hover {
    background: rgba(255, 255, 255, 0.2);
  }
`;

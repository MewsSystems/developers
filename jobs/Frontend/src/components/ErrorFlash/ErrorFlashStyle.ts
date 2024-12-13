import styled from "styled-components";

export const ErrorContainer = styled.div`
  position: fixed;
  bottom: 16px;
  left: 50%;
  transform: translateX(-50%);
  background: rgba(255, 0, 0, 0.8);
  color: #fff;
  padding: ${({ theme }) => theme.spacing(2)} ${({ theme }) => theme.spacing(3)};
  border-radius: 4px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
  display: flex;
  align-items: center;
  gap: ${({ theme }) => theme.spacing(2)};
  z-index: 9999;
`;

export const ErrorMessage = styled.div`
  font-size: 1rem;
  font-weight: 500;
`;

export const CloseButton = styled.button`
  background: none;
  border: none;
  color: #fff;
  font-size: 1.2rem;
  cursor: pointer;
  line-height: 1;
  padding: 8px;

  &:hover {
    opacity: 0.8;
  }
`;

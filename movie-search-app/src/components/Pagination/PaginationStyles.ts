import styled from "styled-components";

export const PaginationContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 0.75rem;
  margin: 2rem 0;
`;

export const PageButton = styled.button<{ disabled?: boolean }>`
  padding: 0.5rem 1rem;
  background: ${({ disabled }) =>
    disabled ? "#bdc3c7" : "linear-gradient(135deg, #8e44ad, #3498db)"};
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  cursor: ${({ disabled }) => (disabled ? "not-allowed" : "pointer")};
  transition: transform 0.15s ease, box-shadow 0.15s ease;

  &:hover {
    transform: ${({ disabled }) => (disabled ? "none" : "translateY(-2px)")};
    box-shadow: ${({ disabled }) =>
      disabled ? "none" : "0 6px 12px rgba(0, 0, 0, 0.15)"};
  }

  &:focus {
    outline: 2px solid #8e44ad;
    outline-offset: 2px;
  }
`;

export const PageIndicator = styled.span`
  font-size: 1rem;
  color: #2c3e50;
  font-weight: 500;
`;

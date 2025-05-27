import styled from 'styled-components';

export const CardContainer = styled.div`
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  height: 380px;
  width: 170px;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  cursor: pointer;
  transition: all 0.3s ease;
  position: relative;
  border: 1px solid #e3e3e3;

  &:hover {
    transform: translateY(-5px);
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
  }
`;

export const CardImageContainer = styled.div`
  width: 100%;
  height: 250px;
  position: relative;
`;

export const CardBody = styled.div`
  padding: 4px 6px;
  white-space: normal;
  display: flex;
  text-overflow: ellipsis;
  flex-wrap: wrap;
  gap: 2px;
  flex: 1;
`;

export const CardTitle = styled.h3`
  font-size: 1rem;
  color: #333;
  font-weight: 700;
  margin-top: 20px;
  overflow: hidden;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  transition: color 0.3s ease;

  ${CardContainer}:hover & {
    color: #007bff;
  }
`;

export const CardFooter = styled.div`
  padding: 10px;
  color: #666;
  font-size: 0.875rem;
  margin-top: auto;
`;

export const ScoreWrapper = styled.span<{ size: number }>`
  position: absolute;
  top: 230px;
  left: 8px;
  background-color: #081c22;
  border-radius: 50%;
  width: ${({ size }) => size}px;
  height: ${({ size }) => size}px;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1;
`;

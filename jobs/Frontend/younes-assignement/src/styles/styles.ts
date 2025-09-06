import styled, { keyframes } from "styled-components";

export const PageContainer = styled.div`
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
  font-family: sans-serif;
`;

export const MovieSearchInput = styled.input`
  width: 100%;
  padding: 10px;
  font-size: 16px;
  border: 1px solid #ccc;
  border-radius: 6px;
  margin-bottom: 20px;
`;

export const MovieCardGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
  gap: 36px;
`;

export const MovieCardWrapper = styled.div`
  background-color: #fcfcfc;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: center;
  border-radius: 8px;
  overflow: hidden;
  box-sizing: border-box;
  width: 100%;
  height: 100%;
  &:hover {
    background: #f2f2f2;
  }
`;

export const MovieCardTitle = styled.h3`
  font-size: 14px;
  font-weight: 600;
  margin-top: 16px;
  text-align: center;
`;

export const LoadMoreButton = styled.button`
  display: block;
  margin: 20px auto;
  padding: 10px 20px;
  font-size: 14px;
  border: none;
  border-radius: 6px;
  background: #007bff;
  color: white;
  cursor: pointer;
  &:hover {
    background: #0056b3;
  }
`;

export const MovieDetailPoster = styled.img`
  width: 100%;
  max-width: 400px;
  border-radius: 8px;
  margin-bottom: 20px;
`;

export const PlaceholderBox = styled.div<{ width?: string; height?: string }>`
  width: ${({ width }) => width || "100%"};
  height: ${({ height }) => height || "270px"};
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: #eee;
  color: #666;
  border-radius: 6px;
  font-size: 16px;
  font-weight: 500;
  text-align: center;
`;

export const MovieTextContainer = styled.div`
  max-width: 600px;
  line-height: 1.6;
`;

export const EmptyStateContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 300px;
  text-align: center;
  color: #666;
`;

export const EmptyStateMessage = styled.p`
  margin-top: 12px;
  font-size: 16px;
  font-weight: 500;
`;

const spin = keyframes`
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
`;

export const Spinner = styled.div`
  animation: ${spin} 1s linear infinite;
`;

export const ErrorMessageContainer = styled(EmptyStateContainer)`
  color: #d9534f;
`;

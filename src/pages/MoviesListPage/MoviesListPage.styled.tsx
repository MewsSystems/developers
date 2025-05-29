import styled from 'styled-components';

export const Container = styled.div`
  min-height: 100vh;
  display: flex;
  flex-direction: column;
`;

export const Header = styled.div`
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  background: white;
  padding: 8px;
  z-index: 10;
`;

export const SearchContainer = styled.div`
  position: relative;
  max-width: 800px;
  margin: 0 auto;
  width: 100%;
`;

export const SearchInput = styled.input`
  width: 100%;
  padding: 10px 16px;
  font-size: 15px;
  background-color: #f3f4f6;
  border: none;
  border-radius: 6px;
  outline: none;
  transition: all 0.2s ease;

  &:focus {
    background-color: #e5e7eb;
  }
`;

export const SearchWarning = styled.div<{isError: boolean}>`
  color: ${({isError}) => (isError ? '#dc2626' : '#d97706')};
  font-size: 14px;
  margin-top: 4px;
  position: absolute;
`;

export const Content = styled.div`
  flex: 1;
  padding: 16px;
  max-width: 1600px;
  margin: 72px auto 0;
  width: 100%;
`;

export const MoviesGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 16px;
  margin-bottom: 24px;
`;

import styled, {css} from 'styled-components';

type MovieCoverContainerProps = {
  $isMinimized: boolean;
};

export const MovieCoverContainer = styled.div<MovieCoverContainerProps>`
  position: relative;
  width: 100%;
  background: #f3f4f6;
  overflow: hidden;

  ${({$isMinimized}) =>
    $isMinimized
      ? css`
          height: 150px;
          border-radius: 8px 8px 0 0;
        `
      : css`
          border-radius: 12px;
          aspect-ratio: 2/3;
          box-shadow:
            0 4px 6px -1px rgb(0 0 0 / 0.1),
            0 2px 4px -2px rgb(0 0 0 / 0.1);
        `}
`;

export const MovieCoverImage = styled.img`
  width: 100%;
  height: 100%;
  object-fit: cover;
`;

export const PlaceholderContainer = styled.div`
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(
    45deg,
    #e5e7eb 25%,
    #d1d5db 25%,
    #d1d5db 50%,
    #e5e7eb 50%,
    #e5e7eb 75%,
    #d1d5db 75%
  );
  background-size: 20px 20px;
  color: #6b7280;
`;

export const PlaceholderIcon = styled.div`
  width: 24px;
  height: 24px;
  border: 2px solid currentColor;
  border-radius: 4px;
  display: flex;
  align-items: center;
  justify-content: center;

  &::before {
    content: '🎬';
    font-size: 14px;
  }
`;

import {LoadingContainer} from './PopcornLoader.styled.tsx';
import PopcornIcon from './PopcornIcon.tsx';

export default function PopcornLoader() {
  return (
    <LoadingContainer>
      <PopcornIcon />
    </LoadingContainer>
  );
}

PopcornLoader.displayName = 'PopcornLoader';

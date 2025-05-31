import {LoadingContainer} from './PopcornLoader.styled.tsx';
import popcornIcon from './popcorn.svg';

export default function PopcornLoader() {
  return (
    <LoadingContainer>
      <img src={popcornIcon} alt="Loading..." width={150} height={150} />
    </LoadingContainer>
  );
}

PopcornLoader.displayName = 'PopcornLoader';

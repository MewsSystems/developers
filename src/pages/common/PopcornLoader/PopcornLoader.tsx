import {LoadingContainer} from './PopcornLoader.styled';
import popcornIcon from './popcorn.svg';

export default function PopcornLoader() {
  return (
    <LoadingContainer>
      <img src={popcornIcon} alt="Loading..." width={150} height={150} />
    </LoadingContainer>
  );
}

PopcornLoader.displayName = 'PopcornLoader';

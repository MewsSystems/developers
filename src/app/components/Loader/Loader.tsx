import {LoadingOverlay, Spinner} from './Loader.styled.ts';

export default function Loader() {
  return (
    <LoadingOverlay>
      <Spinner />
    </LoadingOverlay>
  );
}

Loader.displayName = 'Loader';

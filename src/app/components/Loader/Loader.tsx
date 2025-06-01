import {LoadingOverlay, Spinner} from './styled';

export default function Loader() {
  return (
    <LoadingOverlay>
      <Spinner />
    </LoadingOverlay>
  );
}

Loader.displayName = 'Loader';

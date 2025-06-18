import type { RenderErrorLayoutParams } from '../../../types';
import { ErrorLayout } from '../ErrorLayout/ErrorLayout';

export const RenderErrorLayout: React.FC<RenderErrorLayoutParams> = ({ error }) => {
  return error ? <ErrorLayout title={error.message} /> : null;
};
